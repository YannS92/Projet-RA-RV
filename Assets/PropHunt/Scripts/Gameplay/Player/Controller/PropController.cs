using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PropController : ClassController
{
    [SerializeField] protected float baseRigidbodyMass = 60;
    [SerializeField] protected float cameraDistanceFromBody = 3;
    [SerializeField] protected float _baseMorphingRange = 1f;
    [SerializeField] protected float _rangeSizeMultiplier = 1f;
    [SerializeField] protected string _baseAddressableName = "PlayerProp";
    [SerializeField] protected string _playerLayerName = "Player";
    [SerializeField] private TMP_Text _targetPropText;

    public float DistanceFromCenter;

    [Header("Morph Action")]
    private Rigidbody _rigidbody;
    private Renderer _currentRenderer;

    [Header("Taunt Action")]
    public AudioClip[] TauntList;
    [SerializeField] protected AudioSource _audioSource;

    protected int _playerLayer;
    protected Prop focusedProp;
    AsyncOperationHandle<GameObject> _loadPropHandle;

    private NetworkVariable<FixedString64Bytes> _currentMorphName = new NetworkVariable<FixedString64Bytes>();

    [SerializeField] protected float _tauntCooldownTime = 2.0f;
    private float _nextTauntAvailableTime;
    private bool _isTauntAvailable = true;
    ClientRpcParams clientRpcParams;

    #region Unity event functions
    protected override void Awake()
    {
        base.Awake();
        if (_audioSource == null)
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }
        _playerLayer = LayerMask.NameToLayer(_playerLayerName);

        SetBodyRenderer();
        ComputeGroundedOffset();
        ComputeDistanceBoundFromCenter();
        _camera = transform.parent.GetComponentInChildren<Camera>(true);
        _rigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //Only the server can assign NetworkVariable values
        if (IsServer)
        {
            _currentMorphName.Value = _baseAddressableName;
        }
        _targetPropText = _camera.GetComponentInChildren<TMP_Text>();
        _currentMorphName.OnValueChanged += MorphPropCallback;
        if (IsClient && !IsOwner && _currentMorphName.Value != _baseAddressableName)
        {
            StartCoroutine(LoadPropCoroutine(_currentMorphName.Value.ToString()));
        }


        clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { OwnerClientId }
            }
        };
    }

    private void Update()
    {
        CheckForTauntServer();
    }


    void FixedUpdate()
    {
        if (!IsOwner) return;
        GetNewTargetProp();
    }
    #endregion

    #region Activation / Deactivation
    /// <summary>
    /// Activate the PropController, resetting the Camera position and the animator.
    /// </summary>
    public override void Activate()
    {
        gameObject.SetActive(true);
        _camera.transform.SetParent(transform);
        ProcessCamPosition();
        ResetAnimator();
    }

    /// <summary>
    /// Deactivate the PropController, emptying the propName field and resetting the Prop's body.
    /// </summary>
    public override void Deactivate()
    {
        CancelMorph();
        _targetPropText.text = string.Empty;
        gameObject.SetActive(false);
    }
    #endregion

    #region Action methods
    public void MorphProp()
    {
        if (focusedProp == null) return;
        ChangeCurrentPropServerRPC(focusedProp.PropName);
    }




    public void CancelMorph()
    {
        ChangeCurrentPropServerRPC(_baseAddressableName);
        ResetAnimator();

    }

    public void Taunt()
    {
        var tauntNumber = Random.Range(0, TauntList.Length);
        if (_isTauntAvailable)
        {
            PlayTaunt(tauntNumber);
            SendTauntServerRpc(tauntNumber);
        }
    }




    private void CheckForTauntServer()
    {
        if (IsServer && Time.time >= _nextTauntAvailableTime)
        {
            _isTauntAvailable = true;
            AllowTauntClientRpc(clientRpcParams);
        }
    }

    [ClientRpc]
    private void AllowTauntClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner)
        {

            return;
        }
        _isTauntAvailable = true;
    }

    #endregion

    #region Morph

    /// <summary>
    /// Loads the Prefab corresponding to the parameter <paramref name="propName"/>. On success, apply the prop's parameters to the player.
    /// </summary>
    /// <param name="propName">Name of the loaded prop.</param>
    private IEnumerator LoadPropCoroutine(string propName)
    {
        _loadPropHandle = Addressables.LoadAssetAsync<GameObject>(propName);
        yield return _loadPropHandle;

        if (_loadPropHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject gameObjectModel = _loadPropHandle.Result;
            ApplyProp(gameObjectModel.GetComponent<Prop>());
        }
    }

    /// <summary>
    /// Apply <paramref name="propModel"/> prop's properties to the player.
    /// </summary>
    /// <param name="propModel"></param>
    protected void ApplyProp(Prop propModel)
    {
        Destroy(Body);
        Body = Instantiate(propModel.BodyGameObject, transform);
        _rigidbody.mass = propModel.Rigidbody.mass;
        var newPosY = transform.position.y + 0.15f;
        transform.parent.position = new Vector3(transform.position.x, newPosY, transform.position.z);
        Body.transform.localScale = propModel.transform.localScale;
        ComputeDistanceBoundFromCenter();
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.gameObject.layer = _playerLayer;
        }
        SetBodyRenderer();
        ComputeGroundedOffset();
        ProcessCamPosition();
        ResetAnimator();
    }

    /// <summary>
    /// Set the _currentRenderer property to the currently used Renderer.
    /// </summary>
    private void SetBodyRenderer()
    {
        _currentRenderer = Body.GetComponent<Renderer>();
        if (_currentRenderer == null)
        {
            _currentRenderer = Body.GetComponentInChildren<Renderer>();
        }
    }

    /// <summary>
    /// Moves the Camera depending on the size of the current renderer.
    /// </summary>
    private void ProcessCamPosition()
    {
        Vector3 boundsSize = _currentRenderer.localBounds.size;
        Vector3 bodySize = _currentRenderer.transform.lossyScale;
        Vector3 size = new Vector3(
            boundsSize.x * bodySize.x,
            boundsSize.y * bodySize.y,
            boundsSize.z * bodySize.z
            );
        Vector3 camPos = _camera.transform.localPosition;
        camPos.z = Mathf.Clamp(-(size.y / 2 * 3f), -10, -3);
        camPos.y = Mathf.Clamp(size.y * 0.8f + 0.3f, 1.2f, 10);
        _camera.transform.localPosition = camPos;
    }
    #endregion

    #region Taunt

    /// <summary>
    /// Plays the taunt in local.
    /// </summary>
    /// <param name="tauntNumber">AudioClip number to play</param>
    public void PlayTaunt(int tauntNumber)
    {
        _audioSource.clip = TauntList[tauntNumber];
        _audioSource.Play();
    }

    #endregion

    #region Other


        /// <summary>
    /// Send all clients 
    /// </summary>
    /// <param name="tauntNumber"></param>
    [ServerRpc]
    public void SendTauntServerRpc(int tauntNumber)
    {
        if (_isTauntAvailable)
        {
            SendTauntClientRpc(tauntNumber);

            _nextTauntAvailableTime = Time.time + _tauntCooldownTime;
            _isTauntAvailable = false;
        }
    }

    [ClientRpc]
    private void SendTauntClientRpc(int tauntNumber)
    {
        if (IsOwner)
        {
            _nextTauntAvailableTime = Time.time + _tauntCooldownTime;
            _isTauntAvailable = false;
        }
        PlayTaunt(tauntNumber);
    }

    /// <summary>
    /// Compute the distance between the border on the y axis and its center.
    /// </summary>
    private void ComputeGroundedOffset()
    {
        GroundedOffset = _currentRenderer.localBounds.extents.y;
    }
    /// <summary>
    /// Callback called when the NetworkVariable _currentMorphName has changed. Start the Coroutine loading the addressable corresponding to the Prop.
    /// </summary>
    /// <param name="prevPropName">Previous _currentMorphName value.</param>
    /// <param name="newPropName">New _currentMorphName value.</param>
    private void MorphPropCallback(FixedString64Bytes prevPropName, FixedString64Bytes newPropName)
    {
        StartCoroutine(LoadPropCoroutine(newPropName.ToString()));
    }

    /// <summary>
    /// Change the Network Variable _currentMorphName value to <paramref name="propName"/>.
    /// </summary>
    /// <param name="propName">Name of the new prop used as a body.</param>
    [ServerRpc]
    private void ChangeCurrentPropServerRPC(string propName)
    {
        _currentMorphName.Value = propName;
    }


    /// <summary>
    /// Compute the distance between the border of the model and its center.
    /// </summary>
    private void ComputeDistanceBoundFromCenter()
    {
        DistanceFromCenter = _currentRenderer.bounds.extents.z;
    }

    /// <summary>
    /// Get the prop in the middle of the screen and set him as the focused prop.
    /// </summary>
    protected void GetNewTargetProp()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitpoint;
        if (Physics.Raycast(
        ray.origin, ray.direction, out hitpoint,
        -(_camera.transform.localPosition.z) + (DistanceFromCenter * _rangeSizeMultiplier) + _baseMorphingRange,
        LayerMask.GetMask(LayerEnum.Props.ToString())))
        {
            Prop prop = hitpoint.transform.GetComponent<Prop>();
            if (prop == null)
            {
                return;
            }
            _targetPropText.text = prop.PropName;
            focusedProp = prop;
            return;
        }
        _targetPropText.text = "";
        focusedProp = null;
    }
    #endregion
}

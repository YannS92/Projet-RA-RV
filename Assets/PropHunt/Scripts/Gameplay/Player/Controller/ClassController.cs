using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class ClassController : NetworkBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected Camera _camera;
    [SerializeField] protected Animator _animator;
    public GameObject Body {
        get;
        protected set;
    }

    public ClassInput ClassInput
    {
        get;
        protected set;
    }

    public float GroundedOffset
    {
        get;
        protected set;
    }
    public Animator Animator {
        get { return _animator; }
        protected set { _animator = value; }
    }

    protected virtual void Awake()
    {
        Body = transform.GetChild(0).gameObject;
        if(ClassInput == null)
        {
            ClassInput = GetComponent<ClassInput>();
        }
        var renderer = Body.GetComponent<Renderer>();
        if(renderer == null)
        {
            renderer = Body.GetComponentInChildren<Renderer>();
        }
        GroundedOffset = renderer.localBounds.extents.y;
    }

    public abstract void Activate();
    public abstract void Deactivate();
    public override void OnNetworkSpawn()
    {
        if(_camera == null) _camera = transform.parent.GetComponentInChildren<Camera>(true);

        if(_animator == null)
        {
            _animator = GetComponentInParent<Animator>();
        }

        if (this.IsOwner)
        {
            enabled = true;
        }
    }
    protected void ResetAnimator()
    {
        gameObject.transform.SetSiblingIndex(0); //Animator targets the first compatible mesh.
        Animator.Rebind();
        Animator.Update(0f);
    }
}
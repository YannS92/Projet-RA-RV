using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class MovePlateformValueSlider : MonoBehaviour
{
    [SerializeField] private Slider _sliderX;

    [SerializeField] private Slider _sliderY;

    [SerializeField] private Slider _sliderZ;

    [SerializeField] private Slider _speed;

    [SerializeField] public Vector3 posToMove;
    [SerializeField] public float speed;
    [SerializeField] private Transform objectToMove; // Référence à l'objet à déplacer

    // Start is called before the first frame update
    void Start()
    {
        _sliderX.onValueChanged.AddListener((value) =>
        {
            UpdatePosition();
        });

        _sliderY.onValueChanged.AddListener((value) =>
        {
            UpdatePosition();
        });

        _sliderZ.onValueChanged.AddListener((value) =>
        {
            UpdatePosition();
        });

        _sliderZ.onValueChanged.AddListener((value) =>
        {
            UpdatePosition();
        });
        _speed.onValueChanged.AddListener((value) =>
        {
            speed = _speed.value;
        });
        
    }

    void UpdatePosition()
    {
        float x = _sliderX.value;
        float y = _sliderY.value;
        float z = _sliderZ.value;

        posToMove = new Vector3(x, y, z);

        objectToMove.position = new Vector3(x, (y + 0.5f), z); ;
    }
}

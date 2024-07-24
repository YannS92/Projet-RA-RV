using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider; // Correction ici : utiliser le type Slider au lieu de ScriptSlider

    [SerializeField] private TextMeshProUGUI _sliderText;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((v) =>
        {
            _sliderText.text = v.ToString("0");
        });
    }

    // Update is called once per frame
    void Update()
    {
        // Aucune action nécessaire dans la mise à jour pour l'instant
    }
}

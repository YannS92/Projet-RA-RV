using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    public Transform selectionTransform;
    public Transform cursorTransform;

    public RadialSection top;
    public RadialSection right;
    public RadialSection bottom;
    public RadialSection left;

    public Vector2 touchPosition = Vector2.zero;
    private List<RadialSection> sections;
    private RadialSection highlightedSection = null;

    private readonly float degreeIncrement = 90.0f;

    private void Awake()
    {
        CreateAndSetupSection();
    }

    private void CreateAndSetupSection()
    {
        sections = new List<RadialSection>()
        {
            top, right, bottom, left
        };

        foreach (RadialSection section in sections)
        {
            section.content.text = section.text;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Show(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Vector2.zero + touchPosition;
        float rotation = GetDegree(direction);
        SetCursorPosition();
        SetSelectionRotation(rotation);
        SetSelectedEvent(rotation);
    }

    private void SetSelectionRotation(float newRotation)
    {
        float snappedRotation = SnapRotation(newRotation);
        selectionTransform.localEulerAngles = new Vector3(0,0,-snappedRotation);
    }

    private float SnapRotation(float rotation)
    {
        return GetNearestIncrement(rotation) * degreeIncrement;
    }

    private int GetNearestIncrement(float rotation)
    {
        return Mathf.RoundToInt(rotation / degreeIncrement);
    }

    private void SetSelectedEvent(float currentRotation)
    {
        int index = GetNearestIncrement(currentRotation);

        if (index == 4)
            index = 0;

        highlightedSection = sections[index];

    }

    public void ActivateHighlightedSection()
    {
        highlightedSection.onSelect.Invoke();
    }

    public void Show(bool value)
    {
        gameObject.SetActive(value);
    }

    private float GetDegree(Vector2 direction)
    {
        float value = Mathf.Atan2(direction.x, direction.y);
        value *= Mathf.Rad2Deg;

        if (value < 0)
        {
            value += 360f;
        }

        return value;
    }

    private void SetCursorPosition()
    {
        cursorTransform.localPosition = touchPosition;
    }

    public void SetTouchPosition(Vector2 newValue)
    {
        touchPosition = newValue;
    }
}

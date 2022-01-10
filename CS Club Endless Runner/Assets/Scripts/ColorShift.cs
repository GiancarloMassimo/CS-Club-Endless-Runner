using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorShift : MonoBehaviour
{
    [SerializeField]
    float colorIncrement, shiftSpeed;

    Volume volume;
    ColorAdjustments colorAdjustments;

    float colorShift = 0;

    void OnEnable()
    {
        PortalCollision.OnPortalCollision += ShiftColor;
    }

    void OnDisable()
    {
        PortalCollision.OnPortalCollision -= ShiftColor;
    }

    void Start()
    {
        volume = GetComponent<Volume>();

        ColorAdjustments temp;
        if (volume.profile.TryGet<ColorAdjustments>(out temp))
        {
            colorAdjustments = temp;
        }

    }

    void Update()
    {
        colorAdjustments.hueShift.value = Mathf.Lerp(colorAdjustments.hueShift.value, colorShift, shiftSpeed * Time.deltaTime);
    }

    void ShiftColor()
    {
        colorShift += colorIncrement;

        if (colorShift > 180)
        {
            colorShift -= 360;
        }
    }
}

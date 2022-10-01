using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class NumberPresenter : CanvasGroupMonobehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txtValue;
    [SerializeField]
    protected TextMeshProUGUI txtUnit;
    [SerializeField]
    protected EMeasurementUnit measurementUnit = EMeasurementUnit.Unknown;

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(txtValue);
    }

    public void SetValue(int value)
    {
        txtValue.text = value.ToString();
        SetUnitByValue(value);
    }

    void SetUnitByValue(int value)
    {
        if (txtUnit == null) return;

        string text = string.Empty;
        switch (measurementUnit)
        {
            case EMeasurementUnit.Seconds:
                text = value > 1 ? "seconds" : "second";
                break;
            default:
                break;
        }
        txtUnit.text = text;
    }
}

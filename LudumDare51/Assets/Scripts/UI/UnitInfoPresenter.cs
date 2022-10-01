using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitInfoPresenter : CanvasGroupMonobehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txtName;
    [SerializeField]
    protected TextMeshProUGUI txtHealth;
    [SerializeField]
    protected TextMeshProUGUI txtArmor;
    [SerializeField]
    protected TextMeshProUGUI txtDamage;
    [SerializeField]
    protected TextMeshProUGUI txtAttackSpeed;
    [SerializeField]
    protected TextMeshProUGUI txtMoveSpeed;
    [SerializeField]
    protected TextMeshProUGUI txtDodge;

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(txtName);
        Assert.IsNotNull(txtHealth);
        Assert.IsNotNull(txtArmor);
        Assert.IsNotNull(txtDamage);
        Assert.IsNotNull(txtAttackSpeed);
        Assert.IsNotNull(txtMoveSpeed);
        Assert.IsNotNull(txtDodge);
    }

    public void SetData(IUnitBlueprint value)
    {
        txtName.text = value.ToNameAndLevelString();
        txtHealth.text = value.ToHealthValueString();
        txtArmor.text = value.ToArmorValueString();
        txtDamage.text = value.ToDamageValueString();
        txtAttackSpeed.text = value.ToAttackSpeedValueString();
        txtMoveSpeed.text = value.ToMoveSpeedValueString();
        txtDodge.text = value.ToDodgeValueString();
    }

    //void SetUnitByValue(int value)
    //{
    //    if (txtUnit == null) return;

    //    string text = string.Empty;
    //    switch (measurementUnit)
    //    {
    //        case EMeasurementUnit.Seconds:
    //            text = value > 1 ? "seconds" : "second";
    //            break;
    //        default:
    //            break;
    //    }
    //    txtUnit.text = text;
    //}
}

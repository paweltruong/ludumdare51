using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitBlueprintSlot : CanvasGroupMonobehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txtName;
    [SerializeField]
    protected TextMeshProUGUI txtDesc;
    [SerializeField]
    protected int slotIndex = -1;
    [SerializeField]
    protected ESlotStatus status = ESlotStatus.None;

    protected IUnitBlueprint unitBlueprint;

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(txtName);
        Assert.IsNotNull(txtDesc);
        Assert.IsNotNull(masterCanvasGroup);

        ResetSlot();
    }

    public virtual void SetData(IUnitBlueprint unitBp)
    {
        this.unitBlueprint = unitBp;

        if (unitBlueprint == null)
        {
            ResetSlot();
            return;
        }

        txtName.text = unitBlueprint.GetName();
        txtDesc.text = unitBlueprint.ToShortLevelString();
    }

    protected virtual void ResetSlot()
    {
        unitBlueprint = null;
        txtName.text = string.Empty;
        txtDesc.text = string.Empty;
        SetStatus(ESlotStatus.None);
    }

    public virtual void SetStatus(ESlotStatus newStatus)
    {
        this.status = newStatus;
    }
}

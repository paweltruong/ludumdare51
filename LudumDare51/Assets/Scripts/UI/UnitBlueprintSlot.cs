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

        Assert.IsNotNull(masterCanvasGroup);

        ResetSlot();
    }

    public virtual void SetData(IUnitBlueprint unitBp)
    {
        this.unitBlueprint = unitBp;

        UpdateUI();
    }
    public virtual void SetStatus(ESlotStatus newStatus)
    {
        this.status = newStatus;
        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        if (unitBlueprint != null)
        {
            if (txtName) txtName.text = unitBlueprint.GetName();
            if (txtDesc) txtDesc.text = unitBlueprint.ToShortLevelString();
        }
        else
        {
            if (txtName) txtName.text = string.Empty;
            if (txtDesc) txtDesc.text = string.Empty;
        }
    }


    protected virtual void ResetSlot()
    {
        this.unitBlueprint = null;
        UpdateUI();
    }
}

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PawnSlotPresenter : UnitBlueprintSlot
{
    [SerializeField]
    protected Image imgAvailable;
    [SerializeField]
    protected Image imgUnavailable;
    [SerializeField]
    protected Image imgSelected;
    [SerializeField]
    protected Image imgNone;

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(imgAvailable);
        Assert.IsNotNull(imgUnavailable);
        Assert.IsNotNull(imgSelected);
        Assert.IsNotNull(imgNone);
    }

    public override void SetData(IUnitBlueprint unitBp)
    {
        base.SetData(unitBp);
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();
        txtDesc.text = "empty";
    }

    public override void SetStatus(ESlotStatus newStatus)
    {
        base.SetStatus(newStatus);

        imgAvailable.gameObject.SetActive(status == ESlotStatus.Available);
        imgUnavailable.gameObject.SetActive(status == ESlotStatus.Unavailable);
        imgSelected.gameObject.SetActive(status == ESlotStatus.Selected);
        imgNone.gameObject.SetActive(status == ESlotStatus.None);
    }
}

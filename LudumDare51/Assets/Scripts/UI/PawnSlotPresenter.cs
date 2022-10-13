using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
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
    [SerializeField]
    protected Button button;

    public UnitInstance UnitInstance { get { return unitBlueprint as UnitInstance; } }

    public UnityEvent<UnitInstance, int> OnSlotUnselected;
    public UnityEvent<UnitInstance, int> OnSlotSelected;
    public UnityEvent<UnitInstance, int> OnRemoveFromLineupConfirmed;

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(txtName);
        Assert.IsNotNull(txtDesc);

        Assert.IsNotNull(imgAvailable);
        Assert.IsNotNull(imgUnavailable);
        Assert.IsNotNull(imgSelected);
        Assert.IsNotNull(imgNone);
        Assert.IsNotNull(button);

        Singleton.Instance.GameInstance.GameState.OnSlotChanged.AddListener(GameState_OnSlotChanged);
        Singleton.Instance.GameInstance.GameState.OnSelectedUnitChanged.AddListener(GameState_OnSelectedUnitChanged);

        button.onClick.AddListener(Button_OnClick);

        UpdateFromGameState();
    }

    private void GameState_OnSelectedUnitChanged(UnitInstance unit)
    {
        UpdateFromGameState();
    }

    void UpdateFromGameState()
    {
        SetData(Singleton.Instance.GameInstance.GameState.Slots[slotIndex]);
    }

    void Button_OnClick()
    {
        //this works in any phase
        if (Singleton.Instance.GameInstance.GameState.SelectedUnit == UnitInstance)
        {
            //Unselect
            Singleton.Instance.GameInstance.GameState.SelectUnit(null);
        }
        else
        {
            switch (Singleton.Instance.GameInstance.GameState.CurrentPhase)
            {
                case EGamePhase.Preparation:
                    if (unitBlueprint == null && Singleton.Instance.GameInstance.GameState.SelectedUnit != null)
                    {
                        if (Singleton.Instance.GameInstance.GameState.SelectedUnit.IsInLineup)
                        {
                            Singleton.Instance.GameInstance.GameState.ReturnFromLineupToSlot(Singleton.Instance.GameInstance.GameState.SelectedUnit, slotIndex);
                            Singleton.Instance.GameInstance.GameState.SelectUnit(null);

                        }
                        else
                        {
                            //Change to this slot
                            Singleton.Instance.GameInstance.GameState.ChangeSlot(Singleton.Instance.GameInstance.GameState.SelectedUnit, slotIndex);
                        }
                        return;
                    }
                    if (unitBlueprint != null && Singleton.Instance.GameInstance.GameState.SelectedUnit == null)
                    {
                        Singleton.Instance.GameInstance.GameState.SelectUnit(UnitInstance);
                    }
                    break;
                case EGamePhase.Trial:
                    Singleton.Instance.GameInstance.GameState.SelectUnit(UnitInstance);
                    break;
                default:
                    break;

            }
        }
    }

    private void GameState_OnSlotChanged(int slotIndex)
    {
        if (this.slotIndex == slotIndex)
        {
            SetData(Singleton.Instance.GameInstance.GameState.Slots[slotIndex]);
        }
    }

    public override void SetData(IUnitBlueprint unitBp)
    {
        base.SetData(unitBp);

        //if unit is selected then Selected
        if (UnitInstance != null && UnitInstance == Singleton.Instance.GameInstance.GameState.SelectedUnit)
        {
            SetStatus(ESlotStatus.Selected);
        }
        else if (Singleton.Instance.GameInstance.GameState.SelectedUnit
            && Singleton.Instance.GameInstance.GameState.CurrentPhase == EGamePhase.Preparation)
        {
            if (unitBp == null)
            {
                //this slot is available for remove from lineup to this
                SetStatus(ESlotStatus.Available);
            }
            else
            {
                //this slot is NOT available for remove from lineup to this
                SetStatus(ESlotStatus.Unavailable);
            }
        }
        else if (Singleton.Instance.GameInstance.GameState.SelectedUnit)
        {
            if (unitBp != null)
            {
                //Set new unit to slot (recruit)?
                SetStatus(ESlotStatus.None);
            }
            else
            {
                //removing from slot (sold?)
                ResetSlot();
            }
        }
        else
        {
            SetStatus(ESlotStatus.None);
            if (unitBp == null)
            {
                //There is no selected unit
                ResetSlot();
            }
            else
            {
            }
        }
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();

        if(unitBlueprint == null && txtDesc) txtDesc.text = "empty";
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();
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

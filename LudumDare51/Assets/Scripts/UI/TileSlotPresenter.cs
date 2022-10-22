using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class TileSlotPresenter : UnitBlueprintSlot
{
    [SerializeField]
    protected Color colorAvailable;
    [SerializeField]
    protected Color colorUnavailable;
    [SerializeField]
    protected Color colorSelected;
    [SerializeField]
    protected Color colorNone;
    [SerializeField]
    protected Image imgButton;
    [SerializeField]
    protected Image imgUnitIcon;
    [SerializeField]
    protected Button button;


    public UnitInstance UnitInstance { get { return unitBlueprint as UnitInstance; } }

    public UnityEvent<UnitInstance, int> OnSlotUnselected;
    public UnityEvent<UnitInstance, int> OnSlotSelected;
    public UnityEvent<UnitInstance, int> OnRemoveFromLineupConfirmed;

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(imgButton);
        Assert.IsNotNull(imgUnitIcon);
        Assert.IsNotNull(button);

        Singleton.Instance.GameInstance.GameState.OnSelectedUnitChanged.AddListener(GameState_OnSelectedUnitChanged);

        button.onClick.AddListener(Button_OnClick);
    }

    private void GameState_OnSelectedUnitChanged(UnitInstance unit)
    {
        UpdateFromGameState();
    }

    void UpdateFromGameState()
    {
        SetData(Singleton.Instance.GameInstance.GameState.PlayerTiles[slotIndex]);
    }

    void Button_OnClick()
    {
        switch (Singleton.Instance.GameInstance.GameState.CurrentPhase)
        {
            case EGamePhase.Preparation:
                if (!Singleton.Instance.GameInstance.GameState.SelectedUnit)
                {
                    if (UnitInstance)
                    {
                        //Select unit on lineup
                        Singleton.Instance.GameInstance.GameState.SelectUnit(UnitInstance);
                    }
                    else
                    {
                        //clicking on empty lineuptile - no effect   
                    }
                }
                else
                {
                    if (Singleton.Instance.GameInstance.GameState.SelectedUnit == UnitInstance)
                    {
                        //Clicked on selected lineup unit - deselect
                        Singleton.Instance.GameInstance.GameState.SelectUnit(null);
                    }
                    else if (unitBlueprint == null)
                    {
                        //Move selected unit to empty placement tile
                        Singleton.Instance.GameInstance.GameState.MoveToLineupFromPawnSlot(Singleton.Instance.GameInstance.GameState.SelectedUnit, slotIndex);
                    }
                    else
                    {
                        //Clicked on non empty placemnt tile - swap unit?
                    }
                }
                break;
            default:
                break;

        }
    }

    public override void SetData(IUnitBlueprint unitBp)
    {
        base.SetData(unitBp);

        if (!Singleton.Instance.GameInstance.GameState.SelectedUnit)
        {
            SetStatus(ESlotStatus.None);
        }
        else
        {
            if (UnitInstance)
            {
                if (unitBlueprint == unitBp)
                {
                    SetStatus(ESlotStatus.Selected);
                }
                else
                {
                    SetStatus(ESlotStatus.Unavailable);
                }
            }
            else
            {
                SetStatus(ESlotStatus.Available);
            }
        }

        //if (UnitInstance == null && Singleton.Instance.GameInstance.GameState.SelectedUnit)
        //{
        //    //Unit was placed
        //    SetStatus(ESlotStatus.Selected);
        //}
        ////if unit is selected then Selected
        //if (UnitInstance != null && UnitInstance != Singleton.Instance.GameInstance.GameState.SelectedUnit)
        //{

        //}
        //else if()

        ////-------------
        //else if (Singleton.Instance.GameInstance.GameState.SelectedUnit != null
        //    && Singleton.Instance.GameInstance.GameState.CurrentPhase == EGamePhase.Preparation)
        //{
        //    if (unitBp == null)
        //    {
        //        //this slot is available for remove from lineup to this
        //        SetStatus(ESlotStatus.Available);
        //    }
        //    else
        //    {
        //        //this slot is NOT available for remove from lineup to this
        //        SetStatus(ESlotStatus.Unavailable);
        //    }
        //}
        //else if (Singleton.Instance.GameInstance.GameState.SelectedUnit == null)
        //{
        //    if (unitBp != null)
        //    {
        //        //Set new unit to slot (recruit)?
        //        SetStatus(ESlotStatus.None);
        //    }
        //    else
        //    {
        //        //removing from slot (sold?)
        //        ResetSlot();
        //    }
        //}
    }

    public override void UpdateUI()
    {
        base.UpdateUI();

        Color newColor = imgButton.color;
        switch (status)
        {
            case ESlotStatus.Available:
                newColor = colorAvailable;
                break;
            case ESlotStatus.Unavailable:
                newColor = colorUnavailable;
                break;
            case ESlotStatus.Selected:
                newColor = colorSelected;
                break;
            default:
                newColor = colorNone;
                break;
        }
        imgButton.color = newColor;

        if (unitBlueprint == null)
        {
            imgUnitIcon.sprite = null;
            imgUnitIcon.gameObject.SetActive(false);
        }
        else
        {
            imgUnitIcon.sprite = unitBlueprint.GetIcon();
            imgUnitIcon.gameObject.SetActive(true);
        }
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();
        if (txtDesc) txtDesc.text = "empty";
    }

    public override void SetStatus(ESlotStatus newStatus)
    {
        base.SetStatus(newStatus);
    }
}

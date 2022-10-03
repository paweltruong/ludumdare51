using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class RecruitmentSlotPresenter : UnitBlueprintSlot
{
    [SerializeField]
    TextMeshProUGUI txtCost;
    [SerializeField]
    int slotIndex = -1;

    [SerializeField]
    GameObject ButtonGO;
    [SerializeField]
    GameObject CostGO;

    [SerializeField]
    Color enoughCoinsColor;
    [SerializeField]
    Color notEnoughCoinsColor;

    public event UnitBlueprintEventHandler OnRecruitmentConfirmed;
    

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(txtCost);
        Assert.IsNotNull(ButtonGO);
        Assert.IsNotNull(CostGO);

        //ResetSlot();
        Singleton.Instance.GameInstance.GameState.OnTotalCoinsChanged += GameState_OnTotalCoinsChanged;
        Singleton.Instance.GameInstance.GameState.OnRecruitChanged += GameState_OnRecruitChanged;

        UpdateFromGameState();
    }

    private void GameState_OnRecruitChanged(int index)
    {
        if (slotIndex == index)
        {
            UpdateFromGameState();
        }
    }

    void UpdateFromGameState()
    {
        SetData(Singleton.Instance.GameInstance.GameState.Recruits[slotIndex]);
    }

    private void GameState_OnTotalCoinsChanged(int coins)
    {
        UpdateCostColor(coins);
    }


    public void Recruit()
    {
        if(unitBlueprint == null) 
        {
            Debug.LogError("Unit BP is null");
            return;
        }

        if (Singleton.Instance.GameInstance.GameState.GetAvailableFreeSlotsCount() <= 0)
        {
            //No free slots cannot recruit
            return;
        }

        if (Singleton.Instance.GameInstance.GameState.PlayerCoins < unitBlueprint.GetCost())
        {
            //Not enough coins
            return;
        }

        OnRecruitmentConfirmed.Invoke(unitBlueprint);

        //Slot used - Disable this slot
        ResetSlot();
    }

    public override void SetData(IUnitBlueprint unitBp)
    {
        base.SetData(unitBp);

        if (unitBlueprint == null)
        {
            SetStatus(ESlotStatus.Unavailable);
            return;
        }
        SetStatus(ESlotStatus.Available);

        txtCost.text = String.Format("{0} {1}",unitBlueprint.GetCost(), Singleton.Instance.GameInstance.Configuration.SpriteAtlas_Coin);
        UpdateCostColor(Singleton.Instance.GameInstance.GameState.PlayerCoins);
    }

    void UpdateCostColor(int availableCoins)
    {
        txtCost.color = availableCoins >= unitBlueprint.GetCost() ? enoughCoinsColor : notEnoughCoinsColor;
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();

        txtCost.text = string.Empty;
        SetStatus(ESlotStatus.Unavailable);
    }

    public override void SetStatus(ESlotStatus newStatus)
    {
        base.SetStatus(newStatus);

        switch (this.status)
        {
            case ESlotStatus.Available:
                ButtonGO.SetActive(true);
                CostGO.SetActive(true);
                break;
            default:
                ButtonGO.SetActive(false);
                CostGO.SetActive(false);
                break;
        }
    }
}

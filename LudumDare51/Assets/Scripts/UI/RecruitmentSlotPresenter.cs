using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecruitmentSlotPresenter : UnitBlueprintSlot
{
    [SerializeField]
    TextMeshProUGUI txtCost;

    [SerializeField]
    Button button;
    [SerializeField]
    GameObject CostGO;

    [SerializeField]
    Color enoughCoinsColor;
    [SerializeField]
    Color notEnoughCoinsColor;

    public UnityEvent<IUnitBlueprint, int> OnRecruitmentConfirmed;
    

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(txtName);
        Assert.IsNotNull(txtDesc);

        Assert.IsNotNull(txtCost);
        Assert.IsNotNull(button);
        Assert.IsNotNull(CostGO);

        //ResetSlot();
        Singleton.Instance.GameInstance.GameState.OnTotalCoinsChanged.AddListener(GameState_OnTotalCoinsChanged);
        Singleton.Instance.GameInstance.GameState.OnRecruitChanged.AddListener(GameState_OnRecruitChanged);
        Singleton.Instance.GameInstance.GameState.OnLineupChanged.AddListener(GameState_OnLineupChanged);

        button.onClick.AddListener(Button_OnClick);

        UpdateFromGameState();

    }

    private void GameState_OnRecruitChanged(int index)
    {
        if (slotIndex == index)
        {
            UpdateFromGameState();
        }
    }

    void Button_OnClick()
    {
        Recruit();
    }

    void UpdateFromGameState()
    {
        SetData(Singleton.Instance.GameInstance.GameState.Recruits[slotIndex]);
    }

    private void GameState_OnTotalCoinsChanged(int coins)
    {
        UpdateCostColor(coins);
    }

    void GameState_OnLineupChanged()
    {
        if (Singleton.Instance.GameInstance.GameState.IsLineupFull())
        {
            SetStatus(ESlotStatus.Unavailable);
        }
        else
        {
            SetStatus(ESlotStatus.Available);
        }
    }

    public void Recruit()
    {
        if (unitBlueprint == null) 
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

        OnRecruitmentConfirmed.Invoke(unitBlueprint, slotIndex);

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
        if (unitBlueprint == null) return;
        txtCost.color = availableCoins >= unitBlueprint.GetCost() ? enoughCoinsColor : notEnoughCoinsColor;
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();

        txtCost.text = string.Empty;
        SetStatus(ESlotStatus.Unavailable);
    }

    public override void UpdateUI()
    {
        base.UpdateUI();

        switch (this.status)
        {
            case ESlotStatus.Available:
                button.gameObject.SetActive(true);
                CostGO.SetActive(true);
                break;
            default:
                button.gameObject.SetActive(false);
                CostGO.SetActive(false);
                break;
        }
    }

    public override void SetStatus(ESlotStatus newStatus)
    {
        base.SetStatus(newStatus);
    }
}

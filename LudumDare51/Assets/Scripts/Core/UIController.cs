using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Announcer announcer;
    [SerializeField]
    NumberPresenter coins;
    [SerializeField]
    NumberPresenter incomeTimer;
    [SerializeField]
    NumberPresenter trialCountdown;
    [SerializeField]
    PawnSlotPresenter[] pawnSlots;

    [Header("Units")]
    [SerializeField]
    TextMeshProUGUI txtLineUpValue;

    [Header("Recruitment")]
    [SerializeField]
    RecruitmentSlotPresenter[] recruitmentSlots;
    [SerializeField]
    Button btnReroll;
    [SerializeField]
    TextMeshProUGUI txtReroll;
    [Header("Shop")]
    [SerializeField]
    CanvasGroup shopCanvasGroup;
    [SerializeField]
    Button btnSell;
    [SerializeField]
    TextMeshProUGUI txtSell;
    [Header("UnitPlacement")]
    [SerializeField]
    CanvasGroup tilesCanvasGroup;


    public event UnitBlueprintEventHandler OnRecruitmentConfirmed;

    public event UnitToSlotEventHandler OnPawnSlotUnselected;
    public event UnitToSlotEventHandler OnPawnSlotSelected;
    public event UnitToSlotEventHandler OnPawnRemoveFromLineupConfirmed;

    public event System.Action OnRecruitmentReroll;
    public event System.Action OnSellSelected;



    void Start()
    {
        Assert.IsNotNull(announcer);
        Assert.IsNotNull(coins);
        Assert.IsNotNull(incomeTimer);
        Assert.IsNotNull(trialCountdown);
        Assert.IsNotNull(txtLineUpValue);
        Assert.IsTrue(pawnSlots.Length == 8);
        Assert.IsTrue(recruitmentSlots.Length == 3);
        Assert.IsNotNull(btnReroll);
        Assert.IsNotNull(txtReroll);
        Assert.IsNotNull(shopCanvasGroup);
        Assert.IsNotNull(btnSell);
        Assert.IsNotNull(txtSell);
        Assert.IsNotNull(tilesCanvasGroup);

        foreach (var pawnSlot in pawnSlots)
        {
            pawnSlot.OnSlotUnselected += PawnSlot_OnSlotUnselected;
            pawnSlot.OnSlotSelected += PawnSlot_OnSlotSelected;
            pawnSlot.OnRemoveFromLineupConfirmed += PawnSlot_OnRemoveFromLineupConfirmed;
        }

        foreach (var recruitmentSlot in recruitmentSlots)
        {
            recruitmentSlot.OnRecruitmentConfirmed += RecruitmentSlot_OnRecruitmentConfirmed;
        }
        btnReroll.onClick.AddListener(BtnReroll_OnClick);
        btnSell.onClick.AddListener(BtnSell_OnClick);

        Singleton.Instance.GameInstance.GameState.OnTotalCoinsChanged += GameState_OnTotalCoinsChanged;
        Singleton.Instance.GameInstance.GameState.OnLineupLimitChanged += GameState_OnLineupLimitChanged;
        Singleton.Instance.GameInstance.GameState.OnSelectedUnitChanged += GameState_OnSelectedUnitChanged;

    }

    private void GameState_OnSelectedUnitChanged(UnitInstance obj)
    {
        UpdateSellUI();
    }

    private void PawnSlot_OnRemoveFromLineupConfirmed(UnitInstance lineupUnit, int targetSlotIndex)
    {
        if (OnPawnRemoveFromLineupConfirmed != null) OnPawnRemoveFromLineupConfirmed(lineupUnit, targetSlotIndex);
    }

    private void PawnSlot_OnSlotUnselected(UnitInstance lineupUnit, int targetSlotIndex)
    {
        if (OnPawnSlotUnselected != null) OnPawnSlotUnselected(lineupUnit, targetSlotIndex);
    }

    private void PawnSlot_OnSlotSelected(UnitInstance lineupUnit, int targetSlotIndex)
    {
        if (OnPawnSlotSelected != null) OnPawnSlotSelected(lineupUnit, targetSlotIndex);
    }

    private void GameState_OnLineupLimitChanged(int lineupLimit)
    {
        UpdateLineupCount(Singleton.Instance.GameInstance.GameState.GetPlayerLineUp().Count(), lineupLimit);
    }
    public void UpdateLineupCount()
    {
        UpdateLineupCount(Singleton.Instance.GameInstance.GameState.GetPlayerLineUp().Count(), Singleton.Instance.GameInstance.GameState.LineUpLimit);
    }
    void UpdateLineupCount(int lineupCount, int lineupLimit)
    {
        txtLineUpValue.text = string.Format("{0}/{1}", lineupCount, lineupLimit);
    }

    private void GameState_OnTotalCoinsChanged(int coins)
    {
        UpdateCoinsUIInternal(coins);
        UpdateRerollUI();
    }


    void BtnReroll_OnClick()
    {
        OnRecruitmentReroll.Invoke();
    }
    void BtnSell_OnClick()
    {
        OnSellSelected.Invoke();
    }

    private void RecruitmentSlot_OnRecruitmentConfirmed(IUnitBlueprint obj)
    {
        this.OnRecruitmentConfirmed.Invoke(obj);
    }

    public void Announce(string text)
    {
        announcer.Announce(text, Singleton.Instance.GameInstance.Configuration.AnnouncementDuration);
    }
    public void ShowPawnSlots()
    {

    }
    public void HidePawnSlots()
    {
        foreach (var pawnSlot in pawnSlots)
        {
            pawnSlot.HideImmediate();
        }
    }

    public void UpdateCoinsUI()
    {
        UpdateCoinsUIInternal(Singleton.Instance.GameInstance.GameState.PlayerCoins);
    }

    void UpdateCoinsUIInternal(int coins)
    {
        this.coins.SetValue(coins);
        UpdateRerollUI();
    }

    public void UpdateRerollUI()
    {
        var rollCost = Singleton.Instance.GameInstance.GameState.GetCurrentRerollCost();
        txtReroll.text = string.Format("Reroll recruits for {0} {1}",
            rollCost,
            Singleton.Instance.GameInstance.Configuration.SpriteAtlas_Coin
            );

        bool lastCoinToSpendOnRerollAtStartOfGame = (Singleton.Instance.GameInstance.GameState.PlayerCoins - rollCost <= 0)
            && Singleton.Instance.GameInstance.GameState.PlayerUnits.Count == 0;

        btnReroll.gameObject.SetActive(
            rollCost >= 0
            && Singleton.Instance.GameInstance.GameState.PlayerCoins >= rollCost
            && !lastCoinToSpendOnRerollAtStartOfGame
            );
    }


    public void UpdateSellUI()
    {
        //TODO:nie mozna sprzedac ostaniej jednostki jak nie stac na reroll
        bool cannotSellIfNoRecruitsAndNoCoinForReroll = true;
            //Singleton.Instance.GameInstance.GameState.SelectedUnit != null 
            //&& Singleton.Instance.GameInstance.GameState.AvailableRecruitsCount() > 0 ||
            //(Singleton.Instance.GameInstance.GameState.SelectedUnit.GetCost() );

        btnSell.gameObject.SetActive(Singleton.Instance.GameInstance.GameState.SelectedUnit != null 
            && Singleton.Instance.GameInstance.GameState.SelectedUnit.GetCost() > 0
            && cannotSellIfNoRecruitsAndNoCoinForReroll);

        if (Singleton.Instance.GameInstance.GameState.SelectedUnit != null)
        {
            var sellCost = Singleton.Instance.GameInstance.GameState.SelectedUnit.GetCost();
            txtSell.text = string.Format("Sell unit for {0} {1}",
                sellCost,
                Singleton.Instance.GameInstance.Configuration.SpriteAtlas_Coin
                );
        }
    }

    public void ShowShopUI()
    {
        shopCanvasGroup.gameObject.SetActive(true);
    }

    public void HideShopUI()
    {
        shopCanvasGroup.gameObject.SetActive(false);
    }
    public void ShowPlacementUI()
    {
        tilesCanvasGroup.gameObject.SetActive(true);
    }

    public void HidePlacementUI()
    {
        tilesCanvasGroup.gameObject.SetActive(false);
    }
}

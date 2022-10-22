using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Announcer announcer;
    [SerializeField] NumberPresenter coins;
    [SerializeField] NumberPresenter incomeTimer;
    [SerializeField] NumberPresenter trialCountdown;
    [SerializeField] PawnSlotPresenter[] pawnSlots;

    [Header("Units")]
    [SerializeField] TextMeshProUGUI txtLineUpValue;
    [Header("Recruitment")]
    [SerializeField] RecruitmentSlotPresenter[] recruitmentSlots;
    [SerializeField] Button btnReroll;
    [SerializeField] TextMeshProUGUI txtReroll;
    [Header("Shop")]
    [SerializeField] CanvasGroup shopCanvasGroup;
    [SerializeField] Button btnSell;
    [SerializeField] TextMeshProUGUI txtSell;
    [Header("UnitPlacement")]
    [SerializeField] CanvasGroup tilesCanvasGroup;
    [SerializeField] TileSlotPresenter[] Tiles;


    public UnityEvent<IUnitBlueprint, int> OnRecruitmentConfirmed;

    public UnityEvent<UnitInstance, int> OnPawnSlotUnselected;
    public UnityEvent<UnitInstance, int> OnPawnSlotSelected;
    public UnityEvent<UnitInstance, int> OnPawnRemoveFromLineupConfirmed;

    public UnityEvent OnRecruitmentReroll;
    public UnityEvent OnSellSelected;



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
        Assert.IsTrue(Tiles.Length > 0);

        foreach (var pawnSlot in pawnSlots)
        {
            pawnSlot.OnSlotUnselected.AddListener(PawnSlot_OnSlotUnselected);
            pawnSlot.OnSlotSelected.AddListener(PawnSlot_OnSlotSelected);
            pawnSlot.OnRemoveFromLineupConfirmed.AddListener(PawnSlot_OnRemoveFromLineupConfirmed);
        }

        foreach (var recruitmentSlot in recruitmentSlots)
        {
            recruitmentSlot.OnRecruitmentConfirmed.AddListener(RecruitmentSlot_OnRecruitmentConfirmed);
        }
        btnReroll.onClick.AddListener(BtnReroll_OnClick);
        btnSell.onClick.AddListener(BtnSell_OnClick);

        Singleton.Instance.GameInstance.GameState.OnTotalCoinsChanged.AddListener(GameState_OnTotalCoinsChanged);
        Singleton.Instance.GameInstance.GameState.OnLineupLimitChanged.AddListener(GameState_OnLineupLimitChanged);
        Singleton.Instance.GameInstance.GameState.OnSelectedUnitChanged.AddListener(GameState_OnSelectedUnitChanged);
        Singleton.Instance.GameInstance.GameState.OnLineupChanged.AddListener(GameState_OnLineupChanged);
        Singleton.Instance.GameInstance.GameState.OnTrialCountdownChanged.AddListener(GameState_OnTrialCountdownChanged);
        Singleton.Instance.GameInstance.GameState.OnPhaseChanged.AddListener(GameState_OnPhaseChanged);
    }

    private void GameState_OnSelectedUnitChanged(UnitInstance obj)
    {
        UpdateSellUI();
    }

    private void PawnSlot_OnRemoveFromLineupConfirmed(UnitInstance lineupUnit, int targetSlotIndex)
    {
        if (OnPawnRemoveFromLineupConfirmed != null) OnPawnRemoveFromLineupConfirmed.Invoke(lineupUnit, targetSlotIndex);
    }

    private void PawnSlot_OnSlotUnselected(UnitInstance lineupUnit, int targetSlotIndex)
    {
        if (OnPawnSlotUnselected != null) OnPawnSlotUnselected.Invoke(lineupUnit, targetSlotIndex);
    }

    private void PawnSlot_OnSlotSelected(UnitInstance lineupUnit, int targetSlotIndex)
    {
        if (OnPawnSlotSelected != null) OnPawnSlotSelected.Invoke(lineupUnit, targetSlotIndex);
    }

    private void GameState_OnLineupLimitChanged(int lineupLimit)
    {
        UpdateLineupCountInternal(Singleton.Instance.GameInstance.GameState.GetPlayerLineUp().Count(), lineupLimit);
    }
    public void UpdateLineupCount()
    {
        UpdateLineupCountInternal(Singleton.Instance.GameInstance.GameState.GetPlayerLineUp().Count(), Singleton.Instance.GameInstance.GameState.LineUpLimit);
    }
    void UpdateLineupCountInternal(int lineupCount, int lineupLimit)
    {
        txtLineUpValue.text = string.Format("{0}/{1}", lineupCount, lineupLimit);
    }
    void UpdateTrialCountdown()
    {
        trialCountdown.SetValue(Mathf.FloorToInt(Singleton.Instance.GameInstance.GameState.TrialCountdown));
        if (Singleton.Instance.GameInstance.GameState.TrialCountdownEnabled)
        {
            trialCountdown.ShowImmediate();
        }
        else
        {
            trialCountdown.HideImmediate();
        }
    }

    private void GameState_OnTotalCoinsChanged(int coins)
    {
        UpdateCoinsUIInternal(coins);
        UpdateRerollUI();
    }

    private void GameState_OnLineupChanged()
    {
        UpdateLineupCount();
    }
    void GameState_OnTrialCountdownChanged(float value)
    {
        UpdateTrialCountdown();
    }
    void GameState_OnPhaseChanged(EGamePhase newPhase)
    {
        if (newPhase == EGamePhase.Trial)
        {
            HidePlacementUI();
        }
    }

    void BtnReroll_OnClick()
    {
        OnRecruitmentReroll.Invoke();
    }
    void BtnSell_OnClick()
    {
        OnSellSelected.Invoke();
    }

    private void RecruitmentSlot_OnRecruitmentConfirmed(IUnitBlueprint obj, int slotIndex)
    {
        this.OnRecruitmentConfirmed.Invoke(obj, slotIndex);
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

        bool rerollEnabled = true;

        if (rollCost < 0)
        {
            //error?
            rerollEnabled = false;
        }
        if (Singleton.Instance.GameInstance.GameState.PlayerCoins <= rollCost)
        {
            //not enough gold
            rerollEnabled = false;
        }
        if (Singleton.Instance.GameInstance.GameState.PlayerUnits.Count == 0
            && Singleton.Instance.GameInstance.GameState.PlayerCoins - rollCost <= 0)
        {
            //player dont have any units, and after reroll will not have any gold
            rerollEnabled = false;
        }

        btnReroll.gameObject.SetActive(rerollEnabled);
    }


    public void UpdateSellUI()
    {
        bool sellEnabled = true;

        if (!Singleton.Instance.GameInstance.GameState.SelectedUnit)
        {
            //no unit is selected
            sellEnabled = false;
        }
        if (Singleton.Instance.GameInstance.GameState.SelectedUnit
            && Singleton.Instance.GameInstance.GameState.SelectedUnit.GetCost() < 0)
        {
            //selected units value is graeter than 0
            sellEnabled = false;
        }
        if (Singleton.Instance.GameInstance.GameState.SelectedUnit
            && Singleton.Instance.GameInstance.GameState.GetPlayerUnitsCount() == 1
            && Singleton.Instance.GameInstance.GameState.GetAvailableRecruitsCount() == 0
            && Singleton.Instance.GameInstance.GameState.SelectedUnit.GetCost() < Singleton.Instance.GameInstance.GameState.GetCurrentRerollCost() + 1)
        {
            //There ar no more available recruits and selling last unit will not allow reroll(not enough gold
            sellEnabled = false;
        }

        btnSell.gameObject.SetActive(sellEnabled);

        if (Singleton.Instance.GameInstance.GameState.SelectedUnit)
        {
            var sellCost = Singleton.Instance.GameInstance.GameState.SelectedUnit.GetCost();
            txtSell.text = string.Format("Sell unit for {0} {1}",
                sellCost,
                Singleton.Instance.GameInstance.Configuration.SpriteAtlas_Coin
                );
        }
    }

    void UpdatePlacement()
    {
        foreach (var tile in Tiles)
        {
            tile.UpdateUI();
        }
    }

    public void UpdatePlacementData()
    {
        var selectedUnit = Singleton.Instance.GameInstance.GameState.SelectedUnit;

        for (int i = 0; i < Singleton.Instance.GameInstance.GameState.PlayerTiles.Length; ++i)
        {
            var tile = Tiles[i];

            if (selectedUnit)
            {
                if (!Singleton.Instance.GameInstance.GameState.PlayerTiles[i])
                    tile.SetStatus(ESlotStatus.Available);
                else
                {
                    if (selectedUnit == Singleton.Instance.GameInstance.GameState.PlayerTiles[i])
                        tile.SetStatus(ESlotStatus.Selected);
                    else
                        tile.SetStatus(ESlotStatus.Unavailable);
                }
            }
            else
            {
                tile.SetStatus(ESlotStatus.None);
            }
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
        UpdatePlacement();
    }

    public void HidePlacementUI()
    {
        tilesCanvasGroup.gameObject.SetActive(false);
    }
}

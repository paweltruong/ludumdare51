using System.Collections;
using System.Collections.Generic;
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
    [Header("UnitPlacement")]
    [SerializeField]
    CanvasGroup tilesCanvasGroup;


    public event UnitBlueprintEventHandler OnRecruitmentConfirmed;
    public event System.Action OnRecruitmentReroll;


    void Start()
    {
        Assert.IsNotNull(announcer);
        Assert.IsNotNull(coins);
        Assert.IsNotNull(incomeTimer);
        Assert.IsNotNull(trialCountdown);
        Assert.IsTrue(pawnSlots.Length == 8);
        Assert.IsTrue(recruitmentSlots.Length == 3);
        Assert.IsNotNull(btnReroll);
        Assert.IsNotNull(txtReroll);
        Assert.IsNotNull(shopCanvasGroup);
        Assert.IsNotNull(tilesCanvasGroup);

        foreach (var recruitmentSlot in recruitmentSlots)
        {
            recruitmentSlot.OnRecruitmentConfirmed += RecruitmentSlot_OnRecruitmentConfirmed;
        }
        btnReroll.onClick.AddListener(BtnReroll_OnClick);

        Singleton.Instance.GameInstance.GameState.OnTotalCoinsChanged += GameState_OnTotalCoinsChanged;
    }

    private void GameState_OnTotalCoinsChanged(int coins)
    {
        UpdateCoinsUIInternal(coins);
    }


    void BtnReroll_OnClick()
    {
        OnRecruitmentReroll.Invoke();
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

        btnReroll.gameObject.SetActive(rollCost >= 0 && Singleton.Instance.GameInstance.GameState.PlayerCoins >= rollCost);
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

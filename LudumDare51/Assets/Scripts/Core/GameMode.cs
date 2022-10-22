using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    Transform mapContainer;
    [SerializeField]
    UIController uiController;
    [SerializeField]
    TutorialController tutorialController;



    void Start()
    {
        Assert.IsNotNull(mapContainer);
        Assert.IsNotNull(uiController);
        Assert.IsNotNull(tutorialController);
        Assert.IsNotNull(Singleton.Instance);
        Assert.IsNotNull(Singleton.Instance.GameInstance);
        Assert.IsNotNull(Singleton.Instance.GameInstance.GameState);

        Singleton.Instance.GameInstance.GameState.ResetState();

        uiController.OnPawnSlotUnselected.AddListener(UiController_OnPawnSlotUnselected);
        uiController.OnPawnSlotSelected.AddListener(UiController_OnPawnSlotSelected);
        uiController.OnPawnRemoveFromLineupConfirmed.AddListener(UiController_OnPawnRemoveFromLineupConfirmed);
        uiController.OnRecruitmentConfirmed.AddListener(UiController_OnRecruitmentConfirmed);
        uiController.OnRecruitmentReroll.AddListener(UiController_OnRecruitmentReroll);
        uiController.OnSellSelected.AddListener(UiController_OnSellSelected);

        Singleton.Instance.GameInstance.GameState.OnTrialBegins.AddListener(GameState_OnTrialBegins);

        Stage_Intro_00();
    }

    private void Update()
    {
        TrialCountdownTick();
    }


    void GameState_OnTrialBegins()
    {
        Singleton.Instance.GameInstance.GameState.SetPhase(EGamePhase.Trial);
        //load map
        StartNextBattle();
        //start game, spawn units
    }


    void TrialCountdownTick()
    {
        if (Singleton.Instance.GameInstance.GameState.TrialCountdownEnabled)
        {
            if (Singleton.Instance.GameInstance.GameState.TrialCountdown > 0)
            {
                Singleton.Instance.GameInstance.GameState.UpdateTrialCountdown(
                    Mathf.Clamp(Singleton.Instance.GameInstance.GameState.TrialCountdown - Time.deltaTime, 0, float.MaxValue));
            }
        }
    }

    private void UiController_OnSellSelected()
    {
        Singleton.Instance.GameInstance.GameState.SellSelectedUnit();
    }

    private void UiController_OnPawnRemoveFromLineupConfirmed(UnitInstance lineupUnit, int targetSlotIndex)
    {
        throw new System.NotImplementedException();
    }

    private void UiController_OnPawnSlotSelected(UnitInstance unit, int targetSlotIndex)
    {
        //Singleton.Instance.GameInstance.GameState.SelectUnit(unit);
        uiController.UpdatePlacementData();
    }

    private void UiController_OnPawnSlotUnselected(UnitInstance unit, int targetSlotIndex)
    {

    }
    private void UIController_OnUnitSelectedInIntro01(UnitInstance unit, int targetSlotIndex)
    {
        uiController.OnPawnSlotSelected.RemoveListener(UIController_OnUnitSelectedInIntro01);
        Stage_Intro_02();
    }

    private void UiController_OnRecruitmentConfirmed(IUnitBlueprint unitBlueprint, int slotIndex)
    {
        RecruitUnit(unitBlueprint, slotIndex);
    }
    private void UiController_OnRecruitmentReroll()
    {
        Singleton.Instance.GameInstance.GameState.SubstractCoins(Singleton.Instance.GameInstance.GameState.GetCurrentRerollCost());
        RollRecruits();
    }


    void RecruitUnit(IUnitBlueprint unitBlueprint, int slotIndex)
    {
        Assert.IsNotNull(unitBlueprint);
        if (unitBlueprint == null)
        {
            Debug.LogError("Recruiting unit is null?");
            return;
        }

        //Create unit instance
        var newUnit = UnitsPool.Instance.GetNewUnitFromPool();
        newUnit.Setup(unitBlueprint, EUnitOwner.Player1);

        //Add to slot
        Singleton.Instance.GameInstance.GameState.AddToFreeSlot(newUnit);

        //Remove recruit
        Singleton.Instance.GameInstance.GameState.ResetRecruitAt(slotIndex);

        //Substract cost
        Singleton.Instance.GameInstance.GameState.SubstractCoins(unitBlueprint.GetCost());

        //Add unit
        Singleton.Instance.GameInstance.GameState.AddPlayerUnit(newUnit);
    }

    void StartNextBattle()
    {
        LoadRandomMap();
    }

    void LoadRandomMap()
    {
        DeleteOldMap();

        var configuration = Singleton.Instance.GameInstance.Configuration;
        //TODO:do not repeat last X maps
        var randomMapIndex = Random.Range(0, configuration.Maps.Length);
        var mapPrefab = configuration.Maps[randomMapIndex];

        var mapInstance = Instantiate(mapPrefab, mapContainer);
        mapInstance.Bake();
    }

    void DeleteOldMap()
    {
        if (mapContainer.childCount > 0)
        {
            var map = mapContainer.GetChild(0);
            map.gameObject.SetActive(false);
            Destroy(map.gameObject);
        }
    }

    void Stage_Intro_00()
    {
        Singleton.Instance.GameInstance.GameState.SetPhase(EGamePhase.Preparation);

        uiController.UpdateCoinsUI();
        uiController.UpdateRerollUI();
        uiController.UpdateSellUI();
        uiController.UpdateLineupCount();
        uiController.HidePlacementUI();

        uiController.OnRecruitmentConfirmed.AddListener(UiController_OnRecruitmentConfirmedInIntro);

        RollRecruits();
        tutorialController.ShowStage(0);
    }

    private void UiController_OnRecruitmentConfirmedInIntro(IUnitBlueprint unitBlueprint, int slotIndex)
    {
        uiController.OnRecruitmentConfirmed.RemoveListener(UiController_OnRecruitmentConfirmedInIntro);
        Stage_Intro_01();
    }

    void Stage_Intro_01()
    {
        tutorialController.ShowStage(1);

        if (!Singleton.Instance.GameInstance.GameState.SelectedUnit)
        {
            uiController.OnPawnSlotSelected.AddListener(UIController_OnUnitSelectedInIntro01);
        }
        else
        {
            Stage_Intro_02();
        }
    }

    void Stage_Intro_02()
    {
        uiController.OnPawnSlotSelected.RemoveListener(UIController_OnUnitSelectedInIntro01);
        tutorialController.ShowStage(2);
        uiController.ShowPlacementUI();
        Singleton.Instance.GameInstance.GameState.OnLineupChanged.AddListener(GameState_OnLineupChangedStage02);
    }

    void GameState_OnLineupChangedStage02()
    {
        if (Singleton.Instance.GameInstance.GameState.IsLineupFull())
        {
            Singleton.Instance.GameInstance.GameState.OnLineupChanged.RemoveListener(GameState_OnLineupChangedStage02);
            Stage_Intro_03();
        }
    }

    void Stage_Intro_03()
    {
        //Prepare for trial
        tutorialController.ShowStage(3);

        Singleton.Instance.GameInstance.GameState.StartTrialCountdown(5);
        Singleton.Instance.GameInstance.GameState.OnTrialBegins.AddListener(GameState_OnTrialBeginsStage03);
    }
    void GameState_OnTrialBeginsStage03()
    {
        Singleton.Instance.GameInstance.GameState.OnTrialBegins.RemoveListener(GameState_OnTrialBeginsStage03);
        tutorialController.HideAll();
    }


    void RollRecruits()
    {
        var currentTrialIndex = Singleton.Instance.GameInstance.GameState.CurrentTrialIndex;
        var unitSetConfig = Singleton.Instance.GameInstance.Configuration.RecruitsConfigPerTrialIndex[currentTrialIndex];

        for (int i = 0; i < 3; ++i)
        {
            var unitBP = unitSetConfig.GetRandomUnit();
            Singleton.Instance.GameInstance.GameState.SetRecruit(i, unitBP);
        }
    }
}

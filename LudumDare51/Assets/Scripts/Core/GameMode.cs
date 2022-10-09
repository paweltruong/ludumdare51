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



        Stage_Intro_00();

        ////StartNextBattle();
        ////uiController.Announce("Game begins!");
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
        Singleton.Instance.GameInstance.GameState.SelectUnit(unit);
    }

    private void UiController_OnPawnSlotUnselected(UnitInstance unit, int targetSlotIndex)
    {
        throw new System.NotImplementedException();
    }

    private void UiController_OnRecruitmentConfirmed(IUnitBlueprint unitBlueprint)
    {
        RecruitUnit(unitBlueprint);
    }
    private void UiController_OnRecruitmentReroll()
    {
        Singleton.Instance.GameInstance.GameState.SubstractCoins(Singleton.Instance.GameInstance.GameState.GetCurrentRerollCost());
        RollRecruits();
    }


    void RecruitUnit(IUnitBlueprint unitBlueprint)
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

        //Substract cost
        Singleton.Instance.GameInstance.GameState.SubstractCoins(unitBlueprint.GetCost());
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

        //w web gl sie crushuje
        uiController.UpdateCoinsUI();
        //dotad dziala
        uiController.UpdateRerollUI();
        //dot¹d dzia³¹ czyli UpdateLineupCount wywala
        uiController.UpdateLineupCount();
        //nie przechodzi przed tym
        uiController.HidePlacementUI();

        uiController.OnRecruitmentConfirmed.AddListener(UiController_OnRecruitmentConfirmedInIntro);

        RollRecruits();
        tutorialController.ShowStage(0);
    }

    private void UiController_OnRecruitmentConfirmedInIntro(IUnitBlueprint unitBlueprint)
    {
        uiController.OnRecruitmentConfirmed.AddListener(UiController_OnRecruitmentConfirmedInIntro);
        Stage_Intro_01();
    }

    void Stage_Intro_01()
    {
        uiController.ShowPlacementUI();
        tutorialController.ShowStage(1);
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

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    Transform mapContainer;
    [SerializeField]
    UIController uiController;



    void Start()
    {
        Assert.IsNotNull(mapContainer);
        Assert.IsNotNull(uiController);


        Singleton.Instance.GameInstance.GameState.ResetState();

        uiController.OnRecruitmentConfirmed += UiController_OnRecruitmentConfirmed;
        uiController.OnRecruitmentReroll += UiController_OnRecruitmentReroll;



        Stage_Intro_01();

        //StartNextBattle();
        //uiController.Announce("Game begins!");
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
        var newUnit = UnitsPool.Instance.GetNewUnitFromPool();
        newUnit.Setup(unitBlueprint, EUnitOwner.Player1);
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

    public void Stage_Intro_01()
    {
        uiController.UpdateCoinsUI();
        uiController.UpdateRerollUI();
        uiController.HideShopUI();
        uiController.HidePlacementUI();
        uiController.HidePawnSlots();

        RollRecruits();
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

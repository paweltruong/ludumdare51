using UnityEngine;
using UnityEngine.Assertions;

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



        Stage_Intro_01();

        //StartNextBattle();
        //uiController.Announce("Game begins!");
    }

    private void UiController_OnRecruitmentConfirmed(IUnitBlueprint unitBlueprint)
    {
        RecruitUnit(unitBlueprint);
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

        //RollRecruits();
    }

    void RollRecruits()
    {

    }
}

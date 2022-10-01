using System.Collections;
using System.Collections.Generic;
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

        StartNextBattle();
        uiController.Announce("Game begins!");
    }

    void StartNextBattle()
    {
        LoadRandomMap();
    }

    void LoadRandomMap()
    {
        DeleteOldMap();

        var configuration = Singleton.Instance.GameInstance.GetConfiguration();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    Transform mapContainer;

    // Start is called before the first frame update
    void Start()
    {
        StartNextBattle();
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            Destroy(map);
        }
    }
}

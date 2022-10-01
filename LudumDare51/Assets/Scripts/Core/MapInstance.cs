using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class MapInstance : MonoBehaviour
{

    NavMeshSurface2d navMesh;



    private void Awake()
    {
        navMesh = GetComponentInChildren<NavMeshSurface2d>();
        Assert.IsNotNull(navMesh, string.Format("Nav mesh not found within MapInstance {0}", name));
    }

    public void Bake()
    {
        var operation = navMesh.BuildNavMeshAsync();
        operation.completed += NavMesh_OnBuildNavMeshCompleted;
    }

    void NavMesh_OnBuildNavMeshCompleted(AsyncOperation operation)
    {
        Debug.Log("Nav Mesh baked");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

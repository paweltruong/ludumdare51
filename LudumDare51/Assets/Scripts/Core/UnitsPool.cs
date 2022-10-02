using System.Collections.Generic;
using UnityEngine;

public class UnitsPool : MonoBehaviour
{
    public static UnitsPool Instance;
    public List<IPoolable> pool;
    public GameObject unitPrefab;
    public int amountToPool;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pool = new List<IPoolable>();
        
        for (int i = 0; i < amountToPool; i++)
        {
            AddPoolItem();
        }
    }

    UnitInstance AddPoolItem()
    {
        GameObject tmp;
        tmp = Instantiate(unitPrefab);
        var unitInstanceComponent = tmp.GetComponent<UnitInstance>();
        tmp.SetActive(false);
        pool.Add(unitInstanceComponent);
        return unitInstanceComponent;
    }
    public UnitInstance GetNewUnitFromPool()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pool[i].CanBeTakenFromPool())
            {
                return (UnitInstance)pool[i];
            }
        }

        //Extend pool        
        return AddPoolItem();
    }
}

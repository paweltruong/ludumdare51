using System.Linq;
using UnityEngine;

/// <summary>
/// list of units and their probability of spawning
/// </summary>
[CreateAssetMenu(fileName = "UnitSpawnConfiguration", menuName = "ScriptableObjects/UnitSpawnConfiguration", order = 1)]
public class UnitSpawnConfiguration : ScriptableObject
{
    [Range(0f, 1f)]
    public float Tier2Chance = 0;
    [Range(0f, 1f)]
    public float Tier3Chance = 0;

    public UnitSpawnWeight[] UnitWeights;

    public UnitBlueprint GetRandomUnit()
    {
        var tier = GetRandomTier();
        var unitsFromTier = UnitWeights.Where(x=>x.Unit.GetTier() == tier);

        var weightsSum = unitsFromTier.Sum(x => x.Weight);
        float previousRangePercent = 0;
        float rangePercent = 0;
        float roll = Random.Range(0f, 1f);
        foreach (var unit in unitsFromTier)
        {
            rangePercent = unit.Weight / weightsSum;
            if (previousRangePercent + rangePercent >= roll)
                return unit.Unit;

            previousRangePercent += rangePercent;
            //Debug.LogFormat("PrevPer:{0} Roll:{1} TotalW:{2} rPer {3}", 
            //    previousRangePercent.ToString("0.00"), 
            //    roll.ToString("0.00"), 
            //    weightsSum,
            //    rangePercent.ToString("0.00"));
        }

        //should not enter here
        Debug.LogErrorFormat("Invalid roll for {0}", tier.ToString());

        return null;
    }

    EUnitTier GetRandomTier()
    {
        if (Tier3Chance > 0 && CommonExtensions.IsRollSuccess(Tier3Chance)) return EUnitTier.Tier3;
        if (Tier2Chance > 0 && CommonExtensions.IsRollSuccess(Tier2Chance)) return EUnitTier.Tier2;
        return EUnitTier.Tier1;
    }

}

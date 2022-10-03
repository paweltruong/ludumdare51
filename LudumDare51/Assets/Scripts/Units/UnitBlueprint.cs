using UnityEngine;

[CreateAssetMenu(fileName = "UnitBlueprint", menuName = "ScriptableObjects/UnitBlueprint", order = 1)]
public class UnitBlueprint : ScriptableObject, IUnitBlueprint
{
    public string unitName = string.Empty;
    public string description = string.Empty;
    [Tooltip("Upgrades to")]
    public UnitBlueprint upgrade;
    public Sprite sprite;
    public EUnitTier tier = EUnitTier.Tier1;
    [Range(0, 100)]
    public int cost = 1;
    [Range(1, 3)]
    public int level = 1;
    [Range(0, 10)]
    public float baseRange = 1;
    [Range(0, 100)]
    public float baseHp = 1;
    [Range(0, 10)]
    public float baseArmor = 0;
    [Range(0, 100)]
    public float baseMinDamage = 0;
    [Range(0, 100)]
    public float baseMaxDamage = 0;
    [Range(0, 10)]
    public float baseAttackSpeed = 0;
    [Range(0, 1)]
    public float baseDodge = 0;
    [Range(0, 10)]
    public float baseDodgeCooldown = 0;
    [Header("Agent")]
    [Range(0,10)]
    public float baseMoveSpeed = 0;
    public float angularSpeed = 120;
    public float acceleration = 5;
    public float obstacleAvoidanceRadius = .5f;
    public float coliderRadius = .5f;

    #region IUnitBlueprint

    public string GetName() { return unitName; }
    public string GetDesc() { return description; }
    public EUnitTier GetTier() { return tier; }
    public int GetCost() { return cost; }
    public int GetLevel() { return level; }
    IUnitBlueprint IUnitBlueprint.GetUpgradeBlueprint() { return upgrade; }
    public float GetBaseHp() { return baseHp; }
    public float GetBaseArmor() { return baseArmor; }
    public float GetBaseMinDamage() { return baseMinDamage; }
    public float GetBaseMaxDamage() { return baseMaxDamage; }   
    public float GetBaseAttackSpeed() { return baseAttackSpeed; }
    public float GetBaseMoveSpeed() { return baseMoveSpeed; }
    public float GetBaseDodge() { return baseDodge; }
    public float GetBaseDodgeCooldown() { return baseDodgeCooldown; }
    public float GetBaseRange() { return baseRange; }

    public Sprite GetSprite() { return sprite; }

    #endregion
}
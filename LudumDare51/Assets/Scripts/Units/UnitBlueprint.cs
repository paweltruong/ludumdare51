using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBlueprint", menuName = "ScriptableObjects/UnitBlueprint", order = 1)]
public class UnitBlueprint : ScriptableObject, IUnitBlueprint
{
    public string unitName = string.Empty;
    public string description = string.Empty;
    [Tooltip("Upgrades to")]
    public UnitBlueprint upgrade;
    [Range(0, 100)]
    public int cost = 1;
    [Range(0, 3)]
    public int level = 1;
    [Range(0,10)]
    public float baseMoveSpeed = 0;
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

    #region IUnitBlueprint

    public string GetName() { return unitName; }
    public string GetDesc() { return description; }
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

    #endregion
}
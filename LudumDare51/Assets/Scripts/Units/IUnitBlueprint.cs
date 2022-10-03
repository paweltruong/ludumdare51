using UnityEngine;


public interface IUnitBlueprint
{
    string GetName();
    string GetDesc();
    EUnitTier GetTier();
    int GetCost();
    int GetLevel();
    IUnitBlueprint GetUpgradeBlueprint();
    float GetBaseHp();
    float GetBaseArmor();
    float GetBaseMinDamage();
    float GetBaseMaxDamage();
    float GetBaseAttackSpeed();
    float GetBaseMoveSpeed();
    float GetBaseDodge();
    float GetBaseDodgeCooldown();
    float GetBaseRange();
    Sprite GetSprite();
}
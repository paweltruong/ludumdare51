using System;
using UnityEngine;

public static class CommonExtensions
{
    const float Armor_ReductionByPoint = 0.1f;
    const float Armor_NoneMax = 0;
    const float Armor_LightMax = 3;
    const float Armor_MediumMax = 6;
    const float Armor_HeavyMax = 9;
    //armor above max is v.hard

    const float AttackSpeed_VFastMax = .5f;
    const float AttackSpeed_FastMax = 1.8f;
    const float AttackSpeed_NormalMax = 3f;
    const float AttackSpeed_SlowMax = 6f;
    //AttackSpeed_VSlowMax everything above

    const float MoveSpeed_VFastMax = 2f;
    const float MoveSpeed_FastMax = 4f;
    const float MoveSpeed_NormalMax = 5f;
    const float MoveSpeed_SlowMax = 7f;
    //MoveSpeed_VSlowMax everything above
    public static string ToHealthValueString(this IUnitBlueprint unitBP)
    {
        return String.Format("{0}", (int)Math.Ceiling(unitBP.GetBaseHp()));
    }
    public static string ToHealthValueString(this IUnitInstance unit)
    {
        return String.Format("{0}/{1}", (int)Math.Ceiling(unit.GetCurrentHp()), (int)Math.Ceiling(unit.GetMaxHp()));
    }
    /// <summary>
    /// {0}/{1} f.e 100/100
    /// </summary>
    /// <param name="currentHealth"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static string ToHealthValueString(this float currentHealth, float max)
    {
        return String.Format("{0}/{1}", (int)Math.Ceiling(currentHealth), (int)Math.Ceiling(max));
    }
    public static string ToArmorValueString(this IUnitBlueprint unitBP)
    {
        return unitBP.GetBaseArmor().ToArmorValueString();
    }
    public static string ToArmorValueString(this float armor)
    {
        if (armor <= Armor_NoneMax) return "None";
        if (armor <= Armor_LightMax) return "Light";
        if (armor <= Armor_MediumMax) return "Medium";
        if (armor <= Armor_HeavyMax) return "Hard";
        return "V. Hard";
    }
    public static float ArmorToDamageReduction(this float armor, float damageToReduce)
    {
        return armor * Armor_ReductionByPoint * damageToReduce;
    }

    public static string ToDamageValueString(this IUnitBlueprint unitBP)
    {
        return String.Format("{0}-{1}", Math.Floor(unitBP.GetBaseMinDamage()), Math.Floor(unitBP.GetBaseMaxDamage()));
    }

    public static string ToAttackSpeedValueString(this IUnitBlueprint unitBP)
    {
        return unitBP.GetBaseAttackSpeed().ToAttackSpeedValueString();
    }
    public static string ToAttackSpeedValueString(this float value)
    {
        if (value <= AttackSpeed_VFastMax) return "V.Fast";
        if (value <= AttackSpeed_FastMax) return "Fast";
        if (value <= AttackSpeed_NormalMax) return "Normal";
        if (value <= AttackSpeed_SlowMax) return "Slow";
        return "V.Slow";
    }
    public static string ToMoveSpeedValueString(this IUnitBlueprint unitBP)
    {
        return unitBP.GetBaseMoveSpeed().ToMoveSpeedValueString();
    }
    public static string ToMoveSpeedValueString(this float value)
    {
        if (value <= MoveSpeed_VFastMax) return "V.Fast";
        if (value <= MoveSpeed_FastMax) return "Fast";
        if (value <= MoveSpeed_NormalMax) return "Normal";
        if (value <= MoveSpeed_SlowMax) return "Slow";
        return "V.Slow";
    }
    public static string ToDodgeValueString(this IUnitBlueprint unitBP)
    {
        return unitBP.GetBaseDodge().ToDodgeValueString();
    }
    public static string ToDodgeValueString(this float value)
    {
        return String.Format("{0}%", Math.Floor(value * 100));
    }

    public static string ToNameAndLevelString(this IUnitBlueprint unitBP)
    {
        return String.Format("{0} lvl {1}", unitBP.GetName(), unitBP.GetLevel());
    }

    public static string ToShortLevelString(this IUnitBlueprint unitBP)
    {
        return String.Format("lvl {0}", unitBP.GetLevel());
    }
    public static float ToMaxExp(this IUnitBlueprint unitBP)
    {
        switch (unitBP.GetLevel())
        {
            case 1:
                return 10;
            case 2:
                return 20;
            default:
                return 0;
        }
    }


    public static bool IsRollSuccess(float chance)
    {
        var roll = UnityEngine.Random.Range(0.0f, 1f);
        return roll <= chance;
    }
}
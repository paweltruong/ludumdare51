using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitInstance : MonoBehaviour, IUnitInstance
{
    IUnitBlueprint blueprint;
    SpriteRenderer spriteRenderer;

    float currentHp;

    float currentExp;
    float maxExp;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Assert.IsNotNull(spriteRenderer);
    }

    public void Setup(IUnitBlueprint blueprint)
    {
        this.blueprint = blueprint;
        spriteRenderer.sprite = blueprint.GetSprite();

    }

    void Reset()
    {
        currentHp = 0;
        currentExp = 0;
        maxExp = 0;
    }

    #region IUnitInstance

    public float GetBaseArmor()
    {
        return blueprint.GetBaseArmor();
    }

    public float GetBaseAttackSpeed()
    {
        return blueprint.GetBaseAttackSpeed();
    }

    public float GetBaseDodge()
    {
        return blueprint.GetBaseDodge();
    }

    public float GetBaseDodgeCooldown()
    {
        return blueprint.GetBaseDodgeCooldown();
    }

    public float GetBaseHp()
    {
        return blueprint.GetBaseHp();
    }
    public float GetBaseMinDamage()
    {
        return blueprint.GetBaseMinDamage();
    }

    public float GetBaseMaxDamage()
    {
        return blueprint.GetBaseMaxDamage();
    }


    public float GetBaseMoveSpeed()
    {
        return blueprint.GetBaseMoveSpeed();
    }

    public float GetBaseRange()
    {
        return blueprint.GetBaseRange();
    }

    public int GetCost()
    {
        return blueprint.GetCost();
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }

    public string GetDesc()
    {
        return blueprint.GetDesc();
    }

    public int GetLevel()
    {
        return blueprint.GetLevel();
    }

    public float GetMaxHp()
    {
        return blueprint.GetBaseHp();
    }

    public string GetName()
    {
        return blueprint.GetName();
    }

    public IUnitBlueprint GetUpgradeBlueprint()
    {
        return blueprint.GetUpgradeBlueprint();
    }

    public float RollDamage()
    {

        return Random.Range(blueprint.GetBaseMinDamage(), blueprint.GetBaseMaxDamage());
    }

    public Sprite GetSprite()
    {
        return blueprint.GetSprite();
    }
    #endregion IUnitInstance

}

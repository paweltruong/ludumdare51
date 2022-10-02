using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class UnitInstance : MonoBehaviour, IUnitInstance
{
    [SerializeField]
    bool disableAgentOnAwake = true;

    NavMeshAgent agent;
    IUnitBlueprint blueprint;
    SpriteRenderer spriteRenderer;
    EUnitOwner owner;

    float currentHp;

    float currentExp;
    float maxExp;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        agent = GetComponentInChildren<NavMeshAgent>();

        Assert.IsNotNull(spriteRenderer);
        Assert.IsNotNull(agent);
        if (disableAgentOnAwake)
        {
            agent.enabled = false;
        }
    }

    public void Setup(IUnitBlueprint blueprint, EUnitOwner owner)
    {
        this.owner = owner;
        this.blueprint = blueprint;
        spriteRenderer.sprite = blueprint.GetSprite();

        currentHp = blueprint.GetBaseHp();
        currentExp = 0;
        maxExp = blueprint.ToMaxExp();
        agent.enabled = true;
    }

    void ResetData()
    {
        owner = EUnitOwner.None;
        blueprint = null;
        currentHp = 0;
        currentExp = 0;
        maxExp = 0;

        agent.enabled = false;
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

    #region IPoolable
    public bool CanBeTakenFromPool()
    {
        return !gameObject.activeInHierarchy
            && blueprint == null
            && owner == EUnitOwner.None;
    }
    public void CleanupForPooling()
    {
        gameObject.SetActive(false);

        ResetData();
    }
    #endregion IPoolable
}

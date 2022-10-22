using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class UnitInstance : MonoBehaviour, IUnitInstance
{
    [SerializeField]
    bool disableAgentOnAwake = true;
    [SerializeField]
    NavMeshAgent agent;
    IUnitBlueprint blueprint;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    EUnitOwner owner;

    float currentHp;

    float currentExp;
    float maxExp;

    bool isInLineup = false;
    public bool IsInLineup { get { return isInLineup; } set { isInLineup = value; } }
    Vector2 lineUpPosition;

    private void Awake()
    {
    }

    private void Start()
    {
        Assert.IsNotNull(spriteRenderer);
        Assert.IsNotNull(agent);
        if (disableAgentOnAwake)
        {
            agent.enabled = false;
        }
    }

    public void Setup(IUnitBlueprint blueprint, EUnitOwner owner)
    {
        if (blueprint == null) 
        {
            ResetData();
            return;
        }

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
        isInLineup = false;
        lineUpPosition = new Vector2(-1,-1);

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
        return blueprint == null ? 0 : blueprint.GetBaseArmor();
    }

    public float GetBaseAttackSpeed()
    {
        return blueprint == null ? 0 : blueprint.GetBaseAttackSpeed();
    }

    public float GetBaseDodge()
    {
        return blueprint == null ? 0 : blueprint.GetBaseDodge();
    }

    public float GetBaseDodgeCooldown()
    {
        return blueprint == null ? 0 : blueprint.GetBaseDodgeCooldown();
    }

    public float GetBaseHp()
    {
        return blueprint == null ? 0 : blueprint.GetBaseHp();
    }
    public float GetBaseMinDamage()
    {
        return blueprint == null ? 0 : blueprint.GetBaseMinDamage();
    }

    public float GetBaseMaxDamage()
    {
        return blueprint == null ? 0 : blueprint.GetBaseMaxDamage();
    }


    public float GetBaseMoveSpeed()
    {
        return blueprint == null ? 0 : blueprint.GetBaseMoveSpeed();
    }

    public float GetBaseRange()
    {
        return blueprint == null ? 0 : blueprint.GetBaseRange();
    }

    public EUnitTier GetTier()
    {
        return blueprint == null ? EUnitTier.Tier1 : blueprint.GetTier();
    }
    public int GetCost()
    {
        return blueprint == null ? 0 : blueprint.GetCost();
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }

    public string GetDesc()
    {
        return blueprint == null ? string.Empty : blueprint.GetDesc();
    }

    public int GetLevel()
    {
        return blueprint == null ? 1 : blueprint.GetLevel();
    }

    public float GetMaxHp()
    {
        return blueprint == null ? 0 : blueprint.GetBaseHp();
    }

    public string GetName()
    {
        return blueprint == null ? string.Empty : blueprint.GetName();
    }

    public IUnitBlueprint GetUpgradeBlueprint()
    {
        return blueprint == null? null : blueprint.GetUpgradeBlueprint();
    }

    public float RollDamage()
    {

        return blueprint == null ? 0 : Random.Range(blueprint.GetBaseMinDamage(), blueprint.GetBaseMaxDamage());
    }

    public Sprite GetSprite()
    {
        return blueprint == null ? null : blueprint.GetSprite();
    }
    public Sprite GetIcon()
    {
        return blueprint == null ? null : blueprint.GetIcon();
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

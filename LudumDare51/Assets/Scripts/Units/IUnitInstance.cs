public interface IUnitInstance : IUnitBlueprint, IPoolable
{    
    float GetCurrentHp();
    float GetMaxHp();

    float RollDamage();

}
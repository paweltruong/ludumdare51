using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Units
{
    internal interface IUnitBlueprint
    {
        string GetName();
        string GetDesc();
        int GetCost();
        int GetLevel();
        IUnitBlueprint GetUpgradeBlueprint();
        float GetBaseHp();
        float GetBaseArmor();
        float GetBaseDamage();
        float GetBaseAttackSpeed();
        float GetBaseMoveSpeed();
        float GetBaseDodge();
        float GetBaseDodgeCooldown();
        float GetBaseRange();
    }
}

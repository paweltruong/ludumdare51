using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IUnitInstance : IUnitBlueprint
{    
    float GetCurrentHp();
    float GetMaxHp();

    float RollDamage();

}
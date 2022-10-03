using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameState
{
    public int PlayerCoins;
    public int CurrentTrialIndex = 0;

    public int SlotsUnlocked = 0;
    public UnitInstance[] Slots = new UnitInstance[8];
    public List<UnitInstance> PlayerUnits;
    public UnitBlueprint[] Recruits = new UnitBlueprint[3];


    public event Action<int> OnTotalCoinsChanged;

    public event Action<int> OnRecruitChanged;

    public void ResetState()
    {
        PlayerCoins = Singleton.Instance.GameInstance.Configuration.InitialCoins;
        SlotsUnlocked = Singleton.Instance.GameInstance.Configuration.InitialSlots;
    }

    public void SetRecruit(int index, UnitBlueprint unitBlueprint)
    {
        if (index >= Recruits.Length)
        {
            Debug.LogError("Invalid recruit index");
            return;
        }
        Recruits[index] = unitBlueprint;
        if(OnRecruitChanged != null) OnRecruitChanged.Invoke(index);
    }

    public void SubstractCoins(int amount)
    {
        PlayerCoins = Math.Clamp(PlayerCoins - amount, 0, 100000);
        if (OnTotalCoinsChanged != null) OnTotalCoinsChanged.Invoke(PlayerCoins);
    }

    public int GetAvailableFreeSlotsCount()
    {
        int count = 0;
        for (int i = 0; i < SlotsUnlocked; i++)
        {
            if (Slots[i] == null) ++count;
        }
        return count;
    }
    public int GetCurrentRerollCost()
    {
        if (CurrentTrialIndex < 0 || CurrentTrialIndex > Singleton.Instance.GameInstance.Configuration.RerollCostPerTrialIndex.Length)
        {
            Debug.LogError("Invalid TrialIndex or RerollCost configuration");
            return -1;
        }
        return Singleton.Instance.GameInstance.Configuration.RerollCostPerTrialIndex[CurrentTrialIndex];
    }

}
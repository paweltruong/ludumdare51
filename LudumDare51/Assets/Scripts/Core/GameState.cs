using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public int PlayerCoins;
    public int CurrentTrialIndex = 0;

    public int SlotsUnlocked = 0;
    public UnitInstance[] Slots = new UnitInstance[8];
    public List<UnitInstance> PlayerUnits;

    public void ResetState()
    {
        PlayerCoins = Singleton.Instance.GameInstance.Configuration.InitialCoins;
        SlotsUnlocked = Singleton.Instance.GameInstance.Configuration.InitialSlots;
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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

public class GameState
{
    public int PlayerCoins;
    public int CurrentTrialIndex = 0;

    public int LineUpLimit = 1;
    public int SlotsUnlocked = 0;
    public UnitInstance[] Slots = new UnitInstance[8];
    public UnitInstance[] PlayerTiles = new UnitInstance[32];
    public List<UnitInstance> PlayerUnits = new List<UnitInstance>();
    public UnitBlueprint[] Recruits = new UnitBlueprint[3];
    public UnitInstance SelectedUnit;
    public EGamePhase CurrentPhase = EGamePhase.None;
    public List<UnitInstance> Lineup = new List<UnitInstance>();

    public bool TrialCountdownEnabled = false;
    public float TrialCountdown = 0;

    public UnityEvent OnTrialBegins = new UnityEvent();
    public UnityEvent<float> OnTrialBeginCountdownChanged = new UnityEvent<float>();
    public UnityEvent OnLineupChanged = new UnityEvent();
    public UnityEvent<int> OnTotalCoinsChanged = new UnityEvent<int>();
    public UnityEvent<int> OnRecruitChanged = new UnityEvent<int>();
    public UnityEvent<int> OnLineupLimitChanged = new UnityEvent<int>();
    public UnityEvent<int> OnSlotChanged = new UnityEvent<int>();
    public UnityEvent<EGamePhase> OnPhaseChanged = new UnityEvent<EGamePhase>();
    public UnityEvent<UnitInstance> OnSelectedUnitChanged = new UnityEvent<UnitInstance>();

    /// <summary>
    /// Try to return first overflow unit to pawn slots from lineup. If there is no space unit will be sold
    /// </summary>
    /// <returns></returns>
    public bool TryReturnFromLineUp()
    {
        if (Singleton.Instance.GameInstance.GameState.Lineup.Count > Singleton.Instance.GameInstance.GameState.LineUpLimit)
        {
            int lineupOverflowIndex = Singleton.Instance.GameInstance.GameState.LineUpLimit;
            var unit = Singleton.Instance.GameInstance.GameState.Lineup[lineupOverflowIndex];
            if (GetAvailableFreeSlotsCount() > 0)
            {
                int freeSlotIndex = GetFirstFreeSlotIndex();
                if (freeSlotIndex < 0)
                {
                    Debug.LogError("Could not return from lineup");
                    return false;
                }

                //Put to slot
                ReturnFromLineupToSlot(unit, freeSlotIndex);
                return true;
            }
            else
            {
                //sell
                SellUnit(unit);
                return false;
            }
        }
        return false;
    }

    int GetFirstFreeSlotIndex()
    {
        int freeSlotIndex = -1;
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (!Slots[i])
            {
                freeSlotIndex = i;
                break;
            }
        }
        return freeSlotIndex;
    }
    public void StartTrialCountdown(float SecondsToStart)
    {
        TrialCountdown = SecondsToStart;
        TrialCountdownEnabled = true;
        OnTrialBeginCountdownChanged.Invoke(SecondsToStart);
    }

    public void UpdateTrialCountdown(float UpdateValue)
    {
        bool trialBegins = false;
        TrialCountdown = UpdateValue;
        if (TrialCountdown <= 0)
        {
            TrialCountdownEnabled = false;
            trialBegins = true;
        }

        OnTrialBeginCountdownChanged.Invoke(UpdateValue);

        if (trialBegins)
        {
            OnTrialBegins.Invoke();
        }
    }
    public void ChangeSlot(UnitInstance unit, int destinationSlotIndex)
    {
        if (destinationSlotIndex > Slots.Length || destinationSlotIndex < 0)
        {
            Debug.LogError("Invalid destination slot index");
            return;
        }
        if (Slots[destinationSlotIndex] != null)
        {
            Debug.LogError("Destination slot is not empty");
            return;
        }

        int oldSlotIndex = GetSlotIndex(unit);
        if (oldSlotIndex < 0)
        {
            Debug.LogError("Unit is not in slot.");
            return;
        }

        Slots[destinationSlotIndex] = unit;
        Slots[oldSlotIndex] = null;

        if (SelectedUnit)
        {
            SelectUnit(null);
        }
        OnSlotChanged.Invoke(oldSlotIndex);
        OnSlotChanged.Invoke(destinationSlotIndex);
    }
    public void SellSelectedUnit()
    {
        SellUnit(SelectedUnit);
    }
    void SellUnit(UnitInstance unit)
    {
        if (!unit)
        {
            Debug.LogError("Unit to sell is null");
            return;
        }
        AddCoins(unit.GetCost());
        RemoveUnit(unit);
    }
    public bool IsLineupFull()
    {
        return Lineup.Count >= LineUpLimit;
    }
    void RemoveUnit(UnitInstance unit)
    {
        Singleton.Instance.GameInstance.GameState.SelectUnit(null);

        if (unit.IsInLineup)
        {
            //remove from tiles
            for (int i = 0; i < PlayerTiles.Length; i++)
            {
                if (PlayerTiles[i] == unit)
                {
                    unit.CleanupForPooling();
                    PlayerTiles[i] = null;
                    Lineup.Remove(unit);
                    if (OnLineupChanged != null) OnLineupChanged.Invoke();
                }
            }
        }
        else
        {
            //remove from slots
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] == unit)
                {
                    unit.CleanupForPooling();
                    Slots[i] = null;
                    if (OnSlotChanged != null) OnSlotChanged.Invoke(i);
                }
            }
        }

        PlayerUnits.Remove(unit);
    }

    public void ReturnFromLineupToSlot(UnitInstance unit, int slotIndex)
    {
        Slots[slotIndex] = unit;
        Lineup.Remove(unit);
        for (int i = 0; i < PlayerTiles.Length; ++i)
        {
            if (PlayerTiles[i] == unit)
            {
                PlayerTiles[i] = null;
                break;
            }
        }
        unit.IsInLineup = false;
        SelectUnit(null);
        OnLineupChanged.Invoke();
    }

    public void MoveToLineupFromPawnSlot(UnitInstance unit, int tileSlotIndex)
    {
        //remove from pawn slot
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (Slots[i] == unit)
            {
                Slots[i] = null;
            }
        }

        //remove from lineupslots
        if (unit.IsInLineup)
        {
            for (int i = 0; i < PlayerTiles.Length; ++i)
            {
                if (PlayerTiles[i] == unit)
                {
                    PlayerTiles[i] = null;
                }
            }
        }

        PlayerTiles[tileSlotIndex] = unit;
        Lineup.Add(unit);
        unit.IsInLineup = true;

        SelectUnit(null);
        OnLineupChanged.Invoke();
    }


    public void SelectUnit(UnitInstance unit)
    {
        SelectedUnit = unit;
        if (OnSelectedUnitChanged != null) OnSelectedUnitChanged.Invoke(unit);
    }

    int GetSlotIndex(UnitInstance queryUnit)
    {
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (Slots[i] == queryUnit) return i;
        }
        return -1;
    }
    public void SetPhase(EGamePhase newPhase)
    {
        CurrentPhase = newPhase;
        if (SelectedUnit && SelectedUnit.IsInLineup)
        {
            SelectUnit(null);
        }
        if (OnPhaseChanged != null) OnPhaseChanged.Invoke(CurrentPhase);
    }

    public IEnumerable<UnitInstance> GetPlayerLineUp()
    {
        return PlayerUnits.Where(x => x.IsInLineup);
    }

    public void ResetState()
    {
        PlayerCoins = Singleton.Instance.GameInstance.Configuration.InitialCoins;
        SlotsUnlocked = Singleton.Instance.GameInstance.Configuration.InitialSlots;
    }

    void SetLineupLimit()
    {
        if (OnLineupLimitChanged != null) OnLineupLimitChanged.Invoke(LineUpLimit);
    }

    public void AddToFreeSlot(UnitInstance unit)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (!Slots[i])
            {
                Slots[i] = unit;
                OnSlotChanged.Invoke(i);
                return;
            }
        }
    }

    public void SetRecruit(int index, UnitBlueprint unitBlueprint)
    {
        if (index >= Recruits.Length)
        {
            Debug.LogError("Invalid recruit index");
            return;
        }
        Recruits[index] = unitBlueprint;
        if (OnRecruitChanged != null) OnRecruitChanged.Invoke(index);
    }

    public void ResetRecruitAt(int recruitSlotIndex)
    {
        if (Recruits.Length > recruitSlotIndex) Recruits[recruitSlotIndex] = null;
    }

    public void AddCoins(int amount)
    {
        PlayerCoins = Math.Clamp(PlayerCoins + amount, 0, 100000);
        if (OnTotalCoinsChanged != null) OnTotalCoinsChanged.Invoke(PlayerCoins);
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
            if (!Slots[i]) ++count;
        }
        return count;
    }

    public int GetAvailableRecruitsCount()
    {
        int count = 0;
        for (int i = 0; i < Recruits.Length; i++)
        {
            if (Recruits[i]) ++count;
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

    public bool CanAffordAnyRecruit(out int recruitCost)
    {
        recruitCost = 0;
        for (int i = 0; i < Recruits.Length; i++)
        {
            if (Recruits[i] && Singleton.Instance.GameInstance.GameState.PlayerCoins >= Recruits[i].GetCost())
            {
                recruitCost = Recruits[i].GetCost();
                return true;
            }
        }
        return false;
    }

    public int GetPlayerUnitsCount()
    {
        return PlayerUnits.Count;
    }


    public void AddPlayerUnit(UnitInstance newUnit)
    {
        if (!PlayerUnits.Contains(newUnit))
            PlayerUnits.Add(newUnit);
    }

}
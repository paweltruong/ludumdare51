using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Announcer announcer;
    [SerializeField]
    NumberPresenter coins;
    [SerializeField]
    NumberPresenter incomeTimer;
    [SerializeField]
    NumberPresenter trialCountdown;

    void Start()
    {
        Assert.IsNotNull(announcer);
        Assert.IsNotNull(coins);
        Assert.IsNotNull(incomeTimer);
        Assert.IsNotNull(trialCountdown);
    }

    public void Announce(string text)
    {
        announcer.Announce(text, Singleton.Instance.GameInstance.GetConfiguration().AnnouncementDuration);
    }
}

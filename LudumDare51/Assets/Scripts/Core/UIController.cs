using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Announcer announcer;

    void Start()
    {
        Assert.IsNotNull(announcer);
    }

    public void Announce(string text)
    {
        announcer.Announce(text, Singleton.Instance.GameInstance.GetConfiguration().AnnouncementDuration);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class Announcer : CanvasGroupMonobehaviour
{
    [SerializeField]
    TextMeshProUGUI txtAnnouncer;

    float requestedDuration = 0;
    float durationCountdown = 0;

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(txtAnnouncer);
    }

    public override void Update()
    {
        base.Update();

        TickDuration(Time.deltaTime);
    }

    public void Announce(string text, float duration)
    {
        Debug.Log("Announcing!");
        txtAnnouncer.text = text;
        FadeIn();
        StartDurationCountdown(duration);
        Singleton.Instance.AudioManager.PlaySFX_Announcement();
    }

    void StartDurationCountdown( float duration)
    {
        requestedDuration = duration;
        durationCountdown = 1 / speedDivider + duration;
    }

    void TickDuration(float deltaTime)
    {
        if (durationCountdown > 0)
        {
            durationCountdown -= deltaTime;


            //if duration expired then hide announcer
            if (durationCountdown <= 0)
            {
                OnDurationExpired();
            }
        }
        Debug.LogFormat("Announcing alpha {0}", masterCanvasGroup.alpha.ToString("0.000000"));
    }

    void OnDurationExpired()
    {
        Debug.Log("OnDurationExpired!");
        durationCountdown = 0;
        FadeOut();
    }

}

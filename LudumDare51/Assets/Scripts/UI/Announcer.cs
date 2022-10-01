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
        txtAnnouncer.text = text;
        FadeIn();
        StartDurationCountdown(duration);
    }

    void StartDurationCountdown( float duration)
    {
        requestedDuration = duration;
        durationCountdown = fadeSpeed + duration;
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
    }

    void OnDurationExpired()
    {
        durationCountdown = 0;
        FadeOut();
    }

}

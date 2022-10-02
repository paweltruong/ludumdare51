using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource musicSource;
    [SerializeField]
    AudioSource sfxSource;

    GameConfiguration configuration;

    void Start()
    {
        Assert.IsNotNull(musicSource);
        Assert.IsNotNull(sfxSource);

        configuration = Singleton.Instance.GameInstance.Configuration;
        Assert.IsNotNull(configuration);
        Assert.IsNotNull(configuration.ClickSound);
        Assert.IsNotNull(configuration.AnnouncementSound);
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    public void PlaySFX_Click()
    {        
        PlaySFX(Singleton.Instance.GameInstance.Configuration.ClickSound);
    }
    public void PlaySFX_Announcement()
    {
        PlaySFX(Singleton.Instance.GameInstance.Configuration.AnnouncementSound);
    }
}

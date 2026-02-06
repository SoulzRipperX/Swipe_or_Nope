using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("SFX")]
    public AudioClip clickClip;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    [Header("BGM")]
    public AudioClip mainBgmClip;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Playclick()
    {
        if (sfxSource == null || clickClip == null) return;
        sfxSource.PlayOneShot(clickClip);
    }

    public void PlayCorrect()
    {
        if (sfxSource == null || correctClip == null) return;
        sfxSource.PlayOneShot(correctClip);
    }

    public void PlayWrong()
    {
        if (sfxSource == null || wrongClip == null) return;
        sfxSource.PlayOneShot(wrongClip);
    }

    public void StopWrong()
    {
        if (sfxSource == null) return;
        sfxSource.Stop();
    }


    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void StopBGM()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }
    public void PlayMainBGM(AudioClip clip)
    {
        if (bgmSource == null || mainBgmClip == null) return;

        bgmSource.clip = mainBgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
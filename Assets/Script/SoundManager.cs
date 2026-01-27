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


    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }
    public void PlayMainBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
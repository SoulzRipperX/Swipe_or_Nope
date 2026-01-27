using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;

    [Header("SFX")]
    public AudioClip clickClip;

    void Start()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayClickSound();
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlayClickSound();
        }
    }

    void PlayClickSound()
    {
        if (sfxSource != null && clickClip != null)
        {
            sfxSource.PlayOneShot(clickClip);
        }
    }
}

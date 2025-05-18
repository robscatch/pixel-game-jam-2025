using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Manager<SoundManager>
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public AudioSource LoopingSource;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;


    public void PlayTheme(AudioClip theme)
    {
        LoopingSource.Stop(); // Stop the looping source if it's playing
        if (theme == null)
        {
            Debug.Log("No Sound configured");
            return;
        }

        musicSource.clip = theme;
        musicSource.Play();
    }


    public void PlayLoop(AudioClip theme)
    {
        if (theme == null)
        {
            Debug.Log("No Sound configured");
            return;
        }
        LoopingSource.clip = theme;
        LoopingSource.Play();
    }

    public void PlaySingle(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("No Sound configured");
            return;
        }

        efxSource.clip = clip;
        // multiple sounds without cutting each other off
        efxSource.PlayOneShot(efxSource.clip);
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.Log("No Sounds configured");
            return;
        }

        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();

    }

}

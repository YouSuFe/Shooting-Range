using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShootSound(AudioClip shootSound)
    {
        audioSource.PlayOneShot(shootSound);
    }

    public void PlayReloadSound(AudioClip reloadSound)
    {
        audioSource.PlayOneShot(reloadSound);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SoundType
{
    BackGroundMusic,
    EnemyDie,
    Dash,
    BombExplode
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] SoundList;

    private static SoundManager instance;
    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private AudioClip _audioClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _sfxSource = GetComponent<AudioSource>();
        _musicSource = GetComponent<AudioSource>();
        PlaySoundMusic(SoundType.BackGroundMusic,0.3f);
    }

    private void PlaySoundMusic(SoundType sound, float volume = 1)
    {
        instance._musicSource.PlayOneShot(instance.SoundList[(int)sound], volume);
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance._sfxSource.PlayOneShot(instance.SoundList[(int)sound], volume);
    }

    public static void PlaySound(AudioClip source, float volume = 1)
    {
        instance._sfxSource.PlayOneShot(source, volume);
    }

    public static void PlayLoop(AudioClip clip)
    {
        instance._sfxSource.loop = true;
        instance._audioClip = clip;
        instance._sfxSource.Play();
    }

    public static void AdjustSFXVolume(float sfxVolume) //function to adjust SFX Volume
    {
        instance._sfxSource.volume = sfxVolume;
    }
}

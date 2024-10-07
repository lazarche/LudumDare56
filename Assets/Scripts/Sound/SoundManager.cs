using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        LoadSounds();
    }
    public static SoundManager Instance { get { return instance; } }
    #endregion


    private Dictionary<string, SoundKey> sounds;

    void LoadSounds()
    {
        sounds = new Dictionary<string, SoundKey>();
        SoundKey[] data = Resources.LoadAll<SoundKey>("Sounds");
        for (int i = 0; i < data.Length; i++)
            sounds.Add(data[i].key, data[i]);
    }


    public void PlaySound(string key)
    {
        if (!SettingsManager.soundEnabled)
            return;

        SoundKey sound = null;
        sounds.TryGetValue(key, out sound);

        if (sound == null)
        {
            Debug.LogWarning("Key: " + key + " not contained in music dict");
            return;
        }
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.loop = false;
        newSource.volume = sound.baseVolume * SettingsManager.soundVolume;
        newSource.clip = sound.clip;
        newSource.playOnAwake = false;
        StartCoroutine(PlayClip(newSource));
    }
    public void PlaySound(AudioClip sound, float volume = 1f)
    {
        if (!SettingsManager.soundEnabled)
            return;

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.loop = false;
        newSource.volume = SettingsManager.soundVolume * volume;
        newSource.clip = sound;
        newSource.playOnAwake = false;
        StartCoroutine(PlayClip(newSource));
    }
    IEnumerator PlayClip(AudioSource audioSource)
    {
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(audioSource);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    #region Singleton
    static EnemySoundManager instance;
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
    }
    public static EnemySoundManager Instance { get { return instance; } }
    #endregion

    public AudioSource audioSource;
    public AudioClip[] clip;

    private void Start()
    {
       
    }

    public AudioSource GetSource(GameObject gameObject)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 1;
        source.maxDistance = 30;
        source.clip = clip[Random.Range(0, clip.Length)];
        source.loop = false;
        source.Play();
        return source;
    }
}

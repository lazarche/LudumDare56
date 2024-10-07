using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound", order = 1)]
public class SoundKey : ScriptableObject
{
    public string key;
    public float baseVolume = 1;
    public AudioClip clip;
}
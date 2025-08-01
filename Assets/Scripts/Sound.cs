using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    
    public AudioSource audioSource;
    
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume;
    
    [Range(0f, 3f)]
    public float pitch;
    
    public bool loop;
}

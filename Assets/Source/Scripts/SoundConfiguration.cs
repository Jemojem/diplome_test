using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/SoundConfiguration")]
public class SoundConfiguration : ScriptableObject
{
    [SerializeField] private SoundConfigurations[] _sounds;
    [SerializeField] private MusicConfigurations[] _music;

    public AudioClip GetSound(SoundType soundType)
    {
        return _sounds.First(t => t.SoundType == soundType).Sounds;
    }

    public AudioClip GetMusic(MusicType musicType)
    {
        return _music.First(t => t.SoundType == musicType).Sounds;
    }
}

[Serializable]
public class SoundConfigurations
{
    public AudioClip Sounds;
    public SoundType SoundType;
}

[Serializable]
public class MusicConfigurations
{
    public AudioClip Sounds;
    public MusicType SoundType;
}
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class SoundManager
{
    private static SoundConfiguration _configuration;
    private static AudioSource _soundAudioSource;
    private static AudioSource _musicAudioSource;
    private static GameObject _audioGameObject;
    private static float _soundVolume = 1f;
    private static float _musicVolume = 1f;

    public static void Initialize(SoundConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static void PlaySound(SoundType soundType)
    {
        if (_configuration == null)
            return;

        var clip = _configuration.GetSound(soundType);
        if (clip == null)
            return;

        EnsureAudioSources();
        _soundAudioSource.PlayOneShot(clip, _soundVolume);
    }

    public static void PlayMusic(MusicType musicType, bool loop = true)
    {
        if (_configuration == null)
            return;

        var clip = _configuration.GetMusic(musicType);
        if (clip == null)
            return;

        EnsureAudioSources();
        _musicAudioSource.clip = clip;
        _musicAudioSource.loop = loop;
        _musicAudioSource.volume = _musicVolume;
        _musicAudioSource.Play();
    }

    public static void StopMusic()
    {
        if (_musicAudioSource != null)
            _musicAudioSource.Stop();
    }

    public static void SetSoundVolume(float volume)
    {
        _soundVolume = Mathf.Clamp01(volume);
    }

    public static void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        if (_musicAudioSource != null)
            _musicAudioSource.volume = _musicVolume;
    }

    private static void EnsureAudioSources()
    {
        if (_audioGameObject != null)
            return;

        _audioGameObject = new GameObject("SoundManager");
        Object.DontDestroyOnLoad(_audioGameObject);

        _soundAudioSource = _audioGameObject.AddComponent<AudioSource>();
        _soundAudioSource.playOnAwake = false;

        _musicAudioSource = _audioGameObject.AddComponent<AudioSource>();
        _musicAudioSource.playOnAwake = false;
        _musicAudioSource.loop = true;
    }

    public static float GetSoundVolume() => _soundVolume;
    public static float GetMusicVolume() => _musicVolume;
}
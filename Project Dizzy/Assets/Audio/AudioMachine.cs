using UnityEngine;
using System.Collections.Generic;

public class AudioMachine : MonoBehaviour
{
    public static AudioMachine Instance;

    [System.Serializable]
    public class SoundEntry
    {
        public string soundName;
        public AudioClip clip;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Library")]
    [SerializeField] private SoundEntry[] musicClips;

    [Header("SFX Library")]
    [SerializeField] private SoundEntry[] sfxClips;

    private Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildDictionaries();
    }

    private void BuildDictionaries()
    {
        musicDict.Clear();
        sfxDict.Clear();

        foreach (SoundEntry entry in musicClips)
        {
            if (entry != null && entry.clip != null && !musicDict.ContainsKey(entry.soundName))
            {
                musicDict.Add(entry.soundName, entry.clip);
            }
        }

        foreach (SoundEntry entry in sfxClips)
        {
            if (entry != null && entry.clip != null && !sfxDict.ContainsKey(entry.soundName))
            {
                sfxDict.Add(entry.soundName, entry.clip);
            }
        }
    }

    public void PlayMusic(string soundName, bool loop = true)
    {
        if (!musicDict.ContainsKey(soundName))
        {
            Debug.LogWarning("Music not found: " + soundName);
            return;
        }

        musicSource.clip = musicDict[soundName];
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string soundName)
    {
        if (!sfxDict.ContainsKey(soundName))
        {
            Debug.LogWarning("SFX not found: " + soundName);
            return;
        }

        sfxSource.PlayOneShot(sfxDict[soundName]);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}
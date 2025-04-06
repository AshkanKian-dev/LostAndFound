using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource backgroundAudioSource;
    public AudioSource sfxAudioSource;

    [Header("Audio Clips")]
    public AudioClip backgroundClip;
    public AudioClip footstepSandClip;
    public AudioClip footstepGrassClip;

    void Awake()
    {
        // Implementing the Singleton pattern
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

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        backgroundAudioSource.clip = backgroundClip;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void PlayFootstep(string terrainType)
    {
        if (terrainType == "sand")
        {
            sfxAudioSource.PlayOneShot(footstepSandClip);
        }
        else if (terrainType == "grass")
        {
            sfxAudioSource.PlayOneShot(footstepGrassClip);
        }
    }
}

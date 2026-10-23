using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource; 
    public AudioSource sfxSource;   

    [Header("Music Playlist (3 Tracks)")]
    public AudioClip[] musicPlaylist = new AudioClip[3]; 
    private int currentTrackIndex = 0;

    [Header("Sound Effects")]
    public AudioClip buttonClickSound;
    public AudioClip weapon1Sound;
    public AudioClip weapon2Sound;
    public AudioClip weapon3Sound;
    public AudioClip damageSound;
    public AudioClip levelUpSound;

    [Header("Game Over & Win SFX")]
    public AudioClip gameOverSound;
    public AudioClip winSound;
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
            return;
        }
    }

    public void RestartMusic()
    {
        currentTrackIndex = 0;
        PlayNextTrack();
    }

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.loop = false;
        }

        PlayNextTrack();
    }

    void Update()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            if (!musicSource.isPlaying && musicSource.time >= musicSource.clip.length - 0.1f)
            {
                currentTrackIndex = (currentTrackIndex + 1) % musicPlaylist.Length;
                PlayNextTrack();
            }
        }
    }

    void PlayNextTrack()
    {
        if (musicPlaylist.Length > 0 && musicPlaylist[currentTrackIndex] != null)
        {
            musicSource.clip = musicPlaylist[currentTrackIndex];
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayDamageSound()
    {
        PlaySFX(damageSound);
    }
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }
    public void PlayLevelUpSound()
    {
        PlaySFX(levelUpSound);
    }
    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
    }

    public void PlayWinSound()
    {
        PlaySFX(winSound);
    }
    public void PlayWeaponSound(int weaponIndex)
    {
        AudioClip selectedClip = null;

        switch (weaponIndex)
        {
            case 0: selectedClip = weapon1Sound; break;
            case 1: selectedClip = weapon2Sound; break;
            case 2: selectedClip = weapon3Sound; break;
        }

        PlaySFX(selectedClip);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource sfxSource, musicSource;
    public List<AudioClip> possibleSongs = new List<AudioClip>();
    void Awake()
    {
        //singleton declaration
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        musicSource.clip = RandomSong();
        musicSource.Play();
    }

    void Update()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = RandomSong();
            musicSource.Play();
        }
    }

    public void Play(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void RandomPlay(AudioClip clip)
    {
        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip);
        sfxSource.pitch = 1f;
    }

    AudioClip RandomSong()
    {
        int choice = Random.Range(0, possibleSongs.Count-1);
        return possibleSongs[choice];
    }
}

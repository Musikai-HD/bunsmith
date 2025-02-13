using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource sfxSource;
    void Awake()
    {
        //singleton declaration
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
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
}

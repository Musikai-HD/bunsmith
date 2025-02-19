using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource sfxSource, musicSource;
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
        DontDestroyOnLoad(gameObject);
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

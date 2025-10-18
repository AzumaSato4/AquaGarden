using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public enum BGM_Type
    {
        title,
        playing,
        result
    }

    [SerializeField] AudioClip[] bgm;

    public static BGMManager instance;
    AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM(BGM_Type type)
    {
        audioSource.resource = bgm[(int)type];
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}

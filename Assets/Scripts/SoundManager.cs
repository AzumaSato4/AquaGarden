using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum BGM_Type
    {
        title,
        playing,
        result
    }
    public enum SE_Type
    {
        click,
        ng,
        celebrate,
        water,
        turnStart,
        turnEnd,
        getMoney,
        pay
    }

    [SerializeField] AudioClip[] bgm;
    [SerializeField] AudioClip[] se;

    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioSource seAudio;
    [SerializeField] AudioSource turnSEAudio;

    public static SoundManager instance;

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
        }
    }

    public void PlaySE(SE_Type type)
    {
        if (type == SE_Type.turnStart || type == SE_Type.turnEnd)
        {
            turnSEAudio.PlayOneShot(se[(int)type]);
        }
        else
        {
            seAudio.PlayOneShot(se[(int)type]);
        }
    }

    public void PlayBGM(BGM_Type type)
    {
        bgmAudio.resource = bgm[(int)type];
        bgmAudio.Play();
    }

    public void StopBGM()
    {
        bgmAudio.Stop();
    }
}

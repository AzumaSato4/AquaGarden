using UnityEngine;

public class SEManager : MonoBehaviour
{
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

    [SerializeField] AudioClip[] se;

    public static SEManager instance;
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
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySE(SE_Type type)
    {
        audioSource.PlayOneShot(se[(int)type]);
    }
}

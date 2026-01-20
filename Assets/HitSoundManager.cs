using UnityEngine;

public class HitSoundManager : MonoBehaviour
{
    public static HitSoundManager Instance;
    
    [Header("Sons de bonne coupe")]
    public AudioClip[] hitSoundsLeft;
    public AudioClip[] hitSoundsRight;
    
    [Header("Sons de mauvaise coupe")]
    public AudioClip[] badCutSounds;
    
    [Header("AudioSource")]
    public AudioSource audioSource;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        audioSource.playOnAwake = false;
    }
    
    public void PlayHitSound(bool isLeftSaber)
    {
        AudioClip[] sounds = isLeftSaber ? hitSoundsLeft : hitSoundsRight;
        
        if (sounds != null && sounds.Length > 0)
        {
            AudioClip clip = sounds[Random.Range(0, sounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    
    public void PlayBadCutSound()
    {
        if (badCutSounds != null && badCutSounds.Length > 0)
        {
            AudioClip clip = badCutSounds[Random.Range(0, badCutSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}

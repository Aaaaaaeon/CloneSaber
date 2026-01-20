using UnityEngine;
using UnityEngine.UI;

public class SongProgressBar : MonoBehaviour
{
    [Header("UI")]
    public Slider progressSlider;
    public Image fillImage;
    
    [Header("Couleurs")]
    public Color startColor = Color.cyan;
    public Color endColor = Color.magenta;
    
    private AudioSource musicSource;
    
    void Start()
    {
        // Trouver l'AudioSource de la musique
        BeatMapSpawner spawner = FindObjectOfType<BeatMapSpawner>();
        if (spawner != null)
        {
            musicSource = spawner.GetComponent<AudioSource>();
            if (musicSource == null)
            {
                musicSource = spawner.GetComponentInChildren<AudioSource>();
            }
        }
        
        if (progressSlider != null)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = 1;
            progressSlider.value = 0;
        }
    }
    
    void Update()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            float progress = musicSource.time / musicSource.clip.length;
            
            if (progressSlider != null)
            {
                progressSlider.value = progress;
            }
            
            // Changer la couleur selon la progression
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(startColor, endColor, progress);
            }
        }
    }
}

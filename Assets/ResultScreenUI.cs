using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultScreenUI : MonoBehaviour
{
    [Header("Affichage des résultats")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxComboText;
    public TextMeshProUGUI perfectText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI rankText;
    
    [Header("Boutons")]
    public Button retryButton;
    public Button menuButton;
    
    [Header("Son")]
    public AudioClip fireworksSound;
    private AudioSource audioSource;
    
    void Start()
    {
        // Jouer le son de feux d'artifice
        if (fireworksSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(fireworksSound);
        }
        
        DisplayResults();
        
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryClicked);
        }
        
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(OnMenuClicked);
        }
    }
    
    void DisplayResults()
    {
        if (titleText != null)
        {
            titleText.text = "RÉSULTATS";
        }
        
        // Lire les données statiques sauvegardées
        int score = ScoreManager.FinalScore;
        int maxCombo = ScoreManager.FinalMaxCombo;
        int perfects = ScoreManager.FinalPerfects;
        int goods = ScoreManager.FinalGoods;
        int misses = ScoreManager.FinalMisses;
        
        // Score total
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {score:N0}";
        }
        
        // Max Combo
        if (maxComboText != null)
        {
            maxComboText.text = $"MAX COMBO: {maxCombo}x";
        }
        
        // Perfect (cyan)
        if (perfectText != null)
        {
            perfectText.text = $"PERFECT: {perfects}";
            perfectText.color = Color.cyan;
        }
        
        // Good (yellow)
        if (goodText != null)
        {
            goodText.text = $"GOOD: {goods}";
            goodText.color = Color.yellow;
        }
        
        // Miss (red)
        if (missText != null)
        {
            missText.text = $"MISS: {misses}";
            missText.color = Color.red;
        }
        
        // Rang
        if (rankText != null)
        {
            int total = perfects + goods + misses;
            float accuracy = total > 0 ? ((float)(perfects + goods) / total) * 100f : 100f;
            
            string rank = "D";
            if (accuracy >= 95) rank = "S";
            else if (accuracy >= 85) rank = "A";
            else if (accuracy >= 70) rank = "B";
            else if (accuracy >= 50) rank = "C";
            
            rankText.text = rank;
            
            // Couleur selon le rang
            switch (rank)
            {
                case "S": rankText.color = new Color(1f, 0.84f, 0f); break; // Or
                case "A": rankText.color = Color.green; break;
                case "B": rankText.color = Color.cyan; break;
                case "C": rankText.color = Color.yellow; break;
                default: rankText.color = Color.red; break;
            }
        }
    }
    
    void OnRetryClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    void OnMenuClicked()
    {
        // Réinitialiser la sélection par défaut
        GameManager.SelectedBeatMapFile = "beatmap_badapple";
        GameManager.SelectedMusicFile = "Bad Apple";
        
        SceneManager.LoadScene("MenuScene");
    }
}

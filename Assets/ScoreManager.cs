using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [Header("Score")]
    private int currentScore = 0;
    private int combo = 0;
    private int maxCombo = 0;
    private int perfectCuts = 0;
    private int goodCuts = 0;
    private int misses = 0;
    private int totalNotes = 0;
    
    [Header("Points")]
    public int pointsPerGoodCut = 100;
    public int comboBonus = 10;
    
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    
    // Données statiques pour persister vers ResultScreen
    public static int FinalScore;
    public static int FinalMaxCombo;
    public static int FinalPerfects;
    public static int FinalGoods;
    public static int FinalMisses;
    
    void Awake()
    {
        // Pas de DontDestroyOnLoad - nouveau ScoreManager à chaque scène
        Instance = this;
        ResetScore();
    }
    
    public void ResetScore()
    {
        currentScore = 0;
        combo = 0;
        maxCombo = 0;
        perfectCuts = 0;
        goodCuts = 0;
        misses = 0;
        totalNotes = 0;
        UpdateUI();
        Debug.Log("ScoreManager: Score reset!");
    }
    
    public void AddPerfectCut()
    {
        perfectCuts++;
        totalNotes++;
        combo++;
        
        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
        
        int points = pointsPerGoodCut + (combo * comboBonus);
        currentScore += points;
        
        UpdateUI();
    }
    
    public void AddGoodCut()
    {
        goodCuts++;
        totalNotes++;
        combo++;
        
        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
        
        int points = (int)((pointsPerGoodCut + (combo * comboBonus)) * 0.7f);
        currentScore += points;
        
        UpdateUI();
    }
    
    public void AddBadCut()
    {
        misses++;
        totalNotes++;
        combo = 0;
        UpdateUI();
    }
    
    public void AddMiss()
    {
        misses++;
        totalNotes++;
        combo = 0;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString("N0");
        }
        
        if (comboText != null)
        {
            comboText.text = $"{combo}x";
        }
    }
    
    // Sauvegarder pour le ResultScreen avant de changer de scène
    public void SaveForResults()
    {
        FinalScore = currentScore;
        FinalMaxCombo = maxCombo;
        FinalPerfects = perfectCuts;
        FinalGoods = goodCuts;
        FinalMisses = misses;
    }
    
    // Getters
    public int GetScore() => currentScore;
    public int GetCurrentCombo() => combo;
    public int GetMaxCombo() => maxCombo;
    public int GetPerfectCuts() => perfectCuts;
    public int GetGoodCuts() => goodCuts;
    public int GetMisses() => misses;
    public int GetTotalNotes() => totalNotes;
    
    public float GetAccuracy()
    {
        if (totalNotes == 0) return 100f;
        return ((float)(perfectCuts + goodCuts) / totalNotes) * 100f;
    }
    
    public string GetRank()
    {
        float accuracy = GetAccuracy();
        if (accuracy >= 95) return "S";
        if (accuracy >= 85) return "A";
        if (accuracy >= 70) return "B";
        if (accuracy >= 50) return "C";
        return "D";
    }
}

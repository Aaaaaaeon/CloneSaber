using UnityEngine;
using TMPro;

public class MissIndicator : MonoBehaviour
{
    public static MissIndicator Instance;
    
    public TextMeshProUGUI missText;
    public float displayDuration = 0.5f;
    
    [Header("Couleurs")]
    public Color missColor = Color.red;
    public Color goodColor = Color.yellow;
    public Color perfectColor = Color.cyan;
    
    private float hideTimer = 0f;
    
    void Awake()
    {
        Instance = this;
        if (missText != null)
        {
            missText.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if (hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0 && missText != null)
            {
                missText.gameObject.SetActive(false);
            }
        }
    }
    
    public void ShowMiss()
    {
        if (missText != null)
        {
            missText.gameObject.SetActive(true);
            missText.text = "MISS!";
            missText.color = missColor;
            hideTimer = displayDuration;
        }
    }
    
    public void ShowGood(int combo)
    {
        if (missText != null)
        {
            missText.gameObject.SetActive(true);
            missText.text = "GOOD";
            missText.color = goodColor;
            hideTimer = displayDuration;
        }
    }
    
    public void ShowPerfect(int combo)
    {
        if (missText != null)
        {
            missText.gameObject.SetActive(true);
            missText.text = "PERFECT!";
            missText.color = perfectColor;
            hideTimer = displayDuration;
        }
    }
    
    // Méthode legacy pour compatibilité
    public void ShowCombo(int combo)
    {
        ShowPerfect(combo);
    }
}

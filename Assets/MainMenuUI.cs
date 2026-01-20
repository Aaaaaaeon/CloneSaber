using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI titleText;
    public Button playButton;
    public Button tutoButton;
    
    void Start()
    {
        if (titleText != null)
        {
            titleText.text = "CLONE SABER";
        }
        
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayClicked);
        }
        
        if (tutoButton != null)
        {
            tutoButton.onClick.AddListener(OnTutoClicked);
        }
    }
    
    void OnPlayClicked()
    {
        // Sélectionner Bad Apple
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SelectSong("beatmap_badapple", "Bad Apple");
            GameManager.Instance.LoadGameScene();
        }
        else
        {
            GameManager.SelectedBeatMapFile = "beatmap_badapple";
            GameManager.SelectedMusicFile = "Bad Apple";
            SceneManager.LoadScene("GameScene");
        }
    }
    
    void OnTutoClicked()
    {
        // Sélectionner le Tuto
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SelectSong("beatmap_tuto", "Tuto");
            GameManager.Instance.LoadGameScene();
        }
        else
        {
            GameManager.SelectedBeatMapFile = "beatmap_tuto";
            GameManager.SelectedMusicFile = "Tuto";
            SceneManager.LoadScene("GameScene");
        }
    }
}

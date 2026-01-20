using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // Info de la chanson sélectionnée
    public static string SelectedBeatMapFile = "beatmap_badapple"; // Nom du fichier JSON (sans .json)
    public static string SelectedMusicFile = "Bad Apple"; // Nom du fichier audio
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SelectSong(string beatMapFile, string musicFile)
    {
        SelectedBeatMapFile = beatMapFile;
        SelectedMusicFile = musicFile;
        Debug.Log($"GameManager: Chanson sélectionnée - Map: {beatMapFile}, Music: {musicFile}");
    }
    
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    
    public void LoadResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }
}

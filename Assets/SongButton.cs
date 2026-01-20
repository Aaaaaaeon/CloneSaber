using UnityEngine;
using UnityEngine.SceneManagement;

public class SongButton : MonoBehaviour
{
    [Header("Chanson")]
    public string beatMapFileName = "beatmap_badapple";
    public string musicFileName = "Bad Apple";
    
    void Awake()
    {
        Debug.Log($"SongButton '{gameObject.name}' initialisé avec: {beatMapFileName} / {musicFileName}");
    }
    
    public void OnClick()
    {
        Debug.Log($"=== SONG BUTTON CLICK ===");
        Debug.Log($"Bouton: {gameObject.name}");
        Debug.Log($"BeatMap: {beatMapFileName}");
        Debug.Log($"Music: {musicFileName}");
        
        // Sauvegarder la sélection
        GameManager.SelectedBeatMapFile = beatMapFileName;
        GameManager.SelectedMusicFile = musicFileName;
        
        Debug.Log($"GameManager.SelectedBeatMapFile = {GameManager.SelectedBeatMapFile}");
        Debug.Log($"GameManager.SelectedMusicFile = {GameManager.SelectedMusicFile}");
        
        // Charger la scène de jeu
        SceneManager.LoadScene("GameScene");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    void Update()
    {
        // Bouton Menu/Start sur la manette gauche Oculus
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            ReturnToMenu();
        }
        
        // Bouton B ou Y pour retourner au menu aussi
        if (OVRInput.GetDown(OVRInput.Button.Two)) // B sur manette droite
        {
            ReturnToMenu();
        }
    }
    
    public void ReturnToMenu()
    {
        Debug.Log("Retour au menu...");
        
        // Arrêter la musique si elle joue
        AudioSource[] audioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in audioSources)
        {
            audio.Stop();
        }
        
        // Charger la scène du menu
        SceneManager.LoadScene("MenuScene");
    }
}

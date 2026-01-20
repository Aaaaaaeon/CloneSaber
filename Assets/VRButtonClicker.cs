using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRButtonClicker : MonoBehaviour
{
    public float rayLength = 10f;
    public LayerMask uiLayer = -1; // Tous les layers par défaut
    
    private Button lastHitButton = null;
    
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        // Raycast avec tous les layers
        if (Physics.Raycast(ray, out hit, rayLength, uiLayer))
        {
            Button button = hit.collider.GetComponent<Button>();
            if (button == null)
            {
                button = hit.collider.GetComponentInParent<Button>();
            }
            
            lastHitButton = button;
            
            // Détecter TOUTES les formes de trigger press
            bool triggerPressed = false;
            
            // Méthode 1: OVRInput.GetDown
            triggerPressed |= OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
            
            // Méthode 2: OVRInput.Get avec seuil
            triggerPressed |= OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9f && 
                              OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.5f;
            triggerPressed |= OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9f && 
                              OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.5f;
            
            // Méthode 3: Bouton A ou X comme fallback
            triggerPressed |= OVRInput.GetDown(OVRInput.Button.One); // A
            triggerPressed |= OVRInput.GetDown(OVRInput.Button.Three); // X
            
            if (triggerPressed && button != null)
            {
                ExecuteButtonAction(button, hit.collider.gameObject.name);
            }
        }
        else
        {
            lastHitButton = null;
        }
    }
    
    void ExecuteButtonAction(Button button, string buttonName)
    {
        string nameLower = buttonName.ToLower();
        
        // Essayer onClick d'abord
        if (button.onClick.GetPersistentEventCount() > 0)
        {
            button.onClick.Invoke();
            return;
        }
        
        // Fallback basé sur le nom
        if (nameLower.Contains("play") || nameLower.Contains("jouer") || 
            nameLower.Contains("song") || nameLower.Contains("apple") ||
            nameLower.Contains("start") || nameLower.Contains("bad"))
        {
            LoadGame();
        }
        else if (nameLower.Contains("menu") || nameLower.Contains("quit") || 
                 nameLower.Contains("quitter") || nameLower.Contains("exit"))
        {
            LoadMenu();
        }
        else if (nameLower.Contains("retry") || nameLower.Contains("rejouer") || 
                 nameLower.Contains("replay") || nameLower.Contains("again"))
        {
            LoadGame();
        }
        else
        {
            // Dernier recours - invoquer onClick
            button.onClick.Invoke();
        }
    }
    
    void LoadGame()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGameScene();
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    
    void LoadMenu()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadMenuScene();
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}

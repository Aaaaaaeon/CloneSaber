using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRMenuInput : MonoBehaviour
{
    [Header("Configuration")]
    public float rayLength = 15f;
    public LineRenderer laserLine;
    public Transform laserOrigin;
    
    [Header("Debug")]
    public bool showDebugLogs = true; // Activé par défaut maintenant
    
    private bool wasPressed = false;
    
    void Start()
    {
        if (laserOrigin == null)
        {
            laserOrigin = transform;
        }
        
        if (laserLine == null)
        {
            laserLine = gameObject.AddComponent<LineRenderer>();
            laserLine.startWidth = 0.005f;
            laserLine.endWidth = 0.005f;
            laserLine.material = new Material(Shader.Find("Unlit/Color"));
            laserLine.material.color = Color.cyan;
            laserLine.positionCount = 2;
        }
        
        Debug.Log("VRMenuInput: Initialisé!");
    }
    
    void Update()
    {
        UpdateLaser();
        
        bool isPressed = GetTriggerPressed();
        
        // Debug constant pour voir ce qui est touché
        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // Afficher ce qu'on touche en permanence
            if (showDebugLogs)
            {
                Debug.Log($"LASER TOUCHE: {hit.collider.gameObject.name}");
            }
        }
        
        if (isPressed && !wasPressed)
        {
            Debug.Log("=== TRIGGER PRESSÉ ===");
            TryClickButton();
        }
        
        wasPressed = isPressed;
    }
    
    bool GetTriggerPressed()
    {
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f)
            return true;
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f)
            return true;
        if (OVRInput.Get(OVRInput.Button.One))
            return true;
        if (OVRInput.Get(OVRInput.Button.Three))
            return true;
        
        // Aussi détecter clic souris pour test dans l'éditeur
        if (Input.GetMouseButtonDown(0))
            return true;
            
        return false;
    }
    
    void UpdateLaser()
    {
        if (laserLine != null)
        {
            Vector3 start = laserOrigin.position;
            Vector3 end = laserOrigin.position + laserOrigin.forward * rayLength;
            laserLine.SetPosition(0, start);
            laserLine.SetPosition(1, end);
        }
    }
    
    void TryClickButton()
    {
        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        RaycastHit hit;
        
        Debug.Log($"Raycast depuis {laserOrigin.position}");
        
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.Log($"TOUCHÉ: {hit.collider.gameObject.name}");
            
            Button button = hit.collider.GetComponent<Button>();
            if (button == null)
            {
                button = hit.collider.GetComponentInParent<Button>();
            }
            
            if (button != null)
            {
                Debug.Log($"BOUTON TROUVÉ: {button.name}");
                HandleButtonClick(button);
            }
            else
            {
                Debug.Log("Pas de component Button sur cet objet");
            }
        }
        else
        {
            Debug.Log("Rien touché par le raycast");
        }
    }
    
    void HandleButtonClick(Button button)
    {
        Debug.Log($"=== CLICK SUR {button.name} ===");
        
        SongButton songButton = button.GetComponent<SongButton>();
        if (songButton != null)
        {
            Debug.Log("SongButton trouvé, appel de OnClick()");
            songButton.OnClick();
            return;
        }
        
        Debug.Log("Pas de SongButton, invocation onClick standard");
        button.onClick.Invoke();
    }
}

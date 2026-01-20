using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class VRUIPointer : MonoBehaviour
{
    [Header("Rayon")]
    public LineRenderer lineRenderer;
    public float rayLength = 10f;
    
    [Header("Couleurs")]
    public Color normalColor = Color.cyan;
    public Color hoverColor = Color.green;
    
    private GameObject currentHover;
    private bool triggerWasPressed = false;
    
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        
        if (lineRenderer != null)
        {
            lineRenderer.startColor = normalColor;
            lineRenderer.endColor = normalColor;
        }
    }
    
    void Update()
    {
        UpdateRay();
        CheckTrigger();
    }
    
    void UpdateRay()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * rayLength;
        
        // Raycast pour détecter les UI
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Camera.main.WorldToScreenPoint(startPos);
        
        // Raycast physique
        Ray ray = new Ray(startPos, transform.forward);
        RaycastHit hit;
        
        currentHover = null;
        
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            endPos = hit.point;
            
            // Chercher un bouton
            Button button = hit.collider.GetComponent<Button>();
            if (button != null && button.interactable)
            {
                currentHover = button.gameObject;
                if (lineRenderer != null)
                {
                    lineRenderer.startColor = hoverColor;
                    lineRenderer.endColor = hoverColor;
                }
            }
        }
        
        // Si pas de hit physique, essayer le UI raycast
        if (currentHover == null)
        {
            // Raycast sur les Canvas UI
            var results = new List<RaycastResult>();
            
            // Créer un pointer event basé sur la position du rayon
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
            
            // Vérifier manuellement les Canvas en World Space
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
                    if (raycaster != null)
                    {
                        // Check intersection avec le plan du canvas
                        Plane canvasPlane = new Plane(canvas.transform.forward, canvas.transform.position);
                        float distance;
                        if (canvasPlane.Raycast(ray, out distance) && distance <= rayLength)
                        {
                            Vector3 hitPoint = ray.GetPoint(distance);
                            endPos = hitPoint;
                            
                            // Vérifier si on touche un bouton
                            Button[] buttons = canvas.GetComponentsInChildren<Button>();
                            foreach (Button btn in buttons)
                            {
                                RectTransform rect = btn.GetComponent<RectTransform>();
                                if (RectTransformUtility.RectangleContainsScreenPoint(rect, 
                                    RectTransformUtility.WorldToScreenPoint(Camera.main, hitPoint), Camera.main))
                                {
                                    currentHover = btn.gameObject;
                                    if (lineRenderer != null)
                                    {
                                        lineRenderer.startColor = hoverColor;
                                        lineRenderer.endColor = hoverColor;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        // Si rien n'est survolé, couleur normale
        if (currentHover == null && lineRenderer != null)
        {
            lineRenderer.startColor = normalColor;
            lineRenderer.endColor = normalColor;
        }
        
        // Mettre à jour le line renderer
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
    
    void CheckTrigger()
    {
        // Détecter la gâchette Oculus
        bool triggerPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
                              OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) ||
                              Input.GetMouseButtonDown(0) || // Souris pour tester
                              Input.GetKeyDown(KeyCode.Space); // Espace pour tester
        
        if (triggerPressed && !triggerWasPressed)
        {
            // Trigger vient d'être pressé
            if (currentHover != null)
            {
                Button button = currentHover.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    Debug.Log("BOUTON CLIQUÉ: " + button.name);
                }
            }
        }
        
        triggerWasPressed = triggerPressed;
    }
}

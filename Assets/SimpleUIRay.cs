using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class SimpleUIRay : MonoBehaviour
{
    public float rayLength = 10f;
    public Color rayColor = Color.cyan;
    
    private LineRenderer lineRenderer;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        
        // Configuration automatique du Line Renderer
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.005f;
        lineRenderer.useWorldSpace = true;
        
        // Créer un matériau simple si aucun n'est assigné
        if (lineRenderer.material == null || lineRenderer.material.name.Contains("Default"))
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
        lineRenderer.startColor = rayColor;
        lineRenderer.endColor = rayColor;
    }
    
    void Update()
    {
        // Position de départ = position de cet objet
        Vector3 startPos = transform.position;
        
        // Direction = forward de cet objet
        Vector3 endPos = startPos + transform.forward * rayLength;
        
        // Si le XR Ray Interactor a touché quelque chose, utiliser ce point
        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            endPos = hit.point;
        }
        
        // Dessiner la ligne
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}

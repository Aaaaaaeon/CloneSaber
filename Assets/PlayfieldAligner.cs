using UnityEngine;

/// <summary>
/// Aligne le jeu avec la direction du joueur au démarrage
/// Placer sur l'OVRCameraRig de CHAQUE scène
/// </summary>
public class PlayfieldAligner : MonoBehaviour
{
    void Start()
    {
        // Attendre un peu que le tracking soit prêt
        Invoke("Align", 0.3f);
    }
    
    void Align()
    {
        // Trouver où regarde le joueur
        OVRCameraRig rig = GetComponent<OVRCameraRig>();
        if (rig == null) rig = FindObjectOfType<OVRCameraRig>();
        if (rig == null) return;
        
        Transform head = rig.centerEyeAnchor;
        if (head == null) return;
        
        // Obtenir la direction horizontale
        Vector3 forward = head.forward;
        forward.y = 0;
        if (forward.magnitude < 0.1f) return;
        
        // Calculer l'angle pour que le jeu soit devant
        float headAngle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        
        // Tourner le rig pour compenser
        rig.transform.rotation = Quaternion.Euler(0, -headAngle, 0);
        
        // Recentrer la position
        Vector3 headPos = head.position;
        headPos.y = 0;
        rig.transform.position = -headPos;
        
        Debug.Log($"Aligned to {headAngle} degrees");
    }
}

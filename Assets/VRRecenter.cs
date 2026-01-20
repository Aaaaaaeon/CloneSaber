using UnityEngine;

public class VRRecenter : MonoBehaviour
{
    [Header("Recentrer automatiquement")]
    public bool recenterOnStart = true;
    
    [Header("Bouton pour recentrer")]
    public bool enableManualRecenter = true;
    
    void Start()
    {
        if (recenterOnStart)
        {
            Recenter();
        }
    }
    
    void Update()
    {
        // Appuyer sur les deux boutons Start/Menu pour recentrer
        if (enableManualRecenter)
        {
            bool leftMenu = OVRInput.GetDown(OVRInput.Button.Start);
            bool rightMenu = OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.RTouch);
            
            // Ou bouton B + Y ensemble
            bool bothGrips = OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.Four);
            
            if (leftMenu || bothGrips)
            {
                Recenter();
                Debug.Log("VRRecenter: Position recentrée!");
            }
        }
    }
    
    public void Recenter()
    {
        // Méthode 1: OVRManager
        if (OVRManager.display != null)
        {
            OVRManager.display.RecenterPose();
        }
        
        // Méthode 2: InputTracking (Unity XR)
        UnityEngine.XR.InputTracking.Recenter();
    }
}

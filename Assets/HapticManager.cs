using UnityEngine;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Vibration rapide pour coupe de note
    public void VibrateOnHit(bool isLeftSaber)
    {
        OVRInput.Controller controller = isLeftSaber ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        
        // Vibration courte et forte
        OVRInput.SetControllerVibration(1f, 0.7f, controller);
        
        // Arrêter après 0.05 secondes
        StartCoroutine(StopVibrationAfterDelay(controller, 0.05f));
    }
    
    // Vibration pour bad cut
    public void VibrateOnBadCut(bool isLeftSaber)
    {
        OVRInput.Controller controller = isLeftSaber ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        
        // Vibration plus longue pour bad cut
        OVRInput.SetControllerVibration(0.5f, 1f, controller);
        StartCoroutine(StopVibrationAfterDelay(controller, 0.15f));
    }
    
    private System.Collections.IEnumerator StopVibrationAfterDelay(OVRInput.Controller controller, float delay)
    {
        yield return new WaitForSeconds(delay);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}

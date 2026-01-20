using UnityEngine;

public class SaberCollision : MonoBehaviour
{
    [Header("Vibration continue")]
    public float vibrationFrequency = 0.5f;
    public float vibrationAmplitude = 0.3f;
    
    private bool isColliding = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saber"))
        {
            isColliding = true;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Saber") && isColliding)
        {
            // Vibrer tant qu'il y a collision
            OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, OVRInput.Controller.RTouch);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Saber"))
        {
            isColliding = false;
            // Arrêter la vibration
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
    }
    
    // Méthode statique pour vibrer quand on coupe une note
    public static void VibrateOnHit(bool isLeftSaber)
    {
        OVRInput.Controller controller = isLeftSaber ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        OVRInput.SetControllerVibration(1f, 0.5f, controller);
        
        // Utilise un MonoBehaviour pour arrêter la vibration
        // Note: On arrête après un court délai via une coroutine globale
    }
}

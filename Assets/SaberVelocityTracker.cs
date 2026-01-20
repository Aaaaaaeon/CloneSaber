using UnityEngine;

public class SaberVelocityTracker : MonoBehaviour
{
    // La vélocité calculée du sabre
    public Vector3 Velocity { get; private set; }
    
    private Vector3 previousPosition;
    
    void Start()
    {
        previousPosition = transform.position;
    }
    
    void Update()
    {
        // Calculer la vélocité basée sur le déplacement depuis la dernière frame
        Velocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
    }
}

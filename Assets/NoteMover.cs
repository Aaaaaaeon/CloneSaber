using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Si le cube passe derrière le joueur, c'est un MISS
        if (transform.position.z > 0f) 
        {
            // Signaler le miss au ScoreManager
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddMiss();
            }
            
            // Afficher MISS
            if (MissIndicator.Instance != null)
            {
                MissIndicator.Instance.ShowMiss();
            }
            
            Destroy(gameObject);
        }
    }
}
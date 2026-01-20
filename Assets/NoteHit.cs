using UnityEngine;

public enum CutDirection
{
    Up,
    Down,
    Left,
    Right
}

public enum NoteColor
{
    Blue,
    Red
}

public class NoteHit : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject badCutEffect;
    
    [HideInInspector]
    public CutDirection requiredDirection = CutDirection.Down;
    
    [HideInInspector]
    public NoteColor noteColor = NoteColor.Blue;
    
    public float angleTolerance = 45f;
    public float minCutSpeed = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saber"))
        {
            // Vérifier la couleur du sabre
            SaberColor saberColorScript = other.GetComponent<SaberColor>();
            if (saberColorScript == null)
            {
                saberColorScript = other.GetComponentInParent<SaberColor>();
            }
            
            if (saberColorScript == null)
            {
                Debug.LogWarning("Pas de SaberColor trouvé sur le sabre!");
                return;
            }
            
            // Vérifier si la couleur correspond
            bool colorMatches = (noteColor == NoteColor.Blue && saberColorScript.saberColor == SaberColorType.Blue) ||
                               (noteColor == NoteColor.Red && saberColorScript.saberColor == SaberColorType.Red);
            
            if (!colorMatches)
            {
                WrongColorCut();
                return;
            }
            
            // Vérifier la vélocité
            Vector3 saberVelocity = Vector3.zero;
            
            SaberVelocityTracker velocityTracker = other.GetComponent<SaberVelocityTracker>();
            if (velocityTracker == null)
            {
                velocityTracker = other.GetComponentInParent<SaberVelocityTracker>();
            }
            
            if (velocityTracker != null)
            {
                saberVelocity = velocityTracker.Velocity;
            }
            else
            {
                Rigidbody saberRb = other.GetComponent<Rigidbody>();
                if (saberRb == null)
                {
                    saberRb = other.GetComponentInParent<Rigidbody>();
                }
                
                if (saberRb != null)
                {
                    saberVelocity = saberRb.linearVelocity;
                }
            }
            
            if (saberVelocity.magnitude > minCutSpeed)
            {
                float cutAngle = GetCutAngle(saberVelocity);
                
                if (cutAngle <= 15f)
                {
                    // PERFECT - angle très précis
                    bool isLeftSaber = saberColorScript.saberColor == SaberColorType.Blue;
                    PerfectCut(isLeftSaber);
                }
                else if (cutAngle <= angleTolerance)
                {
                    // GOOD - angle acceptable
                    bool isLeftSaber = saberColorScript.saberColor == SaberColorType.Blue;
                    GoodCut(isLeftSaber);
                }
                else
                {
                    BadCut();
                }
            }
            else
            {
                BadCut();
            }
        }
    }
    
    float GetCutAngle(Vector3 saberVelocity)
    {
        Vector2 swipeDir = new Vector2(saberVelocity.x, saberVelocity.y).normalized;
        Vector2 expectedDir = GetExpectedDirection();
        return Vector2.Angle(swipeDir, expectedDir);
    }

    bool IsCorrectDirection(Vector3 saberVelocity)
    {
        Vector2 swipeDir = new Vector2(saberVelocity.x, saberVelocity.y).normalized;
        Vector2 expectedDir = GetExpectedDirection();
        float angle = Vector2.Angle(swipeDir, expectedDir);
        return angle <= angleTolerance;
    }

    Vector2 GetExpectedDirection()
    {
        switch (requiredDirection)
        {
            case CutDirection.Up: return Vector2.up;
            case CutDirection.Down: return Vector2.down;
            case CutDirection.Left: return Vector2.left;
            case CutDirection.Right: return Vector2.right;
            default: return Vector2.down;
        }
    }

    void PerfectCut(bool isLeftSaber)
    {
        // Ajouter au score (points max)
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPerfectCut();
            int combo = ScoreManager.Instance.GetCurrentCombo();
            
            // Afficher PERFECT
            if (MissIndicator.Instance != null)
            {
                MissIndicator.Instance.ShowPerfect(combo);
            }
        }
        
        // Jouer le son de hit
        if (HitSoundManager.Instance != null)
        {
            HitSoundManager.Instance.PlayHitSound(isLeftSaber);
        }
        
        // Vibration
        if (HapticManager.Instance != null)
        {
            HapticManager.Instance.VibrateOnHit(isLeftSaber);
        }
        
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
        
        Destroy(gameObject);
    }

    void GoodCut(bool isLeftSaber)
    {
        // Ajouter au score (points réduits)
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddGoodCut();
            int combo = ScoreManager.Instance.GetCurrentCombo();
            
            // Afficher GOOD
            if (MissIndicator.Instance != null)
            {
                MissIndicator.Instance.ShowGood(combo);
            }
        }
        
        // Jouer le son de hit
        if (HitSoundManager.Instance != null)
        {
            HitSoundManager.Instance.PlayHitSound(isLeftSaber);
        }
        
        // Vibration
        if (HapticManager.Instance != null)
        {
            HapticManager.Instance.VibrateOnHit(isLeftSaber);
        }
        
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
        
        Destroy(gameObject);
    }

    void BadCut()
    {
        // Signaler au score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddBadCut();
        }
        
        // Jouer le son de bad cut
        if (HitSoundManager.Instance != null)
        {
            HitSoundManager.Instance.PlayBadCutSound();
        }
        
        if (MissIndicator.Instance != null)
        {
            MissIndicator.Instance.ShowMiss();
        }
        
        if (badCutEffect != null)
        {
            Instantiate(badCutEffect, transform.position, transform.rotation);
        }
        
        Destroy(gameObject);
    }
    
    void WrongColorCut()
    {
        // Signaler au score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddBadCut();
        }
        
        // Jouer le son de bad cut
        if (HitSoundManager.Instance != null)
        {
            HitSoundManager.Instance.PlayBadCutSound();
        }
        
        if (MissIndicator.Instance != null)
        {
            MissIndicator.Instance.ShowMiss();
        }
        
        if (badCutEffect != null)
        {
            Instantiate(badCutEffect, transform.position, transform.rotation);
        }
        
        Destroy(gameObject);
    }
}
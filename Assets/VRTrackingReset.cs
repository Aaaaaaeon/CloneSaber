using UnityEngine;

/// <summary>
/// Script désactivé - remplacé par PlayfieldAligner
/// Tu peux supprimer ce script de tes scènes ou le laisser (il ne fait plus rien)
/// </summary>
public class VRTrackingReset : MonoBehaviour
{
    void Start()
    {
        // Plus de rechargement de scène - PlayfieldAligner gère l'alignement
        if (OVRManager.display != null)
        {
            OVRManager.display.RecenterPose();
        }
    }
}

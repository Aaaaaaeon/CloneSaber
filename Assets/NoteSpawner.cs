using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float beatInterval = 1.0f;
    private float timer;

    [Header("Réglages Aléatoires")]
    public float xRange = 1.5f;
    public float yRange = 0.5f;
    
    [Header("Matériaux de couleur")]
    public Material blueMaterial;
    public Material redMaterial;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > beatInterval)
        {
            SpawnCube();
            timer -= beatInterval;
        }
    }

    void SpawnCube()
    {
        // Position aléatoire
        float randomX = Random.Range(-xRange, xRange);
        float randomY = Random.Range(-yRange, yRange);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX, 
            transform.position.y + randomY, 
            transform.position.z
        );

        // Direction aléatoire
        CutDirection randomDirection = (CutDirection)Random.Range(0, 4);
        Quaternion rotation = GetRotationForDirection(randomDirection);

        // Couleur aléatoire
        NoteColor randomColor = (NoteColor)Random.Range(0, 2);

        // Instancier le cube
        GameObject newCube = Instantiate(cubePrefab, spawnPosition, rotation);
        
        // Configurer le script NoteHit
        NoteHit noteHit = newCube.GetComponent<NoteHit>();
        if (noteHit != null)
        {
            noteHit.requiredDirection = randomDirection;
            noteHit.noteColor = randomColor;
        }
        
        // Appliquer le matériau selon la couleur
        Renderer cubeRenderer = newCube.GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            if (randomColor == NoteColor.Blue && blueMaterial != null)
            {
                cubeRenderer.material = blueMaterial;
            }
            else if (randomColor == NoteColor.Red && redMaterial != null)
            {
                cubeRenderer.material = redMaterial;
            }
        }
        
        Debug.Log($"Cube spawné: {randomColor}, Direction: {randomDirection}");
    }

    Quaternion GetRotationForDirection(CutDirection direction)
    {
        float zRotation = 0f;
        
        switch (direction)
        {
            case CutDirection.Down:
                zRotation = 0f;
                break;
            case CutDirection.Up:
                zRotation = 180f;
                break;
            case CutDirection.Left:
                zRotation = -90f;
                break;
            case CutDirection.Right:
                zRotation = 90f;
                break;
        }
        
        return transform.rotation * Quaternion.Euler(0, 0, zRotation);
    }
}
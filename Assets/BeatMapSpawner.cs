using UnityEngine;
using System.Collections.Generic;

public class BeatMapSpawner : MonoBehaviour
{
    [Header("Prefab et Matériaux")]
    public GameObject cubePrefab;
    public Material blueMaterial;
    public Material redMaterial;
    
    [Header("Musique")]
    public AudioSource musicSource;
    public AudioClip musicClip;
    
    [Header("Beat Map")]
    public TextAsset beatMapFile; // Glisse ton fichier JSON ici
    
    [Header("Réglages de position")]
    public float xSpacing = 0.8f;  // Espacement horizontal entre les colonnes
    public float ySpacing = 0.5f;  // Espacement vertical entre les lignes
    public float spawnDistance = -20f; // Distance de spawn devant le joueur (Z négatif)
    
    [Header("Timing")]
    public float noteSpeed = 10f;       // Vitesse des cubes
    
    // Temps de trajet calculé automatiquement (distance / vitesse)
    private float travelTime;
    
    private BeatMap beatMap;
    private List<NoteData> notesToSpawn;
    private int nextNoteIndex = 0;
    private bool isPlaying = false;
    
    // Position de base (centre de la grille)
    private Vector3 basePosition;
    
    private bool hasStarted = false;
    
    void Start()
    {
        basePosition = transform.position;
        
        // Reset le score au début de la chanson
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        // Calculer le temps de trajet : distance / vitesse
        // Le cube spawn à spawnDistance (négatif) et doit arriver à z=0
        travelTime = Mathf.Abs(spawnDistance) / noteSpeed;
        Debug.Log($"Temps de trajet calculé: {travelTime} secondes");
        
        LoadBeatMap();
        
        // Démarrer automatiquement après 2 secondes
        if (!hasStarted)
        {
            hasStarted = true;
            Invoke("StartSong", 2f);
        }
    }
    
    void LoadBeatMap()
    {
        // Charger le fichier beatmap depuis Resources en utilisant le nom sélectionné
        string beatMapFileName = GameManager.SelectedBeatMapFile;
        Debug.Log($"BeatMapSpawner: Tentative de chargement de '{beatMapFileName}'");
        
        TextAsset loadedBeatMap = Resources.Load<TextAsset>(beatMapFileName);
        
        if (loadedBeatMap == null)
        {
            Debug.LogWarning($"BeatMapSpawner: '{beatMapFileName}' non trouvé dans Resources, utilisation du fallback");
            // Fallback sur le fichier assigné dans l'Inspector
            if (beatMapFile != null)
            {
                loadedBeatMap = beatMapFile;
            }
            else
            {
                Debug.LogError($"Impossible de charger le beatmap: {beatMapFileName}");
                return;
            }
        }
        else
        {
            Debug.Log($"BeatMapSpawner: '{beatMapFileName}' chargé avec succès");
        }
        
        beatMap = JsonUtility.FromJson<BeatMap>(loadedBeatMap.text);
        
        // Charger la musique depuis Resources
        string musicFileName = GameManager.SelectedMusicFile;
        AudioClip loadedMusic = Resources.Load<AudioClip>(musicFileName);
        
        if (loadedMusic != null)
        {
            musicClip = loadedMusic;
            if (musicSource != null)
            {
                musicSource.clip = musicClip;
            }
        }
        
        if (beatMap != null && beatMap.notes != null)
        {
            Debug.Log($"BPM lu: {beatMap.bpm}");
            
            notesToSpawn = new List<NoteData>(beatMap.notes);
            notesToSpawn.Sort((a, b) => a.beat.CompareTo(b.beat));
            
            Debug.Log($"BeatMap chargée: {beatMap.songName}, {notesToSpawn.Count} notes, BPM: {beatMap.bpm}");
        }
    }
    
    // Appelle cette fonction pour démarrer la chanson
    public void StartSong()
    {
        if (beatMap == null)
        {
            Debug.LogError("Pas de BeatMap chargée !");
            return;
        }
        
        nextNoteIndex = 0;
        isPlaying = true;
        
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        
        Debug.Log("Chanson démarrée !");
    }
    
    void Update()
    {
        if (!isPlaying || notesToSpawn == null) return;
        
        float currentTime = musicSource != null ? musicSource.time : Time.time;
        
        // Spawner les notes qui arrivent
        while (nextNoteIndex < notesToSpawn.Count)
        {
            NoteData note = notesToSpawn[nextNoteIndex];
            
            // Convertir le beat en secondes
            float noteTimeInSeconds = beatMap.BeatToSeconds(note.beat);
            
            // On spawn la note pour qu'elle arrive exactement au bon moment
            // spawnTime = quand la note doit arriver - temps de trajet
            float spawnTime = noteTimeInSeconds - travelTime;
            
            if (currentTime >= spawnTime)
            {
                SpawnNote(note);
                nextNoteIndex++;
            }
            else
            {
                break;
            }
        }
        
        // Fin de la chanson
        if (nextNoteIndex >= notesToSpawn.Count && musicSource != null && !musicSource.isPlaying)
        {
            isPlaying = false;
            Debug.Log("Chanson terminée ! Chargement de l'écran de résultats...");
            
            // Attendre 2 secondes avant d'afficher les résultats
            Invoke("LoadResultScene", 2f);
        }
    }
    
    void LoadResultScene()
    {
        // Sauvegarder les scores avant de changer de scène
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SaveForResults();
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
    }
    
    void SpawnNote(NoteData note)
    {
        // Calculer la position basée sur x et y (grille 4x3)
        // x: 0-3 (gauche à droite), y: 0-2 (bas à haut)
        // On inverse X car les cubes vont vers le joueur (axe Z inversé)
        float xPos = basePosition.x - (note.x - 1.5f) * xSpacing;
        float yPos = basePosition.y + (note.y - 1f) * ySpacing;
        
        Vector3 spawnPosition = new Vector3(xPos, yPos, spawnDistance);
        
        // Direction
        CutDirection direction = ParseDirection(note.direction);
        Quaternion rotation = GetRotationForDirection(direction);
        
        // Couleur
        NoteColor color = note.color == "red" ? NoteColor.Red : NoteColor.Blue;
        
        // Instancier
        GameObject cube = Instantiate(cubePrefab, spawnPosition, rotation);
        
        // Configurer NoteHit
        NoteHit noteHit = cube.GetComponent<NoteHit>();
        if (noteHit != null)
        {
            noteHit.requiredDirection = direction;
            noteHit.noteColor = color;
        }
        
        // Configurer NoteMover avec la bonne vitesse
        NoteMover mover = cube.GetComponent<NoteMover>();
        if (mover != null)
        {
            mover.speed = noteSpeed;
        }
        
        // Appliquer le matériau
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = color == NoteColor.Blue ? blueMaterial : redMaterial;
        }
        
        Debug.Log($"Note spawnée: {color} {direction} au beat {note.beat}");
    }
    
    CutDirection ParseDirection(string dir)
    {
        switch (dir.ToLower())
        {
            case "up": return CutDirection.Up;
            case "down": return CutDirection.Down;
            case "left": return CutDirection.Left;
            case "right": return CutDirection.Right;
            default: return CutDirection.Down;
        }
    }
    
    Quaternion GetRotationForDirection(CutDirection direction)
    {
        float zRotation = 0f;
        
        switch (direction)
        {
            case CutDirection.Down: zRotation = 0f; break;
            case CutDirection.Up: zRotation = 180f; break;
            case CutDirection.Left: zRotation = -90f; break;
            case CutDirection.Right: zRotation = 90f; break;
        }
        
        return Quaternion.Euler(0, 0, zRotation);
    }
}

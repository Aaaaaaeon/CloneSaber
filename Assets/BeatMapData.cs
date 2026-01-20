[System.Serializable]
public class NoteData
{
    public float beat;       // Numéro du beat (1, 1.5, 2, 2.25, etc.)
    public int x;            // Position X (0 = gauche, 1 = centre-gauche, 2 = centre-droit, 3 = droite)
    public int y;            // Position Y (0 = bas, 1 = milieu, 2 = haut)
    public string color;     // "blue" ou "red"
    public string direction; // "up", "down", "left", "right"
}

[System.Serializable]
public class BeatMap
{
    public string songName;
    public float bpm;           // Beats per minute
    public float startOffset;   // Décalage en secondes avant le premier beat (optionnel)
    public NoteData[] notes;
    
    // Convertir un beat en secondes
    public float BeatToSeconds(float beat)
    {
        float secondsPerBeat = 60f / bpm;
        return startOffset + (beat * secondsPerBeat);
    }
}

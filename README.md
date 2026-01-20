# Clone Saber

A VR rhythm game inspired by Beat Saber, built for Meta Quest 3 using Unity.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Creating Custom Beatmaps](#creating-custom-beatmaps)
- [Configuration](#configuration)
- [Technical Documentation](#technical-documentation)

---

## Overview

Clone Saber is a fully functional VR rhythm game where players slice colored cubes to the beat of music using virtual sabers. The game includes a menu system, gameplay mechanics, scoring, and results display.

### Requirements

- Unity 2022.3 LTS or later
- Meta Quest 3 headset
- Oculus Integration SDK
- Android Build Support module
---

## Preview
https://www.youtube.com/watch?v=HXY6VPtsjCA

## Features

### Gameplay
- Dual-wield sabers (blue for left hand, red for right hand)
- Directional cutting system (notes must be cut in the indicated direction)
- PERFECT and GOOD cut detection based on swing angle
- Haptic feedback on successful cuts and saber collisions
- Visual trail effects on sabers

### Scoring System
- PERFECT cuts: 100% points (swing angle within 15 degrees of target)
- GOOD cuts: 70% points (swing angle within 45 degrees of target)
- Combo multiplier system
- Miss tracking
- Accuracy calculation and rank assignment (S, A, B, C, D)

### User Interface
- Main menu with song selection
- In-game score and combo display
- Song progress bar with color gradient
- Results screen with detailed statistics
- VR-compatible button interaction system

### Audio
- Per-note hit sounds
- Music synchronization with beatmap
- Fireworks sound on results screen

---

## Installation

### Building for Quest 3

1. Open the project in Unity
2. Go to File > Build Settings
3. Select Android platform
4. In Player Settings:
   - Set Scripting Backend to IL2CPP
   - Enable ARM64 architecture
5. In XR Plug-in Management, enable Oculus
6. Connect Quest 3 via USB and enable USB debugging
7. Click Build And Run

### Editor Testing

The game can be tested in the Unity Editor using Quest Link or through the XR Device Simulator.

---

## Project Structure

```
Assets/
├── Resources/
│   ├── beatmap_badapple.json    # Main song beatmap
│   ├── beatmap_tuto.json        # Tutorial beatmap
│   ├── Bad Apple.mp3            # Main song audio
│   └── Tuto.wav                 # Tutorial audio
├── Prefabs/
│   └── cubePrefab               # Note cube prefab
├── Materials/
│   ├── bleu                     # Blue saber/note material
│   └── rouge                    # Red saber/note material
└── Scripts/
    ├── BeatMapSpawner.cs        # Beatmap loading and note spawning
    ├── GameManager.cs           # Scene management and song selection
    ├── ScoreManager.cs          # Score tracking and statistics
    ├── NoteHit.cs               # Note collision and cut detection
    ├── SaberColor.cs            # Saber identification
    ├── SaberVelocityTracker.cs  # Swing velocity tracking
    ├── VRMenuInput.cs           # VR button interaction
    ├── SongButton.cs            # Song selection buttons
    ├── ResultScreenUI.cs        # Results display
    ├── MissIndicator.cs         # Hit feedback display
    ├── HapticManager.cs         # Controller vibration
    ├── SaberCollision.cs        # Saber-to-saber collision
    ├── PlayfieldAligner.cs      # VR playfield positioning
    ├── SongProgressBar.cs       # Song progress display
    └── HitSoundManager.cs       # Hit audio playback
```

### Scenes

- **MenuScene**: Main menu with song selection
- **GameScene**: Core gameplay
- **ResultScene**: Post-game statistics display

---

## Creating Custom Beatmaps

### Beatmap JSON Format

```json
{
    "songName": "Song Title",
    "bpm": 128,
    "startOffset": 0,
    "notes": [
        { "beat": 4, "x": 1, "y": 1, "color": "blue", "direction": "down" },
        { "beat": 6, "x": 2, "y": 1, "color": "red", "direction": "up" }
    ]
}
```

### Fields

| Field | Description |
|-------|-------------|
| songName | Display name of the song |
| bpm | Beats per minute of the track |
| startOffset | Delay in seconds before first note |
| notes | Array of note objects |

### Note Properties

| Property | Values | Description |
|----------|--------|-------------|
| beat | Number | When the note appears (in beats) |
| x | 0-3 | Horizontal position (0=left, 3=right) |
| y | 0-2 | Vertical position (0=bottom, 2=top) |
| color | "blue" or "red" | Which saber should hit this note |
| direction | "up", "down", "left", "right" | Required swing direction |

### Position Grid

```
     x=0    x=1    x=2    x=3
y=2  [  ]   [  ]   [  ]   [  ]   Top
y=1  [  ]   [  ]   [  ]   [  ]   Middle
y=0  [  ]   [  ]   [  ]   [  ]   Bottom
     Left  Center        Right
```

### Adding a New Song

1. Create a beatmap JSON file in `Assets/Resources/`
2. Add the audio file to `Assets/Resources/`
3. In MenuScene, duplicate an existing song button
4. Add `SongButton` component with the new file names

---

## Configuration

### BeatMapSpawner Settings

| Parameter | Default | Description |
|-----------|---------|-------------|
| xSpacing | 0.8 | Horizontal spacing between note columns |
| ySpacing | 0.5 | Vertical spacing between note rows |
| spawnDistance | -20 | Distance from player where notes spawn |
| noteSpeed | 10 | Speed at which notes approach player |

### ScoreManager Settings

| Parameter | Default | Description |
|-----------|---------|-------------|
| pointsPerGoodCut | 100 | Base points for a successful cut |
| comboBonus | 10 | Additional points per combo level |

### NoteHit Settings

| Parameter | Default | Description |
|-----------|---------|-------------|
| angleTolerance | 45 | Maximum angle for GOOD cut detection |
| perfectAngle | 15 | Maximum angle for PERFECT cut detection |
| minCutVelocity | 2 | Minimum saber speed for valid cut |

---

## Technical Documentation

### Scoring Algorithm

```
Points = (BasePoints + Combo * ComboBonus) * Multiplier

Where:
- BasePoints = 100
- ComboBonus = 10
- Multiplier = 1.0 for PERFECT, 0.7 for GOOD
```

### Cut Detection

1. Note detects collision with saber
2. Saber color is validated against note color
3. Swing velocity is checked against minimum threshold
4. Swing direction is compared to required direction
5. Angle determines PERFECT (<=15 deg) or GOOD (<=45 deg)

### VR Input System

The `VRMenuInput` component handles button interaction:
- Raycast from controller to detect UI elements
- Trigger press or A/X button activates clicked button
- `SongButton` components handle song selection

### Scene Flow

```
MenuScene -> GameScene -> ResultScene -> MenuScene
                |
                v
            (Retry) -> GameScene
```

---

## License

This project is for educational purposes.

## Credits

- Built with Unity and Meta XR SDK
- Inspired by Beat Saber by Beat Games

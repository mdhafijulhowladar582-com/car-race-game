# Car Race Game

A Unity + C# 3D Android racing game project.

## V1 Goals

- 3D car
- Road / track
- Mobile touch controls
- Follow camera
- Coins
- Obstacles
- Start / finish
- Health and game over
- Android APK build
- GitHub source control

## Tech Stack

- Unity
- C#
- Android

## Project Status

✅ Core gameplay, audio, 3D car visuals, bends/ramps, and Android optimization are implemented.

## Final Test Checklist

Before the final APK release, test the following in a real Android build:

- Car acceleration and steering
- Touch controls and sensitivity
- Health and collision damage
- Game Over flow
- Start and finish flow
- Audio playback
- Bends and ramps
- Coins and obstacles
- HUD and restart flow
- FPS stability and overheating
- Landscape orientation
- APK install and launch

## Core Scripts

- `CarController.cs` — forward movement and steering
- `MobileInput.cs` — touch swipe steering with keyboard fallback
- `CameraFollow.cs` — third-person follow camera
- `Coin.cs` — collectible coins
- `Obstacle.cs` — collision damage
- `PlayerHealth.cs` — health and game-over trigger
- `FinishLine.cs` — race completion trigger
- `GameManager.cs` — coins, health, race state, restart
- `AudioManager.cs` — engine and game audio
- `CarVisualBuilder.cs` — procedural 3D car visuals
- `TrackFeatureBuilder.cs` — generated bends, ramps, barriers, and road
- `AndroidOptimization.cs` — mobile frame-rate, physics, and Android performance settings

## Unity Setup

Create a Unity 3D project and place the scripts in `Assets/Scripts/`. Then create the Player, Track, Camera, Coins, Obstacles, Finish Line, and Game Manager objects and connect the script references in the Inspector.

## Build the V1 Race Scene

After opening this project in Unity:

1. Let Unity import the scripts.
2. Open **Car Race > V1 > Build Race Scene** from the Unity Editor menu.
3. Unity will generate the V1 race scene automatically.
4. The scene will be saved as `Assets/Scenes/Race.unity`.
5. Press Play to test the car, steering, coins, obstacles, health, and finish line.

The builder creates the scene from Unity primitives, so no external 3D assets are required for the first prototype.

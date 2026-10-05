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

🚧 Development started — V1 gameplay systems are being built.

## Important

This is an original game project. Game assets, branding, tracks, and gameplay elements will be developed independently.


## V1 Core Scripts

The first gameplay systems are now implemented:

- `CarController.cs` — forward movement and steering
- `MobileInput.cs` — touch swipe steering with keyboard fallback
- `CameraFollow.cs` — third-person follow camera
- `Coin.cs` — collectible coins
- `Obstacle.cs` — collision damage
- `PlayerHealth.cs` — health and game-over trigger
- `FinishLine.cs` — race completion trigger
- `GameManager.cs` — coins, health, race state, restart

## Unity Setup Next

Create a Unity 3D project and place the scripts in `Assets/Scripts/`. Then create the Player, Track, Camera, Coins, Obstacles, Finish Line, and Game Manager objects and connect the script references in the Inspector.

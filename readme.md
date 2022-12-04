Propulsion Gel Studios
Daniel Harris (daniel@gatech.edu), Paul Bockelmann (pbockelmann3@gatech.edu), Itai Yeshrun (@gatech.edu), Oluwasegun Famisa (ofamisa3@gatech.edu)

Game: Disintegrant AI
 

**Start scene file:** MainMenu.unity in Assets/Scenes


### How to play and what parts of level to observe technology requirements

Upon starting our game "Integrant AI", the player is met with a menu screen from which a new game can be started. The game menu presents options to the player including starting a tutorial, or starting a quick action game session.

The player character then starts at the main control room and is immediately thrown into action. There are a number of enemies already spawned randomly around the entire map as well as certain health and weapon pick-ups. 

The player can move around the environment using "WASD" and mouse where W is forward, S is backward, A/D are striving left and right and the mouse is used for aiming. Upon hitting the left mouse button, the player may shoot the selected weapon. When using the mouse wheel, the player may switch weapons that he/she has collected throughout the map. When ESC is hit, the main menu is opened and the game stops. The player may choose to restart, end or resume the game from this menu by left mouse button clicking on the respective buttons.

The objective of the game is to kill all enemies that are spawned on the map using the available weapons. All map areas can be accessed from the starting point through doors without any "loading" of separate levels. 

Furthermore, we comment on the different aspects of our game design as required by the project structure:  

- Tutorial
We added a tutorial section with a story line to give background to the game and add interesting elements. The tutorial walks the player through the controls, how to aim, collect items and switch weapons.

- 3D game feel game
The clear goal of the game is to kill all enemies and be the last man standing. When the player shoots at enemies, their health declines (see their health bar above their heads) and collapse after their complete health is drained. The menu of the game (ESC) can be used to pause/resume and reset or abort a game.

Test: Health-bars, audio, player HUD, kill an NPC, go to the menu (ESC)

- Fun Gameplay aspects
We aim to deliver a fast-paced, engaging 3D 3rd person shooter game with an easy-to-grasp game mechanic in a world that provides completely different flavors and effects. In visually engaging manner, you can use any of the 4 kinds of weapons to engage in combat. The player can choose to attack directly, collect better weapons, search for health or directly flee in order to prevent certain death. Your own character's state and that of the enemies are gently integrated into the game to communicate the current success to the player all while preserving the most real estate to the detailed game world and the shoot-outs. We improved the difficulty levels in the game by managing the abundance of the weaponry and health kits available for collection so that the game is not boring/easy. We also added a minimap/radar for the player to have a good idea of where the enemies are located.

Test: Simultaneously run, aim and shoot with different weapons; visit all game areas for their different "feel"

- 3D Character with real-time control
Our game is mainly built around shooting/moving through a multi-facetted 3D world with lots of enemies. Thus, we took careful design of the moving, shooting gun switching and death animations (rag doll). We tested several different control mechanics with fixed and movable camera and settled with a distant "shoulder view" of the player to preserve the most responsive gameplay with continuous and fine-tuned control input via simultaneously controlling keyboard (main character movement) and mouse (aiming/shooting/gun switching). Animations are fluently blended between the different movements that preserve the continuity of movement, including root motions and several usages of IK. Camera is smoothly following the player and auditory feedback is given for all actions (e.g. footsteps/shots/environment) 

Test: Pay attention to the character control with keyboard/mouse; View the animation blending between different character actions; Listen to different sounds associated with action in the world (can you spot the character that listens to music? ;) )


- 3D World with Physics and Spatial Simulation
We completely designed our own different levels (with different styles) by making use of assets from the unity asset store, light sources, particle systems and sounds. Interaction with the world (e.g. shooting or moving) is based on physical principles, e.g. the player cannot move through obstacles and bullets that hit the player or the NPC hurt the characters. We also made use of lights (point lights) and particle systems, e.g. for electrical sparks that responds to the physical settings of the world (e.g. sparks drop to the ground and get reflected). Doors slide open when the player approaches.

Test: Move through the world and see that player cannot move through walls/enemies but doors slide open; Observe light sources/sparks and also auditory output due to footsteps and shooting for example


- Real-time NPC steering behaviors/AI
We implemented our enemies as NavMeshAgents with different states: Idle/wander, chase, attack and heal. The AI agents also collect weapons and collectables. While idle/wandering is just moving to random waypoints around the viscinity of their current location, chase and attack are triggered when they are within a certain range of the player. They then target the player to run towards them or shoot at them, both dynamically as the player is moving. When the NPC's health drop below a certain point they flee from the attacking player and search for health. All animations/motions are the same as for the player character. They can also move through doors to chase/attack the player. We added some voices to communicate the state of the AI, to improve the general game feel and interaction. So for example, when the AI agent says something like "somebody find me a weapon", the player knows the AI agent is looking for a weapon.

Test: Observe what the NPCs are doing/fight against them; observe which state they are currently in (printed above their head for debugging purposes)
 

- Polish
We implemented a start menu GUI that fits the style of our game and reacts to different actions with sound. It also can be used as an in-game pause menu (hit ESC) for restarting, resuming and aborting a game. The AI states will not be visible in the end game, but we keep it in for the Alpha version, as it is a way for us to debug and for the TA to observe the different AI states. Currently, the player character cannot die (allows for continuous testing currently) but of course that will be not case for the final game. We implemented the actions to show effects in the environment (e.g. footsteps, bullet trails, shooting sounds, sliding doors and a "jump pad"). As part of the polish, we also added some objects that can be destroyed when shot at. The entire world is designed consistently although we have different map areas that provide a different "feel" of the environment. Each room area has a different pace and style of music to switch up the mood.

Test: Move through the world, visit the different map areas; Hear shooting/stepping sounds; Use machine gun for shooting (note rattle, muzzle flash)

Cheats/Tricks: (1) Press "U" key to logically "kill" all the bots and see the game win message. (2) Walk over the red disk near the blue cannister in the main room to leech your health and test teh death/game over message. (3) Use the shotgun for near instant kill. 

### Known problem areas

- Possibility of unused resources/assets existing in the project. We encountered multiple compile issues when we followed the introductions here - https://gatech.instructure.com/courses/267094/pages/assignment-packaging-and-submission
- Occassionally, the player walks through some walls.
- If not properly aimed, the bazooka could harm the player who shot it, resulting in some unexpected loss of health.



### Manifest of which files authored per teammate:

| File                                                     | Authors                        |
| -------------------------------------------------------- | ------------------------------ |
| Scenes/Game                                              | Paul, Oluwasegun, Itai, Daniel |
| Scenes/MainMenu                                          | Daniel, Oluwasegun             |
| Scenes/Intro_Exterior (cutscene)                         | Paul             |
| Scenes/Intro_Interior (cutscene)                         | Paul             |
| Scenes/Final (cutscene)                                  | Paul             |
| Scenes/Tutorial                                          | Daniel             |
| ScenePrefabs/Game/Command & Control Room                 | Itai                           |
| ScenePrefabs/Game/Medical Bay                            | Oluwasegun                     |
| ScenePrefabs/Game/Hanger                                 | Daniel                         |
| ScenePrefabs/Game/Engine Room                            | Paul                           |
| ScenePrefabs/Game/PauseMenu                              | Daniel, Oluwasegun             |
| ScenePrefabs/Game/Game Over Menu                         | Daniel                         |
| ScenePrefabs/Game/SpawnManager                           | Daniel                         |
| Scripts/AI/CleaningBot.cs                                | Itai                           |
| Scripts/AI/EnemyAI.cs                                    | Itai, Paul                     |
| Scripts/AI/NavMeshLine.cs                                | Itai                           |
| Scripts/AI/Raycast.cs                                    | Paul                           |
| Scripts/AI/State.cs                                      | Itai, Paul                     |
| Scripts/AI/StateParams.cs                                | Itai, Paul                     |
| Scripts/Audio/AudioMixerController.cs                    | Segun                          |
| Scripts/Audio/BackgroundMusicTrigger.cs                  | Itai                           |
| Scripts/Audio/GordonVoice.asset                          | Itai                           |
| Scripts/Audio/MusicSwitch.cs                             | Itai                           |
| Scripts/Camera/AnimationEventController.cs               | Itai                           |
| Scripts/Camera/CameraController.cs                       | Daniel                         |
| Scripts/Camera/Iam_InTheWay.cs                           | Paul                           |
| Scripts/Camera/WallKiller.cs                             | Paul                           |
| Scripts/Character/CharacterAttributes.cs                 | Daniel, Paul                   |
| Scripts/Character/CharacterController.cs                 | Itai, Daniel                   |
| Scripts/Character/CharacterModifier.cs                   | Daniel                         |
| Scripts/Character/CharacterSoundEvents.cs                | Itai                           |
| Scripts/Character/Jump.cs                                | Itai                           |
| Scripts/Character/Modifiers/HealthModifier.cs            | Daniel                         |
| Scripts/Character/Modifiers/WeaponModifier.cs            | Daniel                         |
| Scripts/Collectables/BaseCollectable.cs          		   | Itai                           |
| Scripts/Collectables/HealthCollectable.cs      		   | Daniel, Itai                   |
| Scripts/Collectables/HealthLeechCollectable.cs 		   | Daniel, Itai                   |
| Scripts/Collectables/WeaponCollectable.cs                | Daniel, Itai                   |
| Scripts/Environment/CrateExplode.cs                      | Paul                           |
| Scripts/Environment/DoorScript.cs                        | Daniel                         |
| Scripts/Environment/LightFlicker.cs                      | Paul                           |
| Scripts/Events/AudioEventManager.cs                      | Oluwasegun, Paul, Daniel, Itai |
| Scripts/Events/CharacterAttributeChangeEvent.cs          | Daniel                         |
| Scripts/Events/CountDownEvent.cs                         | Itai                           |
| Scripts/Events/FootstepSoundEvent.cs                     | Itai                           |
| Scripts/Events/GameMenuBackgroundAudioEvent.cs           | Oluwasegun                     |
| Scripts/Events/GameMenueBottonAudioEvent.cs              | Oluwasegun                     |
| Scripts/Events/PlayerLandsEvent.cs                       | Daniel                         |
| Scripts/Events/VoiceEvent.cs                             | Itai                           |
| Scripts/Events/WeaponAddEvent.cs                         | Daniel                         |
| Scripts/Events/WeaponFiredEvent.cs                       | Oluwasegun, Daniel             |
| Scripts/Events/WeaponRemoveEvent.cs                      | Daniel                         |
| Scripts/Events/WeaponSwapEvent.cs                        | Oluwasegun, Daniel             |
| Scripts/Menu/MenuBackgroundAudioEventEmitter.cs          | Oluwasegun                     |
| Scripts/Menu/MenuButtonEventListener.cs                  | Oluwasegun                     |
| Scripts/UI/HealthBar/HealthBar.cs                        | Oluwasegun                     |
| Scripts/UI/HealthBar/HealthBarController.cs              | Oluwasegun                     |
| Scripts/UI/Minimap/Minimap.cs                            | Oluwasegun                     |
| Scripts/UI/Minimap/PlayerMapIndicator.cs                 | Oluwasegun                     |
| Scripts/UI/AIStateDisplay.cs                             | Oluwasegun                     |
| Scripts/UI/HUD.cs                                        | Daniel                         |
| Scripts/UI/HUDController.cs                              | Itai, Daniel                   |
| Scripts/Utility/Billboard.cs                             | Oluwasegun                     |
| Scripts/Utility/Bob.cs                                   | Daniel                         |
| Scripts/Utility/GameManager.cs                           | Itai                           |
| Scripts/Utility/GameObjectSpawner.cs                     | Daniel                         |
| Scripts/Utility/GameQuitter.cs                           | Daniel                         |
| Scripts/Utility/Rotate.cs                                | Daniel                         |
| Scripts/Utility/Scene2.cs                                | Paul                           |
| Scripts/Utility/Scene3.cs                                | Paul                           |
| Scripts/Utility/SceneLoader.cs                           | Daniel                         |
| Scripts/Utility/SoundSource.cs                           | Itai                           |
| Scripts/Utility/SunOrbit.cs                              | Itai                           |
| Scripts/Utility/VelocityReporter.cs                      | Itai                           |
| Scripts/Utility/VoicePack.cs                             | Itai                           |
| Scripts/Utility/WeaponSpawn.cs                           | Itai                           |
| Scripts/Weapons/MachineGunFire.cs                        | Paul                           |
| Scripts/Weapons/Projectile.cs                            | Daniel, Paul                   |
| Scripts/Weapons/Schockwave.cs                            | Itai                           |
| Scripts/Weapons/Weapon.cs                                | Daniel, Itai                   |
| Scripts/Weapons/WeaponController.cs                      | Itai, Daniel                   |
| Scripts/Weapons/WeaponTag.cs                             | Itai                           |
| Scripts/Weapons/Projectiles/Bullet.cs                    | Daniel                         |
| Scripts/Weapons/Projectiles/Rocket.cs                    | Itai                           |
| Scripts/Weapons/Projectiles/SpreadProjectile.cs          | Itai                           |
| Scripts/Weapons/Weapons/Pistol.cs                        | Itai                           |
| Scripts/Weapons/Weapons/Rifle.cs                         | Daniel                         |
| Scripts/Weapons/Weapons/RPG.cs                           | Itai                           |
| Scripts/Weapons/Weapons/Shotgun.cs                       | Itai                           |
| Scripts/Tutorial/TriggerLoadScene.cs                     | Daniel                         |
| Scripts/Tutorial/TutorialManager.cs                      | Daniel                         |
| Scripts/Tutorial/TutorialTrigger.cs                      | Daniel                         |


### Third Party Assets:


| Asset Name                  | Source |
| --------------------------- | -------- |
| PolygonSciFiSpace     | [https://assetstore.unity.com/packages/3d/environments/sci-fi/polygon-sci-fi-space-low-poly-3d-art-by-synty-138857](https://assetstore.unity.com/packages/3d/environments/sci-fi/polygon-sci-fi-space-low-poly-3d-art-by-synty-138857)     |
| SciFi Medical Room    | [https://assetstore.unity.com/packages/3d/environments/sci-fi/scifi-medical-room-192838](https://assetstore.unity.com/packages/3d/environments/sci-fi/scifi-medical-room-192838)     |
| SFX And Music For Games (Humble Bundle) | https://www.humblebundle.com/software/sfx-and-music-for-your-games-software |
| Orbitron Font | https://fonts.google.com/specimen/Orbitron |
| GeeKay3D First Aid Set | https://assetstore.unity.com/packages/3d/props/first-aid-set-160073 |
| Character Animations | Mixamo (Downloaded & Configured by Daniel & Itai) |
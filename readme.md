Propulsion Gel Studios
Daniel Harris (daniel@gatech.edu), Paul Bockelmann (pbockelmann3@gatech.edu), Itai Yeshrun (@gatech.edu), Oluwasegun Famisa (ofamisa3@gatech.edu)

Game: Disintegrant AI
 

**Start scene file:** MainMenu.unity in Assets/Scenes


### How to play and what parts of level to observe technology requirements

Upon starting our game "Integrant AI", the player is met with a menu screen from which a new game can be started. The player character then starts at the main control room and is immediately thrown into action. There are a number of enemies already spawned randomly around the entire map as well as certain health and weapon pick-ups. 

The player can move around the environment using "WASD" and mouse where W is forward, S is backward, A/D are striving left and right and the mouse is used for aiming. Upon hitting the left mouse button, the player may shoot the selected weapon. When using the mouse wheel, the player may switch weapons that he/she has collected throughout the map. When ESC is hit, the main menu is opened and the game stops. The player may choose to restart, end or resume the game from this menu by left mouse button clicking on the respective buttons.

The objective of the game is to kill all enemies that are spawned on the map using the available weapons. All map areas can be accessed from the starting point through doors without any "loading" of separate levels. 

Furthermore, we comment on the different aspects of our game design as required by the project structure:  

- 3D game feel game
The clear goal of the game is to kill all enemies and be the last man standing. When the player shoots at enemies, their health declines (see their health bar above their heads) and collapse after their complete health is drained. The menu of the game (ESC) can be used to pause/resume and reset or abort a game.

Test: Health-bars, player HUD, kill an NPC, go to the menu (ESC)

- Fun Gameplay aspects
We aim to deliver a fast-paced, engaging 3D 3rd person shooter game with an easy-to-grasp game mechanic in a world that provides completely different flavors and effects. In the current state we implemented aggressive enemies throughout a spaceship that you can kill with 3 different weapons in visually engaging shootouts. The player can choose to attack directly, collect better weapons, search for health or directly flee in order to prevent certain death. Your own character's state and that of the enemies are gently integrated into the game to communicate the current success to the player all while preserving the most real estate to the detailed game world and the shoot-outs. We aim to fine tune the world in order to avoid an abundance of weaponry, ammo or health to not make the game boring/easy. 

Test: Simultaneously run, aim and shoot with different weapons; visit all game areas for their different "feel"

- 3D Character with real-time control
Our game is mainly built around shooting/moving through a multi-facetted 3D world with lots of enemies. Thus, we took careful design of the moving, shooting gun switching and death animations (rag doll). We tested several different control mechanics with fixed and movable camera and settled with a distant "shoulder view" of the player to preserve the most responsive gameplay with continuous and fine-tuned control input via simultaneously controlling keyboard (main character movement) and mouse (aiming/shooting/gun switching). Animations are fluently blended between the different movements that preserve the continuity of movement, including root motions and several usages of IK. Camera is smoothly following the player and auditory feedback is given for all actions (e.g. footsteps/shots/environment) 

Test: Pay attention to the character control with keyboard/mouse; View the animation blending between different character actions; Listen to different sounds associated with action in the world (can you spot the character that listens to music? ;) )


- 3D World with Physics and Spatial Simulation
We completely designed our own different levels (with different styles) by making use of assets from the unity asset store, light sources, particle systems and sounds. Interaction with the world (e.g. shooting or moving) is based on physical principles, e.g. the player cannot move through obstacles and bullets that hit the player or the NPC hurt the characters. We also made use of lights (point lights) and particle systems, e.g. for electrical sparks that responds to the physical settings of the world (e.g. sparks drop to the ground and get reflected). Doors slide open when the player approaches.

Test: Move through the world and see that player cannot move through walls/enemies but doors slide open; Observe light sources/sparks and also auditory output due to footsteps and shooting for example


- Real-time NPC steering behaviors/AI
We implemented our enemies as NavMeshAgents with different states: Idle/wander, chase, attack and heal. We aim to also let them search for weapons when certain conditions are met. While idle/wandering is just moving to random waypoints around the viscinity of their current location, chase and attack are triggered when they are within a certain range of the player. They then target the player to run towards them or shoot at them, both dynamically as the player is moving. When the NPC's health drop below a certain point they flee from the attacking player and search for health. All animations/motions are the same as for the player character. They can also move through doors to chase/attack the player. Note that for development purposes, their current state is printed above each NPC's head close to their health bar. 

Test: Observe what the NPCs are doing/fight against them; observe which state they are currently in (printed above their head for debugging purposes)
 

- Polish
We implemented a start menu GUI that fits the style of our game and reacts to different actions with sound. It also can be used as an in-game pause menu (hit ESC) for restarting, resuming and aborting a game. The AI states will not be visible in the end game, but we keep it in for the Alpha version, as it is a way for us to debug and for the TA to observe the different AI states. Currently, the player character cannot die (allows for continuous testing currently) but of course that will be not case for the final game. We implemented the actions to show effects in the environment (e.g. footsteps, bullet trails, shooting sounds, sliding doors and a "jump pad"). Further implementation will include objects that can be destroyed. The entire world is designed consistently although we have different map areas that provide a different "feel" of the environment. 

Test: Move through the world, visit the different map areas; Hear shooting/stepping sounds; Use machine gun for shooting (note rattle, muzzle flash)

Cheats/Tricks: (1) Press "U" key to logically "kill" all the bots and see the game win message. (2) Walk over the red disk near the blue cannister in the main room to leech your health and test teh death/game over message. (3) Use the shotgun for near instant kill. 


### Known problem areas

- AI: Getting stuck in walls, not using doors reliably right now, not switching guns (use the best), not collecting guns. These issues will be addressed for the final version.

- Shooting: Hard to aim right now. Blood/sounds for when getting shot not yet implemented; No objects yet that explode/wear down when getting shot; More weapon effects when shooting

- Sound: Game music not implemented yet, some more environment sounds (e.g. sparks, engine roaring)

- Player health can currently go above max if they pick up more health collectibles. This will be fixed in the final version.  



### Manifest of which files authored per teammate:

| File                                                     | Authors                        |
| -------------------------------------------------------- | ------------------------------ |
| Scenes/Game                                              | Paul, Oluwasegun, Itai, Daniel |
| Scenes/MainMenu                                          | Daniel, Oluwasegun             |
| ScenePrefabs/Game/Command & Control Room                 | Itai                           |
| ScenePrefabs/Game/Medical Bay                            | Oluwasegun                     |
| ScenePrefabs/Game/Hanger                                 | Daniel                         |
| ScenePrefabs/Game/Engine Room                            | Paul                           |
| ScenePrefabs/Game/PauseMenu                              | Daniel, Oluwasegun             |
| ScenePrefabs/Game/Game Over Menu                         | Daniel                         |
| ScenePrefabs/Game/SpawnManager                           | Daniel                         |
| Scripts/AI/CleaningBot.cs                                | Itai                           |
| Scripts/AI/EnemyAI.cs                                    | Itai, Paul                     |
| Scripts/AI/State.cs                                      | Itai, Paul                     |
| Scripts/AI/StateParams.cs                                | Itai, Paul                     |
| Scripts/Camera/AnimationEventController.cs               | Itai                           |
| Scripts/Camera/CameraController.cs                       | Daniel                         |
| Scripts/Character/CharacterAttributes.cs                 | Daniel, Paul                   |
| Scripts/Character/CharacterController.cs                 | Itai, Daniel                   |
| Scripts/Character/CharacterModifier.cs                   | Daniel                         |
| Scripts/Character/CharacterSoundEvents.cs                | Itai                           |
| Scripts/Character/Collectables/WeaponCollectable.cs      | Daniel                         |
| Scripts/Character/Jump.cs                                | Itai                           |
| Scripts/Character/Modifiers/HealthModifier.cs            | Daniel                         |
| Scripts/Character/Modifiers/WeaponModifier.cs            | Daniel                         |
| Scripts/Character/Collectables/HealthCollectable.cs      | Daniel                         |
| Scripts/Character/Collectables/HealthLeechCollectable.cs | Daniel                         |
| Scripts/Environment/DoorScript.cs                        | Daniel                         |
| Scripts/Environment/LightFlicker.cs                      | Paul                           |
| Scripts/Events/AudioEventManager.cs                      | Oluwasegun, Paul, Daniel, Itai |
| Scripts/Events/CharacterAttributeChangeEvent.cs          | Daniel                         |
| Scripts/Events/FootstepSoundEvent.cs                     | Itai                           |
| Scripts/Events/GameMenuBackgroundAudioEvent.cs           | Oluwasegun                     |
| Scripts/Events/GameMenueBottonAudioEvent.cs              | Oluwasegun                     |
| Scripts/Events/WeaponAddEvent.cs                         | Daniel                         |
| Scripts/Events/WeaponFiredEvent.cs                       | Oluwasegun, Daniel             |
| Scripts/Events/WeaponRemoveEvent.cs                      | Daniel                         |
| Scripts/Events/WeaponSwapEvent.cs                        | Oluwasegun, Daniel             |
| Scripts/Menu/MenuBackgroundAudioEventEmitter.cs          | Oluwasegun                     |
| Scripts/Menu/MenuButtonEventListener.cs                  | Oluwasegun                     |
| Scripts/UI/HealthBar/HealthBar.cs                        | Oluwasegun                     |
| Scripts/UI/HealthBar/HealthBarController.cs              | Oluwasegun                     |
| Scripts/UI/AIStateDisplay.cs                             | Oluwasegun                     |
| Scripts/UI/HUD.cs                                        | Daniel                         |
| Scripts/Utility/Billboard.cs                             | Oluwasegun                     |
| Scripts/Utility/Bob.cs                                   | Daniel                         |
| Scripts/Utility/GameManager.cs                           | Itai                           |
| Scripts/Utility/GameObjectSpawner.cs                     | Daniel                         |
| Scripts/Utility/GameQuitter.cs                           | Daniel                         |
| Scripts/Utility/PauseMenuToggle.cs                       | Daniel                         |
| Scripts/Utility/Rotate.cs                                | Daniel                         |
| Scripts/Utility/SceneLoader.cs                           | Daniel                         |
| Scripts/Utility/SoundSource.cs                           | Itai                           |
| Scripts/Weapons/MachineGunFire.cs                        | Paul                           |
| Scripts/Weapons/Projectile.cs                            | Daniel, Paul                   |
| Scripts/Weapons/Weapon.cs                                | Daniel, Itai                   |
| Scripts/Weapons/WeaponController                         | Itai, Daniel                   |
| Scripts/Weapons/Projectiles/Bullet.cs                    | Daniel                         |
| Scripts/Weapons/Projectiles/Rocket.cs                    | Itai                           |
| Scripts/Weapons/Projectiles/SpreadProjectile.cs          | Itai                           |
| Scripts/Weapons/Weapons/Rifle.cs                         | Daniel                         |
| Scripts/Weapons/Weapons/RPG.cs                           | Itai                           |


### Third Party Assets:


| Asset Name                  | Source |
| --------------------------- | -------- |
| PolygonSciFiSpace     | [https://assetstore.unity.com/packages/3d/environments/sci-fi/polygon-sci-fi-space-low-poly-3d-art-by-synty-138857](https://assetstore.unity.com/packages/3d/environments/sci-fi/polygon-sci-fi-space-low-poly-3d-art-by-synty-138857)     |
| SciFi Medical Room    | [https://assetstore.unity.com/packages/3d/environments/sci-fi/scifi-medical-room-192838](https://assetstore.unity.com/packages/3d/environments/sci-fi/scifi-medical-room-192838)     |
| SFX And Music For Games (Humble Bundle) | https://www.humblebundle.com/software/sfx-and-music-for-your-games-software |
| Orbitron Font | https://fonts.google.com/specimen/Orbitron |
| GeeKay3D First Aid Set | https://assetstore.unity.com/packages/3d/props/first-aid-set-160073 |
| Character Animations | Mixamo (Downloaded & Configured by Daniel & Itai) |
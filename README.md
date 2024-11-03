# Ladi-Veizar-Adventure
The game looks like "The Legend of Zelda" but with different plot and character.
Ladi Veizar Adventure is a game created in Unity Engine as an individual project from CS211 class "Game Developement"

This game pays homage to classics like Zelda featuring a top-down camera perspective that aims to bring elements of exploration,
combat, power-ups, cinematics, valuable treasures, and hidden secrets within a dungeon filled with mysteries.

Player Actions:

* MovAttack enemies,
* Jump,
* Use consumables,
* Interact with nearby objects,
* Utilize the bomb tool to destroy obstacles.

Developement Process:
I watched a video online and recreate already existing game with additional features. 
Several enhancements have been introduced to enrich the game. Here's a list of what's included in this project.

Technical Features: 

* Camera system using CineMachine for top-down view.
* Character movement with custom physics.
* Support for gamepads and virtual controls.
* Ported to PC Windows, Browser (WebGL), and Android platforms.
* Localization system with two implemented languages and support for expansion: PT-BR, EN.
* Audio and graphical quality control menus.
* Use of post-processing for scene and area atmosphere.
* Customized particles for in-game effects.
* State machines for various character and creature states.

Gameplay Features:

* Combat against monsters with simple behavior scripts and NAVMESH navigation.
* Scripted final battle with unique mechanics.
* Power-ups.
* Map and minimap systems.
* Random drop system.
* Cutscenes implemented using Unity Timeline.

  Code Refinement:
  
  PlayerController Refactoring:
  The previous PlayerController script has been restructured into two separate scripts: PlayerController and PlayerAvatar.
  This division of functionality allows for clearer communication and organization within the game architecture.
  
  PlayerController: This script now handles player character interactions related to game rules and player input domains.
  It serves as the communication hub for interactions related to these aspects.

  PlayerAvatar: Responsible for managing the player avatar in the world and its physics interactions.
  it takes care of animation, rendering, physics and sounds.It serves as the communication hub for interactions related to these aspects.

  Third Party Assets

  Authors of the Third Party Assets used in this project:

* YouFulca
* CatBorg Studio
* Dungeon Mason
* Just Labbing
* Polygonal Stuff
* Kenny
* Leohpaz
* Imphenzia

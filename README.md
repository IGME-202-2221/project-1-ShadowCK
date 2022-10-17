# Project Mecha Angel

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

### Student Info

-   Name: Zhao Jin
-   Section: 5

## Game Design

-   Camera Orientation: Side View
-   Camera Movement: Fixed ~~Mixed~~
    <del>
    1. Tracking when progressing towards the level end
    2. Fixed when facing the boss
    </del>
-   Player Health: Healthbar
-   End Condition: ~~Survive enough time and win the boss fight (finale), OR~~ Lose all health
-   Scoring: Hitting / defeating enemies, let enemies escape will reduce score. ~~collecting items.~~

### Edge Interaction
_The player must have a set interaction with the edges of the screen. It is up to you what the interaction is but must be documented in your README file. Warp, Stop, and Camera Follow are just a few of the options you could choose._

I picked Wrap, because it provides more flexibility for the player's map control ability than Stop. However, stop is classic. I wanted to design linear levels with Camera Follow. However, time was limited. 

### Fire Interval
_There must be a delay between bullets fired so that each bullet is clearly distinguishable from the others. Your solution must be documented in the README file._
I made a Shooter script to store the shootTimer, shootInterval and a lot of other settings. The timer updates in Shooter's Update() method. Once it reaches shootInterval, the player is allowed to shoot again, or the enemy immediately shoots at the player or a nother direction.

### Game Description
_Note: right now there is no boss fight available_

The 19th Angel, Future has come and is dealing threat. Unlike all angels before, it can spawn numerous minions to spread horror. The more fear it collects, the more powerful Future and all its family become. ~~Your mecha can switch between two specialized modes: air and ground to handle different types of enemies. However, the air mode is more powerful and thus requires energy cores to maintain. That means you must fight with Future in the air once you reach it.~~

Be brave, my soldier! The whole life of mankind relies on you.

### Controls

The player can move around to dodge enemy fire ~~and/or collect items~~, shoot bullets to neutralize enemies.
-   Movement
    -   Up ~~/ Jump~~: W
    -   Down: S
    -   Left: A
    -   Right: D
-   Fire: Left Click ~~/ Auto~~

### Inspirations
Great idea, but I didn't manage to implement the two modes.

_The background story is based on Neon Genesis Evangelion_

_The gameplay was inspired by Viking from StarCraft II, a unit which can switch from fighter (air) mode and assault (ground) mode._

- Since we're going to make a shmup game, why not combine the conventional side scrollers of an aircraft and of a gunner?

### Enemy Design
A short description of each enemy type must be documented in your README file.
- Scout: The minion. Common, squishy. The only virtue of it is its moderate speed.
- Castle: Shoots tracking missles, slow but tanky.
- Destroyer: Shoots a volley of bullets, can be a threat in close range.
- Venom: Extremly fast. Splits into two smaller ones on death and becomes faster.
- Voodoo: Rotates and shoots rapidly, spreading bullets all over the space.
- Meteors: They're actually neutral. They block bullets. Sometimes is a good shield; sometimes it's a burden.

## You Additions
1. Scrolling background. The scrolling speed is higher when the player is at lower health.
2. Animated health bar. (Dynamic sliders) Has a MOBA-like presentation of health decay and regeneration.
3. Five distinct enemy types.
4. Procedural EnemySpawner system. Can work with preset spawn points in the scene. Uses non-uniform randomness and weights instead of 0~100%
5. Two different bullets: Bullet and Missile. Missiles can track a target. They function the same as bullets without a target. Bullets can destroy other bullets, depending on which is stronger (more health).
6. Multiple bullets per shot with a spread angle.
7. Player level-ups. Player is stronger with more score, e.g. shoots more bullets or shoots faster.
8. Two difficulties. Accessible on the Replay UI after the player dies. (Just drops the timeScale)

_List out what you added to your game to make it different for you_

### Formerly Planned Gameplay (Unimplemented)
~~- Two combat modes: air and ground.~~
  1. The player can shoot at a free angle on the ground while it becomes classic side view Shmup in the air.
  2. The player need to collect energy cores to switch to the air mode. There are also obstacles on the ground.
  3. Score is doubled in the air mode. The player's damage is also buffed. However it has limited time.
- Enemies are not required to be killed but letting them get away will increase the difficulty.


## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _If an asset is from the Unity store, include a link to the page and the author’s name_
1. Kenney® - Space Shooter Extension
https://www.kenney.nl/assets/space-shooter-extension
2. DinV Studio - Dynamic Space Background Lite
https://assetstore.unity.com/packages/2d/textures-materials/dynamic-space-background-lite-104606 
3. Test Health Bar Assets from Natty Creations - How to make a Better Health Bar in Unity : Chip Away Tutorial
https://www.youtube.com/watch?v=CFASjEuhyf4&ab_channel=NattyCreations
http://www.mediafire.com/file/cl63d6ydeyp8ii5/HealthBarAssets.zip/file

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

All bugs found are fixed. The game is in a comparatively stable state.

### Requirements not completed

_If you did not complete a project requirement, notate that here_

Not really.


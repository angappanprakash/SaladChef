SaladChef

Used:Unity 2017.3.1f1 (64-bit)

Controls:
Player 1
Movement - ASWD
Interaction/Accept - Left Control Key
Player 2
Movement - Arrow Keys
Interaction/Accept - Right Control Key

NOTE:
Every interactable objects will show up or move a white colored tray when player collides with it, press interaction key to process or accept.

Features Modified:
1. Added common game timer

Features Completed:
1. Player
2. Vegetables
3. Salad
4. Chopping Board

Work in progress:
1. Two More Power ups
2. More salad types/More combinations
3. Spawning every object using spawners and scritable object data
4. UI Manager
5. Score Manager
6. Chopping board - Show the vegetables on board and their status

Place holders:
Entire Art required for the game.

Values to Tweak:
1. Chopping time for each vegetable specified in their prefabs
2. Power ups - Life time and bonus score/value specified in their prefabds
3. Game Time specified in game managed in start up scene

Features Missing:
1. Timer/Draining bar for chopping board for showig up the duration.
2. Customer behaviour
3. Chopping board timer
4. Next to chopping board
5. Timer for players
6. Trash can

Bugs to Fix:
1. Random crash 1 out of 20 times
2. Pop up on[attached/picked vegetable notification] top of the players has to be fixed

Things need to done:
1. Importing place holder art assets - 3d/2d from unity assets.
2. Texture packer for after adding more textures. 
3. Every vegetable/salad/power up class can be overridden on their child classes for their specific behaviour.

Future implementaions can be:
1. Different Game modes - Team based, Free for all.
2. Different levels/maps.

Code Design:
1. Its a mixture of data driven and state base and extendable classes.
2. Singleton classes - All manager class, such as game manager, Level manager, Audio manager and etc.
3. Generic Game Event System - To avoid having delegats all over, to keep everything in a place and clean.
4. Base classes with virtual functions - To override specific behaviours in child class, such as vegetable, salad and power up
5. States - Game states and Player states and can be added to every other objects


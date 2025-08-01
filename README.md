Game Title: Final Sector
By: Ethan Tran

The files in this repository contain the source code for the game that I created, Risky Racers, for the Roosevelt Racers' "Roosevelt Dev Prix". 
To use the source code for either modifications or running the game, you must first have Unity Hub installed as this project was made with Unity.
You must create a git clone of this repository before going to the Unity Hub clicking on the arrow next to the ADD button and click "Add Project From Disk".
Then, select the git clone and you can open the game in the editor. Here, you can modify aspects of the game to your liking. If you wish to play the game
outside of the editor, you can select Build and Run and choose your device type (Make sure to have the build settings for your device downloaded). This will
create an application which you can play.

The leaderboard system made by Danqzq contains a public key and secret key for each leaderboard. If you wish to modify the game you will need the create your
own leaderboards on the danqzq leaderboard site and input your own public and secret keys for the leaderboards you create.

Controls:
- Movement: WASD or Arrow Keys
- Braking: Space Bar
- Go to last checkpoint: R
- Settings: Escape

Game Objective:
You must drive an f1 car through three laps of five different circuits. You must pass through all checkpoints before completing each lap and your goal is
to complete the entire race as fast as possible. Make sure to keep track of your tire durability and tire compound to complete the objective. If your tire
durability gets low or reaches zero, stop at the pitstop near the start of the circuit to change tires (make sure to slow your car down to a stop).

Areas for improvement:
- More F1 components
- Even better physics system
- More levels!
- More obstacles
- More customization
- More songs

Bug:
On Circuit 5, the left and right buttons of the pit stop menu do not work for tire compound selection. To fix this bug, you need to enter into the editor go
to the 5th circuit scene and select the left button which can be found inside of the pitStopCanvas. Scroll to the bottom and in the onClick section, drag the
Canvas item onto the gameObject area and select UIManager and the function leftButton. Do the same exact process for the right button to fix it as well.

I had a really great time making this game and I hope that you all enjoy ;)!

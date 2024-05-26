# Wuthering Waves Uncapper

This program modifies the LocalStorage.db in the game, and changes the JSON value for the framerate to 120.

## Prerequisites

- Game should be close and not running before the modifications take effect
- Verify the `LocalStorage.db` path matches your game installation directory.
- Vsync should be set off, and FPS Limit in game is set to 60.

## Disclaimers

- For people who would ask if this is going to get them banned, IT WILL NOT. The program simply edits a database file using JSON parser and updates the values inside that database
- 120 FPS isn't fully supported by the devs, as it is not yet officially implemented. Expect odd game behaviour.
- This does not give you free performance, if the game is already struggling with your hardware this doesn't do jack shit.
- With the current state of the game (poor optimizations), this program is may or may NOT improved your FPS beyond 60. At certain places in my hardware I do dipped below 80.

## How to Download

- Head over the releases page and download the .zip
- Read the instructions inside the zip folder.

## FAQ

- If you're experiencing an error saying 'Disk I/O', simply run the program in admnistrator.
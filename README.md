# UnityPlatformer

Platformer developed in Unity engine. Loosely based on Sebastian Lague's excellent 2D raycast platformer video series. 

## Big Goals
1. I think it's probably worth while to try building a few fun, playable levels with this project. I want to recreate a Mario level, Megaman level and maybe an Adventure of Link level using the same basic scripts. The more re-usable the code, the happier I'll be.

2. I want to figure out how to make a platformer playable on Android. Going to investigate compiling Unity apps on Android and then see about implementing basic touch controls.

### Itty Bitty Goals
1. Nice smooth slope handling. It's non existant right now.
2. Falling animations.
3. Enemies that hurt the player. There's a little dot that lowers an HP stat but I think I want to replace that entirely so I can (ideally) remove the GetComponents on collision. I want the GameManager to hold player vitals.
4. Attacking. 

### Done So Far
Players can walk, run, jump and fall. Slopes not yet handled. Players collide into things pretty well though.

Walking and running are animated. There's an idle pose.



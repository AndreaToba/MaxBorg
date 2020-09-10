# MaxBorg
A 2D game where you shoot down enemy space ships using various weapons.

This was a project created in 2019 using C# and Visual Code 2015, and is played using an Xbox game controller. Project goals 
included using technologies learned in my video game programming class to create a working game.

## How the Game Works
Enemy space ships appear randomly and shoot at the player, who must use their own ship, located in the center of the screen, to defend themselves
and shoot back at the enemies. 

The player's ship has four turrets, but can only shoot from one turret at a time. The selected turret is highlighted in green, and
the player can switch between turrets using the directional pad. The left trigger is used to shoot missiles and the right trigger
shoots different types of phasors.
Four phasors types exist:
* Yellow light
* Red light
* Green light
* Blue light

The player can switch between the selected phasor types by using the Y, B, A, and X buttons, respectively.

Each weapon uses up a different amount of LSU. An LSU bar is located in the top right corner of the screen, and LSU automatically
replenishes over time. A counter shown on the player's ship also indicates the time remaining until the LSU reaches max capacity once again.
If the LSU reaches zero, the turrets turn red and the player must wait until their LSU reaches sufficient levels. Phasors tend to 
take up more LSU than missiles.

To make it more interesting, this is where the last element of the game comes in. There are two separate bars in the top left corner for 
Explosive and Propulsive MJ. Before firing a weapon, the player can increase/decrease both MJs using the left thumb stick.
Moving up and down changes the Explosive MJ and moving left and right changes the Propulsive MJ.

The Explosive MJ controls the strength of the weapons. A greater Explosive MJ will make the missiles bigger and will ensure 
that there are larger explosions. 

The Propulsive MJ controls the distance of the weapons. A greater Propulsive MJ will allow the weapons to travel further
before disappearing.

While increasing the MJs leads to a more powerful effect, it also takes up more LSU. The player must strategically use their
weapons and balance the need for strength, distance, and speed.

The game also includes various sound effects for both the player and enemy ships.


This system is a total rework of Dupre's Token System, so all credit goes to him for the original script. 
All the new scripting was done by Karmageddon.


Things included with this package:
Token Box
Tokens

To install this package just drop all files into your scripts folder.

To add the Token Box just do [add TokenBox. 

Each token box can only be used by it's owner. If you are adding this system into an established server Staff will need to set owners on each of the boxes to each player. 
Also you can have the box added to a new character's backpack and have it set the player as the owner by adding this line into your CharacterCreation.cs: PackItem( new TokenBox( m ) );
This will need to be added in where it is filling the player's backpack.

Then to have the tokens be given to players when they kill a monster you will need to add this line in your BaseCreature.cs right under the lines for awarding fame and karma:
TokenValidate.TokenAward(ds.m_Mobile,this);

Then you are all set. 

Update: 
Package now includes Token Checks. Which players can only add to their Token boxes.
Also you no longer have to have staff set the owner of an unowned token box. It will do it automatically to the first player to open it. 
I have also included a modified version of Daat99's Trashfortokens bag that will work with my token system.
Also included is a version of Raelis's vendor stone to work with my token box.
 
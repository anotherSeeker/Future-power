![[Pasted image 20240610155635.png]]

The Game is controlled by a "Game Board"

the Game Board Takes references to a Consumer Controller and Generator Controller it requests information from these Controllers named Generators and Consumers in the above image and does work with the Power Generation, Requested, Cost and Return from the respective groups.

Additionally the current implementation has it's own UI element that displays the current state and informs the player if it is overloaded.

todo: Display scoring, update how scoring and overloading are calculated, make less ugly
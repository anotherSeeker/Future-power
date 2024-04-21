Current method for calculating "response speed" of different generators is using a float that is scaled by time since last frame to be the maximum "change" allowed in the numbers. 

This causes the generators to change their values linearly until they reach their maximum value if this is intended behaviour is yet to be determined and can be changed later.

```cs
void updatePower()

    {

        desiredGeneration = maxPower * targetGeneration;

        currentGeneration = Mathf.MoveTowards(

                                            currentGeneration,

                                            desiredGeneration,

                                            responseSpeed * Time.deltaTime);

    }

```


The bulk of the generators editable data (and likely every component to come) is handled by Scriptable Objects, in this case Power Gen components specifying a name, maximum generation capacity and response speed.

![[Pasted image 20240421212613.png]]

We also define a prefab for each type so that we can allow the player to later spawn any generator component they want for the variety of situation. Currently this is non functional unsurprisingly.
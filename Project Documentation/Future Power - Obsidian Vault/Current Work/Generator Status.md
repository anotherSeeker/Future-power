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
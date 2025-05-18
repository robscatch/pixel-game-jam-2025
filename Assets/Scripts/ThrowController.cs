using NUnit.Framework;
using UnityEngine;

public class ThrowController : BoardStatsUser
{
    [SerializeField]
    private Throwable[] throwables; // Array of throwable objects


    private bool StartThrowing = false;

    public override void OnBoardStatsSet()
    {
        StartThrowing = true; // Set the flag to start throwing objects
    }

    // Update is called once per frame
    void Update()
    {
        if(!StartThrowing)
        {
            return; 
        }

        //Try throwing every second
        if (Time.time % 1f < Time.deltaTime)
        {
            Debug.Log("Throwing objects..."); // Log the throwing action
            if (Random.value < boardStats.ThrowRate)
            {
                foreach (Throwable throwable in throwables)
                {
                    throwable.ThrowRandomly(); // Call the ThrowRandomly method on each throwable object
                }
            }
        }
    }
}

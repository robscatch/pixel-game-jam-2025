using NUnit.Framework;
using UnityEngine;

public class ThrowController : BoardStatsUser
{
    [SerializeField]
    private Throwable[] throwables; // Array of throwable objects

    [SerializeField]
    private AudioClip throwSound; // Sound to play when throwing objects

    private bool StartThrowing = false;

    public override void OnBoardStatsSet()
    {
        StartThrowing = true; // Set the flag to start throwing objects
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playerIsDead)
        {
            return; // If the player is dead, do not throw objects
        }


        if (!StartThrowing)
        {
            return; 
        }

        //Try throwing every second
        if (Time.time % 1f < Time.deltaTime)
        {
            if (Random.value < boardStats.ThrowRate)
            {
                SoundManager.Instance.PlaySingle(throwSound); // Play the throw sound

                foreach (Throwable throwable in throwables)
                {
                    throwable.ThrowRandomly(); // Call the ThrowRandomly method on each throwable object
                }
            }
        }
    }
}

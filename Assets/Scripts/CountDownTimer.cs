using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public int duration = 60;
    public int timeRemaining;

    [SerializeField]
    private bool isCountingDown = false;

    public Action Finished;

    public bool IsCountingDown { get => isCountingDown; }

    public void Begin()
    {
        if (!IsCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
            CancelInvoke();
            Invoke(nameof(_tick), 1f);
        }
    }

    public void Stop()
    {
        if (IsCountingDown)
        {
            isCountingDown = false;
            CancelInvoke();
        }
    }

    public void Resume()
    {
        if (IsCountingDown)
        {
            return;
        }

        isCountingDown = true;
        Invoke(nameof(_tick), 1f);

    }

    private void _tick()
    {
        if (!IsCountingDown)
        {
            return;
        }

        timeRemaining--;
        if (timeRemaining > 0)
        {
            Invoke(nameof(_tick), 1f);
        }
        else
        {
            timeRemaining = duration;
            isCountingDown = false;

            //Must be called after isCountingDown is set to false
            if (Finished != null)
            {
                Finished();
            }
        }
    }
}
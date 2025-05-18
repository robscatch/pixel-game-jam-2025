using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSlotController : BoardStatsUser
{

    [SerializeField]
    private List<Draggable> Draggables; // List of objects to snap



    [SerializeField]
    private GameObject slotprefab;


    private List<Slot> slots = new List<Slot>();


    [SerializeField]
    private CandleController candleController; // Reference to the CandleController script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    

    public bool AllCorrect()
    {
        if (!candleController.IsFlameOn)
        {
            return false;
        }

        // Check if all slots are occupied and have the correct crystal type
        foreach (var slot in slots)
        {
           if (! slot.isCorrect)
            {
                return false; // Return false if any slot is incorrect
            }
        }
        return true;
    }

    

    public override void OnBoardStatsSet()
    {

        if (slots.Count > 0)
        {
            // Clear existing slots
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject); // Destroy the existing slot GameObjects
            }
        }

        slots.Clear(); // Clear the list of slots

        //Create Slots
        for (int i = 0; i < boardStats.CrystalTypes.Count; i++)
        {
            var slot = Instantiate(slotprefab, transform).GetComponent<Slot>();

            slot.crystalType = boardStats.CrystalTypes[i];
            slot.candleController = candleController; // Assign the CandleController to the slot
            slot.isOccupied = false; // Mark the slot as unoccupied
            slots.Add(slot); // Add the slot to the list
        }

        //Move Draggales above the slots
        foreach (var draggable in Draggables)
        {
            draggable.ResetPosition(); // Reset the position of the draggable object
        }


    }
}

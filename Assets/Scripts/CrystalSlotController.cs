using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSlotController : BoardStatsUser
{

    [SerializeField]
    private List<Draggable> Draggables; // List of objects to snap

    [SerializeField]
    private float snapRange = 0.5f; // Distance within which to snap


    [SerializeField]
    private GameObject slotprefab;


    private List<Slot> slots = new List<Slot>();


    [SerializeField]
    private CandleController candleController; // Reference to the CandleController script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var draggable in Draggables)
        {
            draggable.DragEnded += OnDragEnded; // Subscribe to the DragEnded event
            draggable.DragStarted += OnDragStarted; // Subscribe to the DragStarted event
        }
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

    private void OnDragStarted(Transform transform)
    {
        //Detect if slot is being unoccupied
        for (int i = 0; i < slots.Count; i++) // Use a for loop instead of foreach
        {
            var slot = slots[i];
            if (slot.isOccupied && slot.transform.position == transform.position)
            {
                Debug.Log("Slot is being unoccupied " + slot.transform.name);
                slot.isOccupied = false; // Mark the slot as unoccupied
                slot.SetColorDefault();
                slots[i] = slot; // Update the slot in the list
                break;
            }
        }
    }

    private void OnDragEnded(Transform transform)
    {
        for (int i = 0; i < slots.Count; i++) // Use a for loop instead of foreach
        {
            var slot = slots[i];
            if (!slot.isOccupied && Vector3.Distance(transform.position, slot.transform.position) < snapRange)
            {
                var crystal = transform.GetComponent<CrystalController>().CrystalType;
                if (candleController.IsFlameOn)
                {
                    VerifyCrystals(crystal, slot);
                }

                Debug.Log("Snapping to slot: " + slot.transform.name);
                transform.position = slot.transform.position; // Snap to the position
                slot.isOccupied = true;
                slots[i] = slot; // Update the slot in the list
                break;
            }
        }
    }

    private static void VerifyCrystals(CrystalType crystal, Slot slot)
    {
        //Check crystal type
        if (slot.crystalType != crystal)
        {
            Debug.Log("Wrong crystal type: " + slot.crystalType + " != " + crystal);
            slot.SetCollerWrong(); // Set the color to red
        }
        else
        {
            slot.SetColorSuccess(); // Set the color to green
        }
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

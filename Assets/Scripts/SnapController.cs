using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapController : MonoBehaviour
{

    private struct Slot
    {
        public Transform transform; // The transform of the slot
        public bool isOccupied; // Whether the slot is occupied or not
    }

    [SerializeField]
    private List<Transform> snapPositions; // List of snap positions
    [SerializeField]
    private List<Draggable> Draggables; // List of objects to snap

    [SerializeField]
    private float snapRange = 0.5f; // Distance within which to snap

    private List<Slot> slots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var draggable in Draggables)
        {
            draggable.DragEnded += OnDragEnded; // Subscribe to the DragEnded event
            draggable.DragStarted += OnDragStarted; // Subscribe to the DragStarted event
        }

        slots = new List<Slot>(); // Initialize the slots list

        foreach (var snapPosition in snapPositions)
        {
            slots.Add(new Slot { transform = snapPosition, isOccupied = false }); // Initialize slots
        }

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
                Debug.Log("Snapping to slot: " + slot.transform.name);
                transform.position = slot.transform.position; // Snap to the position
                slot.isOccupied = true;
                slots[i] = slot; // Update the slot in the list
                break;
            }
        }
    }
}

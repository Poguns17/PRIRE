using UnityEngine;

public class SpawnCell
{
    public bool IsOccupied { get; set; }
    public bool IsSpawnable {  get; set; }
    public Vector3 WorldPosition {  get; set; }

    public SpawnCell(Vector3 position)
    {
        IsOccupied = false;
        IsSpawnable = true;
        WorldPosition = position;
    }
}



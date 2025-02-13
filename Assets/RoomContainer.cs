 using UnityEngine;

public class RoomContainer : MonoBehaviour
{
    public RoomEntrance[] entrances;
}

[System.Serializable]
public class RoomEntrance
{
    public Transform entrancePos;
    public DoorDirection direction;
    public enum DoorDirection
    {
        Left,
        Right,
        Up,
        Down
    }
}
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomManager : MonoBehaviour
{
    public List<GameObject> currentRooms;
    public List<RoomEntrance> roomEntrances;

    //static rooms. do NOT touch these!
    public GameObject[] possibleRooms;
    public List<GameObject> leftRooms;
    public List<GameObject> rightRooms;
    public List<GameObject> upRooms;
    public List<GameObject> downRooms;

    //grab all room prefabs and organize them into lists
    void Awake()
    {
        if (currentRooms == null)
        {
            currentRooms = new List<GameObject>();
        }
        if (roomEntrances == null)
        {
            roomEntrances = new List<RoomEntrance>();
        }
        possibleRooms = Resources.LoadAll<GameObject>("Rooms/");
        leftRooms = new List<GameObject>();
        rightRooms = new List<GameObject>();
        upRooms = new List<GameObject>();
        downRooms = new List<GameObject>();
        SortContainers(possibleRooms);
    }

    //scrape room entrances from rooms existing at runtime
    void Start()
    {
        for (int i = 0; i < currentRooms.Count; i++)
        {
            for (int o = 0; o < GetContainer(currentRooms[i]).entrances.Length; o++)
            {
                roomEntrances.Add(GetContainer(currentRooms[i]).entrances[o]);
            }
        }

        for (int i = 0; i < 50; i++)
        {
            //GenerateRoom();
        }
    }

    //get a container from the room gameobject
    RoomContainer GetContainer(GameObject _room)
    {
        return _room.GetComponent<RoomContainer>();
    }

    //organize rooms into lists by direction
    void SortContainers(GameObject[] rooms)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            RoomContainer roomContainer = GetContainer(rooms[i]);
            for (int o = 0; o < roomContainer.entrances.Length; o++)
            {
                switch (roomContainer.entrances[o].direction)
                {
                    case RoomEntrance.DoorDirection.Up:
                        upRooms.Add(rooms[i]);
                        break;
                    case RoomEntrance.DoorDirection.Down:
                        downRooms.Add(rooms[i]);
                        break;
                    case RoomEntrance.DoorDirection.Left:
                        leftRooms.Add(rooms[i]);
                        break;
                    case RoomEntrance.DoorDirection.Right:
                        rightRooms.Add(rooms[i]);
                        break;
                }
            }
        }
    }

    //generate a random room at a random entrance
    public void GenerateRoom(GameObject atRoom = null)
    {
        if (roomEntrances.Count == 0)
        {
            Debug.LogError("ROOM GENERATION ERROR: Tried to generate but no entrances found!");
            return;
        }
        RoomEntrance selectedEntrance = null;
        if (atRoom == null)
        {
            //select a random entrance
            selectedEntrance = roomEntrances[Random.Range(0, roomEntrances.Count - 1)];
        }
        else
        {
            //selectedEntrance = GetContainer(atRoom).entrances;
        }
        //find an appropriate room to add to the entrance
        GameObject selectedRoom;
        RoomEntrance selectedRoomEntrance;
        RoomEntrance.DoorDirection selectedDirection;
        switch (selectedEntrance.direction)
        {
            case RoomEntrance.DoorDirection.Up:
                selectedRoom = downRooms[Random.Range(0, downRooms.Count-1)];
                selectedDirection = RoomEntrance.DoorDirection.Down;
                break;
            case RoomEntrance.DoorDirection.Down:
                selectedRoom = upRooms[Random.Range(0, upRooms.Count-1)];
                selectedDirection = RoomEntrance.DoorDirection.Up;
                break;
            case RoomEntrance.DoorDirection.Left:
                selectedRoom = rightRooms[Random.Range(0, rightRooms.Count-1)];
                selectedDirection = RoomEntrance.DoorDirection.Right;
                break;
            case RoomEntrance.DoorDirection.Right:
                selectedRoom = leftRooms[Random.Range(0, leftRooms.Count-1)];
                selectedDirection = RoomEntrance.DoorDirection.Left;
                break;
            default:
                Debug.LogError("ROOM GENERATION ERROR: Could not find suitable room for generation!");
                return;
        }
        //get selected room entrance by room prefab and desired direction
        selectedRoomEntrance = GetEntranceByDirection(GetContainer(selectedRoom), selectedDirection);
        //instantiate new room at correct position
        Vector3 newRoomPosition = selectedEntrance.entrancePos.position;
        newRoomPosition -= selectedRoomEntrance.entrancePos.position;
        GameObject newRoom = Instantiate(selectedRoom, newRoomPosition, Quaternion.identity);
        currentRooms.Add(newRoom);
        //add new room entrances and remove used ones
        RoomEntrance[] _roomEntrances = GetContainer(newRoom).entrances;
        Debug.Log(_roomEntrances);
        for (int i = 0; i < _roomEntrances.Length; i++)
        {
            if (GetEntrancesAtPosition(_roomEntrances[i].entrancePos.position).Count == 0)
            {
                roomEntrances.Add(_roomEntrances[i]);
            }
        }
        //remove unnecessary entrances if not needed
        if (roomEntrances.Contains(GetEntranceByDirection(GetContainer(newRoom), selectedDirection)))
        {
            roomEntrances.Remove(GetEntranceByDirection(GetContainer(newRoom), selectedDirection));
        }
        if (roomEntrances.Contains(selectedEntrance))
        {
            roomEntrances.Remove(selectedEntrance);
        }
    }

    //
    public RoomEntrance GetEntranceByDirection(RoomContainer container, RoomEntrance.DoorDirection direction)
    {
        for (int i = 0; i < container.entrances.Length; i++)
        {
            if (container.entrances[i].direction == direction)
            {
                return container.entrances[i];
            }
        }
        return null;
    }

    public List<RoomEntrance> GetEntrancesAtPosition(Vector3 position)
    {
        List<RoomEntrance> preOverlappingEntrances = new List<RoomEntrance>();
        for (int i = 0; i < roomEntrances.Count; i++)
        {
            if (roomEntrances[i].entrancePos.position == position)
            {
                preOverlappingEntrances.Add(roomEntrances[i]);
            }
        }
        return preOverlappingEntrances;
    }

    public void CleanEntrancesAtPosition(Vector3 position)
    {
        List<RoomEntrance> preOverlappingEntrances = GetEntrancesAtPosition(position);
        if (preOverlappingEntrances.Count > 1)
        {
            for (int i = 0; i < preOverlappingEntrances.Count; i++)
            {
                roomEntrances.Remove(preOverlappingEntrances[i]);
                Debug.Log("Removed entrance " + preOverlappingEntrances[i]);
            }
        }
    }

    Transform GetClosestRoom(Transform[] rooms, Vector3 pos)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = pos;
        foreach (Transform t in rooms)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        for (int i = 0; i < roomEntrances.Count; i++)
        {
            Gizmos.DrawSphere(roomEntrances[i].entrancePos.position, 1f);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomManager rm = (RoomManager)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Room"))
        {
            rm.GenerateRoom();
        }
    }
}
#endif
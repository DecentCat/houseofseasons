using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGeneration : MonoBehaviour
{
    // how many rooms should be generated, excluding the starting rom
    [SerializeField] private int _howManyRoomsTotal = 5;

    [SerializeField] private List<Room> _possibleRooms;

    [SerializeField] private Room _exitRoom;
    [SerializeField] private Room _itemRoom;

    private Level _level;
    private Grid _grid;
    private Queue<Action> _funcQueue = new Queue<Action>();

    private List<Vector2> _blockedPos = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        OnValidate();
        GenerateRooms();
        _level.OnValidate();
        _level.Rescale();
    }

    void OnValidate()
    {
        _level = GetComponent<Level>();
        _grid = GetComponentInChildren<Grid>();
    }

    void GenerateRooms()
    {
        _blockedPos.Clear();
        _funcQueue.Clear();
        int roomsLeft = _howManyRoomsTotal;
        Room startRoom = GetComponentInChildren<Room>();
        _blockedPos.Add(startRoom.transform.position);
        GenerateRoomsRecursive(roomsLeft, startRoom, startRoom.transform.position, -1, true, true);
    }

    // fromDir:
    // -1 => none, check open doors
    // 0 => north
    // 1 => east
    // 2 => south
    // 3 => west
    void GenerateRoomsRecursive(int roomsLeft, Room room, Vector2 coords, int fromDir,
                                bool generateExit, bool generateItem)
    {
        roomsLeft -= 1;
        //_blockedPos.Add(coords);
        if (roomsLeft <= 0)
        {
            return;
        }
        Room.RoomType roomType = room.type;
        int roomTypeInt = (int) roomType;
        int amtOpenDoors = 0;
        List<Vector2> positions = new List<Vector2>();
        List<int> fromDirs = new List<int>();
        for (int i = 0;i < 4;i++)
        {
            // N, E, S, W
            bool openDoor;
            if (fromDir < 0)
            {
                openDoor = (((int)roomType) & (0b1 << i)) > 0;
            }
            else
            {
                if (fromDir == i || amtOpenDoors >= roomsLeft)
                {
                    openDoor = false; // set to false in order to not generate in this direction
                }
                else
                {
                    openDoor = (UnityEngine.Random.value < 0.5); // random bool
                }
            }
            if (openDoor)
            {
                amtOpenDoors++;
                Vector2 pos = Vector2.zero;
                int dir = -1;
                switch(i)
                {
                    case 0: // NORTH
                        pos = new Vector2(coords.x, coords.y + 14);
                        dir = 2;
                        break;
                    case 1: // EAST
                        pos = new Vector2(coords.x + 22, coords.y);
                        dir = 3;
                        break;
                    case 2: // SOUTH
                        pos = new Vector2(coords.x, coords.y - 14);
                        dir = 0;
                        break;
                    case 3: // WEST
                        pos = new Vector2(coords.x - 22, coords.y);
                        dir = 1;
                        break;
                }
                if (_blockedPos.Contains(pos))
                {
                    amtOpenDoors--;
                    // EDGE CASE: force taken last door, but blocked. hopefully that never happens
                    continue;
                }
                else
                {
                    positions.Add(pos);
                    fromDirs.Add(dir);
                    roomTypeInt |= (0b1 << i);
                }
            }
        }
        // if no door open, force open doors
        if (amtOpenDoors == 0)
        {
            for (int i = 0;i < 4;i++)
            {
                if (fromDir == i || amtOpenDoors >= roomsLeft)
                {
                    continue;
                }
                amtOpenDoors++;
                Vector2 pos = Vector2.zero;
                int dir = -1;
                switch(i)
                {
                    case 0: // NORTH
                        pos = new Vector2(coords.x, coords.y + 14);
                        dir = 2;
                        break;
                    case 1: // EAST
                        pos = new Vector2(coords.x + 22, coords.y);
                        dir = 3;
                        break;
                    case 2: // SOUTH
                        pos = new Vector2(coords.x, coords.y - 14);
                        dir = 0;
                        break;
                    case 3: // WEST
                        pos = new Vector2(coords.x - 22, coords.y);
                        dir = 1;
                        break;
                }
                Debug.Log("Position check: " + pos + " " + _blockedPos.Count);
                if (_blockedPos.Contains(pos))
                {
                    amtOpenDoors--;
                    // EDGE CASE: force taken last door, but blocked. hopefully that never happens
                    continue;
                }
                else
                {
                    positions.Add(pos);
                    fromDirs.Add(dir);
                    roomTypeInt |= (0b1 << i);
                }
            }
        }
        if(amtOpenDoors == 0)
        {
            Debug.LogError("Something went wrong with level generation!");
        }
        Debug.Log("Generating rooms, amtOpenDoors = " + amtOpenDoors + ", positions = " + positions.Count);
        if (fromDir >= 0)
        {
            room.type = (Room.RoomType) roomTypeInt;
        }
        int exitRoomIdx = UnityEngine.Random.Range(0, amtOpenDoors);
        if (!generateExit) exitRoomIdx = -1;
        int itemRoomIdx = UnityEngine.Random.Range(0, amtOpenDoors);
        while (amtOpenDoors > 1 && exitRoomIdx == itemRoomIdx)
        {
            itemRoomIdx = UnityEngine.Random.Range(0, amtOpenDoors);
        }
        if (!generateItem) itemRoomIdx = -1;
        for(int i = 0;i < amtOpenDoors;i++)
        {
            Vector2 pos = positions[i];
            int howMany = -1;
            if (i != amtOpenDoors - 1) // not last open door
            {
                // if only two doors => 5 - 2 + 1 + 1 = 5 (random from 1 to 4)
                howMany = UnityEngine.Random.Range(1, roomsLeft - amtOpenDoors + 1 + 1);
            }
            else
            {
                howMany = roomsLeft;
            }
            roomsLeft -= howMany;
            Room toGenerate;
            bool isExitThere = (i == exitRoomIdx);
            bool isItemThere = (i == itemRoomIdx);
            // DECIDE WHICH ROOM
            if (howMany <= 1 && isExitThere)
            {
                toGenerate = _exitRoom;
            }
            else if (howMany <= 1 && isItemThere)
            {
                toGenerate = _itemRoom;
            }
            else
            {
                int roomIdx = UnityEngine.Random.Range(0, _possibleRooms.Count);
                toGenerate = _possibleRooms[roomIdx];
            }
            Room childObj = Instantiate<Room>(toGenerate, pos, Quaternion.identity, _grid.transform);
            Debug.Log("Call new room " + childObj.name + " = " + howMany + " " + pos + " " + _blockedPos.Count + " " + fromDirs[i] + " " + generateExit);
            Debug.Log("Exit " + i + " " + exitRoomIdx);
            int fromdir = fromDirs[i];
            int newRoomType = 0;
            switch(fromdir)
            {
                case 0: newRoomType |= 1; break;
                case 1: newRoomType |= 2; break;
                case 2: newRoomType |= 4; break;
                case 3: newRoomType |= 8; break;
            }
            childObj.type = (Room.RoomType) newRoomType;
            _blockedPos.Add(pos);
            _funcQueue.Enqueue(() => GenerateRoomsRecursive(howMany, childObj, pos, fromdir, isExitThere, isItemThere));
        }
        while (_funcQueue.Count > 0)
        {
            Action action = _funcQueue.Dequeue();
            action();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

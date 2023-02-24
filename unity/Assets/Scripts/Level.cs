using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    // dynamically adjusted based on Room position in Level
    private Room[,] _rooms;
    private Room[] _allRooms;

    // Start is called before the first frame update
    void Start()
    {
        OnValidate();
        Rescale();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tuple<int,int> FindRoomPosition(Room room)
    {
        for(int i = 0;i < _rooms.GetLength(0);i++)
        {
            for(int j = 0;j < _rooms.GetLength(1);j++)
            {
                if (_rooms[i, j] == room)
                {
                    return Tuple.Create(i, j);
                }
            }
        }
        return null;
    }

    public void SetCameraParent(Room playerRoom, int direction)
    {
        Debug.Log("SetCameraParent " + direction);
        Tuple<int,int> pos = FindRoomPosition(playerRoom);
        PlayerScript playerScript = playerRoom.playerscript;
        int y = pos.Item1;
        int x = pos.Item2;
        Vector2 newPos = new Vector2(playerScript.transform.position.y, playerScript.transform.position.x);
        int oppositeDir = -1;
        Vector2 movVec = Vector2.zero;
        switch(direction)
        {
            case 0: // WEST
                x -= 1;
                oppositeDir = 2;
                movVec = Vector2.left;
                break;
            case 1: // SOUTH
                y -= 1;
                oppositeDir = 3;
                movVec = Vector2.down;
                break;
            case 2: // EAST
                x += 1;
                oppositeDir = 0;
                movVec = Vector2.right;
                break;
            case 3: // NORTH
                y += 1;
                oppositeDir = 1;
                movVec = Vector2.up;
                break;
        }
        Room newRoom = _rooms[y, x];
        Vector2 doorPos = newRoom.doorpositions[oppositeDir];
        Vector2 newPlayerPos = doorPos + (movVec * 1f);
        newPlayerPos.x += 2f;
        newPlayerPos.y += 2f;
        Debug.Log("Moved in direction " + newPlayerPos + ", " + movVec);

        playerScript.transform.parent = newRoom.transform;
        playerScript.transform.localPosition = newPlayerPos;
        newRoom.playerscript = playerScript;
        playerRoom.playerscript = null;

        Vector3 prevLocalPos = _camera.transform.localPosition;
        _camera.transform.parent = newRoom.transform;
        _camera.transform.localPosition = prevLocalPos;
    }

    public void Rescale()
    {
        foreach (Room r in _allRooms)
        {
            r.transform.localPosition *= 2;
        }
    }

    // called when value in inspector changes
    // puts rooms into _rooms variable
    // condition that have to be met in order for it to work:
    // * rooms on same horizontal level have same y value
    // * rooms on same vertical level have same x value
    public void OnValidate()
    {
        _allRooms = GetComponentsInChildren<Room>();
        int maxWidth = 0, maxHeight = 0;
        float minX = float.MaxValue, minY = float.MaxValue,
              maxX = float.MinValue, maxY = float.MinValue;
        // fetch maxWidth, maxHeight for Level Size
        for (int i = 0;i < _allRooms.Length;i++)
        {
            Transform t = _allRooms[i].transform;
            if (t.position.x < minX || t.position.x > maxX)
            {
                maxWidth++;
                if (t.position.x < minX)
                    minX = t.position.x;
                if (t.position.x > maxX)
                    maxX = t.position.x;
            }
            if (t.position.y < minY || t.position.y > maxY)
            {
                maxHeight++;
                if (t.position.y < minY)
                    minY = t.position.y;
                if (t.position.y > maxY)
                    maxY = t.position.y;
            }
        }
        //Debug.Log("[Level] Roomsize: " + maxWidth + " " + maxHeight);
        _rooms = new Room[maxHeight, maxWidth];
        Dictionary<float, int> yToIdx = new Dictionary<float, int>();
        Dictionary<float, int> xToIdx = new Dictionary<float, int>();
        // set rooms inside of the _rooms variable
        List<Room> horizontalSorted = new List<Room>(_allRooms);
        horizontalSorted.Sort((x, y) => x.transform.position.x.CompareTo(y.transform.position.x));
        List<Room> verticalSorted = new List<Room>(_allRooms);
        verticalSorted.Sort((x, y) => x.transform.position.y.CompareTo(y.transform.position.y));
        int currIdx = 0;
        foreach (Room r in horizontalSorted)
        {
            if(!xToIdx.ContainsKey(r.transform.position.x))
            {
                xToIdx.Add(r.transform.position.x, currIdx++);
            }
        }
        currIdx = 0;
        foreach (Room r in verticalSorted)
        {
            if(!yToIdx.ContainsKey(r.transform.position.y))
            {
                yToIdx.Add(r.transform.position.y, currIdx++);
            }
        }
        foreach (Room r in _allRooms)
        {
            int x = xToIdx[r.transform.position.x];
            int y = yToIdx[r.transform.position.y];
            _rooms[y, x] = r;
        }
        string mapOutput = "";
        for(int i = 0;i < _rooms.GetLength(0);i++)
        {
            string mapOutputY = "";
            for(int j = 0;j < _rooms.GetLength(1);j++)
            {
                if (_rooms[i, j] != null)
                    mapOutputY += _rooms[i, j].name + " | ";
                else
                    mapOutputY += "null | ";
            }
            mapOutput = mapOutputY + "\n" + mapOutput;
        }
        Debug.Log("[Level] Parsed Room:\n" + mapOutput);
    }

    public void LevelQuit()
    {
        SceneManager.LoadScene(0);
    }
}

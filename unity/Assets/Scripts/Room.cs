using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode]
public class Room : MonoBehaviour
{
    public enum RoomType
    {
        NO_DOORS = 0,
        N_DOOR = 0b0001,
        E_DOOR = 0b0010,
        NE_DOOR = 0b0011,
        S_DOOR = 0b0100,
        NS_DOOR = 0b0101,
        ES_DOOR = 0b0110,
        NES_DOOR = 0b0111,
        W_DOOR = 0b1000,
        WN_DOOR = 0b1001,
        WE_DOOR = 0b1010,
        WEN_DOOR = 0b1011,
        WS_DOOR = 0b1100,
        WSN_DOOR = 0b1101,
        WSE_DOOR = 0b1110,
        ALL_DOOR = 0b1111
    };

    public RoomType type {get {return _type;}}
    public List<Vector2Int> doorpositions {get {return _doorpositions;}}

    public PlayerScript playerscript {set {_playerscript = value; } get { return _playerscript; }}

    [SerializeField] private RoomType _type;
    // idx: 0..3,4..7,8..11,12..15 => WEST,SOUTH,EAST,NORTH
    // 0,1,2,3 tiles in order top left, top right, bottom left, bottom right
    [SerializeField] private List<TileBase> _doortiles;
    [SerializeField] private List<Vector2Int> _doorpositions;
    [SerializeField] private PlayerScript _playerscript;
    private RoomTilemap[] _roomtilemaps;
    private Level _level;

    // Start is called before the first frame update
    void Start()
    {
        OnValidate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDoorEntered()
    {
        if (_playerscript == null)
        {
            Debug.LogError("PlayerScript null but door was entered in the room");
            return;
        }
        float minDistance = float.MaxValue;
        int doorDir = -1;
        for(int i = 0; i < _doorpositions.Count; i++)
        {
            Vector2Int playerPos = new Vector2Int((int)_playerscript.transform.localPosition.x, (int)_playerscript.transform.localPosition.y);
            float distance = Vector2Int.Distance(_doorpositions[i], playerPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                doorDir = i;
            }
        }
        _level.SetCameraParent(this, doorDir);
    }

    // called when value in inspector changes
    void OnValidate()
    {
        _level = GetComponentInParent<Level>();
        _playerscript = GetComponentInChildren<PlayerScript>();
        _roomtilemaps = GetComponentsInChildren<RoomTilemap>();
        for(int i = 0;i < _roomtilemaps.Length;i++)
        {
            _roomtilemaps[i].UpdateTiles(_type, _doorpositions);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
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

    [SerializeField] private RoomType _type;
    // idx: 0,1,2,3 => WEST,SOUTH,EAST,NORTH
    [SerializeField] private List<TileBase> _doortiles;
    [SerializeField] private List<Vector2Int> _doorpositions;
    private RoomTilemap[] _roomtilemaps;

    // Start is called before the first frame update
    void Start()
    {
        _roomtilemaps = GetComponentsInChildren<RoomTilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // called when value in inspector changes
    void OnValidate()
    {
        Start();
        for(int i = 0;i < _roomtilemaps.Length;i++)
        {
            _roomtilemaps[i].UpdateTiles();
        }
    }
}

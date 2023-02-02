using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class RoomTilemap : MonoBehaviour
{
    public static string DOORTILEMAPTAG = "DoorTilemap";

    [SerializeField] private bool _reversePos;
    [SerializeField] private List<TileBase> _tiles;
    private Tilemap _tilemap;

    private Room _room;

    // Start is called before the first frame update
    public void Start()
    {
        OnValidate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate()
    {
        _tilemap = GetComponent<Tilemap>();
        _room = GetComponentInParent<Room>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        PlayerScript script;
        if (other.TryGetComponent<PlayerScript>(out script))
        {
            Debug.Log("Move Through Door");
            _room.OnDoorEntered();
        }
    }

    public void UpdateTiles(Room.RoomType type, List<Vector2Int> doorpositions)
    {
        OnValidate();
        for(int idx = 0;idx < 4;idx++)
        {
            bool isPos = (((int)type >> idx) & 1) == 1;
            isPos ^= _reversePos;
            Vector2Int pos = doorpositions[3-idx];
            for(int i = 0;i < 4;i++)
            {
                Vector3Int specificPos = new Vector3Int(pos.x + (i % 2), pos.y + (i / 2), 0);
                TileBase toSet;
                if (isPos)
                {
                    toSet = null;
                }
                else
                {
                    //Debug.Log("[RoomTilemap] Setting Tile at Position " + specificPos);
                    toSet = _tiles[15-idx*4-(3-i)];
                }
                _tilemap.SetTile(specificPos, toSet);
            }
        }
    }
}

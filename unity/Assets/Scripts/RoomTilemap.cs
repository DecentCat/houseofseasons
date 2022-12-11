using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class RoomTilemap : MonoBehaviour
{
    [SerializeField] private bool _setOnNotPosition;
    [SerializeField] private List<TileBase> _tiles;
    private Tilemap _tilemap;

    // Start is called before the first frame update
    public void Start()
    {
        //_tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void UpdateTiles(Room.RoomType type, List<Vector2Int> doorpositions)
    {
        OnValidate();
        for(int idx = 0;idx < 4;idx++)
        {
            bool isPos = (((int)type >> idx) & 1) == 1;
            isPos ^= _setOnNotPosition;
            Vector2Int pos = doorpositions[3-idx];
            TileBase toSet;
            if (isPos)
            {
                toSet = null;
            }
            else
            {
                Debug.Log("[RoomTilemap] Setting Tile at Position " + new Vector3Int(pos.x, pos.y, 0));
                toSet = _tiles[3-idx];
            }
            _tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), toSet);
        }
    }
}

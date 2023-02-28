using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoomSpawn : MonoBehaviour
{
    [SerializeField] private List<ItemPickupScript> _possibleItemSpawns;
    [SerializeField] private List<Vector2> _positions;

    // Start is called before the first frame update
    void Start()
    {
        Room room = GetComponent<Room>();
        Debug.Log("ROOM " + room);
        for (int i = 0; i < _positions.Count; i++)
        {
            int rIdx = Random.Range(0, _possibleItemSpawns.Count);
            ItemPickupScript itemPickup = _possibleItemSpawns[rIdx];
            ItemPickupScript generated = Instantiate<ItemPickupScript>(itemPickup,
                            room.transform.position + new Vector3(_positions[i].x, _positions[i].y, 0), Quaternion.identity, room.transform);
            Debug.Log("Generated Item " + rIdx + " from " + _possibleItemSpawns.Count + " / " + generated);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

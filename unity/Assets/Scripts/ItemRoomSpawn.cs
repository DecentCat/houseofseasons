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
        PlayerScript script = transform.parent.GetComponentInChildren<PlayerScript>();
        List<ItemPickupScript> filterList = new List<ItemPickupScript>(_possibleItemSpawns);
        for(int i = 0;i < _possibleItemSpawns.Count;i++)
        {
            WeaponType type = filterList[i].weaponType;
            switch(type)
            {
                case WeaponType.Heavy:
                    if (script.weaponManager.heavyUnlocked) _possibleItemSpawns.Remove(filterList[i]); break;
                case WeaponType.Laser:
                    if (script.weaponManager.laserUnlocked) _possibleItemSpawns.Remove(filterList[i]); break;
                case WeaponType.Assault:
                    if (script.weaponManager.assaultUnlocked) _possibleItemSpawns.Remove(filterList[i]); break;
                case WeaponType.Shotgun:
                    if (script.weaponManager.shotgunUnlocked) _possibleItemSpawns.Remove(filterList[i]); break;
            }
        }
        Debug.Log("Item Room possible Item Spawns: " + _possibleItemSpawns.Count);
        Room room = GetComponent<Room>();
        if (_possibleItemSpawns.Count == 0)
        {
            // Do not spawn any pickup
            return;
        }
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

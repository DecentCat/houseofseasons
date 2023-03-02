using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemPickup : MonoBehaviour
{
    [SerializeField] private List<ItemPickupScript> _possiblePickups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePickup(bool shouldDestroy)
    {
        int rIdx = Random.Range(0, _possiblePickups.Count);
        ItemPickupScript itemPickupScript = _possiblePickups[rIdx];
        if (shouldDestroy)
        {
            Destroy(gameObject);
        }
        ItemPickupScript generated = Instantiate<ItemPickupScript>(itemPickupScript,
            gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);
        Debug.Log("Placed Item with Pickup Idx " + rIdx + " " + generated.transform.position);
    }
}

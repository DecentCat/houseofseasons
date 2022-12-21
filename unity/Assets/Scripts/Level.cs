using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // dynamically adjusted based on Room position in Level
    private Room[,] _rooms;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called when value in inspector changes
    void OnValidate()
    {
        Room[] rooms = GetComponentsInChildren<Room>();
        int maxWidth = 0, maxHeight = 0;
        float minX = float.MaxValue, minY = float.MaxValue,
              maxX = float.MinValue, maxY = float.MinValue;
        // fetch maxWidth, maxHeight for Level Size
        for (int i = 0;i < rooms.Length;i++)
        {
            Transform t = rooms[i].transform;
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
        Debug.Log("[Level] Roomsize: " + maxWidth + " " + maxHeight);
        _rooms = new Room[maxHeight, maxWidth];
        // set rooms inside of the _rooms variable
        //TODO: parse rooms
    }
}

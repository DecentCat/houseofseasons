using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private Level _level;

    // Start is called before the first frame update
    void Start()
    {
        _level = GetComponentInParent<Level>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        PlayerScript player;
        if (other.TryGetComponent<PlayerScript>(out player))
        {
            _level.LevelQuit(player);
        }
    }
}

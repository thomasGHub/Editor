using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEditorEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventsManager.PlayEvent("Spawn", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

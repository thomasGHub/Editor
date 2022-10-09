using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaitFeedback : GameFeedback
{
    public float timer;

    public override IEnumerator Execute(GameEvent gameEvent)
    {
        yield return new WaitForSeconds(timer);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameFeedback
{
    public virtual IEnumerator Execute(GameEvent gameEvent)
    {
        yield break;
    }
}

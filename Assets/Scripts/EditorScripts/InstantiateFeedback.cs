using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class InstantiateFeedback : GameFeedback
{
    [SerializeField] private GameObject prefab;

    public override IEnumerator Execute(GameEvent gameEvent)
    {
        GameObject.Instantiate(prefab, gameEvent.gameObject.transform.position, Quaternion.identity);
        yield break;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent")]
[Serializable]
public class GameEvent : ScriptableObject
{
    [SerializeReference]
    public List<GameFeedback> gameFeedbacks = new List<GameFeedback>();
    public GameObject gameObject;

    public IEnumerator Execute()
    {
        foreach(var item in gameFeedbacks)
        {
            yield return item.Execute(this);
        }
    }
}

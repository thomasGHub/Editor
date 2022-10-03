using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    static GameEventsManager s_instance;

    public List<GameEvent> gameEvents;

    static Dictionary<string, GameEvent> _events;

    private void Awake()
    {
        s_instance = this;

        _events = new Dictionary<string, GameEvent>(gameEvents.Count);

        foreach(var gameEvent in gameEvents)
        {
            _events.Add(gameEvent.name, gameEvent);
        }
    }

    public static void PlayEvent(string eventName, GameObject gameObject)
    {
        _events[eventName].gameObject = gameObject;
        s_instance.StartCoroutine(_events[eventName].Execute());
    }
}

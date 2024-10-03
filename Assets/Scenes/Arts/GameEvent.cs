using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewGame",menuName ="GemeEvents/New Game Event")]
public class GameEvent : ScriptableObject
{
    public List<IGameEventListener> gameEventListeners = new List<IGameEventListener>();

    public void Fire()
    {
        for (int i= 0; i <gameEventListeners.Count; i++)
        {
            gameEventListeners[i].Notify();

        }
    }
    public void RegisterListener(IGameEventListener gameEventListener)
    {
        if (gameEventListener == null)
            return;
        if (gameEventListeners.Contains(gameEventListener))
            return;
        gameEventListeners.Add(gameEventListener);
    }
    public void UnregisterListener(IGameEventListener gameEventListener)
    {
        if (gameEventListener == null)
            return;
        if (gameEventListeners.Contains(gameEventListener) == false)
            return;
        gameEventListeners.Remove(gameEventListener);

    }
}

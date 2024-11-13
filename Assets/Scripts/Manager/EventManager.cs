using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class EventManager : Singleton<EventManager>
{
    public enum Channel
    {

    }

    private Dictionary<Channel, List<Delegate>> eventHandlers = new Dictionary<Channel, List<Delegate>>();
    public void Subscribe<TEvent>(Channel channel, Action<TEvent> handler) where TEvent : EventArgs
    {
        if (false == eventHandlers?.ContainsKey(channel))
            eventHandlers[channel] = new List<Delegate>();

        eventHandlers?[channel].Add(handler);
    }

    public void Unsubscribe<TEvent>(Channel channel, Action<TEvent> handler) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);
        if (true == eventHandlers?.ContainsKey(channel))
            eventHandlers[channel].Remove(handler);
    }

    public void Publish<TEvent>(Channel channel, TEvent eventArgs) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);
        if (true == eventHandlers?.ContainsKey(channel))
        {
            List<Delegate> handlers = eventHandlers[channel];
            foreach (var handler in handlers)
            {
                if (false == handler is Action<TEvent>)
                    Debug.LogError("publish error : sub event type is not " + typeof(TEvent));
                    

                ((Action<TEvent>)handler)(eventArgs);
            }
        }
    }
}


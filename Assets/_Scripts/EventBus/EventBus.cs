using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> Listeners = new();

    public static void AddListener<T>(Action<T> listener)
    {
        if (listener == null)
        {
            return;
        }

        var eventType = typeof(T);

        if (Listeners.TryGetValue(eventType, out var existingDelegate))
        {
            Listeners[eventType] = Delegate.Combine(existingDelegate, listener);
            return;
        }

        Listeners[eventType] = listener;
    }

    public static void RemoveListener<T>(Action<T> listener)
    {
        if (listener == null)
        {
            return;
        }

        var eventType = typeof(T);
        if (!Listeners.TryGetValue(eventType, out var existingDelegate))
        {
            return;
        }

        var updatedDelegate = Delegate.Remove(existingDelegate, listener);
        if (updatedDelegate == null)
        {
            Listeners.Remove(eventType);
            return;
        }

        Listeners[eventType] = updatedDelegate;
    }

    public static void Publish<T>(T eventData)
    {
        var eventType = typeof(T);
        if (Listeners.TryGetValue(eventType, out var existingDelegate))
        {
            ((Action<T>)existingDelegate)?.Invoke(eventData);
        }
    }

    public static void Clear()
    {
        Listeners.Clear();
    }
}

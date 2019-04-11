using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EventManager<TEventType> where TEventType : struct, IFormattable, IConvertible, IComparable
{
    private Dictionary<int, Delegate> handlers;
    private Dictionary<int, List<int>> listenerPriorityDic;
    private Func<TEventType, int> enumConverter;
    private Delegate[] del_eventsCache;

    static EventManager()
    {
        if (!typeof(TEventType).IsEnum)
            throw new InvalidTypeExpection("event manager : event type must be enum");
    }

    public EventManager()
    {
        handlers = new Dictionary<int, Delegate>();
        listenerPriorityDic = new Dictionary<int, List<int>>();
        enumConverter = EnumConverterCreator.CreateFromEnumConverter<TEventType, int>();

        Array enumsValues = Enum.GetValues(typeof(TEventType));

        foreach (var index in enumsValues)
        {
            Delegate del = null;
            handlers.Add((int)index, del);
            listenerPriorityDic.Add((int)index, new List<int>());
        }
    }

    //eventArgs param, for cases params number more than three and write clearly
    public void AddListener(TEventType type, Action<EventArgs> action, EventPriority priority = EventPriority.High)
    {
        AddListener<EventArgs>(type, action, priority);
    }

    //none param
    public void AddListener(TEventType type, Action action, EventPriority priority = EventPriority.High)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {
            if (handlers[eventKey] != null)
            {
                Delegate del = handlers[eventKey];
                foreach (var item in del.GetInvocationList())
                {
                    if ((Action)item == action)
                        throw new AddListenerExpection(string.Format("register for event {0} ,but action {1} has registered",
                            (TEventType)(object)eventKey, action));
                }
            }
        }
        handlers[eventKey] = (Action)handlers[eventKey] + action;
        listenerPriorityDic[eventKey].Add((int)priority);
        SortListener(eventKey);
    }

    //one param
    public void AddListener<T>(TEventType type, Action<T> action, EventPriority priority = EventPriority.High)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {
            if (handlers[eventKey] != null)
            {
                Delegate del = handlers[eventKey];
                foreach (var item in del.GetInvocationList())
                {
                    if ((Action<T>)item == action)
                        throw new AddListenerExpection(string.Format("register for event {0} ,but action {1} has registered",
                            (TEventType)(object)eventKey, action));
                }
            }
        }
        handlers[eventKey] = (Action<T>)handlers[eventKey] + action;
        listenerPriorityDic[eventKey].Add((int)priority);
        SortListener(eventKey);
    }

    //two param
    public void AddListener<T1, T2>(TEventType type, Action<T1, T2> action, EventPriority priority = EventPriority.High)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {
            if (handlers[eventKey] != null)
            {
                Delegate del = handlers[eventKey];
                foreach (var item in del.GetInvocationList())
                {
                    if ((Action<T1, T2>)item == action)
                        throw new AddListenerExpection(string.Format("register for event {0} ,but action {1} has registered",
                            (TEventType)(object)eventKey, action));
                }
            }
        }
        handlers[eventKey] = (Action<T1, T2>)handlers[eventKey] + action;
        listenerPriorityDic[eventKey].Add((int)priority);
        SortListener(eventKey);
    }

    //three param
    public void AddListener<T1, T2, T3>(TEventType type, Action<T1, T2, T3> action, EventPriority priority = EventPriority.High)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {
            if (handlers[eventKey] != null)
            {
                Delegate del = handlers[eventKey];
                foreach (var item in del.GetInvocationList())
                {
                    if ((Action<T1, T2, T3>)item == action)
                        throw new AddListenerExpection(string.Format("register for event {0} ,but action {1} has registered",
                            (TEventType)(object)eventKey, action));
                }
            }
        }
        handlers[eventKey] = (Action<T1, T2, T3>)handlers[eventKey] + action;
        listenerPriorityDic[eventKey].Add((int)priority);
        SortListener(eventKey);
    }

    public void RemoveListener(TEventType type, Action<EventArgs> action)
    {
        RemoveListener<EventArgs>(type, action);
    }

    public void RemoveListener(TEventType type, Action action)
    {
        int eventKey = enumConverter(type);
        if (!handlers.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event does not exist", (TEventType)(object)eventKey));
        }
        if (!listenerPriorityDic.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event priority list does not exist", (TEventType)(object)eventKey));
        }

        if (handlers[eventKey] != null)
        {
            Delegate[] del = handlers[eventKey].GetInvocationList();
            int index = 0;
            foreach (var item in del)
            {
                if ((Action)item == action)
                {
                    handlers[eventKey] = (Action)handlers[eventKey] - action;
                    listenerPriorityDic[eventKey].Remove(index);
                    break;
                }
                index++;
            }
        }
    }

    public void RemoveListener<T>(TEventType type, Action<T> action)
    {
        int eventKey = enumConverter(type);
        if (!handlers.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event does not exist", (TEventType)(object)eventKey));
        }
        if (!listenerPriorityDic.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event priority list does not exist", (TEventType)(object)eventKey));
        }

        if (handlers[eventKey] != null)
        {
            Delegate[] del = handlers[eventKey].GetInvocationList();
            int index = 0;
            foreach (var item in del)
            {
                if ((Action<T>)item == action)
                {
                    handlers[eventKey] = (Action<T>)handlers[eventKey] - action;
                    listenerPriorityDic[eventKey].Remove(index);
                    break;
                }
                index++;
            }
        }
    }

    public void RemoveListener<T1, T2>(TEventType type, Action<T1, T2> action)
    {
        int eventKey = enumConverter(type);
        if (!handlers.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event does not exist", (TEventType)(object)eventKey));
        }
        if (!listenerPriorityDic.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event priority list does not exist", (TEventType)(object)eventKey));
        }

        if (handlers[eventKey] != null)
        {
            Delegate[] del = handlers[eventKey].GetInvocationList();
            int index = 0;
            foreach (var item in del)
            {
                if ((Action<T1, T2>)item == action)
                {
                    handlers[eventKey] = (Action<T1, T2>)handlers[eventKey] - action;
                    listenerPriorityDic[eventKey].Remove(index);
                    break;
                }
                index++;
            }
        }
    }

    public void RemoveListener<T1, T2, T3>(TEventType type, Action<T1, T2, T3> action)
    {
        int eventKey = enumConverter(type);
        if (!handlers.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event does not exist", (TEventType)(object)eventKey));
        }
        if (!listenerPriorityDic.ContainsKey(eventKey))
        {
            throw new RemoveListenerExpection(string.Format(
                "remove action for event {0},but this event priority list does not exist", (TEventType)(object)eventKey));
        }

        if (handlers[eventKey] != null)
        {
            Delegate[] del = handlers[eventKey].GetInvocationList();
            int index = 0;
            foreach (var item in del)
            {
                if ((Action<T1, T2, T3>)item == action)
                {
                    handlers[eventKey] = (Action<T1, T2, T3>)handlers[eventKey] - action;
                    listenerPriorityDic[eventKey].Remove(index);
                    break;
                }
                index++;
            }
        }
    }

    public void TriggerEvent(TEventType type, EventArgs args)
    {
        TriggerEvent<EventArgs>(type, args);
    }

    public void TriggerEvent(TEventType type)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {
            Delegate del = handlers[eventKey];
            if (del != null)
            {
                Delegate[] delArray = del.GetInvocationList();
                for (int i = 0; i < delArray.Length; i ++)
                {
                    Action action = (Action)delArray[i];
                    if (action != null)
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex.StackTrace);
                        }
                    }
                    else
                    {
                        throw new TriggerEventException(string.Format("send message for event {0} ,but can not get register action",
                            (TEventType)(object)eventKey));
                    }
                }
            }
        }
    }

    public void TriggerEvent<T>(TEventType type, T t)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {

            Delegate del = handlers[eventKey];
            if (del != null)
            {
                foreach (var item in del.GetInvocationList())
                {
                    Action<T> action = (Action<T>)item;
                    if (action != null)
                    {
                        try
                        {
                            action(t);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex.StackTrace);
                        }
                    }
                    else
                    {
                        throw new TriggerEventException(string.Format("send message for event {0} ,but can not get register action",
                            (TEventType)(object)eventKey));
                    }
                }
            }
        }
    }

    public void TriggerEvent<T1, T2>(TEventType type, T1 t1, T2 t2)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {
            Delegate del = handlers[eventKey];
            if (del != null)
            {
                foreach (var item in del.GetInvocationList())
                {
                    Action<T1, T2> action = (Action<T1, T2>)item;
                    if (action != null)
                    {
                        try
                        {
                            action(t1, t2);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex.StackTrace);
                        }
                    }
                    else
                    {
                        throw new TriggerEventException(string.Format("send message for event {0} ,but can not get register action",
                            (TEventType)(object)eventKey));
                    }
                }
            }
        }
    }

    public void TriggerEvent<T1, T2, T3>(TEventType type, T1 t1, T2 t2, T3 t3)
    {
        int eventKey = enumConverter(type);
        if (handlers.ContainsKey(eventKey))
        {
            Delegate del = handlers[eventKey];
            if (del != null)
            {
                foreach (var item in del.GetInvocationList())
                {
                    Action<T1, T2, T3> action = (Action<T1, T2, T3>)item;
                    if (action != null)
                    {
                        try
                        {
                            action(t1, t2, t3);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex.StackTrace);
                        }
                    }
                    else
                    {
                        throw new TriggerEventException(string.Format("send message for event {0} ,but can not get register action",
                            (TEventType)(object)eventKey));
                    }
                }
            }
        }
    }

    private void SortListener(int eventKey)
    {
        del_eventsCache = handlers[eventKey].GetInvocationList();
        if (del_eventsCache.Length <= 1 || del_eventsCache == null)
        {
            return;
        }

        if (del_eventsCache.Length != listenerPriorityDic[eventKey].Count)
        {
            throw new SortListenersExpection
                (string.Format("sort listeners for event {0} , listeners count {1} not equal to priority list count {2} ",
                (TEventType)(object)eventKey, del_eventsCache.Length, listenerPriorityDic[eventKey].Count));
        }

        int end = del_eventsCache.Length - 1;
        for (int i = 0; i < del_eventsCache.Length; i++)
        {
            if (listenerPriorityDic[eventKey][end] < listenerPriorityDic[eventKey][i])
            {
                int temp_priority = listenerPriorityDic[eventKey][end];
                listenerPriorityDic[eventKey][end] = listenerPriorityDic[eventKey][i];
                listenerPriorityDic[eventKey][i] = temp_priority;

                Delegate temp_del = del_eventsCache[end];
                del_eventsCache[end] = del_eventsCache[i];
                del_eventsCache[i] = temp_del;
                handlers[eventKey] = Delegate.Combine(del_eventsCache);
                break;
            }
        }
    }
}

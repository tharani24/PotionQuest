using System;
using System.Collections.Generic;

namespace PotionQuest.Events
{
    public static class EventManager
    {
        private static readonly Dictionary<string, Action<object>> EventDict = new();

        public static void Subscribe(string eventName, Action<object> callback)
        {
            if (!EventDict.ContainsKey(eventName))
                EventDict[eventName] = delegate { };

            EventDict[eventName] += callback;
        }

        public static void Unsubscribe(string eventName, Action<object> callback)
        {
            if (EventDict.ContainsKey(eventName))
                EventDict[eventName] -= callback;
        }

        public static void Fire(string eventName, object parameter = null)
        {
            if (EventDict.ContainsKey(eventName))
                EventDict[eventName]?.Invoke(parameter);
        }
    }
}
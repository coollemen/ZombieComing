using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
    public class EventManager : MonoSingleton<EventManager>
    {

        private Dictionary<string,GameEvent> eventDictionary;


        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, GameEvent>();
            }
        }


        public static void StartListening(string eventName, UnityAction<GameEventArgs> listener)
        {
            GameEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new GameEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction<GameEventArgs> listener)
        {
            if (Instance == null) return;
            GameEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName,GameEventArgs args)
        {
            GameEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(args);
            }
        }
    }
}
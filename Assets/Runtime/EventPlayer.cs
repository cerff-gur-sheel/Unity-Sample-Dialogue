using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SampleDialogue
{
    /// <summary>
    /// Plays UnityEvents associated with dialogue events.
    /// </summary>
    public class EventPlayer : MonoBehaviour
    {
        /// <summary>
        /// Serialized dictionary of dialogue events.
        /// </summary>
        [SerializeField]
        private EventDictionary dialogueEvents;

        /// <summary>
        /// Dictionary mapping event names to UnityEvents.
        /// </summary>
        public Dictionary<string, UnityEvent> Events { get; private set; }

        /// <summary>
        /// Initializes the Events dictionary from the serialized EventDictionary.
        /// </summary>
        private void Start()
        {
            Events = dialogueEvents.ToDictionary();
        }

        /// <summary>
        /// Invokes the UnityEvent associated with the specified event name, if it exists.
        /// Logs a warning if the event name is not found.
        /// </summary>
        /// <param name="event">The name of the event to invoke.</param>
        public void PlayEvent(string @event)
        {
            if (Events.ContainsKey(@event)) Events[@event].Invoke();
            else Debug.LogWarning($"Event {@event} not found in EventPlayer.");
        }
    }

    /// <summary>
    /// Serializable dictionary for storing event name and UnityEvent pairs.
    /// </summary>
    [Serializable]
    public class EventDictionary
    {
        /// <summary>
        /// Array of event dictionary items.
        /// </summary>
        [SerializeField]
        private EventDictionaryItem[] thisEventDictionaryItems;

        /// <summary>
        /// Converts the array of items to a dictionary.
        /// </summary>
        /// <returns>Dictionary mapping event names to UnityEvents.</returns>
        public Dictionary<string, UnityEvent> ToDictionary() =>
            thisEventDictionaryItems.ToDictionary(item => item.eventName, item => item.@event);
    }

    /// <summary>
    /// Represents a single event dictionary item with a name and UnityEvent.
    /// </summary>
    [Serializable]
    public class EventDictionaryItem
    {
        /// <summary>
        /// The name of the event.
        /// </summary>
        [SerializeField]
        public string eventName;

        /// <summary>
        /// The UnityEvent associated with the event name.
        /// </summary>
        [SerializeField]
        public UnityEvent @event;
    }
}
﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace EventStoreApi
{
    public static class DeserializedEventStore
    {
        private static ConcurrentDictionary<Guid, DeserializedStoredEvent> _events { get; set; }
        public static ConcurrentDictionary<Guid, DeserializedStoredEvent> Events { get; set; }
        
        public static void TryAdd(StoredEvent @event)
            => Events.TryAdd(@event.StoredEventId, new DeserializedStoredEvent(@event));

        public static IEnumerable<DeserializedStoredEvent> Get()
        {
            var eventsCount = Events.Count();
            var deserializedStoredEvents = new DeserializedStoredEvent[eventsCount];
            for (var i = 0; i < eventsCount; i++)
                deserializedStoredEvents[i] = Events.ElementAt(i).Value;
            
            Array.Sort(deserializedStoredEvents, (x, y) => DateTime.Compare(x.CreatedOn, y.CreatedOn));

            return deserializedStoredEvents;
        }

        public static IEnumerable<StoredEvent> GetStoredEvents()
            => DeserializeObject<ICollection<StoredEvent>>(File.ReadAllText($@"{Environment.CurrentDirectory}\storedEvents.json"));

    }
}

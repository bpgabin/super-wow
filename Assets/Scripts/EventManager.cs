using UnityEngine;
using System.Collections;

public delegate bool DelegateEventHandler(IEvent evt);

public interface IEventListener {
    bool HandleEvent(IEvent evt);
}

public interface IEvent {
    string GetName();
}

public class BaseEvent : IEvent {
    public string GetName() { return this.GetType().Name; }
}

public class EventManager : MonoBehaviour {

    public bool limitQueueProcessing = false;
    public float queueProcessTime = 0.0f;

    private static EventManager s_instance = null;

    public static EventManager instance {
        get {
            if (s_instance == null) {
                GameObject go = new GameObject("EventManager");
                s_instance = go.AddComponent<EventManager>();
            }
            return s_instance;
        }
    }

    private Hashtable m_listenerTable = new Hashtable();
    private Queue m_eventQueue = new Queue();

    // Add a listener to the event manager that will reviece any events of the supplied event name.
    public bool AddListener(IEventListener listener, string eventName) {
        return AddListener(listener, eventName, listener.HandleEvent);
    }

    // Add a listener to the event manager that will recieve events of the supplied event name.
    public bool AddListener(IEventListener listener, string eventName, DelegateEventHandler handler) {
        if (listener == null || eventName == null) {
            Debug.Log("EventManager: AddListener failed due to no listener or event name specified.");
            return false;
        }

        if (!m_listenerTable.ContainsKey(eventName))
            m_listenerTable.Add(eventName, new ArrayList());

        ArrayList listenerList = m_listenerTable[eventName] as ArrayList;
        if (listenerList.Contains(handler)) {
            Debug.Log("EventManager: Listener: " + listener.GetType().ToString() + " is already in list for event: " + eventName);
            return false;
        }

        listenerList.Add(handler);
        return true;
    }

    // Remove a listener from the subscribed event.
    public bool DetachListener(IEventListener listener, string eventName){
        return DetachListener(listener, eventName, listener.HandleEvent);
    }

    // Remove a listener from the subscribed event.
    public bool DetachListener(IEventListener listener, string eventName, DelegateEventHandler handler){
        if(!m_listenerTable.ContainsKey(eventName))
            return false;

        ArrayList listenerList = m_listenerTable[eventName] as ArrayList;
        if(!listenerList.Contains(handler))
            return false;

        listenerList.Remove(handler);
        return true;
    }

    // Trigger the event instantly, this should be only used in specific circumstances,
    // the QueueEvent function is usually fast enough for the vast majority of uses.
    public bool TriggerEvent(IEvent evt) {
        string eventName = evt.GetName();
        
        if (!m_listenerTable.ContainsKey(eventName)) {
            Debug.Log("EventManager: Event \"" + eventName + "\" triggered has no listeners!");
            return false;
        }

        ArrayList listenerList = m_listenerTable[eventName] as ArrayList;
        foreach (DelegateEventHandler listener in listenerList) {
            if (listener(evt))
                return true;
        }

        return true;
    }

    // Inserts the event into the current queue.
    public bool QueueEvent(IEvent evt) {
        if (!m_listenerTable.ContainsKey(evt.GetName())) {
            Debug.Log("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetName());
            return false;
        }

        m_eventQueue.Enqueue(evt);
        return false;
    }

    // Every update cycle the queue is processed, if the queue processing is limited,
    // a maximum processing time per update can be set after which the events will have
    // to be processed next update loop.
    void Update() {
        float timer = 0.0f;
        while (m_eventQueue.Count > 0) {
            if (limitQueueProcessing) {
                if (timer > queueProcessTime) return;
            }

            IEvent evt = m_eventQueue.Dequeue() as IEvent;
            if(!TriggerEvent(evt))
                Debug.Log("Error when processing event: " + evt.GetName());

            if(limitQueueProcessing)
                timer += Time.deltaTime;
        }
    }

    public void OnApplicationQuit() {
        m_listenerTable.Clear();
        m_eventQueue.Clear();
        s_instance = null;
    }
}

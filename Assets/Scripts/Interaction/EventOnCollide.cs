// Author Devon Wayman 
// Date 12/13/2020
using UnityEngine;
using UnityEngine.Events;

public class EventOnCollide : MonoBehaviour {
    [SerializeField] private UnityEvent mainEvent = null;
    public void Execute() {
        Debug.Log("Executing main event", gameObject);
        mainEvent.Invoke();
    }
}

using UnityEngine;
using System.Collections;

public class EventDispatcher : PriorityEventDispatcher<Screw.EventType, object, Screw.EventTypeComparer> 
{
	static EventDispatcher instance = new EventDispatcher();

	public static EventDispatcher Instance {
		get { return instance; }
	}

}
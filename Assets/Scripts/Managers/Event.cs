using System;
using UnityEngine;

public abstract class Event<T> where T : Event<T>
{
	public delegate void EventListener(T info);
	private static event EventListener Listeners;

	private bool _hasFired;

	public static void RegisterListener(EventListener listener)
	{
		Listeners += listener;
	}

	public static void UnregisterListener(EventListener listener)
	{
		Listeners -= listener;
	}

	public void FireEvent()
	{
		if (_hasFired){throw new Exception("Event already been fired.");}

		_hasFired = true;
		Listeners?.Invoke(this as T);
	}
}

public class PickUpEvent : Event<PickUpEvent>
{
	public GameObject item;
	public GameObject player;
}

public class PlayerSpawnEvent : Event<PlayerSpawnEvent>{
	public int playerIndex;
	public GameObject playerGo;
}

public class PlayerDeathEvent : Event<PlayerDeathEvent>
{
	public GameObject kille; //Player who was killed.
	public GameObject killer;
	public string killedWith;
}

public class PlayerWonEvent : Event<PlayerWonEvent>
{
	public GameObject player;
	public int score;
}
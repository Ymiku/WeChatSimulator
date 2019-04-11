using System;


public class EventFactory
{
	private static EventManager<ChatEvent> _chatEventCenter;
	public static EventManager<ChatEvent> chatEventCenter{
		get {
			if (_chatEventCenter == null)
				_chatEventCenter = new EventManager<ChatEvent>();
			return _chatEventCenter;
		}
	}
}

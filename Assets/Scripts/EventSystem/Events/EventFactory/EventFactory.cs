using System;


public class EventFactory
{
    /// <summary>
    /// 数字小键盘事件
    /// </summary>
    private static EventManager<NumberKeypadEvent> _numberKeypadEM;
    public static EventManager<NumberKeypadEvent> numberKeypadEM {
        get {
            if (_numberKeypadEM == null)
                _numberKeypadEM = new EventManager<NumberKeypadEvent>();
            return _numberKeypadEM;
        }
    }
	private static EventManager<ChatEvent> _chatEventCenter;
	public static EventManager<ChatEvent> chatEventCenter{
		get {
			if (_chatEventCenter == null)
				_chatEventCenter = new EventManager<ChatEvent>();
			return _chatEventCenter;
		}
	}
}

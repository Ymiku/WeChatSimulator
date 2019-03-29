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
}

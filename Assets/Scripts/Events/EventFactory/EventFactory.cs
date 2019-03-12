using System;


public class EventFactory
{
    /// <summary>
    /// 测试event
    /// </summary>
    private static EventManager<TestEvent> _testEvent;
    public static EventManager<TestEvent> testEvent
    {
        get
        {
            if (_testEvent == null)
            {
                _testEvent = new EventManager<TestEvent>();
            }
            return _testEvent;
        }
    }


}

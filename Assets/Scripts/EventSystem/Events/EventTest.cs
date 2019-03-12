using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    void OnEnable()
    {
        EventFactory.testEvent.AddListener(TestEvent.Test1, testHandlerLow, EventPriority.Low);
        EventFactory.testEvent.AddListener(TestEvent.Test1, testHandlerHigh, EventPriority.High);
    }

    void OnDisable()
    {
        EventFactory.testEvent.RemoveListener(TestEvent.Test1, testHandlerLow);
        EventFactory.testEvent.RemoveListener(TestEvent.Test1, testHandlerHigh);
    }

    private void testHandlerLow(EventArgs args)
    {
        Debug.Log("low");
        Debug.Log(args);
    }

    private void testHandlerHigh(EventArgs args)
    {
        Debug.Log("high");
        Debug.Log(args);
    }

    public void OnClick()
    {
        EventFactory.testEvent.TriggerEvent(TestEvent.Test1, new EventArgs("000"));
        EventFactory.testEvent.TriggerEvent(TestEvent.Test1, new EventArgs("000"));
    }
}

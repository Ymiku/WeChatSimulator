using System;


public class AddListenerExpection : Exception
{
    public AddListenerExpection(string msg) : base(msg) { }
}
public class RemoveListenerExpection : Exception
{
    public RemoveListenerExpection(string msg) : base(msg) { }
}
public class TriggerEventException : Exception
{
    public TriggerEventException(string msg) : base(msg) { }
}
public class InvalidTypeExpection : Exception
{
    public InvalidTypeExpection(string msg) : base(msg) { }
}
public class SortListenersExpection : Exception
{
    public SortListenersExpection(string msg) : base(msg) { }
}


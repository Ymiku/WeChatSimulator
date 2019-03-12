using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventArgs
{
    private object[] _args;
    public object[] args
    {
        get
        {
            return _args;
        }
    }

    public EventArgs(params object[] _args)
    {
        this._args = _args;
    }

    public override string ToString()
    {
        if (_args.Length == 0)
            return "no params";
        if (_args.Length == 1)
            return _args[0].ToString();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _args.Length; i++)
        {
            if (i == _args.Length - 1)
            {
                sb.Append(_args[i]);
            }
            else
            {
                sb.Append(_args[i].ToString() + ",");
            }
        }
        return sb.ToString();
    }
}

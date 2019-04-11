using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IRXModel
{
    void Execute();
    void SetRxId(int rxId);
	IRXModel Wait(float t);
    IRXModel ExecuteAfterTime(FrostRX.RXFunc f, float t);
    IRXModel ExecuteUntil(FrostRX.RXFunc f, FrostRX.CondFunc con);
    IRXModel ExecuteWhen(FrostRX.RXFunc f, FrostRX.CondFunc con);
    IRXModel ExecuteWhenStay(FrostRX.RXFunc f, FrostRX.CondFunc con, float t);
    IRXModel Execute(FrostRX.RXFunc f);
    IRXModel ExecuteContinuous(FrostRX.RXFunc f, float t);
    IRXModel GoToBegin();
    void GetId(RXIndex id);
}
public class RXModelBase:IRXModel
{
    public int rxId;
    public IRXModel model = null;
    public FrostRX.RXFunc func;
    public virtual void Execute()
    {

    }
    public void SetRxId(int rxId)
    {
        this.rxId = rxId;
    }
    public void GetId(RXIndex id)
    {
        id.value = rxId;
        FrostRX.Instance.AddIndex(id);
    }
    protected void TryNext()
    {
        if (model != null)
        {
            FrostRX.Instance.activeModelList.Add(model);
        }
        else
        {
            RXIndex.TEMP.value = rxId;
            FrostRX.Instance.EndRxById(RXIndex.TEMP);
        }
        FrostRX.Instance.activeModelList.Remove(this);
    }
    public IRXModel GoToBegin()
    {
        model = new RXGoToBegin();
        model.SetRxId(rxId);
        return model;
    }
	public IRXModel Wait(float time)
	{
		model = new RXWaitModel(time);
		model.SetRxId(rxId);
		return model;
	}
    public IRXModel ExecuteAfterTime(FrostRX.RXFunc f, float t)
    {
        model = new RXTimeModel(f, t);
        model.SetRxId(rxId);
        return model;
    }
    public IRXModel ExecuteWhen(FrostRX.RXFunc f, FrostRX.CondFunc con)
    {
        model = new RXCondModel(f, con);
        model.SetRxId(rxId);
        return model;
    }
    public IRXModel ExecuteUntil(FrostRX.RXFunc f, FrostRX.CondFunc con)
    {
        model = new RXUntilModel(f, con);
        model.SetRxId(rxId);
        return model;
    }
    public IRXModel ExecuteWhenStay(FrostRX.RXFunc f, FrostRX.CondFunc con, float t)
    {
        model = new RXStayModel(f, con, t);
        model.SetRxId(rxId);
        return model;
    }
    public IRXModel Execute(FrostRX.RXFunc f)
    {
        model = new RXExecuteModel(f);
        model.SetRxId(rxId);
        return model;
    }
    public IRXModel ExecuteContinuous(FrostRX.RXFunc f, float t)
    {
        model = new RXContinuousModel(f, t);
        model.SetRxId(rxId);
        return model;
    }
}
public class RXRoot : RXModelBase
{
    public Object debugObj;
    public RXRoot(int rxId)
    {
        this.rxId = rxId;
    }
    public sealed override void Execute()
    {
        TryNext();
    }
}
public class RXGoToBegin : RXModelBase
{
    public sealed override void Execute()
    {
        FrostRX.Instance.RestartRxById(rxId);
        if (model != null)
        {
            FrostRX.Instance.activeModelList.Add(model);
        }
    }
}
public class RXTimeModel : RXModelBase
{
    float waitTime;
    float time;
    public RXTimeModel(FrostRX.RXFunc f, float t)
    {
        waitTime = t;
        func = f;
        time = t;
    }
    public sealed override void Execute()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            time = waitTime;
            func();
            TryNext();
        }
    }
}
public class RXCondModel : RXModelBase
{
    public FrostRX.CondFunc cond;
    public RXCondModel(FrostRX.RXFunc f, FrostRX.CondFunc c)
    {
        func = f;
        cond = c;
    }
    public sealed override void Execute()
    {
        if (cond() == true)
        {
            func();
            TryNext();
        }
    }
}
public class RXUntilModel : RXModelBase
{
    public FrostRX.CondFunc cond;
    public RXUntilModel(FrostRX.RXFunc f, FrostRX.CondFunc c)
    {
        func = f;
        cond = c;
    }
    public sealed override void Execute()
    {
        if (cond() == true)
        {
            TryNext();
        }
        else
        {
            func();
        }
    }
}
public class RXStayModel : RXModelBase
{
    public float time;
    public FrostRX.CondFunc cond;
    private bool _isStay = false;
    private float _time;
    public RXStayModel(FrostRX.RXFunc f, FrostRX.CondFunc c, float t)
    {
        this.func = f;
        this.cond = c;
        time = t;
        this._time = time;
    }
    public sealed override void Execute()
    {
        if (cond())
        {
            _isStay = true;
            time -= Time.deltaTime;
            if (time <= 0f)
            {
                func();
                TryNext();
				time = _time;
				_isStay = false;
            }
        }
        else if (_isStay)
        {
            time = _time;
            _isStay = false;
        }
    }
}
public class RXExecuteModel : RXModelBase
{
    public RXExecuteModel(FrostRX.RXFunc f)
    {
        func = f;
    }
    public sealed override void Execute()
    {
        func();
        TryNext();
    }
}
public class RXContinuousModel : RXModelBase
{
	float waitTime;
    public float time;
    public RXContinuousModel(FrostRX.RXFunc f, float t)
    {
        func = f;
        waitTime = time = t;
    }
    public sealed override void Execute()
    {
        time -= Time.deltaTime;
        func();
        if (time <= 0f)
        {
            TryNext();
			time = waitTime;
        }
    }
}
public class RXWaitModel : RXModelBase
{
	float waitTime;
	public float time;
	public RXWaitModel(float t)
	{
		waitTime = time = t;
	}
	public sealed override void Execute()
	{
		time -= Time.deltaTime;
		if (time <= 0f)
		{
			TryNext();
			time = waitTime;
		}
	}
}
public class FrostRX : Singleton<FrostRX>
{
    public delegate void RXFunc();
    public delegate bool CondFunc();
    private Dictionary<int, IRXModel> _rxDic = new Dictionary<int, IRXModel>();
    private Dictionary<int, RXIndex> _indexDic = new Dictionary<int, RXIndex>();
    public List<IRXModel> activeModelList = new List<IRXModel>();
    int rxId = 0;
    public static IRXModel Start()
    {
        return Instance.StartRX();
    }
    public static IRXModel Start(Object o)
    {
        return Instance.StartRX(o);
    }
	public static void End(RXIndex id)
	{
		Instance.EndRxById(id);
	}
    public IRXModel StartRX()
    {
        IRXModel model;
        int id = GetRxId();
        model = new RXRoot(id);
        _rxDic.Add(id,model);
        activeModelList.Add(model);
        return model;
    }
    public IRXModel StartRX(Object o)
    {
        RXRoot root = StartRX() as RXRoot;
        root.debugObj = o;
        return root;
    }
    public void EndRxById(RXIndex rxId)
    {
		if (rxId.value == -1)
			return;
        if (_rxDic.ContainsKey(rxId.value))
            _rxDic.Remove(rxId.value);
        if (_indexDic.ContainsKey(rxId.value))
            _indexDic.Remove(rxId.value);
        for (int i = 0; i < activeModelList.Count; i++)
        {
            if ((activeModelList[i] as RXModelBase).rxId == rxId.value)
            {
                activeModelList.RemoveAt(i);
                i--;
            }
        }
        rxId.value = -1;
    }
    public void RestartRxById(int rxId)
    {
		for (int i = 0; i < activeModelList.Count; i++)
		{
			if ((activeModelList[i] as RXModelBase).rxId == rxId)
			{
				activeModelList.RemoveAt(i);
				i--;
			}
		}
        if (_rxDic.ContainsKey(rxId))
            activeModelList.Add(_rxDic[rxId]);
    }
    int GetRxId()
    {
        while (_rxDic.ContainsKey(rxId))
        {
            if (rxId == int.MaxValue)
                rxId = -1;
            rxId++;
        }
        return rxId++;
    }
    public void AddIndex(RXIndex index)
    {
        _indexDic.Add(index.value,index);
    }
    public void Execute()
    {
        for (int i = activeModelList.Count - 1; i >= 0; i--)
        {
            try
            {
                activeModelList[i].Execute();
            }
            catch (System.Exception ex)
            {
                int id = (activeModelList[i] as RXModelBase).rxId;
#if UNITY_EDITOR
                Debug.LogError("RX Error:ID="+(activeModelList[i] as RXModelBase).rxId.ToString()+",debugObj="+(_rxDic[id] as RXRoot).debugObj.ToString());
                throw ex;
#endif
                RXIndex.TEMP.value = id;
                EndRxById(RXIndex.TEMP);
            }
        }
    }
    public void Clear()
    {
        activeModelList.Clear();
        _rxDic.Clear();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
namespace Compiler
{
	public class HackStudioCode : UnitySingleton<HackStudioCode> {
		
		System.Type[] statementsTypeArray;
		public GameObject statementGo;
		public GameObject paramGo;
		public List<StatementBase> program = new List<StatementBase>();
		Stack<StatementBase> stateStack = new Stack<StatementBase> ();
        List<IDEItem> statementGoPool = new List<IDEItem>();
        List<IDEItem> paramGoPool = new List<IDEItem>();
        int curLine;
		public override void SingletonInit ()
		{
			base.SingletonInit ();
			statementsTypeArray = typeof(StatementBase).Assembly.GetTypes ().Where (type => type.IsSubclassOf (typeof(StatementBase)) && !type.IsAbstract).ToArray<Type> ();
			statementGo.SetActive (false);
			paramGo.SetActive (false);
			StatementBase b = new StatementEmpty ();
			b.GetParam (0).Set (">>  ");
			program.Add(b);
			OnFocusChange (0);
		}
		public void Show(StatementBase statement)
		{
            for (int i = 0; i < paramGoPool.Count; i++)
            {
                paramGoPool[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < statementGoPool.Count; i++)
            {
                statementGoPool[i].gameObject.SetActive(false);
            }
            Grammar grammar = statement.GetGrammar ();
			IDEItem item;
            int paramIndex = 0;
			for (int i = 0; i < grammar.Count; i++) {
				if (grammar [i].StartsWith ("*Param:")) {
					item = GetPrefab (paramGo);
                    item.id = paramIndex;
                    item.SetIDEData(statement.GetParam(paramIndex));
                    paramGoPool.Add(item);
                    paramIndex++;
				} else {
					item = GetPrefab (statementGo);
					item.SetIDEData (grammar[i]);
                    statementGoPool.Add(item);
				}
				item.gameObject.SetActive (true);
			}
		}
		public void Execute()
		{
			program[program.Count-1].Execute();
			program.Add(new StatementEmpty().SetParam(0,new Parameter().Set(">>  ")));
			OnFocusChange (program.Count-1);
		}
		public void Log(string log)
		{
			
		}
		public void OnFocusChange(int now)
		{
			curLine = now;
			stateStack.Clear ();
			stateStack.Push (program[now]);
			Show (stateStack.Peek());
		}
		public List<Type> GetTypesByReturnValue(VarType t)
		{
			List<Type> types = new List<Type> ();
			if (t != VarType.Void) {
				for (int i = 0; i < statementsTypeArray.Length; i++) {
					MethodInfo method = statementsTypeArray [i].GetMethod ("GetReturnValueType");
					VarType t2 = (VarType)(method.Invoke (null, null));
					if (t2 == t)
						types.Add (statementsTypeArray [i]);
				}
			} else {
				for (int i = 0; i < statementsTypeArray.Length; i++) {
					types.Add (statementsTypeArray [i]);
				}
			}
			Debug.Log (t);
			if (stateStack.Count >= 2) {
				types.Remove (typeof(StatementDefine));
			}
			return types;
		}
		IDEItem GetPrefab(GameObject go)
		{
			GameObject g = GameObject.Instantiate (go);
			g.transform.SetParent(go.transform.parent);
			g.transform.localScale = Vector3.one;
			g.transform.SetAsLastSibling ();
			return g.GetComponent<IDEItem> ();
		}
		public void StepIn(int index)
		{
			StatementBase state = stateStack.Peek ();
			stateStack.Push (state.GetParam(index));
			Show (stateStack.Peek());
		}
		public void SetParam(int i,Parameter p)
		{
			stateStack.Peek ().SetParam (i,p);
		}
        public void TracingBack()
        {
            stateStack.Pop();
            Show(stateStack.Peek());
        }
		Dictionary<string,Parameter> name2p = new Dictionary<string, Parameter>();
		public void AddVar(string name,Parameter p)
		{
			if(!name2p.ContainsKey(name))
				name2p.Add (name,p);
		}
		public Parameter GetVar(string name)
		{
			return name2p [name];
		}
	}
}
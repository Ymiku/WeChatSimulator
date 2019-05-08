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
		int curLine;
		public override void SingletonInit ()
		{
			base.SingletonInit ();
			statementsTypeArray = typeof(StatementBase).Assembly.GetTypes ().Where (type => type.IsSubclassOf (typeof(StatementBase)) && !type.IsAbstract).ToArray<Type> ();
			statementGo.SetActive (false);
			paramGo.SetActive (false);
			program.Add (new StatementEntry());
			program.Add (new StatementFor());
			stateStack.Clear ();
			stateStack.Push (program[1]);
			Show (stateStack.Peek());
			GetTypesByReturnValue (VarType.Void);
		}
		public void Show(StatementBase statement)
		{
			Grammar grammar = statement.GetGrammar ();
			IDEItem item;
            int paramIndex = 0;
			for (int i = 0; i < grammar.Count; i++) {
				if (grammar [i].StartsWith ("*Param:")) {
					item = GetPrefab (paramGo);
                    item.id = paramIndex;
                    item.SetIDEData(statement.GetParam(paramIndex));
                    paramIndex++;
				} else {
					item = GetPrefab (statementGo);
					item.SetIDEData (grammar[i]);
				}
				item.gameObject.SetActive (true);
			}
		}
		public void Log(string log)
		{
			
		}
		public void OnFocusChange(int last,int now)
		{
			
		}
		public List<Type> GetTypesByReturnValue(VarType t)
		{
			List<Type> types = new List<Type> ();
			for (int i = 0; i < statementsTypeArray.Length; i++) {
				MethodInfo method = statementsTypeArray [i].GetMethod ("GetReturnValueType");
				VarType t2 = (VarType)(method.Invoke(null,null));
				if(t2==t)
					types.Add (statementsTypeArray[i]);
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
	}
}
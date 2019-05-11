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
        public TextProxy programCacheText;
		public List<StatementBase> program = new List<StatementBase>();
        List<string> programCache = new List<string>();
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
		public bool isDefine()
		{
			return stateStack.Peek () is StatementDefine;
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
            programCache.Add(program[program.Count - 1].GenerateCode());
            program.Add(new StatementEmpty());
            programCacheText.text = "";
            for (int i = 0; i < programCache.Count; i++)
            {
                programCacheText.text += programCache[i];
                programCacheText.text += "\n";
            }
            programCacheText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,programCacheText.preferredWidth);
            programCacheText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, programCacheText.preferredHeight);
            OnFocusChange (program.Count-1);
		}
		public TextProxy logText;
		public void Log(string log)
		{
			logText.text = "LOG:" + log;
		}
		public void OnFocusChange(int now)
		{
			curLine = now;
			stateStack.Clear ();
			stateStack.Push (program[now]);
			Show (stateStack.Peek());
		}
		public List<string> GetTypesByReturnValue(VarType t)
		{
			List<string> types = new List<string> ();
			if (t != VarType.Void) {
				for (int i = 0; i < statementsTypeArray.Length; i++) {
					MethodInfo method = statementsTypeArray [i].GetMethod ("GetReturnValueType");
					VarType t2 = (VarType)(method.Invoke (null, null));
					if (t2 == t)
						types.Add (statementsTypeArray [i].FullName);
				}
			} else {
				for (int i = 0; i < statementsTypeArray.Length; i++) {
					types.Add (statementsTypeArray [i].FullName);
				}
			}
			if (stateStack.Count >= 2) {
				types.Remove (typeof(StatementDefine).FullName);
			}
			return types;
		}
		public List<string> GetValueTypes()
		{
			List<string> types = new List<string> ();
			for (int i = 0; i < statementsTypeArray.Length; i++) {
				MethodInfo method = statementsTypeArray [i].GetMethod ("GetReturnValueType");
				VarType t2 = (VarType)(method.Invoke (null, null));
				if (t2 != VarType.Void)
					types.Add (statementsTypeArray [i].FullName);
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
            if ((StatementBase)state.GetParam(index) == null)
                return;
            if (!((StatementBase)state.GetParam(index)).HasParam())
                return;
            stateStack.Push (state.GetParam(index));
			Show (stateStack.Peek());
		}
		public void SetParam(int i,Parameter p)
		{
			stateStack.Peek ().SetParam (i,p);
		}
        public void TracingBack()
        {
            if (stateStack.Count <= 1)
                return;
            stateStack.Pop();
            Show(stateStack.Peek());
        }
        void Refresh()
        {
            
        }
		Dictionary<string,Parameter> name2p = new Dictionary<string, Parameter>();
		public void AddVar(string name,Parameter p)
		{
			if (!name2p.ContainsKey (name))
				name2p.Add (name, p);
			else
				name2p[name] = p;
		}
		public Parameter GetVar(string name)
		{
			return name2p [name];
		}

		public void AddVar(string varName,string value)
		{
			if (value.ToLower().Equals ("true")) {
				AddVar (varName,new Parameter().Set(true));
				return;
			}
			if (value.ToLower().Equals ("false")) {
				AddVar (varName,new Parameter().Set(false));
				return;
			}
			try {
				int i = Convert.ToInt32(value);
				AddVar (varName,new Parameter().Set(i));
			} catch (Exception ex) {
				AddVar (varName,new Parameter().Set(value));
			}
		}
		public List<string> GetAllVarByType(VarType t)
		{
			List<string> strs = new List<string> ();
			foreach (var item in name2p.Keys) {
				if (name2p[item].paramType == t)
					strs.Add (item);
			}
			return strs;
		}
	}
}
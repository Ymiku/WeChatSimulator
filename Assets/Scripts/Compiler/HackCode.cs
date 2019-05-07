using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class HackCode : UnitySingleton<HackCode> {
		System.Type[] statementsTypeArray = new System.Type[] {
			typeof(StatementIf),
		};
		public GameObject statementGo;
		public GameObject paramGo;
		public List<StatementBase> program = new List<StatementBase>();
		int curLine;
		public override void SingletonInit ()
		{
			base.SingletonInit ();
			statementGo.SetActive (false);
			paramGo.SetActive (false);
			program.Add (new StatementEntry());
			program.Add (new StatementIf());
			Show (program[1]);
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
                    item.SetData(statement.GetParam(paramIndex));
                    paramIndex++;
				} else {
					item = GetPrefab (statementGo);
                    item.SetData(null);
					item.SetText (grammar[i]);
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
		IDEItem GetPrefab(GameObject go)
		{
			GameObject g = GameObject.Instantiate (go);
			g.transform.SetParent(go.transform.parent);
			g.transform.localScale = Vector3.one;
			g.transform.SetAsLastSibling ();
			return g.GetComponent<IDEItem> ();
		}
	}
}
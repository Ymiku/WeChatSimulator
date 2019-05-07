using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class HackCode : UnitySingleton<HackCode> {
		System.Type[] statementsTypeArray = new System.Type[] {
			typeof(StatementIf),
		};
		public List<StatementBase> program = new List<StatementBase>();
		int curLine;
		public void Log(string log)
		{
			
		}
		public void OnFocusChange(int last,int now)
		{
			
		}
	}
}
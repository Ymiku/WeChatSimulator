using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class StatementLog :  StatementBase{
		public static VarType GetReturnValueType()
		{
			return VarType.Void;
		}
		protected override void GenerateGrammar ()
		{
			grammar.Push ("LOG:");
			grammar.Push(VarType.Void);
			AddParam (new Parameter().SetVoid(true).Set((StatementBase)null));
		}
		public override Parameter Execute ()
		{
			StatementBase statement = GetParam (0);
			if(statement!=null)
				HackStudioCode.Instance.Log(statement.Execute ().ToString());
			return Parameter.Empty;
		}
	}
}
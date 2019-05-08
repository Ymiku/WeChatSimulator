using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class StatementIf :  StatementBase{
		public static VarType GetReturnValueType()
		{
			return VarType.Void;
		}
		protected override void GenerateGrammar ()
		{
			grammar.Push ("if(");
			grammar.Push(VarType.Bool);
			grammar.Push("){");
			grammar.Push (VarType.Void);
			grammar.Push("}");
			SetParam (0,new Parameter().Set(true));
			SetParam (1,new Parameter().Set(null));
		}
		public override Parameter Execute ()
		{
			if (GetParam (0)) {
				StatementBase statement = GetParam (1);
				statement.Execute ();
			}
			return new Parameter ();
		}
	}
}
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
			AddParam (new Parameter().Set(true));
			AddParam (new Parameter().SetVoid(true).Set((StatementBase)null));
		}
		public override Parameter Execute ()
		{
			if (GetParam (0)) {
				StatementBase statement = GetParam (1);
                if(statement!=null)
				statement.Execute ();
			}
			return new Parameter ();
		}
	}
}
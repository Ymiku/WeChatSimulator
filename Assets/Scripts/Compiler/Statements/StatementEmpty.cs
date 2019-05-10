using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class StatementEmpty :  StatementBase{
		public static VarType GetReturnValueType()
		{
			return VarType.Void;
		}
		protected override void GenerateGrammar ()
		{
            grammar.Push(" ");
			grammar.Push(VarType.Void);

			AddParam (new Parameter().SetVoid(true).Set((StatementBase)null));
			Debug.Log (GetParam(0).isVoid);
		}
		public override Parameter Execute ()
		{
			StatementBase statement = GetParam (0);
            if(statement!=null)
			statement.Execute ();


			return new Parameter ();
		}
	}
}
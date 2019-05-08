using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class StatementEntry :  StatementBase{
		public static VarType GetReturnValueType()
		{
			return VarType.Void;
		}
		protected override void GenerateGrammar ()
		{
			grammar.Push(VarType.Void);
			SetParam (0,new Parameter().Set(null));
		}
		public override Parameter Execute ()
		{
			StatementBase statement = GetParam (0);
			statement.Execute ();
			return new Parameter ();
		}
	}
}
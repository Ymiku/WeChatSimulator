using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class StatementDefine :  StatementBase{
		public static VarType GetReturnValueType()
		{
			return VarType.Void;
		}
		protected override void GenerateGrammar ()
		{
			grammar.Push("Define");
			AddParam (new Parameter().SetVoid(true).Set(0));
			grammar.Push("=");
			AddParam (new Parameter().SetVoid(true).Set(0));
		}
		public override Parameter Execute ()
		{
			StatementBase statement = GetParam (0);
			statement.Execute ();
			return new Parameter ();
		}
	}
}
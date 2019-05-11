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
			grammar.Push("Define ");
			grammar.Push(VarType.String);
			grammar.Push(" = ");
			grammar.Push(VarType.Void);
			AddParam (new Parameter().SetVoid(false).Set("var"));
			AddParam (new Parameter().SetVoid(true).Set("0"));
		}
		public override Parameter Execute ()
		{
			string var = GetParam (0);
			string value = GetParam (1).Execute().ToString();
			HackStudioCode.Instance.AddVar (var,value);
			return new Parameter ();
		}
	}
}
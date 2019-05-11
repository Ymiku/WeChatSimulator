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
			HackStudioCode.Instance.Log(GetParam (0).ToString());
			
			return Parameter.Empty;
		}
	}
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class StatementFor :  StatementBase{
		public static VarType GetReturnValueType()
		{
			return VarType.Void;
		}
		protected override void GenerateGrammar ()
		{
			grammar.Push ("for(");
			grammar.Push(VarType.Void);
			grammar.Push(";");
			grammar.Push (VarType.Bool);
			grammar.Push(";");
			grammar.Push (VarType.Void);
			grammar.Push("){");
			grammar.Push (VarType.Void);
			grammar.Push("}");
			AddParam (new Parameter().SetVoid(true).Set((StatementBase)null));
			AddParam (new Parameter().SetVoid(true).Set(true));
			AddParam (new Parameter().SetVoid(true).Set((StatementBase)null));
			AddParam (new Parameter().SetVoid(true).Set((StatementBase)null));
		}
		public override Parameter Execute ()
		{
			IExecuteable statement = GetParam (0);
			IExecuteable statement2 = GetParam (2);
			IExecuteable statement3 = GetParam (3);
			for (statement.Execute(); (bool)GetParam (1); statement2.Execute()) {
				statement3.Execute ();
			}
			return Parameter.Empty;
		}
	}
}
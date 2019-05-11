using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
    public class StatementAssign : StatementBase
    {
        public static VarType GetReturnValueType()
        {
            return VarType.Void;
        }
        protected override void GenerateGrammar()
        {
            grammar.Push(VarType.Void);
            grammar.Push(" = ");
            grammar.Push(VarType.Void);
            AddParam(new Parameter().SetVoid(false).Set("var"));
            AddParam(new Parameter().SetVoid(false).Set(0));
        }
        public override Parameter Execute()
        {
			HackStudioCode.Instance.AddVar (GetParam(0).varName,GetParam(1));
            return Parameter.Empty;
        }
    }
}
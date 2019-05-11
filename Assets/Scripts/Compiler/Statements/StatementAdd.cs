using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
    public class StatementAdd : StatementBase
    {
        public static VarType GetReturnValueType()
        {
            return VarType.Int;
        }
        protected override void GenerateGrammar()
        {
            grammar.Push(VarType.Int);
            grammar.Push(" + ");
            grammar.Push(VarType.Int);
            AddParam(new Parameter().SetVoid(false).Set(0));
            AddParam(new Parameter().SetVoid(false).Set(0));
        }
        public override Parameter Execute()
        {
            int statement1 = GetParam(0);
            int statement2 = GetParam(1);
			int result = statement1 + statement2;
            return new Parameter().Set(result);
        }
    }
}
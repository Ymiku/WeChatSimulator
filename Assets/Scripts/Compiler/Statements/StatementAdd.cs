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
            StatementBase statement1 = GetParam(0);
            StatementBase statement2 = GetParam(0);
            int result = (int)statement1.Execute() + (int)statement2.Execute();
            return new Parameter().Set(result);
        }
    }
}
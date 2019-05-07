using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class Parameter {
		private VarType paramType;
		private StatementBase _statement;
		private bool _bool;
		private int _int;
		private float _float;
		public void Set (StatementBase statement)
		{
			paramType = VarType.Void;
			_statement = statement;
		}
		public void Set (bool b)
		{
			paramType = VarType.Bool;
			_bool = b;
		}
		public void Set (int i)
		{
			paramType = VarType.Int;
			_int = i;
		}
		public void Set (float f)
		{
			paramType = VarType.Float;
			_float = f;
		}
		public static implicit operator StatementBase(Parameter p)
		{
			return p._statement;
		}
		public static implicit operator bool(Parameter p)
		{
			if (p.paramType == VarType.Void)
				return p._statement.Execute ();
			return p._bool;
		}
		public static implicit operator int(Parameter p)
		{
			if (p.paramType == VarType.Void)
				return p._statement.Execute ();
			return p._int;
		}
		public static implicit operator float(Parameter p)
		{
			if (p.paramType == VarType.Void)
				return p._statement.Execute ();
			return p._float;
		}
	}
}
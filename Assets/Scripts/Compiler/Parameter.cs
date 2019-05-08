using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public struct Parameter:IExecuteable {
		public static Parameter Empty = new Parameter();
		public VarType paramType;
		private StatementBase _statement;
		private bool _bool;
		private int _int;
		private float _float;
		public Parameter Execute()
		{
			if (_statement != null)
				return _statement.Execute ();
			return this;
		}
		public Parameter Set (StatementBase statement)
		{
			paramType = VarType.Void;
			_statement = statement;
			return this;
		}
		public Parameter Set (bool b)
		{
			paramType = VarType.Bool;
			_bool = b;
			return this;
		}
		public Parameter Set (int i)
		{
			paramType = VarType.Int;
			_int = i;
			return this;
		}
		public Parameter Set (float f)
		{
			paramType = VarType.Float;
			_float = f;
			return this;
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
		public string GenerateCode ()
		{
			if (_statement != null)
				return _statement.GenerateCode ();
			switch (paramType) {
			case VarType.Bool:
				return _bool.ToString ();
			case VarType.Float:
				return _float.ToString ();
			case VarType.Int:
				return _int.ToString ();
			default:
				return "";
			}
		}
	}
}
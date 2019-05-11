using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class Parameter:IExecuteable {
		public static Parameter Empty = new Parameter();
		public bool isVoid;
		public VarType paramType;
		private StatementBase _statement;
		private bool _bool;
		private int _int;
		private float _float;
		private string _string;
        public string varName = null;
		public Parameter Execute()
		{
			if (_statement != null)
				return _statement.Execute ();
			return this;
		}
		public Parameter SetVoid(bool b)
		{
			isVoid = b;
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
		public Parameter Set (string s)
		{
			paramType = VarType.String;
			_string = s;
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
		public static implicit operator string(Parameter p)
		{
			if (p.paramType == VarType.Void)
				return p._statement.Execute ();
			return p._string;
		}
		public string GenerateCode ()
		{
			if (_statement != null)
				return _statement.GenerateCode ();
			switch (paramType) {
			case VarType.Bool:
				return _bool.ToString ();
			case VarType.Int:
				return _int.ToString ();
			case VarType.String:
				return _string;
			default:
				return "";
			}
		}
		public override string ToString ()
		{
			switch (paramType) {
			case VarType.Bool:
				return _bool.ToString ();
			case VarType.Int:
				return _int.ToString ();
			case VarType.String:
				return _string;
			default:
				return "";
			}
		}
	}
}
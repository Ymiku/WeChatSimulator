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
		public Parameter SetVar(VarType t,string name)
		{
			paramType = t;
			varName = name;
			return this;
		}
		public Parameter Set (StatementBase statement)
		{
			paramType = VarType.Void;
			_statement = statement;
			varName = null;
			return this;
		}
		public Parameter Set (bool b)
		{
			paramType = VarType.Bool;
			_bool = b;
			varName = null;
			return this;
		}
		public Parameter Set (int i)
		{
			paramType = VarType.Int;
			_int = i;
			varName = null;
			return this;
		}
		public Parameter Set (string s)
		{
			paramType = VarType.String;
			_string = s;
			varName = null;
			return this;
		}
		public static implicit operator StatementBase(Parameter p)
		{
			return p._statement;
		}
		public static implicit operator bool(Parameter p)
		{
			if (p.varName != null)
				return (bool)HackStudioCode.Instance.GetVar (p.varName);
			if (p._statement != null)
				return p._statement.Execute ();
			return p._bool;
		}
		public static implicit operator int(Parameter p)
		{
			if (p.varName != null)
				return (int)HackStudioCode.Instance.GetVar (p.varName);
			if (p._statement != null)
				return p._statement.Execute ();
			return p._int;
		}

		public static implicit operator string(Parameter p)
		{
			if (p.varName != null)
				return (string)HackStudioCode.Instance.GetVar (p.varName);
			if (p._statement != null)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class Parameter {
		private Attribute paramType;
		private StatementBase _statement;
		private bool _bool;
		private int _int;
		private float _float;
		public void Set (StatementBase statement)
		{
			paramType = Attribute.Void;
			_statement = statement;
		}
		public void Set (bool b)
		{
			paramType = Attribute.Bool;
			_bool = b;
		}
		public void Set (int i)
		{
			paramType = Attribute.Int;
			_int = i;
		}
		public void Set (float f)
		{
			paramType = Attribute.Float;
			_float = f;
		}
		public static implicit operator StatementBase(Parameter p)
		{
			return p._statement;
		}
		public static implicit operator bool(Parameter p)
		{
			if (p.paramType == Attribute.Void)
				return p._statement.Execute ();
			return p._bool;
		}
		public static implicit operator int(Parameter p)
		{
			if (p.paramType == Attribute.Void)
				return p._statement.Execute ();
			return p._int;
		}
		public static implicit operator float(Parameter p)
		{
			if (p.paramType == Attribute.Void)
				return p._statement.Execute ();
			return p._float;
		}
	}
}
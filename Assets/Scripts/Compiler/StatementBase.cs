using System.Collections;
using System.Collections.Generic;
namespace Compiler
{
	public enum VarType
	{
		Void,
		Int,
		Float,
		Bool
	}
	public class StatementBase {
		protected List<Parameter> _params = new List<Parameter>();
		protected Grammar grammar = new Grammar();
		protected virtual void GenerateGrammar()
		{
		}
		public Grammar GetGrammar()
		{
			return grammar;
		}
		public virtual Parameter Execute()
		{
			return new Parameter ();
		}
		public void SetParam(int attrId,Parameter param)
		{
			while (_params.Count<=attrId) {
				_params.Add (null);
			}
			_params [attrId] = param;
		}
		public Parameter GetParam(int i)
		{
			return _params [i];			
		}
	}
}
using System.Collections;
using System.Collections.Generic;
namespace Compiler
{
	public interface IExecuteable
	{
		Parameter Execute ();
	}
	public enum VarType
	{
		Void,
		Int,
		Float,
		Bool
	}
	public class StatementBase :IExecuteable{
		protected List<Parameter> _params = new List<Parameter>();
		protected Grammar grammar = new Grammar();
		protected System.Text.StringBuilder sb = new System.Text.StringBuilder();

		public StatementBase()
		{
			GenerateGrammar ();
		}
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
				_params.Add (new Parameter());
			}
			_params [attrId] = param;
		}
		public Parameter GetParam(int i)
		{
			return _params [i];			
		}
		public override string ToString ()
		{
			int paramIndex = 0;
			sb.Length = 0;
			for (int i = 0; i < grammar.Count; i++) {
				if (grammar [i].StartsWith ("*Param:")) {
					sb.Append(_params[paramIndex].ToString());
					paramIndex++;
				} else {
					sb.Append (grammar[i]);
				}
			}
			return sb.ToString ();
		}
	}
}
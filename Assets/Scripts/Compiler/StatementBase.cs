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
		Bool,
		String,
	}
	public class StatementBase :IExecuteable{
		public static StatementEmpty Empty = new StatementEmpty();
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
		public StatementBase AddParam(Parameter param)
		{
			_params.Add (param);
			return this;
		}
		public StatementBase SetParam(int attrId,Parameter param)
		{
			param.isVoid = GetParam (attrId).isVoid;
			_params [attrId] = param;
			return this;
		}
        public bool HasParam()
        {
            return _params.Count > 0;
        }
		public Parameter GetParam(int i)
		{
			return _params [i];			
		}
		public virtual string GenerateCode ()
		{
			int paramIndex = 0;
			sb.Length = 0;
			for (int i = 0; i < grammar.Count; i++) {
				if (grammar [i].StartsWith ("*Param:")) {
					sb.Append(_params[paramIndex].GenerateCode());
					paramIndex++;
				} else {
					sb.Append (grammar[i]);
				}
			}
			return sb.ToString ();
		}
	}
}
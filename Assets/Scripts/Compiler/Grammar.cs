using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	public class Grammar{
		private List<string> grammars = new List<string>();
		public int Count{
			get{ return grammars.Count;}
		}
		public string this [int i]
		{
			get{ return grammars[i]; }
		}
		public void Push(string s)
		{
			grammars.Add (s);
		}
		public void Push(VarType attr)
		{
			grammars.Add ("*Param:"+(int)attr);
		}

	}
}

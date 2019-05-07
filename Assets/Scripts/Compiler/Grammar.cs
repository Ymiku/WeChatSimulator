using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Compiler
{
	
	public class Grammar{
		private List<string> grammars = new List<string>();
		public void Push(string s)
		{
			grammars.Add (s);
		}
		public void Push(Attribute attr)
		{
			grammars.Add ("Attr:"+(int)attr);
		}

	}
}

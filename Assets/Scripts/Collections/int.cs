using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int {
	int value;
	public static Int operator< (Int lhs,Int rhs)
	{
		lhs.value = rhs.value;
		return lhs;
	}
	public static Int operator> (Int lhs,Int rhs)
	{
		rhs.value = lhs.value;
		return rhs;
	}
	public static implicit operator Int(int i)
	{
		return new Int(){value = i};
	}
	public static implicit operator int(Int i)
	{
		return i.value;
	}
}

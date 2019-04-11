using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RXIndex {
    public static RXIndex TEMP = new RXIndex();
	public int value = -1;
	public static RXIndex operator< (RXIndex lhs,RXIndex rhs)
	{
		lhs.value = rhs.value;
		return lhs;
	}
	public static RXIndex operator> (RXIndex lhs,RXIndex rhs)
	{
		rhs.value = lhs.value;
		return rhs;
	}
	public static implicit operator int(RXIndex i)
	{
        return i.value;
	}
}

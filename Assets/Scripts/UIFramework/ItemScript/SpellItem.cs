using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : ItemBase
{
    public TextProxy spell;
    public void SetData(string s)
    {
        spell.text = s;
    }
}

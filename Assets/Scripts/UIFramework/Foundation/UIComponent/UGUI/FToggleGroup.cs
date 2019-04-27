using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ToggleGroup))]
public class FToggleGroup : MonoBehaviour
{
    ToggleGroup _group;
    public List<Toggle> toggleList;
    public delegate void GroupEvent(int index);
    public GroupEvent onChangedIndex;

    private void Awake()
    {
        _group = GetComponent<ToggleGroup>();
        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].group == null)
                toggleList[i].group = _group;
            else if (toggleList[i].group != _group)
                toggleList[i].group = _group;
            int index = i;
            toggleList[i].onValueChanged.AddListener((bool active) =>
            {
                if (active && onChangedIndex != null)
                {
                    onChangedIndex(index);
                }
            });
        }
    }
}
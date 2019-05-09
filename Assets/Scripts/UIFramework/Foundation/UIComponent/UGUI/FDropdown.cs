using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FDropdown : MonoBehaviour {
    public float width;
    public ButtonProxy templet;
    public Vector2 insidePadding;
    public Vector2 outsidePadding;
    List<ButtonProxy> buttons = new List<ButtonProxy>();
    Vector2 ori;
    List<Vector2> pos = new List<Vector2>();
    int index = 0;
    // Use this for initialization

    void OnEnable()
    {
        if (GetComponent<Button>()!=null)
            GetComponent<Button>().onClick.AddListener(Show);
    }
    void OnDisable()
    {
        if (GetComponent<Button>() != null)
            GetComponent<Button>().onClick.RemoveAllListeners();
    }
    public void Clear()
    {
        index = 0;
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void AddOption(string s)
    {
        if (buttons.Count <= index)
        {
            buttons.Add(GameObject.Instantiate<ButtonProxy>(templet));
            buttons[index].transform.parent = templet.transform.parent;
            buttons[index].transform.localScale = Vector3.one;
        }
        buttons[index].info = s;
        buttons[index].text.sizeDelta = Vector2.one * 800.0f;
        buttons[index].text.sizeDelta = new Vector2(buttons[index].text.preferredWidth, buttons[index].text.preferredHeight);
        buttons[index].sizeDelta = buttons[index].text.sizeDelta + insidePadding;
        buttons[index].gameObject.SetActive(false);
        int i = index;
        buttons[index].onClick.AddListener(() => { OnClick(i); });
        index++;
    }
    public void OnClick(int i)
    {
        GetComponent<Compiler.IDEItem>().OnValueChange(i);
        Hide();
    }
    bool isShow = false;
    public void Show()
    {
        isShow = true;
        Vector2 oriPos =  new Vector2(0.0f,120.0f);
        if (oriPos.x < 0.0f)
            oriPos = new Vector2(0.0f,oriPos.y);
        if (oriPos.x + width >= Screen.width)
            oriPos = new Vector2(Screen.width-width,oriPos.y);
        this.ori = Vector2.zero;
        float heightCount = oriPos.y;
        float widthCount = oriPos.x;
        pos.Clear();
        for (int i = 0; i < index; i++)
        {
            if (buttons[i].width + widthCount > width)
            {
                widthCount = oriPos.x;
                heightCount += buttons[i].height;
                heightCount += outsidePadding.y;
            }
            buttons[i].anchoredPosition = ori;
            pos.Add(new Vector2(widthCount,heightCount));
            widthCount += buttons[i].width;
            widthCount += outsidePadding.x;
            buttons[i].gameObject.SetActive(true);
        }
    }
    public void Hide()
    {
        isShow = false;
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (isShow)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                buttons[i].anchoredPosition = Vector2.Lerp(buttons[i].anchoredPosition,pos[i],4.0f*Time.deltaTime);
            }
        }
    }
}

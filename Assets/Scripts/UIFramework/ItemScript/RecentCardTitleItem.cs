
public class RecentCardTitleItem : ItemBase
{
    public TextProxy _titleText;

    public void SetData(string o)
    {
        _titleText.text = o.ToString();
    }
}
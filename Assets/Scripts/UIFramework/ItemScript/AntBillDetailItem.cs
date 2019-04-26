
public class AntBillDetailItem : ItemBase
{
    public TextProxy _detailText;
    public TextProxy _moneyText;
    public TextProxy _remarkText;

    public override void SetData(object o)
    {
        base.SetData(o);
        TransactionSaveData data = o as TransactionSaveData;
        _remarkText.text = data.remarkStr;
        _moneyText.text = data.money.ToString("0.00");
        _detailText.text = data.detailStr;
    }
}


public partial class SaveData
{
    public PaywayType curPayway = PaywayType.None;

    public PaywayType SetCurPaywayByMoney(double money)
    {
        AssetsSaveData data = GetAssetsData(GameManager.Instance.curUserId);
        switch (curPayway)
        {
            case PaywayType.None:
                if (data.balance >= money)
                    curPayway = PaywayType.Banlance;
                else if (data.yuEBao >= money)
                    curPayway = PaywayType.YuEBao;
                else if (curUseBankCard != null)
                    curPayway = PaywayType.BankCard;
                break;
            case PaywayType.Banlance:
                if (data.balance < money)
                {
                    curPayway = data.yuEBao >= money ? PaywayType.YuEBao :
                        curUseBankCard != null ? PaywayType.BankCard : PaywayType.None;
                }
                break;
            case PaywayType.YuEBao:
                if (data.yuEBao < money)
                {
                    curPayway = data.balance >= money ? PaywayType.Banlance :
                        curUseBankCard != null ? PaywayType.BankCard : PaywayType.None;
                }
                break;
            case PaywayType.BankCard:
                break;
        }
        return curPayway;
    }
}

public enum PaywayType
{
    None,
    Banlance,
    YuEBao,
    BankCard,
}
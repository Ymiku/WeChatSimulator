
public class ContentHelper{

    public static string Read(int id) {
        return StaticDataContent.GetContent(id);
    }

    //content表里id的define
    public const int CanNotTransToSelf = 2;  //不能给自己转账  
    public const int TransAccountNotExist = 3;  //账户不存在或对方设置了隐私保护
    public const int IllegalInput = 4;  //您的输入不合法
}

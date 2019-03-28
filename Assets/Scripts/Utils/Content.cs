
public class ContentHelper{

    public static string Read(uint id) {
        return StaticDataContent.GetContent(id);
    }

    //content表里id的define
    public const uint CanNotTransToSelf = 2;  //不能给自己转账  
    public const uint TransAccountNotExist = 3;  //账户不存在或对方设置了隐私保护
}

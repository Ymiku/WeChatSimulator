//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: static_data_album.proto
namespace static_data
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ALBUM")]
  public partial class ALBUM : global::ProtoBuf.IExtensible
  {
    public ALBUM() {}
    
    private int _pic_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"pic_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int pic_id
    {
      get { return _pic_id; }
      set { _pic_id = value; }
    }
    private int _user_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"user_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int user_id
    {
      get { return _user_id; }
      set { _user_id = value; }
    }
    private string _album_name;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"album_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string album_name
    {
      get { return _album_name; }
      set { _album_name = value; }
    }
    private string _pic_path;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"pic_path", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string pic_path
    {
      get { return _pic_path; }
      set { _pic_path = value; }
    }
    private bool _is_secret;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"is_secret", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool is_secret
    {
      get { return _is_secret; }
      set { _is_secret = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ALBUM_ARRAY")]
  public partial class ALBUM_ARRAY : global::ProtoBuf.IExtensible
  {
    public ALBUM_ARRAY() {}
    
    private readonly global::System.Collections.Generic.List<static_data.ALBUM> _items = new global::System.Collections.Generic.List<static_data.ALBUM>();
    [global::ProtoBuf.ProtoMember(1, Name=@"items", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<static_data.ALBUM> items
    {
      get { return _items; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: static_data_comment.proto
namespace static_data
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"COMMENT")]
  public partial class COMMENT : global::ProtoBuf.IExtensible
  {
    public COMMENT() {}
    
    private int _comment_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"comment_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int comment_id
    {
      get { return _comment_id; }
      set { _comment_id = value; }
    }
    private int _user_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"user_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int user_id
    {
      get { return _user_id; }
      set { _user_id = value; }
    }
    private int _target_type;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"target_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int target_type
    {
      get { return _target_type; }
      set { _target_type = value; }
    }
    private string _comment_info;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"comment_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string comment_info
    {
      get { return _comment_info; }
      set { _comment_info = value; }
    }
    private int _order;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"order", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int order
    {
      get { return _order; }
      set { _order = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"COMMENT_ARRAY")]
  public partial class COMMENT_ARRAY : global::ProtoBuf.IExtensible
  {
    public COMMENT_ARRAY() {}
    
    private readonly global::System.Collections.Generic.List<static_data.COMMENT> _items = new global::System.Collections.Generic.List<static_data.COMMENT>();
    [global::ProtoBuf.ProtoMember(1, Name=@"items", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<static_data.COMMENT> items
    {
      get { return _items; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
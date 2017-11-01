using System;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Util
{
    public interface IAccessControlListEx
    {
        bool IsGranted(Message msg, params string[] principals);
        IAccessControlListEx Grant(Type opMsgType, string resource, params string[] principals);
        IAccessControlListEx RevokeGrant(Type opMsgType, string resource, params string[] principals);

        bool IsDenied(Message msg, string resource, params string[] principals);
        IAccessControlListEx Deny(Type opMsgType, string resource, params string[] principals);
        IAccessControlListEx RevokeDeny(Type opMsgType, string resource, params string[] principals);

        string Explain(Message msg, params string[] principals);
        string Explain(Type opMsgType, string resource, params string[] principals);
    }
}

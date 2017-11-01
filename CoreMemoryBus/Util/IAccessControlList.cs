using System;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Util
{
    public interface IAccessControlList
    {
        bool IsGranted(Message msg, User user);
        IAccessControlList Grant(Type msgType, params Principal[] principals);
        IAccessControlList RevokeGrant(Type msgType, params Principal[] principals);

        bool IsDenied(Message msg, User user);
        IAccessControlList Deny(Type msgType, params Principal[] principals);
        IAccessControlList RevokeDeny(Type msgType, params Principal[] principals);

        string Explain(Message msg, Principal principal);
        string Explain(Type msgType, Principal principal);
        string Explain(Message msg, User user);
        string Explain(Type msgType, User user);
    }
}

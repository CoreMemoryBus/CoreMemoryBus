using System;

namespace CoreMemoryBus.Util
{
    public interface IAccessControlList
    {
        bool IsGranted(Type msgType, params string[] principals);
        IAccessControlList Grant(Type msgType, params string[] principals);
        IAccessControlList RevokeGrant(Type msgType, params string[] principals);

        bool IsDenied(Type msgType, params string[] principals);
        IAccessControlList Deny(Type msgType, params string[] principals);
        IAccessControlList RevokeDeny(Type msgType, params string[] principals);

        string Explain(Type msgType, params string[] principals);
    }
}

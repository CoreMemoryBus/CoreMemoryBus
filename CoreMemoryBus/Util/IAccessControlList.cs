using System;

namespace CoreMemoryBus.Util
{
    public interface IAccessControlList
    {
        bool IsGranted(string[] principals, Type msgType);
        void Grant(string principal, Type msgType);
        void RevokeGrant(string principal, Type msgType);

        bool IsDenied(string[] principals, Type msgType);
        void Deny(string principal, Type msgType);
        void RevokeDeny(string principal, Type msgType);

        string Explain(string[] principals, Type msgType);
    }
}

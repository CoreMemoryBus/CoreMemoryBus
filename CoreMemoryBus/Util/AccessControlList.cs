using System;
using System.Linq;
using CoreMemoryBus.DataStructures;

namespace CoreMemoryBus.Util
{
    public class AccessControlList : IAccessControlList
    {
        private readonly HashSetDictionary<Type, string> _granted = new HashSetDictionary<Type, string>();
        private readonly HashSetDictionary<Type, string> _denied = new HashSetDictionary<Type, string>();

        public bool IsGranted(string[] principals, Type msgType)
        {
            Ensure.ArgumentIsNotNull(principals, "principals");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            ISetGrouping<Type, string> grouping;
            if (_granted.TryGetGrouping(msgType, out grouping))
            {
                return principals.Any(principal => grouping.Contains(principal));
            }

            return false;
        }

        public void Grant(string principal, Type msgType)
        {
            Ensure.ArgumentIsNotNull(principal, "principal");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _granted.Add(msgType, principal);
        }

        public void RevokeGrant(string principal, Type msgType)
        {
            Ensure.ArgumentIsNotNull(principal, "principal");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _granted.Remove(msgType, principal);
        }

        public bool IsDenied(string[] principals, Type msgType)
        {
            Ensure.ArgumentIsNotNull(principals, "principals");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            ISetGrouping<Type, string> grouping;
            if (_denied.TryGetGrouping(msgType, out grouping))
            {
                return principals.Any(principal => grouping.Contains(principal));
            }

            return false;
        }

        public void Deny(string principal, Type msgType)
        {
            Ensure.ArgumentIsNotNull(principal, "principal");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _denied.Add(msgType, principal);
        }

        public void RevokeDeny(string principal, Type msgType)
        {
            Ensure.ArgumentIsNotNull(principal, "principal");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _denied.Remove(msgType, principal);
        }

        public string Explain(string[] principals, Type msgType)
        {
            Ensure.ArgumentIsNotNull(principals, "principals");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            ISetGrouping<Type, string> grouping;
            if (_denied.TryGetGrouping(msgType, out grouping))
            {
                if (principals.Any(x => grouping.Contains(x)))
                {
                    return $"Permission was explicitly denied to principals: \"{ToCsv(principals)}\" for instruction: {msgType}";
                }
            }

            if (_granted.TryGetGrouping(msgType, out grouping))
            {
                if (principals.Any(x => grouping.Contains(x)))
                {
                    return "Permission granted.";
                }

                return $"No permissions were granted to any principals: \"{ToCsv(principals)}\" for instruction: {msgType}";
            }

            return $"No access control was configured for principals: \"{ToCsv(principals)}\" for instruction: {msgType}";
        }

        private static string ToCsv(string[] values)
        {
            return string.Join(",", values);
        }
    }
}

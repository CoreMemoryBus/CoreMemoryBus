using System;
using System.Linq;
using CoreMemoryBus.DataStructures;
using System.Collections.Generic;

namespace CoreMemoryBus.Util
{
    public class AccessControlList : IAccessControlList
    {
        private readonly HashSetDictionary<Type, string> _granted = new HashSetDictionary<Type, string>();
        private readonly HashSetDictionary<Type, string> _denied = new HashSetDictionary<Type, string>();

        public bool IsGranted(Type msgType, params string[] principals)
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

        public IAccessControlList Grant(Type msgType, params string[] principals)
        {
            Ensure.ArgumentIsNotNull(principals, "principals");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _granted.Add(msgType, new HashSet<string>(principals));

            return this;
        }

        public IAccessControlList RevokeGrant(Type msgType, params string[] principals)
        {
            Ensure.ArgumentIsNotNull(principals, "principals");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _granted.Remove(msgType, new HashSet<string>(principals));

            return this;
        }

        public bool IsDenied(Type msgType, params string[] principals)
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

        public IAccessControlList Deny(Type msgType, params string[] principals)
        {
            Ensure.ArgumentIsNotNull(principals, "principals");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _denied.Add(msgType, new HashSet<string>(principals));

            return this;
        }

        public IAccessControlList RevokeDeny(Type msgType, params string[] principals)
        {
            Ensure.ArgumentIsNotNull(principals, "principals");
            Ensure.ArgumentIsNotNull(msgType, "msgType");

            _denied.Remove(msgType, new HashSet<string>(principals));

            return this;
        }

        public string Explain(Type msgType, params string[] principals)
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

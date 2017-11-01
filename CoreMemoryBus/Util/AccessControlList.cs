using System;
using System.Linq;
using CoreMemoryBus.Messages;
using CoreMemoryBus.DataStructures;
using System.Collections.Generic;

namespace CoreMemoryBus.Util
{
    public class AccessControlList : IAccessControlList
    {
        private readonly HashSetDictionary<Type, Principal> _granted = new HashSetDictionary<Type, Principal>();
        private readonly HashSetDictionary<Type, Principal> _denied = new HashSetDictionary<Type, Principal>();

        public bool IsGranted(Message msg, User user)
        {
            Ensure.ArgumentIsNotNull(msg, "msg");
            Ensure.ArgumentIsNotNull(user, "user");

            var msgType = msg.GetType();

            ISetGrouping<Type, Principal> grouping;
            if (_granted.TryGetGrouping(msgType, out grouping))
            {
                var principals = user.Principals;
                return principals.Any(principal => grouping.Contains(principal));
            }

            return false;
        }

        public IAccessControlList Grant(Type msgType, params Principal[] principals)
        {
            Ensure.ArgumentIsNotNull(msgType, "msgType");
            Ensure.ArgumentIsNotNull(principals, "principals");

            _granted.Add(msgType, new HashSet<Principal>(principals));

            return this;
        }

        public IAccessControlList RevokeGrant(Type msgType, params Principal[] principals)
        {
            Ensure.ArgumentIsNotNull(msgType, "msgType");
            Ensure.ArgumentIsNotNull(principals, "principals");

            _granted.Remove(msgType, new HashSet<Principal>(principals));

            return this;
        }

        public bool IsDenied(Message msg, User user)
        {
            Ensure.ArgumentIsNotNull(msg, "msg");
            Ensure.ArgumentIsNotNull(user, "user");

            var msgType = msg.GetType();

            ISetGrouping<Type, Principal> grouping;
            if (_denied.TryGetGrouping(msgType, out grouping))
            {
                var principals = user.Principals;
                return principals.Any(principal => grouping.Contains(principal));
            }

            return false;
        }

        public IAccessControlList Deny(Type msgType, params Principal[] principals)
        {
            Ensure.ArgumentIsNotNull(msgType, "msgType");
            Ensure.ArgumentIsNotNull(principals, "principals");

            _denied.Add(msgType, new HashSet<Principal>(principals));

            return this;
        }

        public IAccessControlList RevokeDeny(Type msgType, params Principal[] principals)
        {
            Ensure.ArgumentIsNotNull(msgType, "msgType");
            Ensure.ArgumentIsNotNull(principals, "principals");

            _denied.Remove(msgType, new HashSet<Principal>(principals));

            return this;
        }

        public string Explain(Message msg, Principal principal)
        {
            Ensure.ArgumentIsNotNull(msg, "msg");
            return Explain(msg.GetType(), principal);
        }

        public string Explain(Message msg, User user)
        {
            Ensure.ArgumentIsNotNull(msg, "msg");
            return Explain(msg.GetType(), user);
        }

        public string Explain(Type msgType, Principal principal)
        {
            Ensure.ArgumentIsNotNull(msgType, "msgType");
            Ensure.ArgumentIsNotNull(principal, "principal");

            ISetGrouping<Type, Principal> grouping;
            if (_denied.TryGetGrouping(msgType, out grouping))
            {
                if (grouping.Contains(principal))
                {
                    return $"Permission was explicitly denied to user: \"{principal.Name}\" for instruction: {msgType}";
                }
            }

            if (_granted.TryGetGrouping(msgType, out grouping))
            {
                if (grouping.Contains(principal))
                {
                    return "Permission granted.";
                }

                return $"No permissions were granted to user: \"{principal.Name}\" for instruction: {msgType}";
            }

            return $"No access control was configured for user: \"{principal.Name}\" for instruction: {msgType}";
        }

        public string Explain(Type msgType, User user)
        {
            Ensure.ArgumentIsNotNull(msgType, "msgType");
            Ensure.ArgumentIsNotNull(user, "user");

            ISetGrouping<Type, Principal> grouping;
            if (_denied.TryGetGrouping(msgType, out grouping))
            {
                if (user.Principals.Any(principal => grouping.Contains(principal)))
                {
                    return $"Permission was explicitly denied to user: \"{user.Name}\" for instruction: {msgType}";
                }
            }

            if (_granted.TryGetGrouping(msgType, out grouping))
            {
                if (user.Principals.Any(principal => grouping.Contains(principal)))
                {
                    return "Permission granted.";
                }

                return $"No permissions were granted to user: \"{user.Name}\" for instruction: {msgType}";
            }

            return $"No access control was configured for user: \"{user.Name}\" for instruction: {msgType}";
        }
    }
}

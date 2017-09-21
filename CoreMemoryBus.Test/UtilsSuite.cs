using System;
using CoreMemoryBus.Util;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CoreMemoryBus.Test
{
    public class UtilsSuite : TestSuite
    {
        public UtilsSuite()
            : base("UtilsSuite")
        {}

        [TestFixture]
        public class when_using_an_access_control_list
        {
            private static readonly Type msgType = typeof (MemoryBusSuite.TestMessage);
            private const string principal = "SuperUser";

            [Test]
            public void access_can_be_granted_to_a_principal()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsGranted(msgType, principal));

                acl.Grant(msgType, principal);
                Assert.IsTrue(acl.IsGranted(msgType, principal));
            }

            [Test]
            public void granted_access_can_be_revoked()
            {
                var acl = new AccessControlList();
                acl.Grant(msgType, principal);
                Assert.IsTrue(acl.IsGranted(msgType, principal));

                acl.RevokeGrant(msgType, principal);
                Assert.IsFalse(acl.IsGranted(msgType, principal));
            }

            [Test]
            public void access_can_be_denied_to_a_principal()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsDenied(msgType, principal));

                acl.Deny(msgType, principal);
                Assert.IsTrue(acl.IsDenied(msgType, principal));
            }

            [Test]
            public void denied_access_can_be_revoked()
            {
                var acl = new AccessControlList();
                acl.Deny(msgType, principal);
                Assert.IsTrue(acl.IsDenied(msgType, principal));

                acl.RevokeDeny(msgType, principal);
                Assert.IsFalse(acl.IsDenied(msgType, principal));
            }

            [Test]
            public void access_control_can_be_explained()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsGranted(msgType, principal));

                string result = acl.Explain(msgType, principal);
                Assert.IsTrue(result.Contains("No access control was configured"));

                acl.Grant(msgType, principal);
                result = acl.Explain(msgType, principal);
                Assert.IsTrue(result.Contains("Permission granted."));

                result = acl.Explain(msgType, "User");
                Assert.IsTrue(result.Contains("No permissions were granted to any principals"));

                acl.RevokeGrant(msgType, principal);

                acl.Deny(msgType, principal);
                result = acl.Explain(msgType, principal);
                Assert.IsTrue(result.Contains("Permission was explicitly denied"));
            }
        }
    }
}

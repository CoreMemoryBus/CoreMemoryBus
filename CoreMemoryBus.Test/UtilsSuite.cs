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
            private static readonly MemoryBusSuite.TestMessage testMessage = new MemoryBusSuite.TestMessage();
            private const string principal_ = "SuperUser";
            private static readonly Principal principal = new Principal { Name = principal_ };
            private static readonly User TestUser = new User { Name = "TestUser", Principals = new PrincipalSet { new Principal { Name = "TestUser" }, new Principal { Name = principal_ } } };
            private static readonly User BadUser = new User { Name = "BadUser", Principals = new PrincipalSet { new Principal { Name = "BadUser" }, } };

            [Test]
            public void access_can_be_granted_to_a_principal()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsGranted(testMessage, TestUser));

                acl.Grant(testMessage.GetType(), principal);
                Assert.IsTrue(acl.IsGranted(testMessage, TestUser));
            }

            [Test]
            public void granted_access_can_be_revoked()
            {
                var acl = new AccessControlList();
                acl.Grant(testMessage.GetType(), principal);
                Assert.IsTrue(acl.IsGranted(testMessage, TestUser));

                acl.RevokeGrant(testMessage.GetType(), principal);
                Assert.IsFalse(acl.IsGranted(testMessage, TestUser));
            }

            [Test]
            public void access_can_be_denied_to_a_principal()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsDenied(testMessage, TestUser));

                acl.Deny(testMessage.GetType(), principal);
                Assert.IsTrue(acl.IsDenied(testMessage, TestUser));
            }

            [Test]
            public void denied_access_can_be_revoked()
            {
                var acl = new AccessControlList();
                acl.Deny(testMessage.GetType(), principal);
                Assert.IsTrue(acl.IsDenied(testMessage, TestUser));

                acl.RevokeDeny(testMessage.GetType(), principal);
                Assert.IsFalse(acl.IsDenied(testMessage, TestUser));
            }

            [Test]
            public void access_control_can_be_explained()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsGranted(testMessage, TestUser));

                string result = acl.Explain(testMessage, TestUser);
                Assert.IsTrue(result.Contains("No access control was configured"));

                acl.Grant(testMessage.GetType(), principal);
                result = acl.Explain(testMessage, TestUser);
                Assert.IsTrue(result.Contains("Permission granted."));

                result = acl.Explain(testMessage, BadUser);
                Assert.IsTrue(result.Contains("No permissions were granted to user:"));

                acl.RevokeGrant(testMessage.GetType(), principal);

                acl.Deny(testMessage.GetType(), principal);
                result = acl.Explain(testMessage, TestUser);
                Assert.IsTrue(result.Contains("Permission was explicitly denied"));
            }
        }
    }
}

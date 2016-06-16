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
                Assert.IsFalse(acl.IsGranted(new[] { principal }, msgType));

                acl.Grant(principal, msgType);
                Assert.IsTrue(acl.IsGranted(new[] { principal }, msgType));
            }

            [Test]
            public void granted_access_can_be_revoked()
            {
                var acl = new AccessControlList();
                acl.Grant(principal, msgType);
                Assert.IsTrue(acl.IsGranted(new[] { principal }, msgType));

                acl.RevokeGrant(principal, msgType);
                Assert.IsFalse(acl.IsGranted(new[] { principal }, msgType));
            }

            [Test]
            public void access_can_be_denied_to_a_principal()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsDenied(new[] { principal }, msgType));

                acl.Deny(principal, msgType);
                Assert.IsTrue(acl.IsDenied(new[] { principal }, msgType));
            }

            [Test]
            public void denied_access_can_be_revoked()
            {
                var acl = new AccessControlList();
                acl.Deny(principal, msgType);
                Assert.IsTrue(acl.IsDenied(new[] { principal }, msgType));

                acl.RevokeDeny(principal, msgType);
                Assert.IsFalse(acl.IsDenied(new[] { principal }, msgType));
            }

            [Test]
            public void access_control_can_be_explained()
            {
                var acl = new AccessControlList();
                Assert.IsFalse(acl.IsGranted(new[] { principal }, msgType));

                string result = acl.Explain(new[] {principal}, msgType);
                Assert.IsTrue(result.Contains("No access control was configured"));

                acl.Grant(principal, msgType);
                result = acl.Explain(new[] { principal }, msgType);
                Assert.IsTrue(result.Contains("Permission granted."));

                result = acl.Explain(new[] { "User" }, msgType);
                Assert.IsTrue(result.Contains("No permissions were granted to any principals"));

                acl.RevokeGrant(principal, msgType);

                acl.Deny(principal, msgType);
                result = acl.Explain(new[] { principal }, msgType);
                Assert.IsTrue(result.Contains("Permission was explicitly denied"));
            }
        }
    }
}

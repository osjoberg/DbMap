using System;
using Microsoft.Data.SqlClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerAccessTest
    {
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void DoesNotSerializePublicClassPrivateGetterNoAutoProperty()
        {
            DbAssert.IsTrue("SELECT CAST(@PrivateGetterNoAutoProperty AS BIT)", new PublicClass(privateGetterNoAutoProperty: true));
        }

        [TestMethod]
        public void CanSerializePublicClassPublicGetter()
        {
            DbAssert.IsTrue("SELECT CAST(@PublicGetter AS BIT)", new PublicClass(publicGetter: true));
        }

        [TestMethod]
        public void CanSerializePublicClassPrivateGetter()
        {
            DbAssert.IsTrue("SELECT CAST(@PrivateGetter AS BIT)", new PublicClass(privateGetter: true));
        }

        [TestMethod]
        public void CanSerializePublicClassProtectedGetter()
        {
            DbAssert.IsTrue("SELECT CAST(@ProtectedGetter AS BIT)", new PublicClass(protectedGetter: true));
        }

        [TestMethod]
        public void CanSerializePublicClassInternalGetter()
        {
            DbAssert.IsTrue("SELECT CAST(@InternalGetter AS BIT)", new PublicClass(internalGetter: true));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CannotSerializePublicStructPublicGetter()
        {
            DbAssert.IsTrue("SELECT CAST(@PublicGetter AS BIT)", new PublicStruct(publicGetter: true));
        }
    }
}

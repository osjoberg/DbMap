using System;

using DbMap.Test.Assembly;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerAssemblyAccessTest
    {
        [TestMethod]
        public void DoesNotDeserializePublicClassPrivateSetterNoAutoProperty()
        {
            DbAssert.AreEqual(new AssemblyPublicClass(privateSetterNoAutoProperty: false), "SELECT CAST(1 AS BIT) AS PrivateSetterNoAutoProperty", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassPublicSetter()
        {
            DbAssert.AreEqual(new AssemblyPublicClass(publicSetter: true), "SELECT CAST(1 AS BIT) AS PublicSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassPublicSetterCaseInsensitive()
        {
            DbAssert.AreEqual(new AssemblyPublicClass(publicSetter: true), "SELECT CAST(1 AS BIT) AS pUbLiCsEtTeR", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassPrivateSetter()
        {
            DbAssert.AreEqual(new AssemblyPublicClass(privateSetter: true), "SELECT CAST(1 AS BIT) AS PrivateSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassProtectedSetter()
        {
            DbAssert.AreEqual(new AssemblyPublicClass(protectedSetter: true), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassInternalSetter()
        {
            DbAssert.AreEqual(new AssemblyPublicClass(internalSetter: true), "SELECT CAST(1 AS BIT) AS InternalSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassNoSetter()
        {
            DbAssert.AreEqual(new AssemblyPublicClass(noSetter: true), "SELECT CAST(1 AS BIT) AS NoSetter", false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeserializePrivateConstructor()
        {
            DbAssert.AreEqual<AssemblyNoPublicConstructor>(null, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeserializeWithoutParameteredConstructor()
        {
            DbAssert.AreEqual<AssemblyParameteredConstructor>(null, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeserializeStruct()
        {
            DbAssert.AreEqual(new AssemblyPublicStruct { Value = true }, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        public void CanDeserializeInternalClass()
        {
            DbAssert.AreEqual(new AssemblyInternalClass { Value = true }, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassPrivateConstructor()
        {
            DbAssert.AreEqual(new AssemblyPublicClassPrivateConstructor(), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassProtectedConstructor()
        {
            DbAssert.AreEqual(new AssemblyPublicClassProtectedConstructor(), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassInternalConstructor()
        {
            DbAssert.AreEqual(new AssemblyPublicClassInternalConstructor(), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }
    }
}

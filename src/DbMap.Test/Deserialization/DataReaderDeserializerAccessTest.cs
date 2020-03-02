using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerAccessTest
    {
        [TestMethod]
        public void DoesNotDeserializePublicClassPrivateSetterNoAutoProperty()
        {
            DbAssert.AreEqual(new PublicClass(privateSetterNoAutoProperty: false), "SELECT CAST(1 AS BIT) AS PrivateSetterNoAutoProperty", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassPrivateSetter()
        {
            DbAssert.AreEqual(new PublicClass(privateSetter: true), "SELECT CAST(1 AS BIT) AS PrivateSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassProtectedSetter()
        {
            DbAssert.AreEqual(new PublicClass(protectedSetter: true), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassInternalSetter()
        {
            DbAssert.AreEqual(new PublicClass(internalSetter: true), "SELECT CAST(1 AS BIT) AS InternalSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassNoSetter()
        {
            DbAssert.AreEqual(new PublicClass(noSetter: true), "SELECT CAST(1 AS BIT) AS NoSetter", false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeserializePrivateConstructor()
        {
            DbAssert.AreEqual<NoPublicConstructor>(null, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeserializeWithoutParameteredConstructor()
        {
            DbAssert.AreEqual<ParameteredConstructor>(null, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeserializeStruct()
        {
            DbAssert.AreEqual(new PublicStruct { Value = true }, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        public void CanDeserializePrivateClass()
        {
            DbAssert.AreEqual(new PrivateClass { Value = true }, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        public void CanDeserializeInternalClass()
        {
            DbAssert.AreEqual(new InternalClass { Value = true }, "SELECT CAST(1 AS BIT) AS Value", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassPrivateConstructor()
        {
            DbAssert.AreEqual(new PublicClassPrivateConstructor(), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassProtectedConstructor()
        {
            DbAssert.AreEqual(new PublicClassProtectedConstructor(), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }

        [TestMethod]
        public void CanDeserializePublicClassInternalConstructor()
        {
            DbAssert.AreEqual(new PublicClassInternalConstructor(), "SELECT CAST(1 AS BIT) AS ProtectedSetter", false);
        }

        private class PrivateClass
        {
            public bool Value { get; set; }

            public override bool Equals(object obj)
            {
                return Value == ((PrivateClass)obj).Value;
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }
    }
}

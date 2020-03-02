using System;

namespace DbMap.Test.Deserialization
{
    public class PublicClass
    {
        private bool privateSetterNoAutoProperty;

        public PublicClass()
        {
        }

        public PublicClass(bool publicSetter = false, bool protectedSetter = false, bool privateSetter = false, bool noSetter = false, bool internalSetter = false, bool privateSetterNoAutoProperty = false)
        {
            PublicSetter = publicSetter;
            ProtectedSetter = protectedSetter;
            PrivateSetter = privateSetter;
            NoSetter = noSetter;
            InternalSetter = internalSetter;
            this.privateSetterNoAutoProperty = privateSetterNoAutoProperty;
        }

        public bool PublicSetter { get; set; }

        public bool ProtectedSetter { get; protected set; }

        public bool PrivateSetter { get; private set; }

        public bool NoSetter { get; }

        public bool PrivateSetterNoAutoProperty { get => privateSetterNoAutoProperty; private set => privateSetterNoAutoProperty = value; }

        public bool InternalSetter { get; internal set; }

        public override bool Equals(object obj)
        {
            var other = (PublicClass)obj;
            return privateSetterNoAutoProperty == other.privateSetterNoAutoProperty && PublicSetter == other.PublicSetter && ProtectedSetter == other.ProtectedSetter && PrivateSetter == other.PrivateSetter && NoSetter == other.NoSetter && InternalSetter == other.InternalSetter;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
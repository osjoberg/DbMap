using System;

namespace DbMap.Test.Serialization
{
    public class PublicClass
    {
        private bool privateGetterNoAutoProperty;

        public PublicClass()
        {
        }

        public PublicClass(bool publicGetter = false, bool protectedGetter = false, bool privateGetter = false, bool internalGetter = false, bool privateGetterNoAutoProperty = false)
        {
            PublicGetter = publicGetter;
            ProtectedGetter = protectedGetter;
            PrivateGetter = privateGetter;
            InternalGetter = internalGetter;
            this.privateGetterNoAutoProperty = privateGetterNoAutoProperty;
        }

        public bool PublicGetter { get; set; }

        public bool ProtectedGetter { protected get; set; }

        public bool PrivateGetter { private get;  set; }

        public bool InternalGetter { internal get;  set; }

        public bool PrivateGetterNoAutoProperty { private get => privateGetterNoAutoProperty; set => privateGetterNoAutoProperty = value; }

        public override bool Equals(object obj)
        {
            var other = (PublicClass)obj;
            return privateGetterNoAutoProperty == other.privateGetterNoAutoProperty && PublicGetter == other.PublicGetter && ProtectedGetter == other.ProtectedGetter && PrivateGetter == other.PrivateGetter && InternalGetter == other.InternalGetter;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}

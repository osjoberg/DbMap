namespace DbMap.Test.Serialization
{
    public struct PublicStruct
    {
        public PublicStruct(bool publicGetter = false, bool privateGetter = false, bool internalGetter = false)
        {
            PublicGetter = publicGetter;
            PrivateGetter = privateGetter;
            InternalGetter = internalGetter;
        }

        public bool PublicGetter { get; set; }

        public bool PrivateGetter { private get;  set; }

        public bool InternalGetter { internal get;  set; }

        public override bool Equals(object obj)
        {
            var other = (PublicStruct)obj;

            return PublicGetter == other.PublicGetter && PrivateGetter == other.PrivateGetter && InternalGetter == other.InternalGetter;
        }

        public override int GetHashCode()
        {
            return PublicGetter.GetHashCode() ^PrivateGetter.GetHashCode() ^ InternalGetter.GetHashCode();
        }
    }
}

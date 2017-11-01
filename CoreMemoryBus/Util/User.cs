using System.Collections.Generic;

namespace CoreMemoryBus.Util
{
    public class User
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PrincipalSet Principals { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

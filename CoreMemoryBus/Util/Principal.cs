namespace CoreMemoryBus.Util
{
    public class Principal
    {
        public string Name
        {
            get { return _name; }
            set { if (!ReadOnly) _name = value; }
        }

        public string Description { get; set; }

        public bool ReadOnly { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as Principal;
            if (rhs != null)
            {
                return string.Equals(rhs.Name, Name);
            }
            return false;
        }

        private string _name;
    }

    public static class PrincipalExtensions
    {
        public static Principal AsPrincipal(this User user)
        {
            return new Principal { Name = user.Name, ReadOnly = true };
        }
    }
}

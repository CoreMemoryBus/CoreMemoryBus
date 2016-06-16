using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreMemoryBus.Util
{
    public static class StringEnumerableExtensions
    {
        public static string ToCsv(this IEnumerable<string> input, string separator = ",")
        {
            Ensure.ArgumentIsNotNull(input,"input");
            var i = 0;
            var lastIndex = input.Count() - 1;
            var builder = new StringBuilder();
            foreach (var token in input)
            {
                if (i != lastIndex)
                {
                    builder.AppendFormat("{0}{1}",token,separator);
                }
                else
                {
                    builder.Append(token);
                }
            }
            return builder.ToString();
        }
    }
}

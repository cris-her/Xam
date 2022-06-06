using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamContacts.Extensions
{
    public static class EncodeExtensions
    {
        public static Byte[] Encode(this string @string)
        {
            return Encoding.UTF8.GetBytes(@string);
        }

        public static string Decode(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}

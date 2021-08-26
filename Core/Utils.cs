using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YooMoney.Core {
    public static class Utils {
        public static string ToEnumString<T>(T type) where T : Enum {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = (((EnumMemberAttribute[])enumType.GetField(name ?? string.Empty)?.GetCustomAttributes(typeof(EnumMemberAttribute), true)) ?? Array.Empty<EnumMemberAttribute>()).Single();
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(string str) where T : Enum {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType)) {
                var enumMemberAttribute = (((EnumMemberAttribute[])enumType.GetField(name)?.GetCustomAttributes(typeof(EnumMemberAttribute), true)) ?? Array.Empty<EnumMemberAttribute>()).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            return default;
        }
    }
}

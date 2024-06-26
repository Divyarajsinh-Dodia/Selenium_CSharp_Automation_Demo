using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Framework
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the Description Attribute from an Enum.
        /// </summary>
        /// <param name="e">Enum to get Description for.</param>
        /// <returns> desciption.</returns>
        public static string GetDescription(this Enum e)
        {
            var enumType = e.GetType();
            var field = enumType.GetField(e.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var menuItem = attributes.Length == 0 ? e.ToString() : ((DescriptionAttribute)attributes[0]).Description;

            return menuItem;
        }

        /// <summary>
        /// Get a list of decription of the type entered.
        /// </summary>
        /// <param name="type">enum</param>
        /// <returns>List of descriptions of enum.</returns>
        public static List<string> GetDescriptions(Type type)
        {
            if (type.BaseType != typeof(Enum))
            {
                throw new NotSupportedException("type is not Enum");
            }

            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute item in fds)
                {
                    descs.Add(item.Description);
                }
            }

            return descs;
        }
    }
}

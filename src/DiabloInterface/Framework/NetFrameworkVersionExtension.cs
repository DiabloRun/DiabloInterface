using System.ComponentModel;

namespace Zutatensuppe.DiabloInterface.Framework
{
    internal static class NetFrameworkVersionExtension
    {
        public static string FriendlyName(NetFrameworkVersion value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            object[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length <= 0) return value.ToString();
            var descriptionAttribute = (DescriptionAttribute) attributes[0];
            return descriptionAttribute.Description;
        }
    }
}

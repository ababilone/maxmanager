using System;
using System.Linq;
using System.Reflection;

namespace MaxControl
{
    public class PropertySetter : IPropertySetter
    {
        public PropertySetter(Object target, String propertyName)
        {
            Target = target;
            PropertyInfo = target.GetType().GetProperties().Single(p => String.Equals(p.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));
        }

        public Object Target { get; protected set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public void SetValue(Object value)
        {
            PropertyInfo.SetValue(Target, value);
        }
    }
}
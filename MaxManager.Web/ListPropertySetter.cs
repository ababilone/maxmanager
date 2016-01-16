using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaxControl
{
    public class ListPropertySetter : IPropertySetter
    {
        private readonly string _variableName;
        private readonly Dictionary<string, object> _variablesContainer;
        private readonly Dictionary<IPropertySetter, string> _objectsAssigments;

        public ListPropertySetter(string variableName, Dictionary<string, object> variablesContainer, Dictionary<IPropertySetter, string> objectsAssigments)
        {
            _variableName = variableName;
            _variablesContainer = variablesContainer;
            _objectsAssigments = objectsAssigments;
        }

        public void SetValue(Object value)
        {
            if (value == null)
                return;
            
            var valueType = value.GetType();
            var listType = typeof(List<>).MakeGenericType(new[] { valueType });
            var listObject = Activator.CreateInstance(listType);

            if (!_variablesContainer.ContainsKey(_variableName))
            {
                _variablesContainer.Add(_variableName, listObject);
                CheckAssigments(listObject);
            }

            if (_variablesContainer[_variableName] == null)
            {
                _variablesContainer[_variableName] = listObject;
                CheckAssigments(listObject);
            }

            ((IList) _variablesContainer[_variableName]).Add(value);
        }

        private void CheckAssigments(Object value)
        {
            foreach (var a in _objectsAssigments.Where(kvp => kvp.Value == _variableName))
                a.Key.SetValue(value);
        }
    }
}
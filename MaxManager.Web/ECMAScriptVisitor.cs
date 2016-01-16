using System;
using System.Collections.Generic;
using System.Globalization;
using MaxManager.JavaScript;
using MaxControl.State;

namespace MaxControl
{
    public class ECMAScriptVisitor : ECMAScriptBaseVisitor<object>
    {
        private readonly Dictionary<string, object> _objects;
        private readonly Dictionary<IPropertySetter, string> _objectsAssigments;

        public Object Output { get; private set; }

        public ECMAScriptVisitor()
        {
            _objects = new Dictionary<string, object>();
            _objectsAssigments = new Dictionary<IPropertySetter, string>();
        }

        public override object VisitVariableDeclaration(ECMAScriptParser.VariableDeclarationContext context)
        {
            var name = context.GetChild(0).GetText();
            var value = context.ChildCount == 2 ? context.GetChild(1).Accept(this) : null;

            _objects.Add(name, value);

            return base.VisitVariableDeclaration(context);
        }

        public override object VisitNewExpression(ECMAScriptParser.NewExpressionContext context)
        {
            var objectType = context.GetChild(1).GetChild(0).GetText();

            if (objectType == "Date")
            {
                var milliseconds = Convert.ToInt64(context.GetChild(1).GetChild(1).GetChild(1).GetText());
                var initialDate = new DateTime(1970, 1, 1);
                return initialDate.AddMilliseconds(milliseconds);
            }

            var targetType = Type.GetType("MaxControl.State." + objectType);
            if (targetType != null)
                return Activator.CreateInstance(targetType);

            throw new TypeLoadException("Unable to find object type: " + objectType);
        }

        public override object VisitArrayLiteralExpression(ECMAScriptParser.ArrayLiteralExpressionContext context)
        {
            return null;
        }

        public override object VisitAssignmentExpression(ECMAScriptParser.AssignmentExpressionContext context)
        {
            var propertyContext = context.GetChild(0);
            var valueContext = context.GetChild(2);

            var propertySetter = propertyContext.Accept(this) as IPropertySetter;

            if (propertySetter != null)
            {
                var value = valueContext.Accept(this);
                propertySetter.SetValue(value);

                var identifierExpression = valueContext.GetChild(0) as ECMAScriptParser.IdentifierExpressionContext;
                if (identifierExpression != null)
                    _objectsAssigments.Add(propertySetter, identifierExpression.GetText());
            }

            return null;
        }

        public override object VisitMemberIndexExpression(ECMAScriptParser.MemberIndexExpressionContext context)
        {
            var targetObjectName = context.GetChild(0).GetText();

            if (!_objects.ContainsKey(targetObjectName))
                throw new ParseException("Variable undefined: " + targetObjectName);

            return new ListPropertySetter(targetObjectName, _objects, _objectsAssigments);
        }

        public override object VisitArgumentsExpression(ECMAScriptParser.ArgumentsExpressionContext context)
        {
            if (context.GetChild(0).GetText() == "dwr.engine._remoteHandleCallback")
            {
                var maxCubeStateVariableName = context.GetChild(1).GetChild(1).GetChild(4).GetText();
                Output = _objects[maxCubeStateVariableName] as MaxCubeState;
            }

            return base.VisitArgumentsExpression(context);
        }

        public override object VisitMemberDotExpression(ECMAScriptParser.MemberDotExpressionContext context)
        {
            var targetObjectName = context.GetChild(0).GetText();

            if (targetObjectName == "dwr.engine")
                return null;

            if (!_objects.ContainsKey(targetObjectName))
                throw new ParseException("Variable undefined: " + targetObjectName);

            var targetObject = _objects[targetObjectName];
            var targetPropertyName = context.GetChild(2).GetText();

            return new PropertySetter(targetObject, targetPropertyName);
        }

        public override object VisitLiteral(ECMAScriptParser.LiteralContext context)
        {
            var literal = context.GetText();

            if (literal == "null")
                return null;
            if (literal == "true")
                return true;
            if (literal == "false")
                return false;

            return base.VisitLiteral(context) ?? literal.Substring(1, literal.Length - 2);
        }

        public override object VisitNumericLiteral(ECMAScriptParser.NumericLiteralContext context)
        {
            var numericLiteral = context.GetText().Replace(".", ",");

            int intValue;
            if (int.TryParse(numericLiteral, out intValue))
                return intValue;

            long longValue;
            if (long.TryParse(numericLiteral, out longValue))
                return longValue;

            float floatValue;
            if (float.TryParse(numericLiteral, NumberStyles.Float, CultureInfo.CreateSpecificCulture("fr"), out floatValue))
                return floatValue;

            double doubleValue;
            if (double.TryParse(numericLiteral, NumberStyles.Float, CultureInfo.CreateSpecificCulture("fr"), out doubleValue))
                return doubleValue;

            return null;
        }

        public override object VisitIdentifierExpression(ECMAScriptParser.IdentifierExpressionContext context)
        {
            var variableName = context.GetText();
            if (_objects.ContainsKey(variableName))
                return _objects[variableName];

            return base.VisitIdentifierExpression(context);
        }

        public override object VisitProgram(ECMAScriptParser.ProgramContext context)
        {
            base.VisitProgram(context);

            return Output;
        }
    }
}

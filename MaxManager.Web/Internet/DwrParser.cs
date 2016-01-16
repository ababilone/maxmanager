using System;
using Antlr4.Runtime;
using MaxControl.JavaScript;

namespace MaxControl.Internet
{
    public static class DwrParser
    {
        public static object Parse(String data)
        {
            var antlrInputStream = new AntlrInputStream(data);
            var ecmaScriptLexer = new ECMAScriptLexer(antlrInputStream);
            var commonTokenStream = new CommonTokenStream(ecmaScriptLexer);
            var ecmaScriptParser = new ECMAScriptParser(commonTokenStream);
            var ecmaScriptVisitor = new ECMAScriptVisitor();
            var programContext = ecmaScriptParser.program();

            return programContext.Accept(ecmaScriptVisitor);
        }
    }
}
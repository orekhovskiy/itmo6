using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalysisLibray.Parser
{
    public enum NonTerminal
    {
        Root,
        VariableDeclaration,
        ComputingDescription,
        VariablesList,
        OperatorsList,
        Operator,
        Assignment,
        Expression,
        Subexpression,
        Operand,
        ComplexOperator,
        CycleOperator,
        CompoundOperator,
        Separator,
        NullableSeparator,
        VariablesContinuation,
        OperatorsContinuation,
        BinaryOperatorSubexpression,
        Continuation
    }
}

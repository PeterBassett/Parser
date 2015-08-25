using System;
using System.Net;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Statements.Loops;
using AST.Expressions.Function;

namespace AST.Visitor
{
    public class PrintVisitor : IExpressionVisitor<string, Scope>
    {
        public string Visit(IdentifierExpr expr, Scope scope)
        {
            return expr.Name;
        }

        public string Visit(PlusExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "+" + expr.Right.Accept(this, scope);
        }

        public string Visit(ConstantExpr expr, Scope scope)
        {
            return expr.Value.ToString();
        }

        public string Visit(DivExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "/" + expr.Right.Accept(this, scope);
        }

        public string Visit(MinusExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "-" + expr.Right.Accept(this, scope);
        }

        public string Visit(MultExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "*" + expr.Right.Accept(this, scope);
        }

        public string Visit(AssignmentExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "=" + expr.Right.Accept(this, scope);
        }

        public string Visit(PowExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "^" + expr.Right.Accept(this, scope);
        }

        public string Visit(EqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " == " + expr.Right.Accept(this, scope); 
        }

        public string Visit(NotEqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " != " + expr.Right.Accept(this, scope);
        }

        public string Visit(GreaterThanExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " > " + expr.Right.Accept(this, scope);
        }

        public string Visit(LessThanExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " < " + expr.Right.Accept(this, scope);
        }

        public string Visit(GreaterThanOrEqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " >= " + expr.Right.Accept(this, scope);
        }

        public string Visit(LessThanOrEqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " <= " + expr.Right.Accept(this, scope);
        }


        public string Visit(AndExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " && " + expr.Right.Accept(this, scope);
        }

        public string Visit(OrExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " || " + expr.Right.Accept(this, scope);
        }

        public string Visit(NotExpr expr, Scope scope)
        {
            return "!" + expr.Right.Accept(this, scope);
        }

        public string Visit(ConditionalExpr expr, Scope scope)
        {
            return expr.Condition.Accept(this, scope) + " ? " + expr.ThenExpression.Accept(this, scope) + " : " + expr.ElseExpression.Accept(this, scope);
        }

        public string Visit(NegationExpr expr, Scope scope)
        {
            return "-" + expr.Right.Accept(this, scope);
        }

        public string Visit(WhileStmt stmt, Scope scope)
        {
            return string.Format("while({0})\r\n{{\r\n\t{1}\r\n}}",
                stmt.Condition.Accept(this, scope),
                stmt.Block.Accept(this, scope));
        }

        public string Visit(IfStmt stmt, Scope scope)
        {
            var ifstatement = string.Format("if({0})\r\n{{\r\n\t{1}\r\n}}",
                stmt.Condition.Accept(this, scope),
                stmt.ThenExpression.Accept(this, scope));

            if(!(stmt.ElseExpression is NoOpStatement))
                ifstatement += string.Format("else\r\n{{\r\n\t{0}\r\n}}",
                stmt.ThenExpression.Accept(this, scope));

            return ifstatement;
        }

        public string Visit(BlockStmt stmt, Scope scope)
        {
            var block = "{{\r\n";

            foreach (var statement in stmt.Statements)
                block += string.Format("\t{0}\r\n", statement.Accept(this, scope));

            block += "}}";

            return block;
        }

        public string Visit(NoOpStatement stmt, Scope scope)
        {
            return "";
        }

        public string Visit(DoWhileStmt stmt, Scope scope)
        {
            return string.Format("do\r\n{{\r\n\t{0}\r\n}}while({1});",
                stmt.Block.Accept(this, scope),
                stmt.Condition.Accept(this, scope));
        }

        public string Visit(FunctionDefinitionExpr expr, Scope context)
        {
            throw new NotImplementedException();
        }

        public string Visit(ReturnExpr returnExpr, Scope context)
        {
            throw new NotImplementedException();
        }

        public string Visit(VarDefinitionStmt varDefinitionStmt, Scope context)
        {
            throw new NotImplementedException();
        }


        public string Visit(FunctionCallExpr functionCallExpr, Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
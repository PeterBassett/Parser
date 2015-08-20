namespace AST
{
    public class SyntaxTree
    {
        private IExpression _expression;

        public SyntaxTree(IExpression expression)
        {
            _expression = expression;
        }
    }
}

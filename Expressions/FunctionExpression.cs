
using GWent;

public class FunctionExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.FunctionExpression;

    public Tokens.TokenType FunctionType { get; }
    public Expressions? Param { get; }

    public FunctionExpression( Tokens.TokenType functionType, Expressions ?param)
    {
        FunctionType = functionType;
        Param = param;
    }

    public FunctionExpression( Tokens.TokenType functionType)
    {
        FunctionType = functionType;
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }

    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
}
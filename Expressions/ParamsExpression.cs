
using GWent;

public class ParamsExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.ParamsExpression;

    public Statement ParamsStatement { get; }

    public ParamsExpression(Statement paramsStatement)
    {
        ParamsStatement = paramsStatement;
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
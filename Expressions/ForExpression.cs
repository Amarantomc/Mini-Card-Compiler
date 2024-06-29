namespace GWent;

public class ForExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.ForExpression;

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }

    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
}
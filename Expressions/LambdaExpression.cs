
using GWent;

public class LambdaExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.LambdaExpression;

    public Expressions Left { get; }
    public Tokens Do { get; }
    public Expressions Right { get; }

    public LambdaExpression(Expressions left, Tokens Do, Expressions right)
    {
        Left = left;
        this.Do = Do;
        Right = right;
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
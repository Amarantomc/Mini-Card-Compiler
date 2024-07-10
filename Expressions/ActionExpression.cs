
using GWent;

public class ActionExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.ActionExpression;

    public LambdaExpression LambdaExpression { get; }

    public ActionExpression( LambdaExpression lambdaExpression)
     {
        LambdaExpression = lambdaExpression;
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
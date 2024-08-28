
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
    public bool ActionCheckSemantic(Scope scope)
    {
        return LambdaExpression.DelegateCheckSemantic(scope);
    }

    public override object Evaluate(Scope scope)
    {
         return LambdaExpression.Evaluate(scope!);
    }
}
namespace GWent;

public class ForExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.ForExpression;

    public Expressions Condition { get; }
    public Statement Expressions { get; }

    public ForExpression(Expressions condition, Statement expressions){
        Condition = condition;
        Expressions = expressions;
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
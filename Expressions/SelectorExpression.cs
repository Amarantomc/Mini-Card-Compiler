
using GWent;

public class SelectorExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.SelectorExpression;

    public AssignmentExpression Source{get;}
    public AssignmentExpression Single{get;}
    public LambdaExpression Predicate{get;}

    public SelectorExpression(AssignmentExpression source, AssignmentExpression single, LambdaExpression predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
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
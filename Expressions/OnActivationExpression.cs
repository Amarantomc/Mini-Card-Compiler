
using GWent;

public class OnActivationExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.OnActivationExpression;

    public List<Statement> Statements{get;}
 

    public OnActivationExpression( List<Statement> statements)
    {
        Statements = statements;
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

using GWent;

public class PostActionExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.PostActionExpression;

    public Statement PostAction { get; }
    public Expressions Name { get; }

    public PostActionExpression(Statement postAction)
    {
        PostAction = postAction;
        Name=null!;
    }

    public PostActionExpression(Expressions name, Statement postAction)
    {
        Name = name;
        PostAction = postAction;
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
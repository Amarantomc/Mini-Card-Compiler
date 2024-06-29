namespace GWent;

public class WhileExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.WhileExpression;

    
    public Expressions BoolExpression { get; }
    
    public Expressions Body { get; }

    public WhileExpression( Expressions boolExpression,Expressions body)
    {
         
        BoolExpression = boolExpression;
         
        Body = body;
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

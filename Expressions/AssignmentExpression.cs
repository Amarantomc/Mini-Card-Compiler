namespace GWent;

public class  AssignmentExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.AssignmentExpression;

    public Tokens Identifier { get; }
    public Tokens Op { get; }
    public Expressions Right { get; }

    public AssignmentExpression( Tokens identifier,Tokens op, Expressions right){
        Identifier = identifier;
        Op = op;
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
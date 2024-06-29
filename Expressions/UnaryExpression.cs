   namespace GWent;
class UnaryExpression: Expressions{
    public readonly Expressions Right;
     
    public Tokens Op { get; }

    public UnaryExpression( Tokens op, Expressions right)
    {
         
        this.Op = op;
        this.Right = right;
    }

    public override Tokens.TokenType Type => Tokens.TokenType.UnaryExpression;

    public override object Evaluate()
    {
        throw new NotImplementedException();
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }
}
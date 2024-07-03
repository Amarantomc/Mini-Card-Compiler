   namespace GWent;
 
class BinaryExpression: Expressions{
    public readonly Expressions Right;
    public Expressions Left { get; }
    public Tokens Op { get; }

    public BinaryExpression(Expressions left, Tokens op, Expressions right)
    {
         Left = left;
         Op = op;
         Right = right;
    }

    public override Tokens.TokenType Type => Tokens.TokenType.BinaryExpression;

    public override object Evaluate()
    {
        throw new NotImplementedException();
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }
}

using GWent;

public class DotExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.DotExpression;

    public Expressions Left { get; }
    public Tokens Dot { get; }
    public Expressions Right { get; }

    public DotExpression(Expressions left, Tokens dot, Expressions right)
    {
        Left = left;
        Dot = dot;
        Right = right;
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }

    public override object Evaluate(Scope scope)
    {
        var left=Left.Evaluate(scope);
        var right=Right.Evaluate(scope);
        if(left.Equals("context"))
        {
            //Lamar a metodo en unity
        }
        return true;
         
    }
}
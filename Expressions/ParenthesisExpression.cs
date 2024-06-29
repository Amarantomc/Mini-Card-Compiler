   namespace GWent;
 
public class ParenthesisExpression : Expressions
{
     public readonly Tokens OpenParenthesis;
     
     public readonly Expressions Expression; 
     public readonly Tokens CloseParenthesis;

     public ParenthesisExpression(Tokens open, Expressions expression, Tokens close)
     {
        this.OpenParenthesis=open;
        this.Expression=expression;
        this.CloseParenthesis=close;
     }

    public override Tokens.TokenType Type => Tokens.TokenType.ParenthesisExpression;

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }

    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
}
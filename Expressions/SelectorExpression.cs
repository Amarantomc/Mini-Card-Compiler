
using GWent;

public class SelectorExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.SelectorExpression;

    public Statement SelectorStatement { get; }

    public SelectorExpression(params Expressions [] expressions)
    {
         SelectorStatement=new Statement(expressions);
         
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
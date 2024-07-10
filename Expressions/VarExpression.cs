
using GWent;

public class VarExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.VarExpression;

    public Tokens Var { get; }

    public VarExpression(Tokens var)
    {
        Var = var;
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
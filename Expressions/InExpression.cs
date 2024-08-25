using System.Linq.Expressions;
using GWent;

public class InExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.InExpression;

    public VarExpression Var { get; }
    public VarExpression Collection { get; }

    public InExpression(VarExpression var, VarExpression collection){
        Var = var;
        Collection = collection;
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
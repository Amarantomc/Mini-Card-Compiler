
using GWent;

public class ParamsExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.ParamsExpression;

    public Statement ParamsStatement { get; }

    public ParamsExpression(Statement paramsStatement)
    {
        ParamsStatement = paramsStatement;
    }

    public override bool CheckSemantic()
    {
         if(ParamsStatement.Expressions.All(x=>x is VarExpression)) return true;
         throw new Exception("Invalid Expressions on Params Statement");
    }

    public override object Evaluate(Scope scope)
    {
         foreach (var item in ParamsStatement.Expressions)
         {
            item.Evaluate(scope);
         }
         return null!;
    }
}
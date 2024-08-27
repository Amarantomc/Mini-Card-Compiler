
using GWent;

public class OnActivationExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.OnActivationExpression;

    public List<Statement> Statements{get;}
    private Scope ? scope{get;set;}
 

    public OnActivationExpression( List<Statement> statements)
    {
         Statements= new List<Statement>();
         Copy(Statements,statements);
    }

    public override bool CheckSemantic()
    {
           foreach (var item in Statements)
         {
            foreach (var statement in item.Expressions)
            {
                statement.Evaluate(scope!);
            }
         }
         
        return true;
    } 
 

    public override object Evaluate(Scope scope)
    {
       this.scope=scope;
       CheckSemantic();
       
        throw new NotImplementedException();
    }
}
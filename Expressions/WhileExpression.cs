namespace GWent;

public class WhileExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.WhileExpression;

    
    public Expressions BoolExpression { get; }
    
    public Statement Body { get; }

    

    public WhileExpression( Expressions boolExpression,Statement body)
    {
         
        BoolExpression = boolExpression;
         
        Body = body;
    } 

    

    public override bool CheckSemantic()
    {
          return true;
       
    }

    public override object Evaluate(Scope scope)
    {
         Scope scopeStatment=scope.CreateChild();
         if(BoolExpression.Evaluate(scope) is bool condition)
         {
             while (condition)
             {
                Body.Evaluate(scopeStatment);
             }
         } else {
            throw new Exception("Missing or Invalid Expression for While Condition");
         }
         return null!;
         
    }
}

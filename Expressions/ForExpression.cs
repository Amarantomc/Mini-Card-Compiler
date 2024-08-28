namespace GWent;

public class ForExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.ForExpression;

    public InExpression Condition { get; }
    public Statement Expressions { get; }

    public ForExpression(InExpression condition, Statement expressions){
        Condition = condition;
        Expressions = expressions;
    }
    
    public override bool CheckSemantic()
    {
         return Condition.CheckSemantic() && Expressions.CheckSemantic();
    }

    public override object Evaluate(Scope scope)
    {
         Scope statmentScope=scope.CreateChild();
         int i=0;
         if(Condition!.Evaluate(scope)is not bool condition) throw new Exception("Invalid or Missing Expression");
          
        while ( Condition!.Evaluate(scope,i)is bool exp && exp)
            {
               i++;
               Expressions.Evaluate(statmentScope); 
            
         }  

         
         return true;

    }
}
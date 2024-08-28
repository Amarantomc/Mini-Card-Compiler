
using GWent;

public class OnActivationExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.OnActivationExpression;

    public List<Statement> Statements{get;}
     
 

    public OnActivationExpression( List<Statement> statements)
    {
         Statements= new List<Statement>();
         Copy(Statements,statements);
    }

    public override bool CheckSemantic()
    {
        
         
        return true;
    }
    public bool CheckSemantic(Scope scope)
    {
          foreach (var item in Statements)
         {
            foreach (var statement in item.Expressions)
            {  
                if(statement is EffectAssignmentExpression || statement is SelectorExpression || statement is PostActionExpression)
                {
                     if(statement is EffectAssignmentExpression effectExpression)  effectExpression.CheckSemantic(scope!);
                    if(statement is SelectorExpression selector)  selector.CheckSemantic(scope!);
                    if(statement is PostActionExpression postActionExpression)  postActionExpression.CheckSemantic(scope!);
                }
                else throw new Exception("Invalid Expression inside OnActivation");
               
            }
         }
        return true;
    }
 

    public override object Evaluate(Scope scope)
    {
       List<(EffectExpression,SelectorExpression)> result= new List<(EffectExpression, SelectorExpression)>();
       foreach (var item in Statements)
       {
         foreach (var statement in item.Expressions)
         {
            
         }
       }
        
       
        throw new NotImplementedException();
    }
}
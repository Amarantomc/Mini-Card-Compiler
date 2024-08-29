
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
                if(statement is null) continue;
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
          EffectExpression effect=null!;
          SelectorExpression selector=null!;
         foreach (var statement in item.Expressions)
         {
            if(statement is EffectAssignmentExpression effectAssignmentExpression)
            {
                effect=(EffectExpression)effectAssignmentExpression.Evaluate(scope!);
                continue;
            }
            if(statement is SelectorExpression selectorExpression)
            {
                selector=selectorExpression;
                continue;
            }

            if(effect is not null)
            {
                result.Add((effect,selector));
                effect=null!;
            }
            if(statement is PostActionExpression postAction)
            {
               List<(EffectExpression,SelectorExpression)> post=  (List<(EffectExpression, SelectorExpression)>)postAction.Evaluate(scope,selector);
                Copy(result,post);
                continue;
            }

         }
       }
         return result;
       
         
    }
}
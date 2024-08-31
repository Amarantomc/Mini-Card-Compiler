
using GWent;

public class LambdaExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.LambdaExpression;

    public List<VarExpression> Variables { get; }
    public Tokens Do { get; }
    public Statement Body { get; }

    public Tokens.TokenType Delegate{get;}

    private Scope ? scope{get;set;}

    public LambdaExpression( Tokens Do, Statement body, Tokens.TokenType Delegate, params VarExpression[] variables)
    {   
        Variables=new List<VarExpression>();
        foreach (var item in variables)
        {
           Variables.Add(item); 
        }
        this.Do = Do;
        Body=body;
        this.Delegate=Delegate;
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }
    public bool DelegateCheckSemantic(Scope internalScope)
    {
         foreach (var item in Variables)
         {
            FindVar(internalScope,item.Var.Value.ToString()!);
         }
         
         
         return true;
         
    }

    private bool FindVar(Scope internalScope, string name)
    { 
        if (internalScope is null) return true; 
            
             if (internalScope!.Variables.Exists(x=> x.Var.Value.ToString()== name))
            {
                throw new Exception($"Variable {name} was defined already");
            }
            return FindVar(internalScope.Parent!,name);
    }

    public override object Evaluate(Scope scope)
    {    // Devuelve un Action o un Predicate
         if(Delegate == Tokens.TokenType.ActionExpression)
         {
            foreach (var item in Variables )
            {
                scope.Variables.Add(item);
            }

            Action action=()=> Body.Evaluate(scope);
            return action;
         }
         if(Delegate == Tokens.TokenType.PredicateKeyword)
         {  
            VarExpression expression;
            foreach (var item in Variables)
            {
                scope.Variables.Add(item);
                expression=item;
            }
            Predicate<VarExpression> predicate=(expression) => (bool)Body.Expressions.First().Evaluate(scope);
            
            return predicate;
         }
         throw new Exception("Invalid Operation");
    }
}
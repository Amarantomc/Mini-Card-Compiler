
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
    public bool PredicateCheckSemantic(Scope internalScope)
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
    {
        throw new NotImplementedException();
    }
}
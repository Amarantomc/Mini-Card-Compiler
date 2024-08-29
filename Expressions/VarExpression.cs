
using GWent;

public class VarExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.VarExpression;

    public Tokens Var { get; }
    public Tokens.TokenType ? DataType{get;}

    public object? Value{get;set;}

    public VarExpression(Tokens var, Tokens.TokenType dataType,object value)
    {
        Var = var;
        DataType = dataType;
        Value=value;
    }

    public VarExpression(Tokens var,Tokens.TokenType dataType)
    {
        Var = var;
        DataType = dataType;
    }
    public VarExpression(Tokens var)
    {
        Var = var;
         
    }

    public override bool CheckSemantic()
    {    //DUDA
         return true;
    }

    public override object Evaluate(Scope scope)
    {
        if(GetValue(scope, out object? value)) return value!;
        throw new Exception($"{Var.Value} does not exist in the current context");
    }

    private bool GetValue(Scope scope, out object? value)
    {
        if(scope is null) 
        {
           value=null;
           return false;
        }
        if(scope.Variables.Any(x=> x.Var.Value.Equals(Var.Value)))
        {
             foreach (var item in scope.Variables)
             {
                if(item.Var.Value.Equals(Var.Value) && item.Var.Type != Tokens.TokenType.ContextKeyword)
                {
                     value=item.Var.Value;
                     return true;
                } else{
                    value= Tokens.TokenType.ContextKeyword;
                    return true;
                }
             }
        }
         return GetValue(scope.Parent!,out value);
    }
}
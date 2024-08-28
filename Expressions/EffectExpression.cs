
using GWent;

public class EffectExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.EffectExpression;

    public Expressions Name { get; }
    public ActionExpression Action { get; }
    public ParamsExpression Params { get; }
    private Scope? scope{get;set;}

    public EffectExpression(Expressions name, ActionExpression action, ParamsExpression Params)
    {
        Name = name;
        Action = action;
        this.Params = Params;
    }

    public EffectExpression(Expressions name,  ActionExpression action)
    :this(name,action,null!)
    {
        
    }

    public override bool CheckSemantic()
    {
         
        if(Name is null || Name.Evaluate(scope!) is not string) throw new Exception("Missing or Invalid Name Expression");
        if(Action is null) throw new Exception("Missing Acttion Expression");
        if(Params is not null)
        {
            return Params.CheckSemantic() && Action.ActionCheckSemantic(scope!);
        }
        return true  && Action.ActionCheckSemantic(scope!);;
    }
    

    public override object Evaluate(Scope scope)
    {
         this.scope=scope;
         CheckSemantic();
         if(Params is not null) Params.Evaluate(scope!);
         
         return Action.Evaluate(scope!);
    }
}
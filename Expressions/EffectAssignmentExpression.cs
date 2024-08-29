
using GWent;

public class EffectAssignmentExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.EffectAssignmentExpression;

    public AssignmentExpression Name { get; set; }
    public List<AssignmentExpression> Param { get;set; }

    private Scope ? scope{get;set;}

    public EffectAssignmentExpression(AssignmentExpression name, List<AssignmentExpression> param)
    {
        Name = name;
        Param=new List<AssignmentExpression>();
        Copy(Param,param);
        
    }

    public EffectAssignmentExpression(AssignmentExpression name)
    {
        Name = name;
        Param=new List<AssignmentExpression>();
        
    }
    public EffectAssignmentExpression(){
        Name=null!;
        Param=new List<AssignmentExpression>();

        
    }


    public override bool CheckSemantic()
    {
         if(Name is null) throw new Exception("Missing Effect Name");
        if(Name.Right.Evaluate(scope!) is not string) throw new Exception("Invalid Expression, must be String");
        return true;
    }
    public bool CheckSemantic(Scope scope)
    {
         if(Name is null) throw new Exception("Missing Effect Name");
        if(Name.Right.Evaluate(scope!) is not string) throw new Exception("Invalid Expression, must be String");
        return true;
    }

    public override object Evaluate(Scope scope)
    {   
        this.scope=scope;
        var name=Name.Evaluate(scope!);
        var effect=Context.Effects.Find(x=> x.Name.Evaluate(scope!).Equals(name));
        if(effect is null) throw new Exception($"Effect does not exist {Name.Evaluate(scope).ToString()}");

        if(effect.Params is not null)
        {
            foreach (VarExpression item in effect.Params.ParamsStatement.Expressions)
            {
              string varName=item.Var.Text;
              if(Param.Exists(x=>varName==x.Identifier.Var.Text))
              {
                AssignmentExpression param=Param.Find(x=>varName==x.Identifier.Var.Text)!;

                if(item.DataType is null)
                {
                    item.Value=param.Right;
                } else if(item.DataType is not null)
                {
                    var right=param.Right.Evaluate(scope!);
                    if(item.DataType== Tokens.TokenType.NumberKeyword && right is double) item.Value=right;
                    else if(item.DataType== Tokens.TokenType.BoolKeyword && right is bool) item.Value=right;
                    else if(item.DataType== Tokens.TokenType.StringKeyword && right is string) item.Value=right;
                    else throw new Exception($" Cannot convert from {right.GetType()} to {item.DataType}");

                        
                     
                }
              }
            }
        }
        
        return effect;
    }
}
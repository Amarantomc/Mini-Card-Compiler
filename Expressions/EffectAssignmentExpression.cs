
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

    public override object Evaluate(Scope scope)
    {
        this.scope=scope;
        CheckSemantic();
        return 0;
    }
}
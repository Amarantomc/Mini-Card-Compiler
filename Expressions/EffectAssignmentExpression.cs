
using GWent;

public class EffectAssignmentExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.EffectAssignmentExpression;

    public AssignmentExpression Name { get; set; }
    public List<AssignmentExpression> Param { get;set; }

    public EffectAssignmentExpression(AssignmentExpression name, List<AssignmentExpression> param)
    {
        Name = name;
        Param = param;
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
        throw new NotImplementedException();
    }

    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
}
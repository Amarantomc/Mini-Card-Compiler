
using GWent;

public class EffectAssignmentExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.EffectAssignmentExpression;

    public Expressions Name { get; }
    public ParamsExpression Param { get; }

    public EffectAssignmentExpression(Expressions name, ParamsExpression param)
    {
        Name = name;
        Param = param;
    }

    public EffectAssignmentExpression(Expressions name)
    {
        Name = name;
        Param=null!;
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
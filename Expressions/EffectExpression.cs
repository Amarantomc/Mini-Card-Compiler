
using GWent;

public class EffectExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.EffectExpression;

    public Expressions Name { get; }
    public Expressions Action { get; }
    public Expressions Params { get; }

    public EffectExpression(Expressions name, Expressions action, Expressions Params)
    {
        Name = name;
        Action = action;
        this.Params = Params;
    }

    public EffectExpression(Expressions name, Expressions action)
    :this(name,action,null!)
    {
        
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
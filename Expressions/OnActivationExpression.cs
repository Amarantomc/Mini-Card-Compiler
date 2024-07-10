
using GWent;

public class OnActivationExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.OnActivationExpression;

    public Statement EffectAssignment { get; }

    public OnActivationExpression( params Statement [] effectAssignment )
    {   
        EffectAssignment= new Statement();
        foreach (var item in effectAssignment)
        {
            EffectAssignment.Childrens.Enqueue(item);
        }
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
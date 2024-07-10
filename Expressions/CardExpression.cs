
using GWent;

public class CardExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.CardExpression;

    public Expressions Name { get; }
    public Expressions TypeCard { get; }
    public Expressions Faction { get; }
    public Expressions Power { get; }
    public Expressions Range { get; }
    public OnActivationExpression OnActivation { get; }
    

    public CardExpression( Expressions name, Expressions type, Expressions faction, Expressions power, Expressions range, OnActivationExpression onActivation)
    {
        Name = name;
        TypeCard = type;
        Faction = faction;
        Power = power;
        Range = range;
        OnActivation = onActivation;
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
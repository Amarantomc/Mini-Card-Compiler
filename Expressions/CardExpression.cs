
using GWent;

public class CardExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.CardExpression;

    public Expressions Name { get; }
    public Expressions Type1 { get; }
    public Expressions Faction { get; }
    public Expressions Power { get; }
    public Expressions Range { get; }
    public Expressions OnActivation { get; }

    public CardExpression( Expressions name, Expressions type, Expressions faction, Expressions power, Expressions range,Expressions onActivation)
    {
        Name = name;
        Type1 = type;
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
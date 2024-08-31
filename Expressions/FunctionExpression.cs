
using GWent;
using Logic;

public class FunctionExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.FunctionExpression;

    public Tokens.TokenType FunctionType { get; }
    public Expressions? Param { get; }

    public FunctionExpression( Tokens.TokenType functionType, Expressions ?param)
    {
        FunctionType = functionType;
        Param = param;
    }

    public FunctionExpression( Tokens.TokenType functionType)
    {
        FunctionType = functionType;
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }

    public override object Evaluate(Scope scope)
    {
         return true;
    }

    public object Evaluate(Scope scope, object value)
    {
        if(value is Card card)
        {
            if(FunctionType== Tokens.TokenType.PowerKeyword && card is UnitsCard unitsCard) return unitsCard.Power;
            else if(FunctionType== Tokens.TokenType.NameKeyword ) return card.Name;
            else if(FunctionType== Tokens.TokenType.FactionKeyword ) return card.Faccion;
            else if(FunctionType== Tokens.TokenType.RangeKeyword )
            {
              if(card is UnitsCard units) return units.Type.ToString();
              if(card is WeatherCard) return "Clima";
              if(card is Increase) return "Aumento";
              if(card is BossCard) return "Lider";
            }
            else if(FunctionType== Tokens.TokenType.OwnerKeyword) return true;//Unity
        
        } else if(value is List<Card> cards)
        {
            if(FunctionType== Tokens.TokenType.PopKeyword)
            {
                Card card1=cards[0];
                 cards.RemoveAt(0);
                 return card1;
            }
            else if( FunctionType== Tokens.TokenType.PushKeyword)
            {
                if(Param is not null &&Param.Evaluate(scope) is Card card1)
                {
                    cards.Add(card1);
                    cards.Reverse();
                    return card1;
                } else throw new Exception("Invalid Expression in Push Function");
            }
            else if( FunctionType== Tokens.TokenType.SendBottomKeyword)
            {
              if(Param is not null &&Param.Evaluate(scope) is Card card1)
                {
                    cards.Add(card1);
                    return card1;
                } else throw new Exception("Invalid Expression in SendBottom Function");
            }
             else if(FunctionType == Tokens.TokenType.RemoveKeyword)
             { 
                if(Param is not null &&Param.Evaluate(scope) is Card card1)
                {
                   cards.Remove(card1);
                  return card1;
                } else throw new Exception("Invalid Expression in Remove Function");
             }
              else if( FunctionType== Tokens.TokenType.FindKeyword)
              {
                 if(Param is not null && Param is LambdaExpression lambda)
                 {
                    List<Card> aux= new List<Card>();
                    foreach (var card1 in cards)
                    {
                        VarExpression var=lambda.Variables[0];
                        var.Value=card1;
                        if(lambda.Evaluate(scope) is Predicate<VarExpression> predicate && predicate.Invoke(var)) aux.Add(card1);
                    }
                    return aux;
                 }
                  else throw new Exception("Invalid Expression in Find Expression");
              }
               else if(FunctionType == Tokens.TokenType.ShuffleKeyword)
               {

               }
        }
        return true;
    }
}
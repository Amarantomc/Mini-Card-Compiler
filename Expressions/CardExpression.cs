
using GWent;
using Microsoft.Win32.SafeHandles;

public class CardExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.CardExpression;

    public Expressions Name { get; }
    public Expressions TypeCard { get; }
    public Expressions Faction { get; }
    public Expressions Power { get; }
    public List<Expressions> Range { get; }
    public OnActivationExpression OnActivation { get; }

    private Scope? scope{get;set;}

    private string [] types={"Oro","Plata","Lider","Aumento","Clima"};
    

    public CardExpression( Expressions name, Expressions type, Expressions faction, Expressions power, List<Expressions> range, OnActivationExpression onActivation)
    {
        Name = name;
        TypeCard = type;
        Faction = faction;
        Power = power;
        OnActivation = onActivation;
        Range=new List<Expressions>();
        Copy(Range,range);
    }

    public override bool CheckSemantic()
    {
        
        if(Name is null || Name.Evaluate(scope!) is not string) throw new Exception("Name does not exist");
        if(TypeCard is null || types.Contains(TypeCard.Evaluate(scope!))) throw new Exception("Type does not exist");

        if(TypeCard.Evaluate(scope!) is string type)
        { 
            
            if(type== "Oro"|| type=="Plata")
            {
               if(Faction is null || Faction.Evaluate(scope!) is not string) throw new Exception("Faction does not exist");
               if(Power is null || Power.Evaluate(scope!) is not double x || x<=0) throw new Exception("Power does not exist");
               if(Range.Count>3 || !Range.Any() ) throw new Exception("Params Overload or Missing");
               RangeMethod(Range);
            }
            else if(type=="Clima"|| type=="Aumento")
            {
                if(Power is not null|| Power!.Evaluate(scope!) is not double x|| x!=0) throw new Exception("This Card do not have");
                if(Range.Count>3 || !Range.Any() ) throw new Exception("Params Overload or Missing");
                RangeMethod(Range);
            }
            else if(type=="Lider")
            {
              if(Power is null || Power.Evaluate(scope!) is not double x || x<=0) throw new Exception("Power does not exist");
              if(Range.Any()) throw new Exception("Lider do not have Range");

            }
        } 
          OnActivation.Evaluate(scope!);
          return true;

          

        
    }

    private bool [] RangeMethod(List<Expressions> expressions)
    {
         bool[] result = new bool[3];
            foreach(Expressions expression in expressions) 
            {   
                string range=(expression.Evaluate(scope!) is string y) ? y: throw new Exception("Range must be a String Type");
                
                if( range == "Melee" && !result[0] ) 
                {
                     result[0]= true;
                     
                }
            else { throw new Exception("Semantic Error only can be one Melee"); }
            
             if ( range == "Ranged" && !result[1])
                {
                      result[1] = true;
                    
                }
            else { throw new Exception("Semantic Error only can be one Ranged"); }
             
             if ( range == "Siege" && !result[2])
                {
                      result[2] = true;
                    
                }
            else { throw new Exception("Semantic Error only can be one Siege"); }
                
                throw new Exception($"Unexpected Token {range}, Expected 'Melee', 'Ranged' o 'Siege' "); 
            }

            return result;
    }

    public override object Evaluate(Scope scope)
    {
         return 0;
    }
}
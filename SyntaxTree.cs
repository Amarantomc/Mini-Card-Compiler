using Logic;

namespace GWent;
 
public class SyntaxTree{
    
    public Queue< Expressions> root;
     


    public SyntaxTree( )
    {
        root= new Queue<Expressions>();
         
        
    } 

    public static  SyntaxTree Parse(string text){
       var parser= new Parser(text);
       return parser.Parse();
     
    }

    public List<Card> Visitor()
    {    //Puedo setear yo mismo el IEnumerable en el scope 
        List<Card> result=new List<Card>();
        Scope scope=new Scope();
        foreach (Expressions item in root)
        {
            if(item is EffectExpression effect)
            {
               Context.Effects.Add(effect);
               continue;
            }

            var card=item.Evaluate(scope);
            if(card is Card card1) result.Add(card1);

        }
        return result;
    }



   
}
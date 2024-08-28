
using GWent;

public class SelectorExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.SelectorExpression;

    public AssignmentExpression Source{get;set;}
    public AssignmentExpression? Single{get;}
    public LambdaExpression ?Predicate{get;}

    private Scope? scope{get;set;}
    private string [] source={"hand","otherHand","deck","otherDeck","field","board","otherField"};

    public SelectorExpression(AssignmentExpression source, AssignmentExpression single, LambdaExpression predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
    }
     

    public override bool CheckSemantic()
    { 
        if(Source is not null)
        {
           if(Source.Right.Evaluate(scope!) is string exp && source.Contains(exp))
           {
              if(Predicate is not null)
              {
                 return true && Predicate.DelegateCheckSemantic(scope!);
              }
              return true;
           }
           throw new Exception("Invalid expression for Source");
        }
        throw new Exception("Missing Source");
    }

    public bool CheckSemantic(Scope scope)
    {
        if(Source is not null)
        {
           if(Source.Right.Evaluate(scope!) is string exp && source.Contains(exp))
           {
              if(Predicate is not null)
              {
                 return true && Predicate.DelegateCheckSemantic(scope!);
              }
              return true;
           }
           throw new Exception("Invalid expression for Source");
        }
        throw new Exception("Missing Source");  
    }

    public override object Evaluate(Scope scope)
    {    //No completado
         var soucre=Source.Evaluate(scope);
         bool single=(Single is null)? false: (bool)Single.Right.Evaluate(scope!);
         return 0;
    }
}
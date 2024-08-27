
using GWent;

public class SelectorExpression : Expressions
{
    public override Tokens.TokenType Type =>  Tokens.TokenType.SelectorExpression;

    public AssignmentExpression Source{get;}
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
                 return true && Predicate.CheckSemantic();
              }
              return true;
           }
           throw new Exception("Invalid expression for Source");
        }
        throw new Exception("Missing Source");
    }

    public override object Evaluate(Scope scope)
    {
         this.scope=scope;
         CheckSemantic();
         return 0;
    }
}
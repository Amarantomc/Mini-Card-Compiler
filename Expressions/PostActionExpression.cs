
using GWent;

public class PostActionExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.PostActionExpression;

    public Expressions Name { get; }

    public SelectorExpression Selector{get; }

    public List<PostActionExpression> ? PostActions{get;}

     
    public PostActionExpression(Expressions name, SelectorExpression selector, params PostActionExpression  [] postAction)
    {
        Name = name;
        Selector = selector;
        PostActions=new List<PostActionExpression>();
        foreach (var item in postAction)
        {
          PostActions.Add(item);
        }
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }

    public override object Evaluate(Scope scope)
    {
        throw new NotImplementedException();
    }
}
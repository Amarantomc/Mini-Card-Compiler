
using System.Runtime.Serialization;
using GWent;

public class PostActionExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.PostActionExpression;

    public Expressions Name { get; }

    public SelectorExpression Selector{get; set;}

    public PostActionExpression ? Child{get;}

    private Scope? scope{get;set;}
    //Falta Lista de Assigment Expression

     
    public PostActionExpression(Expressions name, SelectorExpression selector)
    {
        Name = name;
        Selector = selector;
         
    }

    public PostActionExpression(Expressions name, SelectorExpression selector, PostActionExpression child)
    {
        Name = name;
        Selector = selector;
        Child = child;
    }

    public override bool CheckSemantic()
    {
         if(Name is null || Name.Evaluate(scope!) is not string) throw new Exception("Invalid or Missing Name Expression");
         if(Selector is not null) Selector.Evaluate(scope!);
         if(Child is not null) Child.Evaluate(scope!);
         return true;
    }

    public bool CheckSemantic(Scope scope)
    {
       if(Name is null || Name.Evaluate(scope!) is not string) throw new Exception("Invalid or Missing Name Expression");
         if(Selector is not null) Selector.CheckSemantic(scope!);
         if(Child is not null) Child.CheckSemantic(scope!);
         return true;  
    }

    public override object Evaluate(Scope scope)
    {
          
         
         return 0;
    }

    public object Evaluate(Scope scope, SelectorExpression parent)
    {
         this.scope=scope;
         if(Selector is not null && Selector.Source.Evaluate(scope!) is string source && source=="parent")
         {
           Selector.Source=parent.Source;
         }
         if(Selector is not null) parent=Selector;
         return GetEffect(this,parent,new List<(EffectExpression,SelectorExpression)>());

    }

    private List<(EffectExpression,SelectorExpression)> GetEffect(PostActionExpression postActionExpression, SelectorExpression parent, List<(EffectExpression, SelectorExpression)> list)
    {
       var effect=Context.Effects.Find(x=> x.Name.Evaluate(scope!).Equals(postActionExpression.Name.Evaluate(scope!) ));
       if(effect is null) throw new Exception($"Effect {postActionExpression.Name.Evaluate(scope!)} does not exist");
       list.Add((effect,parent)); 
       if(postActionExpression.Child is not null) return GetEffect(postActionExpression.Child,parent,list);
       return list;
    }
}
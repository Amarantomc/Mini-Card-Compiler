using System.Linq.Expressions;
using System.Runtime.Serialization;
using GWent;
using Logic;

public class InExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.InExpression;

    public VarExpression Var { get; }
    public VarExpression Collection { get; }

    public InExpression(VarExpression var, VarExpression collection){
        Var = var;
        Collection = collection;
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }

    public override object Evaluate(Scope scope)
    {
        throw new NotImplementedException();
    }
    public bool Evaluate(Scope scope, int index)
    {   //Chequeo que la colleccion coincida y exista
        IEnumerable<Card> cards=(FindScope(scope,Collection.Var.Text).Value is IEnumerable<Card> x ?x: throw new Exception() ) ;
        if(FindVarInScope(scope,Var.Var.Text)) throw new Exception($"Already using this Variable {Var.Var.Text}");
        cards=cards.Skip(index);
        IEnumerator<object> enumerator=cards.GetEnumerator();
        while (enumerator.MoveNext())
        {
            VarExpression var=FindScope(scope,Var.Var.Text);
            var.Value=enumerator.Current;
            return true;
        }
        return false;
    }

    private bool FindVarInScope(Scope scope, string text)
    {
         if(scope is null) return false;
          if(scope.Variables.Exists(x=>x.Var.Text== text ))
         {
            return true;
         }
         return FindVarInScope(scope.Parent!,text);
    }

    private VarExpression FindScope(Scope scope, string text)
    {
         if(scope is null) throw new Exception();
         if(scope.Variables.Exists(x=>x.Var.Text== text ))
         {
            return scope.Variables.Find(x=>x.Var.Text== text)!;
         }
        return FindScope(scope.Parent!,text);
    }
}
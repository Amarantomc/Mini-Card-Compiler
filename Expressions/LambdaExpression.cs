
using GWent;

public class LambdaExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.LambdaExpression;

    public List<Expressions> Variables { get; }
    public Tokens Do { get; }
    public Statement Body { get; }

    public LambdaExpression( Tokens Do, Statement body, params Expressions[] variables)
    {   
        Variables=new List<Expressions>();
        foreach (var item in variables)
        {
           Variables.Add(item); 
        }
        this.Do = Do;
        Body=body;
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
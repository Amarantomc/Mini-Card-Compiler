namespace GWent;

public class  AssignmentExpression : Expressions
{
    public override Tokens.TokenType Type => Tokens.TokenType.AssignmentExpression;

    public VarExpression Identifier { get; }
    public Tokens Op { get; }
    public Expressions Right { get;set; }
    public Tokens.TokenType ? DataType{get;set;}
    private Scope? scope{get;set;}

    public AssignmentExpression( VarExpression identifier,Tokens op, Expressions right){
        Identifier = identifier;
        Op = op;
        Right = right;
        
    }
    public AssignmentExpression( VarExpression identifier,Tokens op, Expressions right, Tokens.TokenType dataType){
        Identifier = identifier;
        Op = op;
        Right = right;
        DataType = dataType;
    }

    public override bool CheckSemantic()
    {
         if(Identifier is null ) throw new Exception("Missing Id");
         if(Op.Type== Tokens.TokenType.Assignment && Right is null) throw new Exception("Missing Right Expression ");
         if(Op.Type!= Tokens.TokenType.Assignment && Op.Type!= Tokens.TokenType.TwoDots && !FindVar(scope!)) throw new Exception($"Missing {Identifier.Var.Text}");
         return true;


    }

    public override object Evaluate(Scope scope)
    {   
        this.scope=scope;
        CheckSemantic();
        var right=Right.Evaluate(scope);
        if(FindVar(scope))
        {
            
           VarExpression variable=null!;
           if(right is bool) variable=new VarExpression(Identifier.Var,Tokens.TokenType.BoolKeyword,right);
           if(right is double) variable=new VarExpression(Identifier.Var, Tokens.TokenType.NumberKeyword,right);
           if(right is string) variable=new VarExpression(Identifier.Var, Tokens.TokenType.StringKeyword,right);
           VarExpression var=ReturnVar(scope);
            if(Op.Type== Tokens.TokenType.Assignment) var.Value=variable!.Value;
            else if(Op.Type== Tokens.TokenType.PlusEquals && var.Value is double x && variable!.Value is double y)
            {   x+=y;
                var.Value=x;
            } 
            else if(Op.Type== Tokens.TokenType.MinusEquals && var.Value is double v && variable!.Value is double w)
            {
                v-=w;
                var.Value=v;
            } 
           else if(Op.Type== Tokens.TokenType.MullEquals && var.Value is double a && variable!.Value is double b)
            {
                a*=b;
                var.Value=a;
            } 
           else if(Op.Type== Tokens.TokenType.DivEquals && var.Value is double c && variable!.Value is double d)
            {
                c/=d;
                var.Value=c;
            }
            else throw new Exception($"Invalid operation between {var.Value!.GetType()}, {variable.Value!} whith the operator {Op.Text}");

            
        } else if(!FindVar(scope))
        {
            if(Op.Type== Tokens.TokenType.Assignment || Op.Type== Tokens.TokenType.TwoDots)
            {
                 
                if(right is double) scope.Variables.Add(new VarExpression(Identifier.Var, Tokens.TokenType.Number,right));
               else if(right is string) scope.Variables.Add(new VarExpression(Identifier.Var, Tokens.TokenType.StringKeyword,right));
                else if(right is bool) scope.Variables.Add(new VarExpression(Identifier.Var, Tokens.TokenType.BoolKeyword,right));
                 

            } else throw new Exception($"Invalid Assignment Expression for {Identifier.Var.Text}");
        }

         return right;
    }

    private VarExpression ReturnVar(Scope scope)
    {
          if(scope is null) throw new Exception("Missing Variable");
        if(scope.Variables.Exists(x=>x.Var.Text==Identifier.Var.Text)){
           return scope.Variables.Find(x=>x.Var.Text==Identifier.Var.Text)!; 
        }
        return ReturnVar(scope.Parent!);
    }

    private bool FindVar(Scope scope)
    {
        if(scope is null) return false;
        if(scope.Variables.Exists(x=>x.Var.Text==Identifier.Var.Text))return true;
        return FindVar(scope.Parent!);
        
    }
}
using System.Xml;

namespace GWent;
class UnaryExpression: Expressions{
    public readonly Expressions Right;
     
    public Tokens Op { get; }

    private Scope? scope{get;set;}

    public UnaryExpression( Tokens op, Expressions right)
    {
         
        Op = op;
        Right = right;
    }

    public override Tokens.TokenType Type => Tokens.TokenType.UnaryExpression;

    public override object Evaluate(Scope scope)
    {
         this.scope=scope;
         var check=CheckSemantic();
         var operand=Right.Evaluate(scope);
         if(Op.Type== Tokens.TokenType.Plus && check)
         {
            return  operand;
         }
         
         if(Op.Type== Tokens.TokenType.Minus && check)
         {
             return -(double)operand;
         }
         if(Op.Type== Tokens.TokenType.Not && check)
         {
            return !(bool)operand;
         }
         if(Op.Type== Tokens.TokenType.PlusPlus && check)
         {  
            double x=Convert.ToDouble(operand);
            VarExpression right=(VarExpression)Right;
            UpdateVariable(scope, right.Var,1);
            return x++;  
         }
          if(Op.Type== Tokens.TokenType.MinusMinus && check)
         {  
            double x=Convert.ToDouble(operand);
            VarExpression right=(VarExpression)Right;
            UpdateVariable(scope, right.Var,-1);
            return x--; 
         }
         
         throw new Exception("Problems with UnaryExpression");

    }

    private void UpdateVariable(Scope scope, Tokens var, int v)
    {
         VarExpression var1=scope.Variables.Find(y=> y.Var.Value.Equals(var.Value))!;
         if(var1 is not null)
         {
            var1.Value=(double) var1.Var.Value +v;
            int index= scope.Variables.FindIndex(y=> y.Var.Value.Equals(var.Value));
            scope.Variables[index].Value=var1.Value;
            return;

         }
         if(scope.Parent is not null) UpdateVariable(scope.Parent,var,v);
    }

    public override bool CheckSemantic()
    {    
         
         var operand=Right.Evaluate(scope!);
        if(Op.Type== Tokens.TokenType.Plus || Op.Type== Tokens.TokenType.Minus)
        {
            if(operand is double  ) return true;
        }
        else if(Op.Type== Tokens.TokenType.PlusPlus || Op.Type== Tokens.TokenType.MinusMinus)
        {
            if(operand is double && Right is VarExpression  ) return true;
        }
         else if(Op.Type== Tokens.TokenType.Not)
         {
            if(operand is bool ) return true;
         }
          
         
             throw new Exception($"Unary operator {Op.Text} is not define for {operand.GetType}");
         
        
    }
}
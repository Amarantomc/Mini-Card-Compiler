
namespace GWent;
 
class BinaryExpression: Expressions{
    public readonly Expressions Right;
    public Expressions Left { get; }
    public Tokens Op { get; }

    

    public BinaryExpression(Expressions left, Tokens op, Expressions right)
    {
         Left = left;
         Op = op;
         Right = right;
    }

    public override Tokens.TokenType Type => Tokens.TokenType.BinaryExpression;

    public override object Evaluate(Scope scope)
    {
         var left=Left.Evaluate(scope);
         var right=Right.Evaluate(scope);
          
         switch (Op.Type)
         {
            case Tokens.TokenType.Plus:
            CheckOperand(left,Op,right);
            return (double)left +  (double)right;
            
            case Tokens.TokenType.Minus:
            CheckOperand(left,Op,right);
            return (double)left - (double)right;

            case Tokens.TokenType.Mull:
            CheckOperand(left,Op,right);
            return (double)left * (double)right;

            case Tokens.TokenType.Div:
            CheckOperand(left,Op,right);
            return (double)left / (double)right;

            case Tokens.TokenType.Pow:
            CheckOperand(left,Op,right);
            return Math.Pow((double)left,(double)right);

            case Tokens.TokenType.Greater:
            CheckOperand(left,Op,right);
            return (double)left > (double)right;

            case Tokens.TokenType.GreaterEquals:
            CheckOperand(left,Op,right);
            return (double)left >= (double)right;

            case Tokens.TokenType.Less:
            CheckOperand(left,Op,right);
            return (double)left < (double)right;

            case Tokens.TokenType.LessEquals:
            CheckOperand(left,Op,right);
            return (double)left <= (double)right;

            case Tokens.TokenType.EqualsEquals:
            CheckOperand(left,Op,right);
            return (double)left == (double)right;

            case Tokens.TokenType.Or:
            CheckOperand(left,Op,right);
            return (bool)left || (bool)right;

            case Tokens.TokenType.And:
            CheckOperand(left,Op,right);
            return (bool)left && (bool)right;

            case Tokens.TokenType.Concat:
            CheckOperand(left,Op,right);
            return (string)left + (string)right;

            case Tokens.TokenType.TwoConcats:
            CheckOperand(left,Op,right);
            return (string)left +" "+ (string)right;

            
            default: throw new Exception($"Invalid Operator {Op.Text}");
         }
    }

    private void CheckOperand(object left, Tokens op, object right)
    {
        switch (op.Type)
        {  
            case Tokens.TokenType.Plus:
            case Tokens.TokenType.Minus:
            case Tokens.TokenType.Mull:
            case Tokens.TokenType.Div:
            case Tokens.TokenType.Pow:
            case Tokens.TokenType.Greater:
            case Tokens.TokenType.GreaterEquals:
            case Tokens.TokenType.Less:
            case Tokens.TokenType.LessEquals:
            case Tokens.TokenType.EqualsEquals:
            {
              if(left is double && right is double) return;
              throw new Exception($"Operator {op.Text} cannot work with {left}, {right}");
            }
             
            case Tokens.TokenType.Or:
            case Tokens.TokenType.And:
            {
                if(left is bool && right is bool) return;
                throw new Exception($"Operator {op.Text} cannot work with {left}, {right}");
            }
            case Tokens.TokenType.Concat:
            case Tokens.TokenType.TwoConcats:
            {
                if(left is string && right is string) return;
                throw new Exception($"Operator {op.Text} cannot work with {left}, {right}");
            }
            

            
            default: throw new Exception($"Invalid Operator {op.Text}");
        }
    }

    public override bool CheckSemantic()
    {  //No se usa en este caso chequeo en el Evaluate
       return true;
    }
}
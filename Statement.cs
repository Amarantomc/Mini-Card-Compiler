 using GWent;
 public class Statement : Expressions{
       public Queue<Expressions> Expressions{get;set;}
       public Queue<Statement> Childrens{get;set;}

       

    public override Tokens.TokenType Type =>  Tokens.TokenType.StatementExpression;

    public Statement( ) {
          Expressions=new Queue<Expressions>();
          Childrens= new Queue<Statement>();
          
      }

      public Statement(params Expressions [] expressions){
         Expressions=new Queue<Expressions>();
         Childrens=new Queue<Statement>();
         foreach (var item in expressions)
         {
            Expressions.Enqueue(item);
         }
      }

    public override object Evaluate(Scope scope)
    {
       foreach (var item in Expressions)
       {
          item.Evaluate(scope);
       } 
       return null!; 
    }

    public override bool CheckSemantic()
    {
         foreach (var item in Expressions)
         {
            if(item is WhileExpression || item is ForExpression|| item is UnaryExpression || item is AssignmentExpression|| item is DotExpression)
            {
              if(item is UnaryExpression unary)
              {
                 if(unary.Op.Type!= Tokens.TokenType.PlusPlus || unary.Op.Type!= Tokens.TokenType.MinusMinus)
                 {
                    throw new Exception("Invalid Expression in Statment");
                 }
                 continue;
              }
              continue;
              
            }
            throw new Exception("Invalid Expression in Statment");
         }
         return true;
         
    }
}
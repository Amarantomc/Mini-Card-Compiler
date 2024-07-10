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

    public override object Evaluate()
    {
        throw new NotImplementedException();
    }

    public override bool CheckSemantic()
    {
        throw new NotImplementedException();
    }
}
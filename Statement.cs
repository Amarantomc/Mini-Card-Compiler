 using GWent;
 public class Statement{
       public Queue<Expressions> Expressions{get;set;}
       public Queue<Statement> Childrens{get;set;}
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

 }
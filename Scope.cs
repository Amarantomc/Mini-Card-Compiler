  
   public class Scope{

     public List<VarExpression> Variables{get;set;}
     public Scope ? Parent{get; set;}

     public Scope()
     {
       Variables= new List<VarExpression>();

     } 

     public Scope ( params VarExpression []  expressions)
     {  
        Variables=new List<VarExpression>();
        foreach (var item in expressions)
        {
           Variables.Add(item);
        }
     }

     public Scope CreateChild()
     {
       Scope child=new Scope();
       child.Parent=this;
       return child;
     }
    
       
    
   }
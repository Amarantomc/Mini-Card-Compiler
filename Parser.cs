
   namespace GWent;
 
public class Parser{ 
    public List<Tokens> tokens = new List<Tokens>();
    
    private int position=0;
    private Tokens CurrentToken {get{if(position>=tokens.Count) 
    return tokens[tokens.Count-1]; 
    return
    tokens[position];}}


     

    public Parser(string text)
    {   
      Error.ErrorList.Clear();
        var lexer = new Lexer(text);
        Tokens token;
        while( lexer.position!=text.Length){
            token=lexer.GetTokens();
            if(token.Type!= Tokens.TokenType.WhiteSpace)
            tokens.Add(token);
             
        } 
         
     
    } 

    private Tokens NextToken(){
     Tokens token = CurrentToken;
        position++;

        return token;
        
    }  
     private Tokens Match( Tokens.TokenType type){
      
        if(CurrentToken.Type==type) return NextToken();
         Error.ErrorList.Add(new Error(Error.ErrorType.Semantic, position, "Unexpected token "+ CurrentToken.Text));
        return new Tokens(null!, CurrentToken.Position, type, null!); 
     }
     
       private Expressions Factor()
    {
        switch (CurrentToken.Type)
        {
            case Tokens.TokenType.OpenParen:
                {
                    var openParentesis = Match(Tokens.TokenType.OpenParen);
                    var expresion = ParseGlobalExpresion();
                    var right = Match(Tokens.TokenType.CloseParen);
                    return new ParenthesisExpression(openParentesis, expresion, right);
                }

            case Tokens.TokenType.TrueKeyword:
            case Tokens.TokenType.FalseKeyword:
                {
                    var token = NextToken();
                    var value = token.Type == Tokens.TokenType.TrueKeyword;
                    return new LiteralExpression(token, value);

                }

                  default:
                var numberToken= Match(Tokens.TokenType.Number);
                return new LiteralExpression(numberToken);
        }

        
       
    } 
     private Expressions ParseExpresion(int parentPrecedence=0){
          Expressions left;
          var unaryPrecedence= UnaryOpPrecedence(CurrentToken.Type);
          if(unaryPrecedence!=0 && unaryPrecedence>=parentPrecedence)
          {
             var op=NextToken();
             var operand=ParseExpresion(unaryPrecedence);
             left=new UnaryExpression(op,operand);
          } else 
          left= Factor();
        while( true){
          var precedence =OpPrecedence(CurrentToken.Type);
          if(precedence==0|| precedence<=parentPrecedence) break;
          var operatorToken= NextToken();
          var right= ParseExpresion(precedence);
          left= new BinaryExpression(left, operatorToken, right);

        }
         return left;
     }  
     
      private static int OpPrecedence(Tokens.TokenType op){
       switch(op){

       case Tokens.TokenType.And:
        case Tokens.TokenType.Or:
        return 1;
       
        case Tokens.TokenType.EqualsEquals:
        case Tokens.TokenType.NotEquals:
        return 2;
        
        case Tokens.TokenType.Greater:
        case Tokens.TokenType.Less:
        return 3;
        case Tokens.TokenType.Plus:
        case Tokens.TokenType.Minus:
        return 4;
        case Tokens.TokenType.Mull:
        case Tokens.TokenType.Div:
        return 5;
         default:
         return 0;

       }
     } 
         private static int UnaryOpPrecedence(Tokens.TokenType op){
       switch(op){
        case Tokens.TokenType.Plus:
        case Tokens.TokenType.Minus:
        case Tokens.TokenType.Not:
        return 6;
         
         default:
         return 0;

       }
     }

       private Expressions ParseGlobalExpresion(){
         if(CurrentToken.Type == Tokens.TokenType.EffectExpression)
         {

         }
         else if(CurrentToken.Type == Tokens.TokenType.CardExpression)
         {

         }
         else if( CurrentToken.Type== Tokens.TokenType.ForExpression)
         {
             return ForExpressions();
         }
           else if( CurrentToken.Type== Tokens.TokenType.WhileExpression)
         {
            return WhileExpression();
         }
        
        
        return ParseExpresion();
       }
     public SyntaxTree Parse(){
        
      var expresion= ParseGlobalExpresion();
      var EOF= Match(Tokens.TokenType.EOF);
      if(EOF.Text!=";") Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,position,"; was Expected"));
      return new SyntaxTree(expresion,EOF);
     } 

     private Tokens LookAhead(int n=0){
       return tokens[position+n];
     }

     private bool CanLookAhead(int n=0){
       return tokens.Count -position> n;
     }

    private Expressions WhileExpression(){

      NextToken();
      Match(Tokens.TokenType.OpenParen);
      var condition=ParseExpresion();
      Match(Tokens.TokenType.CloseParen);
      if(CurrentToken.Type== Tokens.TokenType.OpenKey)
      {   // statment se parsea en factor arreglar
          return new WhileExpression(condition,(Statement)Factor());
      }
       var body= ParseGlobalExpresion();
      
      return new WhileExpression(condition,new Statement(body));
    }

    private Expressions ForExpressions(){
       NextToken();
       var variable= Match(Tokens.TokenType.VarExpression);
       Match(Tokens.TokenType.InKeyword);
       var collection=Match(Tokens.TokenType.VarExpression);
       var inExpression= new InExpression(variable,collection);

       if(CurrentToken.Type== Tokens.TokenType.OpenKey)
       {
         return new ForExpression(inExpression,(Statement)Factor());
       }
       Match(Tokens.TokenType.OpenKey);
       return null!;
       
    }

    private Expressions EffectExpressions(){
      NextToken();
      Match(Tokens.TokenType.OpenKey);
      AssignmentExpression name=null!;
      ParamsExpression effectParams=null!;
      ActionExpression action=null!;

      while(CurrentToken.Type!= Tokens.TokenType.CloseKey)
      {
           if(CurrentToken.Type == Tokens.TokenType.NameKeyword && name is null)
           {
              var nameToken=NextToken();
              var op=Match(Tokens.TokenType.TwoDots);
              name=new AssignmentExpression(nameToken,op, ParseGlobalExpresion());
           }
           if ( CurrentToken.Type== Tokens.TokenType.ParamsKeyword && effectParams is null)
           {
              NextToken();
              Match(Tokens.TokenType.TwoDots);
              Match(Tokens.TokenType.OpenKey);
              List<Expressions> paramsExpression= new List<Expressions>();

              while(CurrentToken.Type!= Tokens.TokenType.CloseKey)
              {
                var variable=Match(Tokens.TokenType.VarExpression);
                if( CurrentToken.Type== Tokens.TokenType.TwoDots)
                {
                    NextToken();
                  if( CurrentToken.Type== Tokens.TokenType.NumberKeyword ||CurrentToken.Type== Tokens.TokenType.BoolKeyword|| CurrentToken.Type== Tokens.TokenType.StringKeyword)
                  {
                    //paramsExpression.Add(new VarExpression(variable, CurrentToken));
                    NextToken();
                  } else Error.ErrorList.Add(new Error(Error.ErrorType.Semantic, CurrentToken.Position,"Invalid Type "+CurrentToken.Text));

                }else Error.ErrorList.Add(new Error(Error.ErrorType.Syntax, CurrentToken.Position,"Missing :"));
                 
                 if(CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
                 else if( CurrentToken.Type== Tokens.TokenType.CloseKey) continue;
                 else throw new Exception("Unexpected Token "+ CurrentToken.Text);

              }
                Match( Tokens.TokenType.CloseKey);
                effectParams=new ParamsExpression(new Statement(paramsExpression.ToArray()));
           }
            if(CurrentToken.Type== Tokens.TokenType.ActionKeyword && action is null)
            {
              NextToken();
              Match(Tokens.TokenType.TwoDots);

            }

           
      }

      return null!;
    }

}
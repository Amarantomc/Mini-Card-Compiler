
   namespace GWent;
 
public class Parser{ 
    public List<Tokens> Token = new List<Tokens>();
    public List<Tokens.TokenType> Properties;
    public List<Tokens.TokenType> Functions;
    
    private int position=0;
    private Tokens CurrentToken {get{if(position>=Token.Count) 
    return Token[Token.Count-1]; 
    return
    Token[position];}}


     

    public Parser(string text)
    {   
      Error.ErrorList.Clear();
        var lexer = new Lexer(text);
        Tokens token;
        while( lexer.position!=text.Length)
        {
            token=lexer.GetTokens();
            if(token.Type!= Tokens.TokenType.WhiteSpace)
            Token.Add(token);
             
        } 
          Properties=new List<Tokens.TokenType>{Tokens.TokenType.DeckKeyword, Tokens.TokenType.FieldKeyword,
         Tokens.TokenType.GraveyardKeyword, Tokens.TokenType.HandKeyword, Tokens.TokenType.NameKeyword, Tokens.TokenType.OwnerKeyword
         , Tokens.TokenType.PowerKeyword, Tokens.TokenType.TriggerPlayerKeyword, Tokens.TokenType.TypeKeyword, Tokens.TokenType.BoardKeyword};

         Functions=new List<Tokens.TokenType>{ Tokens.TokenType.DeckOfPlayerKeyword, Tokens.TokenType.FieldOfPlayerKeyword,
                   Tokens.TokenType.FindKeyword, Tokens.TokenType.GraveyardOfPlayerKeyword, Tokens.TokenType.PopKeyword, Tokens.TokenType.PushKeyword,
                     Tokens.TokenType.RemoveKeyword, Tokens.TokenType.SendBottomKeyword, Tokens.TokenType.ShuffleKeyword, Tokens.TokenType.HandOfPlayerKeyword};
         
        
    } 

    private Tokens NextToken(){
     Tokens token = CurrentToken;
        position++;

        return token;
        
    }  
     private Tokens Match( Tokens.TokenType type){
      
        if(CurrentToken.Type==type) return NextToken();
         Error.ErrorList.Add(new Error(Error.ErrorType.Semantic, position, "Unexpected type: "+ CurrentToken.Type.ToString()+", Expected type "+type.ToString()));
         NextToken();
        return new Tokens(null!, CurrentToken.Position, type, null!); 
     }
     
       private Expressions Factor()
    {
        switch (CurrentToken.Type)
        {   
             case Tokens.TokenType.Number:
               var numberToken= Match(Tokens.TokenType.Number);
                return new LiteralExpression(numberToken,numberToken.Value);
              
             case Tokens.TokenType.String:
                var stringToken=Match(Tokens.TokenType.String);
                return new LiteralExpression(stringToken,stringToken.Value);
              
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
            case Tokens.TokenType.OpenKey:
            {
               NextToken();
               List<Expressions> expressions=new List<Expressions>();
               while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
               {  
                  if(CurrentToken.Type== Tokens.TokenType.EOF)
                  {
                    NextToken();
                    continue;
                  }
                  expressions.Add(ParseGlobalExpresion());
                  //Probar
               }
               Match(Tokens.TokenType.CloseKey);
               return new Statement(expressions.ToArray());
            } 
           
            case Tokens.TokenType.Identifier:
             var id=new VarExpression(NextToken());
             while (CurrentToken.Type== Tokens.TokenType.Dot)
             {
               var op=NextToken();
               var right=Factor();
               return new DotExpression(id,op,right);
             }
             return id;

             case Tokens.TokenType.PopKeyword:
             {
               return new FunctionExpression(Tokens.TokenType.PopKeyword);
             }
             case Tokens.TokenType.ShuffleKeyword:
             {
              return new FunctionExpression(Tokens.TokenType.ShuffleKeyword);
             }
             case Tokens.TokenType.FindKeyword:
             {
               var type=NextToken().Type;
               Match(Tokens.TokenType.OpenParen);
               var body=LambdaExpressions();
               Match(Tokens.TokenType.CloseParen);
               return new FunctionExpression(type,body);
             }

                  default:
                   if(Properties.Contains(CurrentToken.Type))
                   {
                       var type=NextToken().Type;
                       if(CurrentToken.Type== Tokens.TokenType.OpenBracket )
                       {  
                          NextToken();
                          var body=ParseExpresion();
                          Match( Tokens.TokenType.CloseBracket);
                          return new FunctionExpression(type,body);
                       }
                        

                       return new FunctionExpression(type);

                   }
                   else if(Functions.Contains(CurrentToken.Type))
                   {
                     var type=NextToken().Type;
                     if(CurrentToken.Type== Tokens.TokenType.OpenParen)
                     {
                       NextToken();
                       var body=ParseGlobalExpresion();
                       Match(Tokens.TokenType.CloseParen);
                       return new FunctionExpression(type,body);
                     }
                     return new FunctionExpression(type);
                   }

                   Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Unexcpedted token "+CurrentToken.Text));
                   return null!;
                   
                 
                    
                
                  
                
        }

        
       
    } 
     private Expressions ParseExpresion(int parentPrecedence=0){
          Expressions left;
          bool isId=false;
          VarExpression id=null!;
          if(CurrentToken.Type== Tokens.TokenType.Identifier)
          {  
             id= (VarExpression)Factor();
              
             isId=true;
          }
          var unaryPrecedence= UnaryOpPrecedence(CurrentToken.Type);
          if(unaryPrecedence!=0 && unaryPrecedence>=parentPrecedence)
          {
             var op=NextToken();
              if(isId  &&( op.Type== Tokens.TokenType.PlusPlus || op.Type== Tokens.TokenType.MinusMinus))
              {
                 left=new UnaryExpression(op,id);
              } else {
                var operand=ParseExpresion(unaryPrecedence);
                left=new UnaryExpression(op,operand);
              }
             
          } else 
          left= Factor();
        while( true){
          var precedence =OpPrecedence(CurrentToken.Type);
          if(precedence==0|| precedence<=parentPrecedence) break;
          var operatorToken= NextToken();
          if( operatorToken.Type== Tokens.TokenType.Dot){
             var body=ParseGlobalExpresion();
             left=new DotExpression(left,operatorToken,body);
              
          }
           else if( operatorToken.Type== Tokens.TokenType.Assignment || operatorToken.Type== Tokens.TokenType.PlusEquals || 
                operatorToken.Type== Tokens.TokenType.MinusEquals || operatorToken.Type== Tokens.TokenType.MullEquals || operatorToken.Type== Tokens.TokenType.DivEquals)
               { 
                 var right= ParseGlobalExpresion();
                 left=new AssignmentExpression(left,operatorToken,right);
               }
          else {
              var right= ParseExpresion(precedence);
              left= new BinaryExpression(left, operatorToken, right);
              
          } 

        } 
         
         return left;
     }  
     
      private static int OpPrecedence(Tokens.TokenType op){
       switch(op){
      
       case Tokens.TokenType.Assignment:
       case Tokens.TokenType.PlusEquals:
       case Tokens.TokenType.MinusEquals:
       case Tokens.TokenType.MullEquals:
       case Tokens.TokenType.DivEquals:
       return 1;
       case Tokens.TokenType.And:
        case Tokens.TokenType.Or:
        return 2;
       
        case Tokens.TokenType.EqualsEquals:
        case Tokens.TokenType.NotEquals:
        return 3;
        
        case Tokens.TokenType.Greater:
        case Tokens.TokenType.Less:
        case Tokens.TokenType.GreaterEquals:
        case Tokens.TokenType.LessEquals:
        return 4;
        case Tokens.TokenType.Concat:
        case Tokens.TokenType.TwoConcats:
        return 5;
        case Tokens.TokenType.Plus:
        case Tokens.TokenType.Minus:
        return 6;
        case Tokens.TokenType.Mull:
        case Tokens.TokenType.Div:
        return 7;
        case Tokens.TokenType.Pow:
        return 8;
        
        case Tokens.TokenType.Dot:
        return 9;
         default:
         return 0;

       }
     } 
         private static int UnaryOpPrecedence(Tokens.TokenType op){
       switch(op){
        case Tokens.TokenType.Plus:
        case Tokens.TokenType.Minus:
        case Tokens.TokenType.Not:
        case Tokens.TokenType.PlusPlus:
        case Tokens.TokenType.MinusMinus:
        return 10;
         
         default:
         return 0;

       }
     }

       private Expressions ParseGlobalExpresion(){
         if(CurrentToken.Type == Tokens.TokenType.EffectKeyword)
         {
            return EffectExpressions();
         }
         else if(CurrentToken.Type == Tokens.TokenType.CardKeyword)
         {
            return CardExpressions();
         }
         else if( CurrentToken.Type== Tokens.TokenType.ForKeyword)
         {
             return ForExpressions();
         }
           else if( CurrentToken.Type== Tokens.TokenType.WhileKeyword)
         {
            return WhileExpression();
         }
          else if( CurrentToken.Type== Tokens.TokenType.Identifier)
          {
              if(LookAhead(1).Type== Tokens.TokenType.Assignment || LookAhead(1).Type== Tokens.TokenType.EOF || LookAhead(1).Type== Tokens.TokenType.PlusEquals || 
               LookAhead(1).Type== Tokens.TokenType.MinusEquals ||LookAhead(1).Type== Tokens.TokenType.MullEquals ||LookAhead(1).Type== Tokens.TokenType.DivEquals
               || LookAhead(1).Type== Tokens.TokenType.TwoDots )
               {
                 return AssignmentExpressions();
               }
              
               
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
       return Token[position+n];
     }

     private bool CanLookAhead(int n=0){
       return Token.Count -position> n;
     }

    private Expressions WhileExpression(){

      NextToken();
      Match(Tokens.TokenType.OpenParen);
      var condition=ParseGlobalExpresion();
      Match(Tokens.TokenType.CloseParen);
      if(CurrentToken.Type== Tokens.TokenType.OpenKey)
      {    
          return new WhileExpression(condition,(Statement)Factor());
      }
       var body= ParseGlobalExpresion();
      
      return new WhileExpression(condition,new Statement(body));
    }

    private Expressions ForExpressions(){
       NextToken();
       var variable= Match(Tokens.TokenType.Identifier);
       var id=new VarExpression(variable);
       Match(Tokens.TokenType.InKeyword);
       var collection=Match(Tokens.TokenType.Identifier);
       var collection1=new VarExpression(collection);
       var inExpression= new InExpression(id,collection1);

       if(CurrentToken.Type== Tokens.TokenType.OpenKey)
       {
         return new ForExpression(inExpression,(Statement)Factor());

       }
        else{
            Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing { "));
            return null!;
        }
      
       
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
              var nameExpression=new VarExpression(nameToken);
              var op=Match(Tokens.TokenType.TwoDots);
              name=new AssignmentExpression(nameExpression,op, ParseGlobalExpresion());
           }
           if ( CurrentToken.Type== Tokens.TokenType.ParamsKeyword && effectParams is null)
           {
              NextToken();
              Match(Tokens.TokenType.TwoDots);
              Match(Tokens.TokenType.OpenKey);
              List<Expressions> paramsExpression= new List<Expressions>();

              while(CurrentToken.Type!= Tokens.TokenType.CloseKey)
              {
                var variable=Match(Tokens.TokenType.Identifier);
                if( CurrentToken.Type== Tokens.TokenType.TwoDots)
                {
                    NextToken();
                  if( CurrentToken.Type== Tokens.TokenType.NumberKeyword ||CurrentToken.Type== Tokens.TokenType.BoolKeyword|| CurrentToken.Type== Tokens.TokenType.StringKeyword)
                  {
                    paramsExpression.Add(new VarExpression(variable, CurrentToken));
                    NextToken();
                  } else Error.ErrorList.Add(new Error(Error.ErrorType.Semantic, CurrentToken.Position,"Invalid Type "+CurrentToken.Text));

                }else Error.ErrorList.Add(new Error(Error.ErrorType.Syntax, CurrentToken.Position,"Missing :"));
                 
                 if(CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
                 else if( CurrentToken.Type== Tokens.TokenType.CloseKey) continue;
                 else throw new Exception("Unexpected Token "+ CurrentToken.Text);

              }
                Match( Tokens.TokenType.CloseKey);
                Match(Tokens.TokenType.Coma);
                effectParams=new ParamsExpression(new Statement(paramsExpression.ToArray()));
           }
            if(CurrentToken.Type== Tokens.TokenType.ActionKeyword && action is null)
            {
              NextToken();
              Match(Tokens.TokenType.TwoDots);
              action=new ActionExpression(LambdaExpressions());
            }

            if(CurrentToken.Type== Tokens.TokenType.Coma &&(LookAhead(1).Type== Tokens.TokenType.NameKeyword||LookAhead(1).Type== Tokens.TokenType.ParamsKeyword ||LookAhead(1).Type== Tokens.TokenType.ActionKeyword))
            NextToken();

           else if( CurrentToken.Type== Tokens.TokenType.CloseKey) continue;
           else {
            Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing ,"));
                      NextToken();
                      break;
           }
        }
         Match(Tokens.TokenType.CloseKey);
         return new EffectExpression(name,action,effectParams);

       
    }

     private LambdaExpression LambdaExpressions()
     {
         List<Expressions> variables=new List<Expressions>();
         List<Expressions> body=new List<Expressions>();

         if(CurrentToken.Type== Tokens.TokenType.OpenParen)
         {
             NextToken();
             while (CurrentToken.Type!= Tokens.TokenType.CloseParen)
             {
                 var id=Match(Tokens.TokenType.Identifier);
                 variables.Add(new VarExpression(id));

                 if(CurrentToken.Type== Tokens.TokenType.Coma)
                 {
                    NextToken();
                    
                 }
                  else if( CurrentToken.Type== Tokens.TokenType.CloseParen)
                  {
                    NextToken();
                    break;
                  }
                  else
                  {
                      Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Invalid Token "+ CurrentToken.Text));
                      NextToken();
                      break;
                      
                  }
             }

              
               
         } else
         {
           Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing Parenthesis"));
           return null!;
         }

          var op=Match(Tokens.TokenType.Do);
          // Hacer comprobacion con Action
          if(CurrentToken.Type== Tokens.TokenType.OpenKey)
          {
             NextToken();
             while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
             {   //comprobar para cambiar a statment
                body.Add(ParseGlobalExpresion());
                var key=Match(Tokens.TokenType.CloseKey);
                if(key is null) break;
             }

              
          } 
          else 
          {
             body.Add(ParseExpresion());
          }
            return new LambdaExpression(op,new Statement(body.ToArray()),variables.ToArray());
            
     }
    
    private Expressions CardExpressions()
    {   
        NextToken();
        Expressions type = null!;
        Expressions name = null!;
        Expressions faction = null!;
        Expressions power = null!;
        List<Expressions> range = new List<Expressions>();
        OnActivationExpression onActivation = null!;
        Match(Tokens.TokenType.OpenKey);

        while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
        {
            if(CurrentToken.Type== Tokens.TokenType.TypeKeyword ||CurrentToken.Type== Tokens.TokenType.NameKeyword ||CurrentToken.Type== Tokens.TokenType.FactionKeyword || CurrentToken.Type== Tokens.TokenType.PowerKeyword)
            {
               var id=new VarExpression(CurrentToken);
               NextToken();
               var op=Match( Tokens.TokenType.TwoDots);
               var right= ParseExpresion();
               if(id.Type== Tokens.TokenType.TypeKeyword) type=new AssignmentExpression(id,op,right);
              else if(id.Type== Tokens.TokenType.NameKeyword) name=new AssignmentExpression(id,op,right);
              else if(id.Type== Tokens.TokenType.FactionKeyword) faction=new AssignmentExpression(id,op,right);
              else if(id.Type== Tokens.TokenType.PowerKeyword) power=new AssignmentExpression(id,op,right);
           }

           if(CurrentToken.Type== Tokens.TokenType.RangeKeyword && !range.Any())
           {
             NextToken();
             Match(Tokens.TokenType.TwoDots);
             Match(Tokens.TokenType.OpenBracket);
            
             while (CurrentToken.Type!= Tokens.TokenType.CloseBracket )
             {
                 range.Add(ParseExpresion());
                 if(CurrentToken.Type== Tokens.TokenType.TwoDots) NextToken();
                 else if(CurrentToken.Type== Tokens.TokenType.CloseBracket) continue;
                 else{
                     Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Expected ,"));
                     break;
                 }
             }
              Match(Tokens.TokenType.CloseBracket);

           } 
            if(CurrentToken.Type== Tokens.TokenType.OnActivationKeyword && onActivation is null)
            {
                NextToken();
                onActivation=OnActivationExpressions();
            }
             
             if(CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
             else if(CurrentToken.Type == Tokens.TokenType.CloseKey) continue;
             else{
                    Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Expected ,"));
                     break;
             }
        }
         return new CardExpression(name,type,faction,power,range,onActivation);
    }

    private OnActivationExpression OnActivationExpressions()
    {
        Match(Tokens.TokenType.TwoDots);
        Match(Tokens.TokenType.OpenBracket);
        
         
        List<Statement> statements=new List<Statement>();
        SelectorExpression selectorExpression=null!;
        PostActionExpression postAction=null!;
        EffectAssignmentExpression effect=new EffectAssignmentExpression();
         
         
         while(CurrentToken.Type!= Tokens.TokenType.CloseBracket)
         {    // no es obligado q despues de bracket haya key
            if(CurrentToken.Type== Tokens.TokenType.OpenKey)
            {  
              NextToken();
            while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
            {
                if(CurrentToken.Type== Tokens.TokenType.SelectorKeyword)
                 {
                   if(selectorExpression is null) selectorExpression=SelectorExpressions();
                   else{
                      Error.ErrorList.Add(new Error(Error.ErrorType.Semantic,CurrentToken.Position,"Cannot have many Selectors in the same block"));
                      break;
                   }
                   
                 }
                else if(CurrentToken.Type== Tokens.TokenType.PostActionKeyword)
                {
                  postAction=PostActionExpressions();
                } 
                else if(CurrentToken.Type== Tokens.TokenType.EffectCardKeyword && effect.Name is null)
              { 
                 
                NextToken();
                Match(Tokens.TokenType.TwoDots);
                 
                if(CurrentToken.Type== Tokens.TokenType.OpenKey)
                {

                while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
                { 
                  if(CurrentToken.Type== Tokens.TokenType.NameKeyword && effect.Name is null)
                  {
                     effect.Name=AssignmentExpressions();
                  } else if( CurrentToken.Type== Tokens.TokenType.Identifier)
                  {
                     effect.Param.Add(AssignmentExpressions());
                  if(LookAhead(1).Type!= Tokens.TokenType.Identifier)
                  {
                    Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing  }"));
                    NextToken();
                    break;
                  }
                  } else{
                    Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Name already exist"));
                    NextToken();
                    break;
                  }
                  
                   
                }
                NextToken();
               }  
                else if(LookAhead(1).Type== Tokens.TokenType.String)
              {  
                 
                var id=new Tokens("Name",CurrentToken.Position, Tokens.TokenType.NameKeyword,"Name");
                var idExpression=new VarExpression(id);
                var op=LookAhead(-1);
                 effect.Name=new AssignmentExpression(idExpression,op,ParseExpresion());
                NextToken();
            } 
             } else
             {
               Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing { "));
               NextToken();
               break;
             } 
               
               if(CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
            } 
              Match(Tokens.TokenType.CloseKey);
              if(CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
               
              statements.Add(new Statement(postAction,selectorExpression,effect));
              effect=null!;
              postAction=null!;
              selectorExpression=null!;
               

            }

             
            
              
              
            else{
                Error.ErrorList.Add(new Error(Error.ErrorType.Semantic,CurrentToken.Position,"Unexpected token "+ CurrentToken.Text));
               NextToken();
               return null!;
              }
             
             if(CurrentToken.Type== Tokens.TokenType.OpenKey|| CurrentToken.Type== Tokens.TokenType.Coma|| CurrentToken.Type== Tokens.TokenType.CloseKey) NextToken();
             else{
                Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing ,"));
               NextToken();
               return null!;
             }

         }
          return new OnActivationExpression(statements);
    }

    private SelectorExpression SelectorExpressions()
    {
       NextToken();
       Match(Tokens.TokenType.TwoDots);
       Match(Tokens.TokenType.OpenKey);
       AssignmentExpression source=null!;
       AssignmentExpression single=null!;
       LambdaExpression predicate=null!;

        while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
        {
            if(CurrentToken.Type== Tokens.TokenType.SourceKeyword && source is null)
            {
               var name=new VarExpression(CurrentToken);
               NextToken();
               var op=Match(Tokens.TokenType.TwoDots);
               source=new AssignmentExpression(name,op,ParseExpresion());
            }
           else if(CurrentToken.Type== Tokens.TokenType.SingleKeyword && single is null)
            {
                var name=new VarExpression(CurrentToken);
                NextToken();
                var op=Match(Tokens.TokenType.TwoDots);
                single=new AssignmentExpression(name,op,ParseExpresion());
            }
            else if(CurrentToken.Type== Tokens.TokenType.PredicateKeyword && predicate is null)
            {
               NextToken();
               Match(Tokens.TokenType.TwoDots);
               predicate=LambdaExpressions();
            } else
             {
               Error.ErrorList.Add(new Error(Error.ErrorType.Semantic,CurrentToken.Position,"Unexpected token "+ CurrentToken.Text));
               NextToken();
               return null!;
            }

            if( CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
            else if(CurrentToken.Type== Tokens.TokenType.CloseKey) continue;
            else{
              Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing ,"));
               NextToken();
               return null!;
            }
        }
         Match(Tokens.TokenType.CloseKey);
         return new SelectorExpression(source,single,predicate);

    }

    private PostActionExpression PostActionExpressions()
    {
       NextToken();
       Match(Tokens.TokenType.TwoDots);
       Match(Tokens.TokenType.OpenKey);
       AssignmentExpression type=null!;
       SelectorExpression selector=null!;
       List<PostActionExpression> postAction= new List<PostActionExpression>();

        while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
        {
            if(CurrentToken.Type== Tokens.TokenType.TypeKeyword && type is null)
            {
               var name=new VarExpression(CurrentToken);
               NextToken();
               var op=Match(Tokens.TokenType.TwoDots);
               type=new AssignmentExpression(name,op,ParseExpresion());
            }
            else if(CurrentToken.Type== Tokens.TokenType.SelectorKeyword &&  selector is null)
            {
              selector=SelectorExpressions();
            }
            else if( CurrentToken.Type== Tokens.TokenType.PostActionKeyword)
            {
               postAction.Add(PostActionExpressions());
            }
            else
            {
               Error.ErrorList.Add(new Error(Error.ErrorType.Semantic,CurrentToken.Position,"Unexpected token "+ CurrentToken.Text));
               NextToken();
               return null!;
            }
              if( CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
            else if(CurrentToken.Type== Tokens.TokenType.CloseKey) continue;
            else{
              Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing ,"));
               NextToken();
               return null!;
            }

           }

            return new PostActionExpression(type,selector,postAction.ToArray());
    }
    
    private AssignmentExpression AssignmentExpressions()
    {
        AssignmentExpression assignment=null!;
        var id=new VarExpression(CurrentToken);
        
        NextToken();
        var op=CurrentToken;
        if(op.Type== Tokens.TokenType.Assignment|| op.Type== Tokens.TokenType.PlusEquals ||op.Type== Tokens.TokenType.MinusEquals ||op.Type== Tokens.TokenType.MullEquals ||op.Type== Tokens.TokenType.DivEquals)
        {
           NextToken();
           assignment=new AssignmentExpression(id,op,ParseExpresion());
           Match(Tokens.TokenType.EOF);
        }
        else if(op.Type== Tokens.TokenType.TwoDots)
        {
          NextToken();
          assignment=new AssignmentExpression(id,op,ParseExpresion());
          Match(Tokens.TokenType.Coma);
          
        } 
        return assignment;
        
      

    }
}
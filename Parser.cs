
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
             
        } // faltan propiedades de cartas
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

            case Tokens.TokenType.TrueKeyword:
            case Tokens.TokenType.FalseKeyword:
                {
                    var token = NextToken();
                    var value = token.Type == Tokens.TokenType.TrueKeyword;
                    return new LiteralExpression(token, value);

                }   
              
             case Tokens.TokenType.OpenParen:
                {
                    var openParentesis = Match(Tokens.TokenType.OpenParen);
                    var expresion = OrExpressions();
                    var right = Match(Tokens.TokenType.CloseParen);
                    return new ParenthesisExpression(openParentesis, expresion, right);
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
                   
               }
               Match(Tokens.TokenType.CloseKey);
               return new Statement(expressions.ToArray());
            } 

            case Tokens.TokenType.Not:
            {
              var op=NextToken();
              var right=OrExpressions();
              return new UnaryExpression(op,right);
            }

            case Tokens.TokenType.Minus:
            {
              var op=NextToken();
              var right=Factor();
              return new UnaryExpression(op,right);
            }

            case Tokens.TokenType.Plus:
            {
              var op=NextToken();
              var right=Factor();
              return new UnaryExpression(op,right);
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
               NextToken();
               Match(Tokens.TokenType.OpenParen);
               Match(Tokens.TokenType.CloseParen);
               return new FunctionExpression(Tokens.TokenType.PopKeyword);
             }
             
             case Tokens.TokenType.ShuffleKeyword:
             {
               NextToken();
               Match(Tokens.TokenType.OpenParen);
               Match(Tokens.TokenType.CloseParen);
              return new FunctionExpression(Tokens.TokenType.ShuffleKeyword);
             }
            
             case Tokens.TokenType.FindKeyword:
             {
               var type=NextToken().Type;
               Match(Tokens.TokenType.OpenParen);
               var body=LambdaExpressions( Tokens.TokenType.PredicateKeyword);
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
                          var body=OrExpressions();
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
           
        
        
        return OrExpressions();
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
           {  // si no es : es , sin tipo
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
                    paramsExpression.Add(new VarExpression(variable, CurrentToken.Type));
                    NextToken();
                  } else Error.ErrorList.Add(new Error(Error.ErrorType.Semantic, CurrentToken.Position,"Invalid Type "+CurrentToken.Text));

                }else if(CurrentToken.Type== Tokens.TokenType.Coma)
                 {
                   paramsExpression.Add(new VarExpression(variable));
                   NextToken();
                 }
                 
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
              action=new ActionExpression(LambdaExpressions( Tokens.TokenType.ActionKeyword));
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

     private LambdaExpression LambdaExpressions(Tokens.TokenType type)
     {
         List<VarExpression> variables=new List<VarExpression>();
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
           NextToken();
            
         }

          var op=Match(Tokens.TokenType.Do);
          // Hacer comprobacion con Action
          if(type== Tokens.TokenType.ActionKeyword)
          {

            if(CurrentToken.Type== Tokens.TokenType.OpenKey)
          {
             NextToken();
             while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
             {   //comprobar para cambiar a statment
                body.Add(ParseGlobalExpresion());
                var key=Match(Tokens.TokenType.CloseKey);
                if(key is null) break;
             }

              
          } else 
           {
              Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing Key"));
              NextToken();
               
           }
          }
          else 
          {
             body.Add(OrExpressions());
          }
            return new LambdaExpression(op,new Statement(body.ToArray()),type,variables.ToArray());
            
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
            {  var Keyword=CurrentToken;
               var id=new VarExpression(CurrentToken);
               NextToken();
               var op=Match( Tokens.TokenType.TwoDots);
               var right= OrExpressions();
               Match(Tokens.TokenType.Coma);
               if(Keyword.Type== Tokens.TokenType.TypeKeyword && type is null) type=new AssignmentExpression(id,op,right);
              else if(Keyword.Type== Tokens.TokenType.NameKeyword && name is null ) name=new AssignmentExpression(id,op,right);
              else if(Keyword.Type== Tokens.TokenType.FactionKeyword && faction is null) faction=new AssignmentExpression(id,op,right);
              else if(Keyword.Type== Tokens.TokenType.PowerKeyword && power is null) power=new AssignmentExpression(id,op,right);
              else{
                 Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Only can have one "+CurrentToken.Text));
                break;
              }
           }

           else if(CurrentToken.Type== Tokens.TokenType.RangeKeyword)
           { 
              if(range.Any()){
                Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Only can have one "+CurrentToken.Text));
                break;
              }
             NextToken();
             Match(Tokens.TokenType.TwoDots);
             Match(Tokens.TokenType.OpenBracket);
            
             while (CurrentToken.Type!= Tokens.TokenType.CloseBracket )
             {   
                 range.Add(OrExpressions());
                  if(CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
                 else if(CurrentToken.Type== Tokens.TokenType.CloseBracket) continue;
                 else{
                     Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Expected ,"));
                     break;
                 } 
             }
              NextToken();
              Match(Tokens.TokenType.Coma);

           } 
           else if(CurrentToken.Type== Tokens.TokenType.OnActivationKeyword)
            {   
                if(onActivation is not null){
                  Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Only can have one "+CurrentToken.Text));
                break;
                }
                NextToken();
                onActivation=OnActivationExpressions();
                Match(Tokens.TokenType.CloseBracket);
            }else{
               Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Unexpected token "+CurrentToken.Text));
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
         {     
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
                else if(CurrentToken.Type== Tokens.TokenType.EffectCardKeyword)
              { 
                 
                NextToken();
                 if(effect.Name is not null)
                 { 
                    Error.ErrorList.Add(new Error(Error.ErrorType.Semantic,CurrentToken.Position-1,"Cannot have many Effect's Name in the same block"));
                    break;
                 }
                Match(Tokens.TokenType.TwoDots);
                 
                if(CurrentToken.Type== Tokens.TokenType.OpenKey)
                {

                while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
                { 
                  NextToken();
                  if(CurrentToken.Type== Tokens.TokenType.NameKeyword && effect.Name is null)
                  {
                     effect.Name=AssignmentExpressions();
                  }  if( CurrentToken.Type== Tokens.TokenType.Identifier)
                  {  
                     effect.Param.Add(AssignmentExpressions());
                     if(CurrentToken.Type== Tokens.TokenType.CloseKey) continue;
                    
                  if(LookAhead(1).Type!= Tokens.TokenType.Identifier)
                  {
                    Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Missing  }"));
                    NextToken();
                    break;
                  }
                  } 
                  
                   
                }
                NextToken();
               }  
                else if(CurrentToken.Type== Tokens.TokenType.String)
              {  
                 
                var id=new Tokens("Name",CurrentToken.Position, Tokens.TokenType.NameKeyword,"Name");
                var idExpression=new VarExpression(id);
                var op=LookAhead(-1);
                 effect.Name=new AssignmentExpression(idExpression,op,OrExpressions());
                NextToken();
            } 
             } else
             {
               Error.ErrorList.Add(new Error(Error.ErrorType.Syntax,CurrentToken.Position,"Invalid Token "));
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
             
             if(CurrentToken.Type== Tokens.TokenType.OpenKey|| CurrentToken.Type== Tokens.TokenType.Coma|| CurrentToken.Type== Tokens.TokenType.CloseKey) continue;
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
        {   // NO ES OBLIGATORIO SINGLE NI PREDICATE
            if(CurrentToken.Type== Tokens.TokenType.SourceKeyword && source is null)
            {
               var name=new VarExpression(CurrentToken);
               NextToken();
               var op=Match(Tokens.TokenType.TwoDots);
               source=new AssignmentExpression(name,op,OrExpressions());
               Match( Tokens.TokenType.Coma);
            }
           else if(CurrentToken.Type== Tokens.TokenType.SingleKeyword && single is null)
            {
                var name=new VarExpression(CurrentToken);
                NextToken();
                var op=Match(Tokens.TokenType.TwoDots);
                single=new AssignmentExpression(name,op,OrExpressions());
            }
            else if(CurrentToken.Type== Tokens.TokenType.PredicateKeyword && predicate is null)
            {
               NextToken();
               Match(Tokens.TokenType.TwoDots);
               predicate=LambdaExpressions( Tokens.TokenType.PredicateKeyword);
            } else
             {
               if(CurrentToken.Type== Tokens.TokenType.Coma) NextToken();
               else if(CurrentToken.Type== Tokens.TokenType.CloseKey) NextToken();
               else {
                Error.ErrorList.Add(new Error(Error.ErrorType.Semantic,CurrentToken.Position,"Unexpected token "+ CurrentToken.Text));
               NextToken();
               return null!;
               }
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
       List<AssignmentExpression> variable=new List<AssignmentExpression>();
       PostActionExpression postAction= null!;
       //hacer support pa variables
        while (CurrentToken.Type!= Tokens.TokenType.CloseKey)
        {
            if(CurrentToken.Type== Tokens.TokenType.TypeKeyword && type is null)
            {
               var name=new VarExpression(CurrentToken);
               NextToken();
               var op=Match(Tokens.TokenType.TwoDots);
               type=new AssignmentExpression(name,op,OrExpressions());
            }
            else if(CurrentToken.Type== Tokens.TokenType.SelectorKeyword &&  selector is null)
            {
              selector=SelectorExpressions();
            }
            else if( CurrentToken.Type== Tokens.TokenType.PostActionKeyword && postAction is null)
            {
               postAction=PostActionExpressions();
            }
            else if(CurrentToken.Type== Tokens.TokenType.Identifier)
            {
                 variable.Add(AssignmentExpressions());
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
            return new PostActionExpression(type,selector,postAction);
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
           assignment=new AssignmentExpression(id,op,OrExpressions());
           Match(Tokens.TokenType.EOF);
        }
        else if(op.Type== Tokens.TokenType.TwoDots)
        {
          NextToken();
          assignment=new AssignmentExpression(id,op,OrExpressions());
          Match(Tokens.TokenType.Coma);
          
        } 
        return assignment;
        
      

    }

    private Expressions OrExpressions()
    {
       var left=AndExpressions();
      while(CurrentToken.Type== Tokens.TokenType.Or)
      {
        var op=NextToken();
        var right=OrExpressions();
        left=new BinaryExpression(left,op,right);

      }
      return left;


    }

    private Expressions AndExpressions()
    {
         var left=CompareExpressions();
      while(CurrentToken.Type== Tokens.TokenType.And)
      {
        var op=NextToken();
        var right=OrExpressions();
        left=new BinaryExpression(left,op,right);

      }
      return left;
    }

    private Expressions CompareExpressions()
    {
         Expressions left=ConcatExpressions();
          while(CurrentToken.Type== Tokens.TokenType.Greater|| CurrentToken.Type== Tokens.TokenType.GreaterEquals||
                CurrentToken.Type== Tokens.TokenType.Less|| CurrentToken.Type== Tokens.TokenType.LessEquals
                || CurrentToken.Type== Tokens.TokenType.EqualsEquals)
      {
        var op=NextToken();
        var right=ConcatExpressions();
        left=new BinaryExpression(left,op,right);

      }
      return left;
    }

    private Expressions ConcatExpressions()
    {
         Expressions left=ArithmeticExpressions();
         while (CurrentToken.Type== Tokens.TokenType.Concat || CurrentToken.Type== Tokens.TokenType.TwoConcats)
         {
           var op=NextToken();
           var right=ArithmeticExpressions();
           left=new BinaryExpression(left,op,right);
         }
         return left;
    }

    private Expressions ArithmeticExpressions()
    {
         Expressions left=ArithmeticMulOrDivExpressions();
         while (CurrentToken.Type== Tokens.TokenType.Plus || CurrentToken.Type== Tokens.TokenType.Minus)
         {
           var op=NextToken();
           var right=ArithmeticMulOrDivExpressions();
           left=new BinaryExpression(left,op,right);
         }
         return left;
    }

    private Expressions ArithmeticMulOrDivExpressions()
    {
          Expressions left=PowExpressions();
         while (CurrentToken.Type== Tokens.TokenType.Mull || CurrentToken.Type== Tokens.TokenType.Div)
         {
           var op=NextToken();
           var right=PowExpressions();
           left=new BinaryExpression(left,op,right);
         }
         return left;
    }

    private Expressions PowExpressions()
    {
          Expressions left=IncrementExpressions();
         while (CurrentToken.Type== Tokens.TokenType.Pow)
         {
           var op= NextToken();
           var right=IncrementExpressions();
           left=new BinaryExpression(left,op,right);
         }
         return left;
    }

    private Expressions IncrementExpressions()
    {
         Expressions left=DotExpressions();
         if(CurrentToken.Type== Tokens.TokenType.PlusPlus|| CurrentToken.Type== Tokens.TokenType.MinusMinus)
         {
           var op=NextToken();
            
           left=new UnaryExpression(op,left);
         }
         return left;
    }

    private Expressions DotExpressions()
    {
       Expressions left=Factor();
         while (CurrentToken.Type== Tokens.TokenType.Dot)
         {
           var op= NextToken();
           var right=Factor();

            
           left=new DotExpression(left,op,right);
         }
         return left;  
    }
}
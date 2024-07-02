


namespace GWent;
 
public class Lexer{
    
     private string text;

     public int position; 

     private char currentChar { get{ 
        if(position>= text.Length) return '\0'; 
     return text[position];}
     }

     

    public Lexer(string text)
    {
        this.text = text;
    }  
    
     private void Advanced(){
        position++;
     }
    private  double Doubles(){
   string result="";
  while(currentChar!='\0'&& (char.IsDigit(currentChar) || currentChar == ',')){
     result+=currentChar;
     Advanced();
  }  
  return double.Parse(result);
 } 

 
   
   public Tokens GetTokens()
      {  
          if(char.IsWhiteSpace(currentChar)){
             var start=position;
             while(char.IsWhiteSpace(currentChar))  Advanced();
             
             var length=position-start;
             var result=text.Substring(start,length);
            return new Tokens(result, start, Tokens.TokenType.WhiteSpace,null! );
          } 
          else if(currentChar==';'){
            return new Tokens(";",position++,Tokens.TokenType.EOF, null!);
           }
           

        else if(char.IsDigit(currentChar)){
            double result=Doubles();
           return new Tokens(result.ToString(), position, Tokens.TokenType.Number, result);
         }  
            
           else if(char.IsLetter(currentChar)){
               int start=position;
               string result="";
               while(char.IsLetter(currentChar)|| char.IsDigit(currentChar)){
                  result+=currentChar;
                  Advanced();
               }
               Tokens.TokenType type= Tokens.GetKeyword(result);
               return new Tokens(result,start,type,null!);
               
            }
          
         else  if(currentChar == '+'){
             Advanced();
             if(currentChar=='='){
               return new Tokens("+=",position++,Tokens.TokenType.PlusEquals, null!);
             }
               if(currentChar=='+'){
               return new Tokens("++",position++,Tokens.TokenType.PlusPlus, null!);
             }
            int pos=position-1;
             return new Tokens("+",pos,Tokens.TokenType.Plus, null!);
           } 
          else  if(currentChar == '-'){
             Advanced();
             if(currentChar=='='){
               return new Tokens("-=",position++,Tokens.TokenType.MinusEquals, null!);
             }
              if(currentChar=='-'){
               return new Tokens("-=",position++,Tokens.TokenType.MinusMinus, null!);
             }
            int pos=position-1;
             return new Tokens("-",pos,Tokens.TokenType.Minus, null!);
           } 
         else   if(currentChar == '*'){
             
             return new Tokens("*",position++,Tokens.TokenType.Mull, null!);
           }
          else  if(currentChar == '/'){
            return new Tokens("/",position++,Tokens.TokenType.Div, null!);
           } 
            else  if(currentChar == '^'){
               return new Tokens("^",position++,Tokens.TokenType.Pow, null!);
           }
          else if(currentChar == '('){
             return new Tokens("(",position++,Tokens.TokenType.OpenParen, null!);
           } 
         else  if(currentChar == ')'){
             return new Tokens(")",position++,Tokens.TokenType.CloseParen, null!);
           } 
            else  if(currentChar == '{'){
             return new Tokens("{",position++,Tokens.TokenType.OpenKey, null!);
           } 
            else  if(currentChar == '}'){
             return new Tokens("}",position++,Tokens.TokenType.CloseKey, null!);
           } 
            else  if(currentChar == '['){
             return new Tokens("[",position++,Tokens.TokenType.OpenBracket, null!);
           } 
             else  if(currentChar == ']'){
             return new Tokens("]",position++,Tokens.TokenType.CloseBracket, null!);
           } 
             else  if(currentChar == ','){
             return new Tokens(",",position++,Tokens.TokenType.Coma, null!);
           } 
             else  if(currentChar == ':'){
             return new Tokens(":",position++,Tokens.TokenType.TwoDots, null!);
           } 
            else  if(currentChar == '.'){
             return new Tokens(".",position++,Tokens.TokenType.Dot, null!);
           } 
          else if(currentChar=='!'){
            Advanced();
            if(currentChar=='='){
              return new Tokens("!=",position++, Tokens.TokenType.NotEquals,null!);
            }
            int pos=position-1;
            return new Tokens("!",pos, Tokens.TokenType.Not,null!);
          }
          else if(currentChar=='>'){
             Advanced();
            if(currentChar=='='){
              return new Tokens(">=",position++, Tokens.TokenType.GreaterEquals,null!);
            }
            int pos=position-1;
            return new Tokens(">",pos, Tokens.TokenType.Greater,null!);
          }
          else if(currentChar=='<'){
              Advanced();
            if(currentChar=='='){
              return new Tokens("<=",position++, Tokens.TokenType.LessEquals,null!);
            }
            int pos=position-1;
            return new Tokens("<",pos, Tokens.TokenType.Less,null!);
          }
          else if(currentChar== '&'){
             Advanced();
             if(currentChar=='&'){
               return new Tokens("&&",position++, Tokens.TokenType.And,null!);
             }
          }
          else if(currentChar== '|'){
             Advanced();
             if(currentChar=='|'){
               return new Tokens("||",position++, Tokens.TokenType.Or,null!);
             }
          } 
            else if(currentChar== '='){
             Advanced();
             if(currentChar=='='){
               return new Tokens("==",position++, Tokens.TokenType.EqualsEquals,null!);
             }
              if(currentChar=='>'){
                 return new Tokens("=>",position++, Tokens.TokenType.Do,null!);
              }
             int pos=position-1;
             return new Tokens("=",pos, Tokens.TokenType.Assignment,null!);
          } 
            else if(currentChar== '@'){
             Advanced();
             if(currentChar=='@'){
               return new Tokens("@@",position++, Tokens.TokenType.TwoConcats,null!);
             }
             int pos=position-1;
             return new Tokens("@",pos, Tokens.TokenType.Concat,null!);
          } 
            
         
         
         
         
          else  if(currentChar=='"'){
              return new Tokens('"'.ToString(),position,Tokens.TokenType.OpenString,null!);
            }
            
            Error.ErrorList.Add(new Error(Error.ErrorType.Lexical,position,"Unknow Token"));
            return new Tokens(text, position++, Tokens.TokenType.Unknow, null!);

           
          
      }

   
}
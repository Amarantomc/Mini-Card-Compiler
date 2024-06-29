   namespace GWent;
 
public class Tokens{
    public string Text { get; }
    public int Position { get; } 

    public TokenType Type {get;}
    public  object Value {get;}

    public enum TokenType{
       // Literal
        Number,
        WhiteSpace,
        OpenString,

        //Aritmetic
        Plus,
        Minus,
        Mull,
        Div,
        OpenParen,
        CloseParen,
         
        PlusEquals,
        MinusEquals,
        MullEquals,
        DivEquals,
         OpenKey,
        CloseKey,
      
        
         
        
       

        //Expression
        BinaryExpression,
        ParenthesisExpression,
        UnaryExpression, 
        LiteralExpression,
        AssignmentExpression,
         WhileExpression,
        ForExpression,
         
       
        

        //Keyword
        TrueKeyword,
        FalseKeyword,
        ForKeyword,
        WhileKeyword,
        Identifier,
        InKeyword,
      
       //Booleans
        Not,
        And,
        Or,
        Greater,
        Less,
        EqualsEquals,
        NotEquals,
        GreaterEquals,
        LessEquals,
       
       //Syntax
        Coma,
         
        Dot,
        Assignment,
         TwoDots,
        TwoConcats,
        Concat,
         Do,
        EOF,
        Unknow,
       
    }
    public Tokens(string Text, int Position, TokenType Type, object value)
    {
        this.Text = Text;
        this.Position = Position;
        this.Type = Type;
        this.Value = value;
    }
}
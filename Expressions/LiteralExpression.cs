   namespace GWent;
 
 class LiteralExpression: Expressions{
    public Tokens Token;
    public object Value;
     
     public LiteralExpression(Tokens token, object value)
     {
        Token=token;
        Value=value;
     }

         public LiteralExpression(Tokens token)
         :this(token,token.Value)
     {
        
     }

    public override Tokens.TokenType Type => Tokens.TokenType.LiteralExpression;

    public override object Evaluate()
    {
         return Value;
    }

    public override bool CheckSemantic()
    {
         return true;
    }
}
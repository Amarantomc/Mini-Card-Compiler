   namespace GWent;
 
public class Tokens{
    public string Text { get; }
    public int Position { get; } 

    public TokenType Type {get;}
    public  object Value {get;}

    #region TokenTypes 
    public enum TokenType{
       // Literal
        Number,
        WhiteSpace,
        OpenString,
        
        #region Aritmetic
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
      
        #endregion
         
        
       
        #region Expression
        //Expression
        BinaryExpression,
        ParenthesisExpression,
        UnaryExpression, 
        LiteralExpression,
        AssignmentExpression,
         WhileExpression,
        ForExpression,

        #endregion
         
       
        
        #region Keyword
        //Keyword
        TrueKeyword,
        FalseKeyword,
        ForKeyword,
        WhileKeyword,
        InKeyword,
         EffectKeyword,
        NameKeyword,
        ParamsKeyword,
        AmountKeyword,
        NumberKeyword,
        ActionKeyword,
        TargetsKeyword,
        ContextKeyword,
        PowerKeyword,
        DeckKeyword,
        HandKeyword,
        OwnerKeyword,
        BoardKeyword,
        AddKeyword,
        DeckOfPlayerKeyword,
        PushKeyword,
        RemoveKeyword,
        PopKeyword,
        ShuffleKeyword,
        Identifier,

        #endregion
      
      #region Booleans
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
        #endregion
       
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
        TypeKeyword,
        FactionKeyword,
        RangeKeyword,
        OnActivationKeyword,
        SelectorKeyword,
        SourceKeyword,
        SingleKeyword,
        PredicateKeyword,
        PostActionKeyword,
        StringKeyword,
        BoolKeyword,
        TriggerPlayerKeyword,
        FieldOfPlayerKeyword,
        GraveyardOfPlayerKeyword,
        FieldKeyword,
        GraveyardKeyword,
        FindKeyword,
        SendBottomKeyword,
        MeleeKeyword,
        RangedKeyword,
        SiegeKeyword,
        handKeyword,
        otherHandKeyword,
        deckKeyword,
        otherDeckKeyword,
        fieldKeyword,
        otherFieldKeyword,
        parentKeyword,
        OpenBracket,
        CloseBracket,
        PlusPlus,
        MinusMinus,
        Pow,
        InExpression,
        VarExpression,
        CardExpression,
        OnActivationExpression,
        EffectAssignmentExpression,
        ParamsExpression,
        SelectorExpression,
        PostActionExpression,
    }

    #endregion
    public Tokens(string Text, int Position, TokenType Type, object value)
    {
        this.Text = Text;
        this.Position = Position;
        this.Type = Type;
        this.Value = value;
    }
    
    #region GetKeyword by String
     public static TokenType GetKeyword(string result)
    {
        switch (result)
        { 
          case "true":
          return  TokenType.TrueKeyword;
          case "false" :
           return  TokenType.FalseKeyword;
           case "in" :
           return  TokenType.InKeyword;
           case "while":
           return  TokenType.WhileKeyword;
           case "for" :
           return  TokenType.ForKeyword;
           case "effect" :
           return  TokenType.EffectKeyword;
           case "Name" :
           return  TokenType.NameKeyword;
           case "Params" :
           return  TokenType.ParamsKeyword;
           case "Amount" :
           return  TokenType.AmountKeyword;
           case "Number" :
           return  TokenType.NumberKeyword;
           case "String" :
           return  TokenType.StringKeyword;
           case "Bool" :
           return  TokenType.BoolKeyword;
           case "Action" :
           return  TokenType.ActionKeyword;
            
           
           case "Power" :
           return  TokenType.PowerKeyword;
           case "Deck" :
           return  TokenType.DeckKeyword;
           case "Hand" :
           return  TokenType.HandKeyword;
           case "Owner" :
           return  TokenType.OwnerKeyword;
           case "Board" :
           return  TokenType.BoardKeyword;
           case "Add" :
           return  TokenType.AddKeyword;
           case "DeckOfPlayer" :
           return  TokenType.DeckOfPlayerKeyword;
           case "Push" :
           return  TokenType.PushKeyword;
           case "Remove" :
           return  TokenType.RemoveKeyword;
           case "Pop" :
           return  TokenType.PopKeyword;
           case "Shuffle" :
           return  TokenType.ShuffleKeyword;
           case "Type" :
           return  TokenType.TypeKeyword;
           case "Faction" :
           return  TokenType.FactionKeyword;
           case "Range" :
           return  TokenType.RangeKeyword;
           case "OnActivation" :
           return  TokenType.OnActivationKeyword;
           case "Selector" :
           return  TokenType.SelectorKeyword;
           case "Source" :
           return  TokenType.SourceKeyword;
           case "Single" :
           return  TokenType.SingleKeyword;
           case "Predicate" :
           return  TokenType.PredicateKeyword;
           case "PostAction" :
           return  TokenType.PostActionKeyword;
           case "TriggerPlayer" :
           return  TokenType.TriggerPlayerKeyword;
           case "FieldOfPlayer" :
           return  TokenType.FieldOfPlayerKeyword;
           case "GraveyardOfPlayer" :
           return  TokenType.GraveyardOfPlayerKeyword;
           case "Field" :
           return  TokenType.FieldKeyword;
           case "Graveyard" :
           return  TokenType.GraveyardKeyword;
           case "Find" :
           return  TokenType.FindKeyword;
           case "SendBottom" :
           return  TokenType.SendBottomKeyword;
           case "Melee" :
           return  TokenType.MeleeKeyword;
           case "Ranged" :
           return  TokenType.RangedKeyword;
           case "Siege" :
           return  TokenType.SiegeKeyword;
           case "hand" :
           return  TokenType.handKeyword;
           case "otherHand" :
           return  TokenType.otherHandKeyword;
           case "deck" :
           return  TokenType.deckKeyword;
           case "otherDeck" :
           return  TokenType.otherDeckKeyword;
           case "field" :
           return  TokenType.fieldKeyword;
           case "otherField" :
           return  TokenType.otherFieldKeyword;
           case "parent" :
           return  TokenType.parentKeyword;
  
       default:
           return  TokenType.Identifier;
        }
    } 
    #endregion
}
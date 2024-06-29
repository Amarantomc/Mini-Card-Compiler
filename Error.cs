   namespace GWent;
 
public class Error{
   public enum ErrorType{
        Lexical,
        Syntax,
        Semantic
        
    }
     
     public readonly ErrorType Type;
     public readonly int Position;
     public readonly string Text; 
     public static List<Error> ErrorList = new List<Error>();



     public Error(ErrorType type, int position, string text)
     {
        this.Type = type;
        this.Position = position;
        this.Text = text;
    }

}
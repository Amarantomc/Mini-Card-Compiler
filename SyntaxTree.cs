   namespace GWent;
 
public class SyntaxTree{
    
    public Expressions root;
    public Tokens EOF;


    public SyntaxTree(Expressions root, Tokens EOF )
    {
        this.root = root;
        this.EOF= EOF;
    } 

    public static SyntaxTree Parse(string text){
       var parser= new Parser(text);
       return parser.Parse();
     
    }



   
}
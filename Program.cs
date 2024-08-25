using GWent;

internal class Program
{ 
    
    private static void Main(string[] args)
    {
       string a= "Context.Hand.Find((x)=>x.Power==5+2+2)";
       string b="i+++3;";
        string input5 = "effect " +
                "{" +
                "Name: " + '\"' + "Draw" + '\"' + "," +
                "Action: (targets,context) => {" +
                "while( !(1 > -(90+8)) )" +
                "i = 0;" +
                "}" +
                "}";
                  string input1 = "effect " +
                "{" +
                "Name: " + '\"' + "Damage" + '\"' + "," +
                "Params: {" +
                "amount: Number" +
                "} ," +
                "Action: (targets,context) => {" +
                "for target in targets {" +
                "i=0;" +
                "while(i++ < ( amount+ (-(1+4*(4^4)*90)) ))" +
                "    i++;" +
                "};" +
                "}" +
                "}";
                 

       Parser c= new Parser(input5);
       c.Parse();
    }
}
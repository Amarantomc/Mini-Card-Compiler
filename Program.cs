using GWent;
using Logic;


internal class Program 
{ 
    
    private static void Main(string[] args)
    {
       string a= "Context.Hand.Find((x)=>x.Power==5+2+2)";
       string b="i+++3;";
       string e="true || true && false";
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
                string input2 = "card " +
                "{" +
                "Type: " + '\"' + "O" + '\"' + "@" + '\"' + "ro" + '\"' + "," +
                "Name: " + '\"' + "Beluga" + '\"' + "," +
                "Power: " + "-(-1-9)" + "," +
                "Faction: " + '\"' + "Pokemon" + '\"' + "," +
                "Range: " + "[" + '\"' + "Melee"  + '\"' + "," + '\"' + "Ranged" + '\"' + ", "+'\"' + "Siege" + '\"'+"] ," +
                "OnActivation: " +
                "[" +
                "{" +
                "Effect:" +
                "{" +
                "Name: " + '\"' + "Damage" + '\"' + "," +
                "amount: 7-2+(-1-9)^(1 - 1 + -1 +1) - 1 ," +
                "}" +
                "Selector:" +
                "{" +
                "Source: " + '\"' + "board" + '\"' + "," +
                "Predicate: " + "(unit) => unit.Power == 9 , " +
                "Single: " + "false || true && !( 5 > 0) " +
                "}" +
                "PostAction: " +
                "{" +
                "Type: " + '\"' + "Draw"+ '\"' + "," +
                "Selector:" +
                "{" +
                "Source: " + '\"' + "parent" + '\"' + "," +
                "Single: !(true || (false && -7 + 9 > 0))," +
                "Predicate: " + "(unit) => unit.Power == 9" +
                "}" +
                "}" +
                "}," +
                "{" +
                "Effect: " + '\"' + "Draw" + '\"' +
                "}" +
                "]" +
                "}" +
                "";
                 
         
         var tree=SyntaxTree.Parse(input5+input1+input2);
          List<Card> f= tree.Visitor();
          
          
       
    }
}
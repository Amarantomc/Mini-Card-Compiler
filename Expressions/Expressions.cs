namespace GWent;


public abstract class Expressions: Node{
    
    public abstract object Evaluate(Scope scope);

    public abstract bool CheckSemantic();

    public static void Copy<T>(List<T> destination, List<T> origin)
    {
         foreach (var item in origin)
         {
            destination.Add(item);
         }
    }
}

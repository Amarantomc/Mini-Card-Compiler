namespace GWent;


public abstract class Expressions: Node{
    
    public abstract object Evaluate();

    public abstract bool CheckSemantic();
}

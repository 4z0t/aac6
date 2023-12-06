
namespace EMark;
public class Program
{
    private static void PrintStack(Stack<Token> stack)
    {
        var stackCopy = new Stack<Token>(stack.Reverse());
        while (stackCopy.Count != 0)
        {
            Console.WriteLine(stackCopy.Pop());
        }
        Console.WriteLine();
    }



    public static void Main(string[] args)
    {
        var input = @"
        <block rows = 3 columns = 3>
            Hellosad as d dsa d 
            <

            column  halign = top


            > Hello block rows =5 world 1 232 3 23   . - * + </column>
            World  sadas  adas 
        </block>
            ";
        var tokenizer = new Tokenizer();
        var r = tokenizer.Tokenize(input);
        var stack = new Stack<Token>(r.Reverse());
        PrintStack(stack);
    }
}

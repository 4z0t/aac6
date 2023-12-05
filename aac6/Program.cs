
class Program
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
        <block>
            <coloumn> Hello world </coloumn>
        </block>
            ";
        var tokenizer = new Tokenizer();
        var r = tokenizer.Tokenize(input);
        var stack = new Stack<Token>(r);
        r.Reverse();
        PrintStack(stack);
    }
}

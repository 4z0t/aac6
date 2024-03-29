﻿
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
        /*
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

        var block = new Block(1, 0, new[]
        {
            new View(2,0, new []
            {
                new View(1,0, "Hello EMark")
            }),
            new View(1, 1)

        });
        Console.WriteLine(block.ToString());
        */
        var input = @"
        <block rows = 3 columns = 3>
            <row>
                Hellosad as d dsa d
            </row>
            <row>
                world
            </row>
            <row>
                <column>
                    EWWWWWWWW
                </column>
            </row>
        </block>";

        var tokenizer = new Tokenizer();
        var r = tokenizer.Tokenize(input);
        var stack = new Stack<Token>(r.Reverse());

        var parser = new Parser();

        var block = parser.Parse(stack);
        Console.WriteLine(block);

    }
}



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



    enum ScanState
    {
        ElementDefinition,
        ChildrenDefinition
    }


    public static List<Token> ScanTokens(IEnumerable<Token> tokens)
    {
        ScanState state = ScanState.ChildrenDefinition;
        var result = new List<Token>();

        Token textToken = null;

        string text = null;
        foreach (var token in tokens)
        {
            switch (state)
            {
                case ScanState.ElementDefinition:
                    {
                        switch (token.Type)
                        {
                            case TokenType.Bracket:
                                {
                                    result.Add(token);
                                    state = ScanState.ChildrenDefinition;
                                    break;
                                }
                            case TokenType.Space: continue;
                            default:
                                {
                                    result.Add(token);
                                    break;
                                }

                        }
                        break;
                    }
                case ScanState.ChildrenDefinition:
                    {
                        switch (token.Type)
                        {
                            case TokenType.Bracket:
                                {
                                    if (text != null)
                                        result.Add(new Token(text.Trim(), TokenType.Text));
                                    text = null;
                                    result.Add(token);
                                    state = ScanState.ElementDefinition;
                                    break;
                                }
                            case TokenType.Space:
                                {
                                    if (text != null)
                                        text += token.TokenString;
                                    break;
                                }
                            default:
                                {
                                    if (text != null)
                                        text += token.TokenString;
                                    else
                                        text = token.TokenString;
                                    break;
                                }


                        }

                        break;
                    }
                default:
                    break;
            }
        }

        return result;
    }

    public static void Main(string[] args)
    {
        var input = @"
        <block rows = 3 columns = 3>
            Hellosad as d dsa d 
            <column  halign = top> Hello block rows =5 world 1 232 3 23   . - * + </column>
            World  sadas  adas 
        </block>
            ";
        var tokenizer = new Tokenizer();
        var r = tokenizer.Tokenize(input);
        var l = ScanTokens(r);
        l.Reverse();
        var stack = new Stack<Token>(l);
        PrintStack(stack);
    }
}

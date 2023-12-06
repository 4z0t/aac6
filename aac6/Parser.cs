using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMark
{

    public class Block
    {
        public Block(Block parent, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            Children = new List<Block>();
            Parent = parent;
        }

        public Block(Block parent, int columns, int rows, Block[] children)
        {
            Columns = columns;
            Rows = rows;
            Children = new(children);
            Parent = parent;
        }
        public Block Parent { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<Block> Children { get; set; }

        public virtual string Header()
        {
            return $"<block columns={Columns} rows={Rows}>";
        }

        public string ToStringTree(int level)
        {
            string result = new('\t', level);
            result += $"{Header()}\n";
            foreach (var item in Children)
            {
                result += item.ToStringTree(level + 1);
            }
            return result;
        }

        public override string ToString() => ToStringTree(0);

        public void AddChild(Block child)
        {
            Children.Add(child);
        }
    }

    public class View : Block
    {
        public View(Block parent, int columns, int rows) : base(parent, columns, rows)
        {
        }

        public View(Block parent, int columns, int rows, Block[] children) : base(parent, columns, rows, children)
        {
        }

        public View(Block parent, int columns, int rows, string text) : base(parent, columns, rows)
        {
            Text = text;
        }

        public string? Text { get; set; }
        public string? VAlign { get; set; }
        public string? HAlign { get; set; }
        public string? TextColor { get; set; }
        public string? BGColor { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }


        public override string Header()
        {
            return $"<view columns={Columns} rows={Rows}{(VAlign != null ? $" valign={VAlign}" : "")}{(HAlign != null ? $" halign={HAlign}" : "")}{(TextColor == null ? "" : $" textcolor={TextColor}")}>{(Text == null ? "" : $" {Text}")}";
        }
    }

    public class InvalidTokensException : Exception
    {
        public InvalidTokensException(string reason) : base(reason)
        {

        }
        public InvalidTokensException() : base()
        {

        }
    }
    public class Parser
    {
        public Parser()
        {

        }

        public Dictionary<string, string> ProcessAttributes(Stack<Token> tokens)
        {
            Dictionary<string, string> result = new();
            string? key = null;

            return result;
        }

        public Block MakeElement(Block parent, string type, Dictionary<string, string> attributes)
        {
            return null;
        }

        public Block Process(Block parent, Stack<Token> tokens)
        {
            if (tokens.Peek().Type != TokenType.Bracket)
                throw new InvalidTokensException("Expected Bracket during element process");
            if (tokens.Peek().TokenString != "<")
                throw new InvalidTokensException("Expected starting bracket during element process");
            tokens.Pop();
            if (tokens.Peek().Type != TokenType.Element)
                throw new InvalidTokensException("Expected element token type");
            var type = tokens.Pop().TokenString;
            var attributes = ProcessAttributes(tokens);
            var element = MakeElement(type, attributes);
            if (tokens.Peek().Type != TokenType.Bracket || tokens.Peek().TokenString != ">")
                throw new InvalidTokensException("Expected ending bracket here");
            tokens.Pop();





            return element;
        }

        public Block Parse(Stack<Token> tokens)
        {

            return Block;
        }

        private Block Block { get; set; }


    }
}

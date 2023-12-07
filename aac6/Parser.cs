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

        public string Type { get; set; }

        public virtual string Header()
        {
            return $"<block columns={Columns} rows={Rows}>";
        }

        public string ToStringTree(int level)
        {
            string result = new('\t', level);
            result += $"{Header()}\n";
            if (Children != null)
            {
                foreach (var item in Children)
                {
                    result += item.ToStringTree(level + 1);
                }
            }

            return result;
        }

        public override string ToString() => ToStringTree(0);

        public void AddChild(Block child)
        {
            Children.Add(child);
        }

        public virtual void SetAttributes(Dictionary<string, string> attributes)
        {

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
            return $"<{Type} columns={Columns} rows={Rows}{(VAlign != null ? $" valign={VAlign}" : "")}{(HAlign != null ? $" halign={HAlign}" : "")}{(TextColor == null ? "" : $" textcolor={TextColor}")}>{(Text == null ? "" : $" {Text}")}";
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
            bool wasEqual = false;
            while (tokens.Peek().Type != TokenType.Bracket)
            {
                if (key == null)
                {
                    if (tokens.Peek().Type != TokenType.Attribute) throw new InvalidTokensException("Expected attribute");
                    key = tokens.Pop().TokenString;
                }
                else if (!wasEqual)
                {
                    if (tokens.Peek().Type != TokenType.Equal) throw new InvalidTokensException("Expected equal operator");
                    tokens.Pop();
                    wasEqual = true;
                }
                else
                {
                    result.Add(key, tokens.Pop().TokenString);
                    key = null;
                    wasEqual = false;
                }

            }
            if (wasEqual || key != null)
                throw new InvalidTokensException("Premature end of attributes definition");
            return result;
        }

        public Block MakeElement(Block parent, string type, Dictionary<string, string> attributes)
        {
            Console.WriteLine(type);
            //foreach (var (key, value) in attributes)
            //{
            //    Console.WriteLine($"{key}:{value}");
            //}
            if (type == "block")
            {
                var block = new Block(parent, 0, 0);
                block.SetAttributes(attributes);
                block.Type = "block";
                return block;
            }
            else
            {
                var block = new View(parent, 0, 0);
                block.SetAttributes(attributes);
                block.Type = type;
                return block;
            }
        }

        public List<Block> MakeChildren(Block parent, Stack<Token> tokens)
        {
            if (tokens.Peek().Type == TokenType.Text)
            {
                if (parent is not View view)
                    throw new InvalidTokensException("Block can't have a text");
                view.Text = tokens.Pop().TokenString;
                return null;
            }
            List<Block> result = new();
            var type = parent.Type;

            while (tokens.Peek().TokenString != "</")
            {
                result.Add(Process(parent, tokens));
            }

            return result;
        }


        private void CheckBracket(Stack<Token> tokens, string b)
        {
            if (tokens.Peek().Type != TokenType.Bracket)
                throw new InvalidTokensException("Expected bracket during element process");
            if (tokens.Peek().TokenString != b)
                throw new InvalidTokensException("Expected bracket during element process");
        }

        public Block Process(Block parent, Stack<Token> tokens)
        {
            CheckBracket(tokens, "<");
            tokens.Pop();
            if (tokens.Peek().Type != TokenType.Element)
                throw new InvalidTokensException("Expected element token type");

            var type = tokens.Pop().TokenString;
            var attributes = ProcessAttributes(tokens);
            var element = MakeElement(parent, type, attributes);

            if (tokens.Peek().Type != TokenType.Bracket || tokens.Peek().TokenString != ">")
                throw new InvalidTokensException("Expected ending bracket here");
            tokens.Pop();

            element.Children = MakeChildren(element, tokens);

            CheckBracket(tokens, "</");
            tokens.Pop();
            if (tokens.Peek().Type != TokenType.Element)
                throw new InvalidTokensException("Expected element token type");
            if (type != tokens.Pop().TokenString)
                throw new InvalidTokensException("Expected element end to be equal to start");
            CheckBracket(tokens, ">");
            tokens.Pop();

            return element;
        }

        public Block Parse(Stack<Token> tokens)
        {
            Block = Process(null, tokens);
            ValidateTree(Block);
            return Block;
        }

        private void ValidateTree(Block block)
        {

        }

        private Block Block { get; set; }


    }
}

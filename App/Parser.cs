using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMark
{

    public class BaseBlock
    {
        public BaseBlock(BaseBlock parent, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            Children = new List<BaseBlock>();
            Parent = parent;
        }

        public BaseBlock(BaseBlock parent, int columns, int rows, BaseBlock[] children)
        {
            Columns = columns;
            Rows = rows;
            Children = new List<BaseBlock>(children);
            Parent = parent;
        }
        public BaseBlock Parent { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<BaseBlock> Children { get; set; }

        public string Type { get; set; }

        public virtual string Header()
        {
            return $"<block columns={Columns} rows={Rows}>";
        }

        public string ToStringTree(int level)
        {
            string result = new string('\t', level);
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

        public void AddChild(BaseBlock child) => Children.Add(child);


        public static readonly string[] ValidAttributes = new[] { "rows", "columns" };

        public virtual void SetAttributes(Dictionary<string, string> attributes)
        {
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                string attribute = entry.Key;
                string value = entry.Value;
                if (!ValidAttributes.Contains(attribute))
                {
                    throw new Exception($"Invalid attribute {attribute}");
                }
                if (attribute == "rows") Rows = int.Parse(value);
                if (attribute == "columns") Columns = int.Parse(value);
            }
        }
    }

    public class View : BaseBlock
    {
        public View(BaseBlock parent, int columns, int rows) : base(parent, columns, rows)
        {
        }

        public View(BaseBlock parent, int columns, int rows, BaseBlock[] children) : base(parent, columns, rows, children)
        {
        }

        public View(BaseBlock parent, int columns, int rows, string text) : base(parent, columns, rows)
        {
            Text = text;
        }

        public string Text { get; set; } = null;
        public string VAlign { get; set; } = "top";
        public string HAlign { get; set; } = "left";
        public string TextColor { get; set; } = null;
        public string BGColor { get; set; } = null;

        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;


        public static new readonly string[] ValidAttributes = new[] { "rows", "columns", "valign", "halign", "textcolor", "bgcolor", "width", "height" };

        public override void SetAttributes(Dictionary<string, string> attributes)
        {
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                string attribute = entry.Key;
                string value = entry.Value;
                if (!ValidAttributes.Contains(attribute))
                {
                    throw new Exception($"Invalid attribute {attribute}");
                }
                if (attribute == "rows") Rows = int.Parse(value);
                if (attribute == "columns") Columns = int.Parse(value);
                if (attribute == "width") Width = int.Parse(value);
                if (attribute == "height") Height = int.Parse(value);
                if (attribute == "textcolor") TextColor = value;
                if (attribute == "bgcolor") BGColor = value;
                if (attribute == "valign") VAlign = value;
                if (attribute == "halign") HAlign = value;
            }
        }

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
            Dictionary<string, string> result = new Dictionary<string, string>();
            string key = null;
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

        public BaseBlock MakeElement(BaseBlock parent, string type, Dictionary<string, string> attributes)
        {
            //Console.WriteLine(type);
            //foreach (var (key, value) in attributes)
            //{
            //    Console.WriteLine($"{key}:{value}");
            //}
            if (type == "block")
            {
                var block = new BaseBlock(parent, 0, 0)
                {
                    Type = "block"
                };
                block.SetAttributes(attributes);
                return block;
            }
            else
            {
                var block = new View(parent, 0, 0)
                {
                    Type = type
                };
                block.SetAttributes(attributes);
                return block;
            }
        }

        public List<BaseBlock> MakeChildren(BaseBlock parent, Stack<Token> tokens)
        {
            if (tokens.Peek().Type == TokenType.Text)
            {
                if (parent is View view)
                {
                    view.Text = tokens.Pop().TokenString;
                    return null;
                }
                throw new InvalidTokensException("Block can't have a text in it");
            }
            List<BaseBlock> result = new List<BaseBlock>();
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
                throw new InvalidTokensException($"Expected bracket during element process, but got {tokens.Peek().Type}");
            if (tokens.Peek().TokenString != b)
                throw new InvalidTokensException($"Expected bracket '{b}' but  got '{tokens.Peek().TokenString}'");
        }

        public BaseBlock Process(BaseBlock parent, Stack<Token> tokens)
        {
            CheckBracket(tokens, "<");
            tokens.Pop();
            if (tokens.Peek().Type != TokenType.Element)
                throw new InvalidTokensException("Expected element token type");

            var type = tokens.Pop().TokenString;
            var attributes = ProcessAttributes(tokens);
            var element = MakeElement(parent, type, attributes);

            CheckBracket(tokens, ">");
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

        public BaseBlock Parse(Stack<Token> tokens)
        {
            try
            {
                Block = Process(null, tokens);
                ValidateTree(Block);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return Block;
        }

        private void ValidateTree(BaseBlock block)
        {

        }



        private BaseBlock Block { get; set; }


    }
}

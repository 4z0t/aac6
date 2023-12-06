using EMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aac6
{

    public class Block
    {
        public Block(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
        }

        public string? Text { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Block Children { get; set; }
    }

    public class View : Block
    {
        public View(int columns, int rows) : base(columns, rows)
        {
        }

        public string? VAlign { get; set; }
        public string? HAlign { get; set; }
        public string? TextColor { get; set; }
        public string? BGColor { get; set; }
    }

    public class Column : View
    {
        public Column(int columns, int rows) : base(columns, rows)
        {
        }
    }

    public class Row : View
    {
        public Row(int columns, int rows) : base(columns, rows)
        {
        }
    }

    public class Parser
    {
        public Parser()
        {

        }

        public Block Parse(Stack<Token> tokens)
        {

            return Block;
        }

        private Block Block { get; set; }


    }
}

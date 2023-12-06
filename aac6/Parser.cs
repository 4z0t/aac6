﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMark
{

    public class Block
    {
        public Block(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            Children = new List<Block>();
        }

        public Block(int columns, int rows, Block[] children)
        {
            Columns = columns;
            Rows = rows;
            Children = new(children);
        }

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
        public View(int columns, int rows) : base(columns, rows)
        {
        }

        public View(int columns, int rows, Block[] children) : base(columns, rows, children)
        {
        }

        public View(int columns, int rows, string text) : base(columns, rows)
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

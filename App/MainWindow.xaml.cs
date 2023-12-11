using EMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public string EMARKS = @"
        <block rows = 3>
            <row valign=center halign=center height = 20>
                Hellosad as d dsa d
            </row>
            <row columns = 2 halign=center valign=bottom height=100>
                <column valign = bottom width = 100>
                    <block rows=1>
                        <row halign= left>
                            col 1
                        </row>
                    </block>
                </column>
                <column valign=center>
                    col 2
                </column>
            </row>
            <row valign=bottom halign=right textcolor=red bgcolor=black>
                    EWWWWWWWW
               
            </row>
        </block>";

        public Color StringToColor(string str)
        {
            switch (str.ToLower())
            {
                case "black":
                    return Colors.Black;
                case "red":
                    return Colors.Red;
                case "blue":
                    return Colors.Blue;
                case "green":
                    return Colors.Green;
                default: return Colors.White;
            }
        }

        public Grid RenderBlock(BaseBlock block)
        {
            Grid grid = new Grid();
            grid.ShowGridLines = true;

            if (block.Rows != 0)
            {
                for (int i = 0; i < block.Rows; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                }
            }
            else if (block.Columns != 0)
            {
                for (int i = 0; i < block.Columns; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
            //else
            //    throw new Exception("block must have at least one set of rows or columns");
            if (block.Type == "block")
            {
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                grid.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                View view = block as View;
                string valign = view.VAlign;
                string halign = view.HAlign;
                if (view.Type == "row" && view.Height != 0)
                {
                    grid.Height = view.Height;
                }
                else if (view.Type == "column" && view.Width != 0)
                {
                    grid.Width = view.Width;
                }
                switch (valign)
                {
                    case "top":
                        grid.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case "bottom":
                        grid.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    case "center":
                        grid.VerticalAlignment = VerticalAlignment.Center;
                        break;
                    default:
                        throw new Exception("Invalid Valign value");
                }
                switch (halign)
                {
                    case "left":
                        grid.HorizontalAlignment = HorizontalAlignment.Left;
                        break;
                    case "right":
                        grid.HorizontalAlignment = HorizontalAlignment.Right;
                        break;
                    case "center":
                        grid.HorizontalAlignment = HorizontalAlignment.Center;
                        break;
                    default:
                        throw new Exception("Invalid Halign value");
                }
                if (view.Text != null)
                {
                    TextBlock text = new TextBlock();
                    text.Text = view.Text;
                    text.FontSize = 12;
                    text.FontWeight = FontWeights.Bold;

                    if (view.TextColor != null)
                    {
                        text.Foreground = new SolidColorBrush(StringToColor(view.TextColor));
                    }

                    grid.Children.Add(text);
                }
                if (view.BGColor != null)
                {
                    grid.Background = new SolidColorBrush(StringToColor(view.BGColor));
                }
            }

            int index = 0;
            if (block.Children != null)
            {
                foreach (BaseBlock child in block.Children)
                {
                    Grid childGrid = RenderBlock(child);
                    if (block.Rows != 0)
                    {
                        Grid.SetRow(childGrid, index);
                        Grid.SetColumn(childGrid, 0);
                    }
                    else if (block.Columns != 0)
                    {
                        Grid.SetRow(childGrid, 0);
                        Grid.SetColumn(childGrid, index);
                    }
                    grid.Children.Add(childGrid);

                    index++;
                }
            }
            return grid;
        }

        public MainWindow()
        {
            InitializeComponent();

            Tokenizer tokenizer = new Tokenizer();
            Stack<Token> tokens = new Stack<Token>(tokenizer.Tokenize(EMARKS).Reverse());
            try
            {

                Parser parser = new Parser();
                BaseBlock block = parser.Process(null, tokens);
                Console.WriteLine(block);
                Grid grid = RenderBlock(block);
                this.Content = grid;
            }
            catch (Exception ex)
            {

                TextBlock info = new TextBlock();
                info.HorizontalAlignment = HorizontalAlignment.Center;
                info.VerticalAlignment = VerticalAlignment.Center;
                info.FontSize = 20;
                info.FontWeight = FontWeights.Bold;
                info.Text = ex.ToString();
                this.Content = info;
            }

        }
    }
}

using EMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public string EMARKS = @"
        <block rows = 3>
            <row valign=center halign=center height = 50>
                Hellosad as d dsa d
            </row>
            <row columns = 2 halign=center valign=bottom height=100>
                <column valign = bottom width = 200>
                    <block rows=1>
                        <row halign= center valign=bottom bgcolor=black textcolor = green>
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

        public struct LayoutContext
        {

            public LayoutContext(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
            {
                HorizontalAlignment = horizontalAlignment; VerticalAlignment = verticalAlignment;
            }

            public LayoutContext(LayoutContext layout) : this(layout.HorizontalAlignment, layout.VerticalAlignment)
            {
            }

            public HorizontalAlignment HorizontalAlignment { get; set; }
            public VerticalAlignment VerticalAlignment { get; set; }

            public void HAlignFromString(string halign)
            {
                switch (halign)
                {
                    case "left":
                        HorizontalAlignment = HorizontalAlignment.Left;
                        break;
                    case "right":
                        HorizontalAlignment = HorizontalAlignment.Right;
                        break;
                    case "center":
                        HorizontalAlignment = HorizontalAlignment.Center;
                        break;
                    default:
                        throw new Exception("Invalid Halign value");
                }
            }

            public void VAlignFromString(string valign)
            {
                switch (valign)
                {
                    case "top":
                        VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case "bottom":
                        VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    case "center":
                        VerticalAlignment = VerticalAlignment.Center;
                        break;
                    default:
                        throw new Exception("Invalid Valign value");
                }
            }
        }


        public Grid RenderBlock(BaseBlock block, LayoutContext context)
        {
            Grid grid = new Grid();
            //grid.ShowGridLines = true;

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
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            if (block.Type == "block")
            {
            }
            else
            {
                View view = block as View;
                string valign = view.VAlign;
                string halign = view.HAlign;
                context.HAlignFromString(halign);
                context.VAlignFromString(valign);

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
                    text.HorizontalAlignment = context.HorizontalAlignment;
                    text.VerticalAlignment = context.VerticalAlignment;
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
                    Grid childGrid = RenderBlock(child, new LayoutContext(context));
                    if (block.Rows != 0)
                    {
                        Grid.SetRow(childGrid, index);
                        Grid.SetColumn(childGrid, 0);
                        if (child is View view)
                        {
                            if (view.Height != 0)
                                grid.RowDefinitions[index].Height = new GridLength(view.Height);
                        }
                    }
                    else if (block.Columns != 0)
                    {
                        Grid.SetRow(childGrid, 0);
                        Grid.SetColumn(childGrid, index);
                        if (child is View view)
                        {
                            if (view.Width != 0)
                                grid.ColumnDefinitions[index].Width = new GridLength(view.Width);
                        }
                    }
                    grid.Children.Add(childGrid);

                    index++;
                }
            }
            return grid;
        }

        public void Update(string text)
        {
            Tokenizer tokenizer = new Tokenizer();
            Stack<Token> tokens = new Stack<Token>(tokenizer.Tokenize(text).Reverse());
            EMark.Children.Clear();
            try
            {
                Parser parser = new Parser();
                BaseBlock block = parser.Process(null, tokens);
                Grid grid = RenderBlock(block, new LayoutContext(HorizontalAlignment.Left, VerticalAlignment.Top));
                grid.HorizontalAlignment = HorizontalAlignment.Left;
                grid.VerticalAlignment = VerticalAlignment.Top;
                grid.Width = 800;
                grid.Height = 240;
                EMark.Children.Add(grid);
            }
            catch (Exception ex)
            {
                TextBlock info = new TextBlock();
                info.HorizontalAlignment = HorizontalAlignment.Center;
                info.VerticalAlignment = VerticalAlignment.Top;
                info.FontSize = 16;
                info.FontWeight = FontWeights.Bold;
                info.Text = ex.ToString();
                EMark.Children.Add(info);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
           
            File.Open("file.txt", FileMode.OpenOrCreate).Close();
            Program.Text = File.ReadAllText("file.txt");
            Closing += (s, e) => File.WriteAllText("file.txt", Program.Text);
            Program.TextChanged += Program_TextChanged;
        }



        private void Program_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update(Program.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update(Program.Text);
        }
    }
}

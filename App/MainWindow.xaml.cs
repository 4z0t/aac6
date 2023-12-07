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
        public MainWindow()
        {
            InitializeComponent();
            Grid grid = new Grid();
            grid.Width = 500;
            grid.Height = 300;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.ShowGridLines = true;

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);
            grid.RowDefinitions.Add(rowDef3);
            grid.RowDefinitions.Add(rowDef4);


            // Add the first text cell to the Grid
            TextBlock txt1 = new TextBlock();
            txt1.Text = "2005 Products Shipped";
            txt1.FontSize = 20;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetColumnSpan(txt1, 3);
            Grid.SetRow(txt1, 0);

            // Add the second text cell to the Grid
            TextBlock txt2 = new TextBlock();
            txt2.Text = "Quarter 1";
            txt2.FontSize = 12;
            txt2.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt2, 1);
            Grid.SetColumn(txt2, 0);

            // Add the third text cell to the Grid
            TextBlock txt3 = new TextBlock();
            txt3.Text = "Quarter 2";
            txt3.FontSize = 12;
            txt3.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt3, 1);
            Grid.SetColumn(txt3, 1);

            // Add the fourth text cell to the Grid
            TextBlock txt4 = new TextBlock();
            txt4.Text = "Quarter 3";
            txt4.FontSize = 12;
            txt4.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt4, 1);
            Grid.SetColumn(txt4, 2);

            // Add the sixth text cell to the Grid
            TextBlock txt5 = new TextBlock();
            Double db1 = new Double();
            db1 = 50000;
            txt5.Text = db1.ToString();
            Grid.SetRow(txt5, 2);
            Grid.SetColumn(txt5, 0);

            // Add the seventh text cell to the Grid
            TextBlock txt6 = new TextBlock();
            Double db2 = new Double();
            db2 = 100000;
            txt6.Text = db2.ToString();
            Grid.SetRow(txt6, 2);
            Grid.SetColumn(txt6, 1);

            // Add the final text cell to the Grid
            TextBlock txt7 = new TextBlock();
            Double db3 = new Double();
            db3 = 150000;
            txt7.Text = db3.ToString();
            Grid.SetRow(txt7, 2);
            Grid.SetColumn(txt7, 2);

            // Total all Data and Span Three Columns
            TextBlock txt8 = new TextBlock();
            txt8.FontSize = 16;
            txt8.FontWeight = FontWeights.Bold;
            txt8.Text = "Total Units: " + (db1 + db2 + db3).ToString();
            Grid.SetRow(txt8, 3);
            Grid.SetColumnSpan(txt8, 3);

            // Add the TextBlock elements to the Grid Children collection
            grid.Children.Add(txt1);
            grid.Children.Add(txt2);
            grid.Children.Add(txt3);
            grid.Children.Add(txt4);
            grid.Children.Add(txt5);
            grid.Children.Add(txt6);
            grid.Children.Add(txt7);
            grid.Children.Add(txt8);

            this.Content = grid;
        }
    }
}

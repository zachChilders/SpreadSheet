using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TinySpreadsheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<String, Column> Columns = new Dictionary<String,Column>();

        public Cell GetCell(String coordinate)
        {
            StringBuilder alpha = new StringBuilder();
            int alphaIndex = 0;
            while (Char.IsLetter(coordinate[alphaIndex]))
            {
              alphaIndex++;
            }
            return Columns[coordinate.Slice(0,alphaIndex)][Int32.Parse(coordinate.Slice(alphaIndex, coordinate.Length))];
        }

        public MainWindow()
        {
            InitializeComponent();
            
            for(int j = 0; j < 26; j++)
            {
                String name = ((char)('A' + j)).ToString();
                Column c = new Column() { Name = name };
                RowStack.Children.Add(c);
                Columns.Add(name, c);  //A needs to be determined on the fly
            }

        }
    }
}

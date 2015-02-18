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

        public Dictionary<String, Column> Columns = new Dictionary<String,Column>();

        public MainWindow()
        {
            InitializeComponent();
            
            for(int j = 0; j < 26; j++)
            {
                Column c = new Column();
                RowStack.Children.Add(c);
                Columns.Add(('A'+ j).ToString(), c);  //A needs to be determined on the fly
            }

        }
    }
}

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
    /// Interaction logic for Column.xaml
    /// </summary>
    public partial class Column : UserControl
    {
        public List<Cell> cells = new List<Cell>();

        public Column()
        {
            InitializeComponent();
            for (int i = 0; i < 30; i++)
            {
                Cell c = new Cell();

                CellColumn.Items.Add(c);
                cells.Add(c);
            }
        }
    }
}

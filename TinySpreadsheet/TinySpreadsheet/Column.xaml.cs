using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    /// A simple UI class that holds a list of Cells.
    /// </summary>
    public partial class Column : UserControl
    {
        private readonly List<Cell> cells = new List<Cell>();

        public Column()
        {
            InitializeComponent();
        }

        /// <summary>
        /// A simple constructor initializing 30 cells. Temporary.
        /// </summary>
        public Column(String name)
        {
            InitializeComponent();

            Name = name;

            for (int i = 0; i < MainWindow.RowCount; i++)
            {
                Cell c = new Cell() { Name = name + i.ToString() }; //Make sure we name in addrow
                ColumnName.Content = name;

                Binding heightBind = new Binding();
                heightBind.Path = new PropertyPath(Grid.ActualHeightProperty);
                heightBind.Source = MainWindow.Instance.RowColumn[i];
                c.SetBinding(Cell.HeightProperty, heightBind);

                CellColumn.Items.Add(c);
                cells.Add(c);

                
            }
        }

        /// <summary>
        /// Indexer for columns
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Cell this[int index]
        {
            get
            {
                if (index < 0)
                    return new Cell() { CellDisplay = "NaN" };
                return cells[index];
            }
            set
            {
                cells[index] = value;
            }
        }

        /// <summary>
        /// Adds a cell to the column
        /// </summary>
        public void AddRow()
        {
            Cell c = new Cell() { Name = Name + cells.Count.ToString() };

            Binding heightBind = new Binding();
            heightBind.Path = new PropertyPath(Grid.ActualHeightProperty);
            heightBind.Source = MainWindow.Instance.RowColumn[MainWindow.RowCount];
            c.SetBinding(Cell.HeightProperty, heightBind);

            CellColumn.Items.Add(c);
            cells.Add(c);
        }

    }
}

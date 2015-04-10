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
    [Serializable]
    public partial class Column : UserControl, ISerializable
    {
        private readonly List<Cell> Cells = new List<Cell>();

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
                CellColumn.Items.Add(c);
                Cells.Add(c);
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
                return Cells[index];
            }
            set
            {
                Cells[index] = value;
            }
        }

        /// <summary>
        /// Adds a cell to the column
        /// </summary>
        public void AddRow()
        {
            Cell c = new Cell();
            CellColumn.Items.Add(c);
            Cells.Add(c);
        }

        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("cells", Cells, typeof(List<Cell>));
        }

        /// <summary>
        /// Column Deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Column(SerializationInfo info, StreamingContext context)
        {
            Cells = (List<Cell>) info.GetValue("cells", typeof(List<Cell>));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace TinySpreadsheet
{
    public partial class MainWindow
    {
        public static Dictionary<String, Column> SpreadSheet = new Dictionary<String, Column>();
        public static int RowCount { get; private set; }

        internal static List<int> rowMax = new List<int>();
        internal static List<String> colMax = new List<String>(); 

        /// <summary>
        /// Creates a new column.
        /// </summary>
        private void CreateNewColumn()
        {
            String name = GenerateName();
            Column c = new Column(name);
            RowStack.Children.Add(c);
            SpreadSheet.Add(c.Name, c);
        }

        /// <summary>
        /// Generates a new row in every column
        /// </summary>
        private static void CreateNewRow()
        {
            foreach (KeyValuePair<string, Column> c in SpreadSheet)
            {
                c.Value.AddRow();
            }
            RowCount++;
        }

        /// <summary>
        /// Generates a column name by converting the cell number to base 26.
        /// </summary>
        /// <returns></returns>
        private static String GenerateName()
        {
            int index = SpreadSheet.Count + 1;

            const int columnBase = 26;
            const int digitMax = 7; // ceil(log26(Int32.Max))
            const string digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            if (index <= columnBase)
                return digits[index - 1].ToString();

            StringBuilder sb = new StringBuilder().Append(' ', digitMax);
            int current = index;
            int offset = digitMax;
            while (current > 0)
            {
                sb[--offset] = digits[--current % columnBase];
                current /= columnBase;
            }
            return sb.ToString(offset, digitMax - offset);
        }

        /// <summary>
        /// Gets the greatest relevant column
        /// </summary>
        /// <returns></returns>
        public String GetMaxColumn()
        {
            return colMax[colMax.Count - 1];
        }

        /// <summary>
        /// Gets the greatest relevant row
        /// </summary>
        /// <returns></returns>
        public String GetMaxRow()
        {
            return colMax[rowMax.Count - 1];
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            StateManager.Save();
        }

        private void Undo_OnClick(object sender, RoutedEventArgs e)
        {
            StateManager.Load();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace TinySpreadsheet
{
    public partial class MainWindow: RibbonWindow, ISerializable
    {
        public static Dictionary<String, Column> Columns = new Dictionary<String, Column>();

        /// <summary>
        /// Creates 26 new columns.
        /// </summary>
        private void CreateVerticalPage()
        {
            for (int j = 0; j < 26; j++)
            {
                String name = GenerateName();//Name needs to be determined on the fly.
                Column c = new Column() { Name = name };
                RowStack.Children.Add(c);
                Columns.Add(name, c);

                Console.WriteLine(name);
            }
        }

        /// <summary>
        /// Generates a new row in every column
        /// </summary>
        private static void CreateNewRow()
        {
            foreach (var c in Columns)
            {
                c.Value.AddRow();
            }
        }

        /// <summary>
        /// Let's never speak of this again.
        /// </summary>
        /// <returns></returns>
        private static String GenerateName()
        {
            int index = Columns.Count + 1;

            const int ColumnBase = 26;
            const int DigitMax = 7; // ceil(log26(Int32.Max))
            const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            if (index <= ColumnBase)
                return Digits[index - 1].ToString();

            var sb = new StringBuilder().Append(' ', DigitMax);
            var current = index;
            var offset = DigitMax;
            while (current > 0)
            {
                sb[--offset] = Digits[--current % ColumnBase];
                current /= ColumnBase;
            }
            return sb.ToString(offset, DigitMax - offset);
        }




    }
}

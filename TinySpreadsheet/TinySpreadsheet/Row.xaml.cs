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

namespace TinySpreadsheet
{
    /// <summary>
    /// Interaction logic for Row.xaml
    /// </summary>
    public partial class Row : UserControl
    {
        public Row()
        {
            InitializeComponent();
            for (int i = 0; i < MainWindow.RowCount; i++)
            {
                RowStack.Children.Add(CreateLabel(i));
            }
        }

        public void AddRow()
        {
            RowStack.Children.Add(CreateLabel(MainWindow.RowCount));
        }

        private Label CreateLabel(int row)
        {
            return new Label()
            {
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(0.25),
                Content = row.ToString(),
                Height = 35.5,
                Margin = new Thickness(0, 0, 0, 0)
            };
        }
    }
}

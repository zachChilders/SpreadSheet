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
                RowStack.Children.Add(CreateRowCell(i));
            }
        }

        public void AddRow()
        {
            RowStack.Children.Add(CreateRowCell(MainWindow.RowCount));
        }

        private Grid CreateRowCell(int row)
        {
            Grid cell = new Grid() { VerticalAlignment = System.Windows.VerticalAlignment.Stretch, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };
            cell.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35.5) });
            cell.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            cell.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            cell.Children.Add(CreateLabel(row));

            GridSplitter gs = new GridSplitter()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                Background = new SolidColorBrush(Colors.Transparent),
                Margin = new Thickness(1),
                ShowsPreview = true,
                Height = 5
            };

            cell.Children.Add(gs);
            return cell;
        }

        private Label CreateLabel(int row)
        {
            return new Label()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(0.25),
                Content = row.ToString(),
                Margin = new Thickness(0, 0, 0, 0)
            };
        }
    }
}

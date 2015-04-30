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

namespace TinySpreadsheet.Spreadsheet.Components
{
    /// <summary>
    /// Interaction logic for Row.xaml
    /// </summary>
    public partial class Row : UserControl
    {
        /// <summary>
        /// Creates the row labels based on the number of rows specified by the spreadsheet.
        /// </summary>
        public Row()
        {
            InitializeComponent();
            for (int i = 0; i < SpreadsheetWindow.RowCount; i++)
            {
                RowStack.Children.Add(CreateRowCell(i));
            }
        }

        /// <summary>
        /// Adds in a new label to the end.
        /// </summary>
        public void AddRow()
        {
            RowStack.Children.Add(CreateRowCell(SpreadsheetWindow.RowCount));
        }

        /// <summary>
        /// Creation logic for adding in a new row label.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>A Grid containing all of the visual components for the label.</returns>
        private Grid CreateRowCell(int row)
        {
            Grid cell = new Grid() { VerticalAlignment = System.Windows.VerticalAlignment.Stretch, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };
            cell.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35.5) });
            cell.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            cell.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            cell.Children.Add(CreateLabel(row+1));

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

        /// <summary>
        /// Creates the actual label portion of the view.
        /// </summary>
        /// <param name="row">The row number as to be shown to the user.</param>
        /// <returns>A label with the given row number.</returns>
        private Label CreateLabel(int row)
        {
            return new Label()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                BorderBrush = new SolidColorBrush(Colors.Wheat),
                BorderThickness = new Thickness(0.25),
                Content = row.ToString(),
                Margin = new Thickness(0, 0, 0, 0),
                Background = new SolidColorBrush(Colors.Wheat)
            };
        }

        /// <summary>
        /// Gets a row Grid by the given index.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>A Grid representing the row.</returns>
        public Grid this[int row]
        {
            get
            {
                return RowStack.Children[row+1] as Grid;
            }
        }
    }
}

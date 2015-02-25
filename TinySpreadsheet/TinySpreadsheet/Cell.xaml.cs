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
using TinySpreadsheet.Dependencies;

namespace TinySpreadsheet
{
    /// <summary>
    /// Interaction logic for Cell.xaml
    /// </summary>
    public partial class Cell : UserControl
    {
        ListBox listParent;

        public DependencyMap Dependencies { get; private set; }

        public String CellFormula{
            get;
            set;
        }
        public String cellDisplay
        {
            get;
            set;
        }


        public Cell()
        {
            //GUI
            InitializeComponent();
            BorderBrush = new SolidColorBrush(Colors.Black);
            BorderThickness = new Thickness(0.25);
            CellText.MouseDoubleClick += Cell_MouseDoubleClick;
            CellText.LostFocus += CellText_LostFocus;
            CellText.GotFocus += CellText_GotFocus;
            CellText.KeyDown += CellText_KeyDown;

            //!GUI
            CellFormula = "";

        }

        FrameworkElement GetParent(Type t)
        {
            FrameworkElement parent = this;

            while ((parent = parent.Parent as FrameworkElement) != null && parent.GetType() != t) ;

            return parent;
        }

        //Custom event
        /// <summary>
        /// Triggered when a change to the value of this Cell occurs.
        /// </summary>
        public event Action<Cell> Changed;
        protected virtual void IChanged()
        {
            if (Changed != null)
                Changed(this);
        }


        //Event subscriptions
        void DependencyChanged(Cell sender)
        {
            throw new NotImplementedException("DependencyChanged not implemented.");
            IChanged();
        }

        void CellText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (listParent == null)
                listParent = GetParent(typeof(ListBox)) as ListBox;
            TextBox t = sender as TextBox;

            listParent.SelectedItems.Add(this);
            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == 0 && (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == 0)
                HighlightCleanup();
        }

        void CellText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;
            t.IsReadOnly = true;

            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == 0 && (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == 0)
                HighlightCleanup();

            this.CellFormula = t.Text;
            cellDisplay = Formula.solve(this).ToString();
            t.Text = cellDisplay;
        }

        void Cell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox t = sender as TextBox;
            t.IsReadOnly = false;
            Keyboard.Focus(t);
            t.Select(0, 0);

            listParent.SelectedItems.Remove(this);
            HighlightCleanup();
        }

        void HighlightCleanup()
        {
            List<Cell> cells = new List<Cell>();
            foreach (Cell cell in listParent.SelectedItems)
            {
                if (!cell.CellText.IsFocused)
                    cells.Add(cell);
            }

            foreach (Cell cell in cells)
            {
                listParent.SelectedItems.Remove(cell);
            }
        }

        void CellText_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (e.Key == Key.Enter)
            {
                if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) != 0 || (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) != 0)
                {
                    t.Text += Environment.NewLine;
                }
                else
                {
                    this.CellFormula = t.Text;
                    cellDisplay = Formula.solve(this).ToString();
                    t.Text = cellDisplay;
                }
            }

        }

    }
}

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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using TinySpreadsheet.Tokenize;
using System.Windows.Threading;


namespace TinySpreadsheet
{
    /// <summary>
    /// A cell setup for UI with Spreadsheet interaction logic built in.
    /// </summary>
    [Serializable]
    public partial class Cell : UserControl, ISerializable
    {
        ListBox listParent;
        private static Cell lastSelected;
        private static Cell dragged;

        /// <summary>
        /// Gets the DependencyMap held by this Cell.
        /// </summary>
        public DependencyMap Dependencies { get; private set; }

        /// <summary>
        /// Gets or sets this Cell's formula used for evaluation. Setting triggers a Changed event.
        /// </summary>
        public String CellFormula { get; set; }

        public String OriginalFormula;
        /// <summary>
        /// Gets or sets the display text for this Cell. This is typically the evaluated CellFormula.
        /// </summary>
        public String CellDisplay { get; set; }

        /// <summary>
        /// Creates a new Cell
        /// </summary>
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
            Focusable = true;

            //!GUI
            CellFormula = "";
            CellDisplay = "";

            Dependencies = new DependencyMap(this);
        }

        /// <summary>
        /// Attempts to get the most immediate parent of this Cell of a given type.
        /// </summary>
        /// <param name="t">The type of the parent being searched for.</param>
        /// <returns>The parent with the given type. If no parent of the given type exists, null is returned.</returns>
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
            //StateManager.SaveState(); //Save our current state
            EvaluateMaxBounds(); //Check for maxes
        }

        //Event subscriptions
        void DependencyChanged(Cell sender)
        {
            CellDisplay = Formula.Solve(this).ToString();
            CellText.Text = CellDisplay;

            IChanged();

        }

        public void Select()
        {
            if (listParent == null)
                listParent = GetParent(typeof(ListBox)) as ListBox;

            listParent.SelectedItems.Add(this);
        }

        public void Deselect()
        {
            if (listParent == null)
                listParent = GetParent(typeof(ListBox)) as ListBox;

            listParent.SelectedItems.Remove(this);
        }

        /// <summary>
        /// Called when a cell is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CellText_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;

            if (((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) != 0 || (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) != 0) && lastSelected != null)
            {
                Queue<String> cells = Function.ExpandCellRange(lastSelected.Name + ":" + Name);
                if (cells.Count == 0)
                    cells = Function.ExpandCellRange(Name + ":" + lastSelected.Name);

                foreach (String s in cells)
                {
                    Cell c = Tokenizer.ExtractCell(s);

                    c.Select();
                }
            }
            else
            {
                Select();

                if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == 0 && (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == 0)
                    HighlightCleanup();
            }
        }

        /// <summary>
        /// Called when a cell is no longer selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CellText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;

            if ((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) != 0 || (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) != 0)
                lastSelected = this;
            else
                lastSelected = null;

            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == 0 && (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == 0 &&
                (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == 0 && (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == 0)
                HighlightCleanup();

            if (!t.IsReadOnly)
            {
                //Save cell state when we lose focus.
                if (Dependencies != null)
                    Dependencies.Unsubscribe();
                OriginalFormula = t.Text;
                CellFormula = t.Text;
                if ((CellFormula != "") && (CellFormula[0] == '='))
                {
                    CellDisplay = Formula.Solve(this).ToString();

                    if (!String.IsNullOrEmpty(CellDisplay) && CellDisplay != "NaN")
                    {
                        Dependencies = Tokenizer.GetDependencies(this);
                        Dependencies.SubscribeCallback = DependencyChanged;
                    }
                    else
                    {
                        CellDisplay = CellFormula;
                    }

                }
                else
                {
                    CellDisplay = t.Text;
                }
                t.Text = CellDisplay;

                IChanged();
            }
            t.IsReadOnly = true;

        }

        /// <summary>
        /// MouseListener.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Cell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox t = sender as TextBox;
            t.IsReadOnly = false;
            Keyboard.Focus(t);

            t.Text = CellFormula == CellDisplay ? CellFormula : "=" + CellFormula;

            t.Select(t.Text.Length, 0);

            listParent.SelectedItems.Remove(this);
            HighlightCleanup();
        }

        /// <summary>
        /// Clears all highlighted cells except for any that are focused.
        /// </summary>
        void HighlightCleanup()
        {
            if (listParent == null)
                listParent = GetParent(typeof(ListBox)) as ListBox;

            List<Cell> cells = listParent.SelectedItems.Cast<Cell>().Where(cell => !cell.CellText.IsFocused).ToList();

            foreach (Cell cell in cells)
            {
                listParent.SelectedItems.Remove(cell);
            }

            foreach (KeyValuePair<String, Column> kv in MainWindow.SpreadSheet)
            {
                Column col = kv.Value;
                if (listParent != col.CellColumn)
                    col.CellColumn.SelectedItems.Clear();
            }
        }

        /// <summary>
        /// Key Listener for cells.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CellText_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (e.Key != Key.Enter) return;

            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) != 0 || (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) != 0)
            {
                if (t != null) t.Text += Environment.NewLine;
            }
            else if (!t.IsReadOnly)
            {
                if (Dependencies != null)
                    Dependencies.Unsubscribe();

                OriginalFormula = t.Text;
                CellFormula = t.Text;
                if ((CellFormula != "") && (CellFormula[0] == '='))
                {
                    CellDisplay = Formula.Solve(this).ToString();
                    if (!String.IsNullOrEmpty(CellDisplay) && CellDisplay != "NaN")
                    {
                        Dependencies = Tokenizer.GetDependencies(this);
                        Dependencies.SubscribeCallback = DependencyChanged;
                    }
                    else
                    {
                        CellDisplay = CellFormula;
                    }
                }
                else
                {
                    CellDisplay = t.Text;
                }
                t.Text = CellDisplay;

                IChanged();
                t.IsReadOnly = true;

                //Resets the carot
                t.IsEnabled = false;
                t.IsEnabled = true;

                Select();
            }
        }

        /// <summary>
        /// Checks if a cell is either the max row or max column,
        /// and then updates spreadsheet if it is.
        /// </summary>
        private void EvaluateMaxBounds()
        {
            int i = 0;
            char c = Name[i];
            while (char.IsLetter(c))
            {
                c = Name[++i];
            }

            String column = Name.Slice(0, i);
            int row = Int32.Parse(Name.Slice(i, Name.Length));

            if (CellFormula != "") //If we have something in the cell, we found a new max.
            {
                MainWindow.colMax.SortedInsert(column);
            }
            else //If we don't have something in the cell, we deleted the old max.
            {
                if (MainWindow.colMax.Count > 0)
                    MainWindow.colMax.RemoveAt(MainWindow.colMax.Count - 1);
            }

            if (CellFormula != "") //If we have something in the cell, we found a new max.
            {
                MainWindow.rowMax.SortedInsert(row);
            }
            else //If we don't have something in the cell, we deleted the old max.
            {
                if (MainWindow.colMax.Count > 0)
                    MainWindow.rowMax.RemoveAt(MainWindow.rowMax.Count - 1);
            }

        }

        private void Cell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CellGrid.RowDefinitions[0].Height = new GridLength(e.NewSize.Height);
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            StringBuilder row = new StringBuilder();

            //Go through each character and append to the appropriate StringBuilder
            foreach (char c in Name)
            {
                if (!Char.IsLetter(c))
                    row.Append(c);

            }

            //Get the index of the cell in the column.
            int index;
            Int32.TryParse(row.ToString(), out index);
            MainWindow.Instance.RowColumn[index].RowDefinitions[0].Height = new GridLength(CellGrid.RowDefinitions[0].Height.Value + 0.5);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeChanged += Cell_SizeChanged;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("formula", CellFormula);
            info.AddValue("display", CellDisplay);
            info.AddValue("ogFormula", OriginalFormula);
        }

        void DoDragDrop(object parameter)
        {
            DragDrop.DoDragDrop(this, parameter, DragDropEffects.All);
        }

        private void Cell_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && dragged == this)
            {
                // Inititate the drag-and-drop operation.

                Application.Current.Dispatcher.BeginInvoke(
                                DispatcherPriority.Background,
                                new System.Threading.ParameterizedThreadStart(DoDragDrop),
                                Name);
                e.Handled = true;
            }
        }

        private void Cell_PreviewDragOver(object sender, DragEventArgs e)
        {
            HighlightCleanup();
            Mouse.SetCursor(Cursor);

            String name = e.Data.GetData(typeof(String)).ToString();
            Queue<String> cells = Function.ExpandCellRange(name + ":" + Name);
            if (cells.Count == 0)
                cells = Function.ExpandCellRange(Name + ":" + name);

            foreach (String s in cells)
            {
                Cell c = Tokenizer.ExtractCell(s);

                c.Select();
            }
        }

        private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == 0 && (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == 0)
                dragged = this;
            else
                dragged = null;
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            Mouse.SetCursor(Cursors.Cross);

            e.Handled = true;
        }
    }
}

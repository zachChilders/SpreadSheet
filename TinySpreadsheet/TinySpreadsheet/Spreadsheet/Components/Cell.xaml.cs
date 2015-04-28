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
using TinySpreadsheet.Majik;


namespace TinySpreadsheet.Spreadsheet.Components
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

            foreach (KeyValuePair<String, Column> kv in SpreadsheetWindow.SpreadSheet)
            {
                Column col = kv.Value;
                if (listParent != col.CellColumn)
                    col.CellColumn.SelectedItems.Clear();
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
                SpreadsheetWindow.colMax.SortedInsert(column);
            }
            else //If we don't have something in the cell, we deleted the old max.
            {
                if (SpreadsheetWindow.colMax.Count > 0)
                    SpreadsheetWindow.colMax.RemoveAt(SpreadsheetWindow.colMax.Count - 1);
            }

            if (CellFormula != "") //If we have something in the cell, we found a new max.
            {
                SpreadsheetWindow.rowMax.SortedInsert(row);
            }
            else //If we don't have something in the cell, we deleted the old max.
            {
                if (SpreadsheetWindow.colMax.Count > 0)
                    SpreadsheetWindow.rowMax.RemoveAt(SpreadsheetWindow.rowMax.Count - 1);
            }

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("formula", CellFormula);
            info.AddValue("display", CellDisplay);
            info.AddValue("ogFormula", OriginalFormula);
        }
    }
}

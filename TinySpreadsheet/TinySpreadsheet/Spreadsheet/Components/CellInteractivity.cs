using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TinySpreadsheet.Majik;
using TinySpreadsheet.Tokenize;

namespace TinySpreadsheet.Spreadsheet.Components
{
    public partial class Cell
    {
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
            if (t.IsReadOnly)
            {
                t.IsReadOnly = false;
                Keyboard.Focus(t);

                t.Text = CellFormula == CellDisplay ? CellFormula : "=" + CellFormula;

                t.Select(t.Text.Length, 0);

                listParent.SelectedItems.Remove(this);
                HighlightCleanup();
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
            SpreadsheetWindow.Instance.RowColumn[index].RowDefinitions[0].Height = new GridLength(CellGrid.RowDefinitions[0].Height.Value + 0.5);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeChanged += Cell_SizeChanged;
        }

        /**********************
         ***Not implemented ***
         **********************/
        void DoDragDrop(object parameter)
        {
            DragDrop.DoDragDrop(this, parameter, DragDropEffects.All);
        }

        private void Cell_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && dragged == this && false)
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

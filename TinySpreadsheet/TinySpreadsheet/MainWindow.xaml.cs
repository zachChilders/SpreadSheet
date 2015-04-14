using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TinySpreadsheet.Tokenize;

namespace TinySpreadsheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public static MainWindow Instance;
       
        public MainWindow()
        {
            Instance = this;
            RowCount = 60;
            const int initialColumnCount = 26;

            InitializeComponent();

            for (int i = 0; i < initialColumnCount; i++ )
                CreateNewColumn();
            
        }

        /// <summary>
        /// This locks up UI.  Fix it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            const double tolerance = 0.0000000001;
            ScrollViewer sv = sender as ScrollViewer;
            if (sv != null && ((sv.ScrollableWidth - sv.HorizontalOffset) < 1 && Math.Abs(e.HorizontalChange) > tolerance))
            {
                CreateNewColumn();
                sv.ScrollToHorizontalOffset(sv.HorizontalOffset + 75);
            }

            if (sv != null && ((sv.ScrollableHeight - sv.VerticalOffset) < 1 && Math.Abs(e.VerticalChange) > tolerance))
            {
                CreateNewRow();
                sv.ScrollToVerticalOffset(sv.VerticalOffset + 30);
            }
            //Console.WriteLine()
            e.Handled = true;
        }

        /// <summary>
        /// Ribbon is loaded.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SheetScroll.ScrollChanged += ScrollViewer_OnScrollChanged;
        }

        /// <summary>
        /// Allows our main ScrollViewer to handle the mouse wheel.
        /// </summary>
        /// <param name="sender">The ScrollViewer</param>
        /// <param name="e">The wheel event information.</param>
        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}

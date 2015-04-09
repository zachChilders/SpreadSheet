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
    public partial class MainWindow : RibbonWindow, ISerializable
    {
       
        public MainWindow()
        {
            InitializeComponent();
            CreateVerticalPage();
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MainWindow(SerializationInfo info, StreamingContext context)
        {
            Columns = (Dictionary<String, Column>) info.GetValue("columns", typeof(Dictionary<String, Column>));
        }

        /// <summary>
        /// This locks up UI.  Fix it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            if ((sv.ScrollableWidth - sv.HorizontalOffset) < 1 && e.HorizontalChange != 0)
            {
                CreateVerticalPage();
            }

            if ((sv.ScrollableHeight - sv.VerticalOffset) < 1 && e.VerticalChange != 0)
            {
                CreateNewRow();
            }
            //Console.WriteLine()
            e.Handled = true;
        }

        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("columns", Columns, typeof(Dictionary<String,Column>));
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SheetScroll.ScrollChanged += ScrollViewer_OnScrollChanged;
        }
    }
}

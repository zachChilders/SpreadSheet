using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

namespace TinySpreadsheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISerializable
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
            if ((e.HorizontalOffset/e.ExtentWidth) > 0.75)
            {
                CreateVerticalPage();
            }

            if ((e.VerticalOffset/e.ExtentHeight) > 0.66)
            {
                CreateNewRow();
            }
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
    }
}

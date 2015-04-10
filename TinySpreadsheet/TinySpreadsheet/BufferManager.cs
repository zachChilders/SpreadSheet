using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TinySpreadsheet
{
    public class BufferManager
    {
        public bool Work { get; set; }
        public Thread OwningThread { get; private set; }

        public BufferManager()
        {

        }

        private void work()
        {
            Work = true;
            while (Work)
            {
                manageColumns();
            }
        }

        public void Start()
        {
            OwningThread = new Thread(new ThreadStart(work));
            OwningThread.SetApartmentState(ApartmentState.STA);
            OwningThread.Start();
        }

        private void manageColumns()
        {
            if (MainWindow.columnBuffer.Count < 10)
            {
                while (MainWindow.columnBuffer.Count < 10)
                {
                    Column c = null;
                    MainWindow.Instance.Dispatcher.Invoke(new Action(() =>
                    {
                        String name = MainWindow.GenerateName();
                        c = new Column(name) { Visibility = System.Windows.Visibility.Collapsed };
                        MainWindow.Instance.RowStack.Children.Add(c);
                        MainWindow.columnBuffer.AddLast(c);
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);

                }
            }
        }
    }
}

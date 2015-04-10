using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TinySpreadsheet
{
    public class Momento
    {
        private readonly MemoryStream state;
        private readonly IFormatter formatter;
        private static MainWindow SpreadSheet = MainWindow.Instance;

        /// <summary>
        /// The momento constructor takes a snapshot of our current spreadsheet,
        /// allowing us to save the current state.  It should be handled through
        /// StateManager only.
        /// </summary>
        public Momento()
        {
            formatter = new BinaryFormatter();
            state = new MemoryStream();
        }

        /// <summary>
        /// Captures the current spreadsheet state into a memory stream.
        /// </summary>
        public void Capture()
        {
            formatter.Serialize(state, SpreadSheet);
        }

        /// <summary>
        /// This gets rid of our memory stream.
        /// </summary>
        ~Momento()
        {
            state.Dispose();
        }

        /// <summary>
        /// Writes a memento out to a file.
        /// </summary>
        public void Save()
        {
            using (FileStream fs = new FileStream("Spreadsheet.ts", FileMode.Create))
            {
                state.WriteTo(fs);
              //  state.Close();
            }
        }

        /// <summary>
        /// Reads a momento in from a file.
        /// </summary>
        public void Load()
        {
            using (FileStream fs = new FileStream("Spreadsheet.ts", FileMode.Open))
            {
                fs.CopyTo(state);
            }

            try
            {
                state.Position = 0; //We have to read from beginning of the stream.
                SpreadSheet = (MainWindow) formatter.Deserialize(state);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize: " + e.Message);
            }
        }
    }
}

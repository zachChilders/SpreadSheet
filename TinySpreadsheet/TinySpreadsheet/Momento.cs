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
        private MemoryStream state;

        public Momento()
        {
            IFormatter formatter = new BinaryFormatter();
            state = new MemoryStream();

        }

    }
}

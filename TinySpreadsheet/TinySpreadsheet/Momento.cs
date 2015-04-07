using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TinySpreadsheet
{
    public class Momento
    {
        private SerializationInfo state;

        public Momento()
        {
            IFormatter formatter = new BinaryFormatter();
            

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TinySpreadsheet.Persistence
{
    [DataContract]
    [KnownType(typeof(State))]
    public class State
    {
        [DataMember]
        public Dictionary<String, Column> Columns = new Dictionary<String, Column>();
        [DataMember]
        public int RowCount { get; set; }
    }
}

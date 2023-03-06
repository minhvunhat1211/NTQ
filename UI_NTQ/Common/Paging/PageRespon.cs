using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_NTQ.Common
{
    public class PageRespon<T>
    {
        public int TotalRecord { get; set; }
        public List<T> Items { get; set; }
    }
}

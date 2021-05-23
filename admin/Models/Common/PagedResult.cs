using System;
using System.Collections.Generic;
using System.Text;

namespace view_model.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { get; set; }
        public int TotalRecord { get; set; }
    }
}

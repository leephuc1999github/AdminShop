using System;
using System.Collections.Generic;
using System.Text;

namespace view_model.Common
{
    public class PagingRequestBase : RequestBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}

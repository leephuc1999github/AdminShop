using System;
using System.Collections.Generic;
using System.Text;
using view_model.Common;

namespace view_model.Catalog.Shipments
{
    public class GetShipmentPagingRequest : PagingRequestBase
    {
        public string Code { get; set; }
    }
}

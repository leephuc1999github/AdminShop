using System;
using System.Collections.Generic;
using System.Text;

namespace view_model.Catalog.Shipments
{
    public class ShipmentStatusVm
    {
        public string Status { get; set; }
        public float MoneyShipment { get; set; }
        public string CodeShipment { get; set; }
        public string NoteShipping { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace view_model.Catalog.Shipments
{
    public class ShipmentVm
    {
        public string NameReceiver { get; set; }
        public string PhoneNumberReceiver { get; set; }
        public string AddressReceiver { get; set; }
        public float MoneyOrder { get; set; }
        public string PaymentFormat { get; set; }
        public DateTime DateReceived { get; set; }
        public string NoteOrder { get; set; }

        public string NameSender { get; set; }
        public string PhoneNumberSender { get; set; }
        public string AddressSender { get; set; }

        public string Status { get; set; }
        public string Code { get; set; }
        public string NoteShipping { get; set; }
        public int Id { get; set; }
    }
}

using System;

namespace Domain.Entities
{
    public class Order
    {
        public int VendorID { get; set; }
        public List<Item> Items { get; set; }

    }

}



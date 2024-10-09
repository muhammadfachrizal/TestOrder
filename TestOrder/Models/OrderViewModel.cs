using System.ComponentModel.DataAnnotations;

namespace TestOrder.Models
{
    public class OrderViewModel
    {
        
        public long SO_ORDER_ID { get; set; }
        public string? ORDER_NO { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public int COM_CUSTOMER_ID { get; set; }
        public string? ADDRESS { get; set; }
        public string? CUSTOMER_NAME { get; set; }
        public List<ItemViewModel>? ItemList { get; set; }
    }

    public class ItemViewModel
    {
        
        public long SO_ITEM_ID { get; set; }
        public long SO_ORDER_ID { get; set; }
        public string? ITEM_NAME { get; set; }
        public int QUANTITY { get; set; }
        public double PRICE { get; set; }
    }

    public class CustomerViewModel
    {
        
        public int COM_CUSTOMER_ID { get; set; }
        public string? CUSTOMER_NAME { get; set; }
    }

    public class IdNamePairVM
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
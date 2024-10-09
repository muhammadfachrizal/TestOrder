using System.ComponentModel.DataAnnotations;

namespace TestOrder.Models
{
    public class Order
    {
        [Key]
        public long SO_ORDER_ID { get; set; }
        public string? ORDER_NO { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public int COM_CUSTOMER_ID { get; set; }
        public string? ADDRESS { get; set; }
        //public Customer? CUSTOMER { get; set; }
        //public List<Item>? ItemList { get; set; }
    }

    public class Item
    {
        [Key]
        public long SO_ITEM_ID { get; set; }
        public long SO_ORDER_ID { get; set; }
        public string? ITEM_NAME { get; set; }
        public int QUANTITY { get; set; }
        public double PRICE { get; set; }
    }

    public class Customer
    {
        [Key]
        public int COM_CUSTOMER_ID { get; set; }
        public string? CUSTOMER_NAME { get; set; }
    }
}
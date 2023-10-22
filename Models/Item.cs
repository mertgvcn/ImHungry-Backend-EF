namespace WebAPI_Giris.Models
{
    public class Item
    {
        public int itemID { get; set; }

        public string itemName { get; set;}

        public string itemDescription { get; set;}

        public string imageSource { get; set; }

        public int categoryID { get; set; }

        public double price { get; set; }
    }
}

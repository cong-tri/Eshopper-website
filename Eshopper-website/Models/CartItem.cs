﻿namespace Eshopper_website.Models
{
    public class CartItem
    {
        public long PRO_ID { get; set; }
        public string? PRO_Name { get; set; }
        public int PRO_Quantity { get; set; }
        public decimal PRO_Price { get; set; }
        public string PRO_Image { get; set; }
        public decimal PRO_Total { get { return PRO_Price * PRO_Quantity; } }

        public CartItem() { }
        public CartItem(Product product) 
        {
            PRO_ID = product.PRO_ID;
            PRO_Name = product.PRO_Name;
            PRO_Price = product.PRO_Price;
            PRO_Image = product.PRO_Image ?? "";
            PRO_Quantity = 1;
        }
    }
}
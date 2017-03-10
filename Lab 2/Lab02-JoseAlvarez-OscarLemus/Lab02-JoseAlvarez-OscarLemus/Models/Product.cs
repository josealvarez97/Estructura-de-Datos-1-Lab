using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab02_JoseAlvarez_OscarLemus.Models
{
    public class Product
    {

        [Key]
        [Required]
        [DisplayName("Codigo de producto")]
        public int product_key { get; set; }

        [Required]
        [DisplayName("Descripcion del producto")]
        public string product_description { get; set; }

        [Required]
        [DisplayName("Precio")]
        public int product_price { get; set; }

        [Required]
        [DisplayName("Cantidad en inventario")]
        public int quantity_of_product { get; set; }

        public Product(int _product_key, string _product_description, int _product_price, int _quantity_of_product)
        {
            this.product_key = _product_key;
            this.product_description = _product_description;
            this.product_price = _product_price;
            this.quantity_of_product = _quantity_of_product;
        }
    }
}
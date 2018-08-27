using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopDemo.Models.Entities
{
    public class Product
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Precio")]
        public decimal Price { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public string Type { get; set; }
        [Required]
        [Display(Name = "Stock")]
        public int StockQuantity { get; set; }
        [Display(Name = "Imagen")]
        public string Image { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        [Display(Name = "Porcentaje de descuento")]
        public int DiscountPercent { get; set; }

        [NotMapped]
        public HttpPostedFileBase File { get; set; }
        [NotMapped]
        public string DealPrice
        {
            get
            {
                return (DiscountPercent == 0) ? Price.ToString("0.##") : (Price - (Price * (decimal)DiscountPercent) / 100).ToString("0.##");
            }
            set { }
        }
        [NotMapped]
        public bool Stock
        {
            get
            {
                return (StockQuantity > 0);
            }
            set
            {
            }
        }
    }
}
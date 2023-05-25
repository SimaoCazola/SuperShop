using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace SuperShop.Data.Entities
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}",ApplyFormatInEditMode=false)]// Apicar duas casas decimal nos precos "c2=currency", manter a formatacao normal no modo de edicao 
        public decimal Price { get; set; }

        [Display(Name="Image")] // Data notation
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }
    }
}

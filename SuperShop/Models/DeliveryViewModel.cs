using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace SuperShop.Models
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Deliver Date")]
        [DisplayFormat(DataFormatString = "{0:yyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? DeliverDate { get; set; }  

    }
}

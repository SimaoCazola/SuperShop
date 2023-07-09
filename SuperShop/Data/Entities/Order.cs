using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SuperShop.Data.Entities
{
    public class Order: IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name ="Order date")]
        [DisplayFormat(DataFormatString = "{0:yyy / MM / dd hh: mm tt}", ApplyFormatInEditMode=false)]
        public DateTime OrderDate { get; set; }


        [Required]
        [Display(Name = "Deliver Date")]
        [DisplayFormat(DataFormatString = "{0:yyy / MM / dd hh: mm tt}", ApplyFormatInEditMode=false)]
        public DateTime DeliverDate { get; set; }



        [Required]
        public User User { get; set; }


        // Aqui e onde fazemos a Ligacao da tabela, de 1 para muitos:
        public IEnumerable<OrderDetail> Items { get; set; }


        // Propriedade para aparecer as linhas depois de confirmar os detalhes temporarios
        [DisplayFormat(DataFormatString ="{0:N0}")]
        public int Lines => Items == null ? 0 : Items.Count();


        // Calculo da quantidade total
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => Items == null ? 0 : Items.Sum(i => i.Quantity);


        // Calculo do valor total
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Items == null ? 0 : Items.Sum(i => i.Value);


        // Calculo da hora local
        [Display(Name ="Order date")]
        [DisplayFormat(DataFormatString = "{0:yyy / MM / dd hh: mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDateLocal => this.OrderDate == null ? null : this.OrderDate.ToLocalTime();
         
    }
}

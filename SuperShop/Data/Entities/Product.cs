using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace SuperShop.Data.Entities
{
    public class Product: IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage ="The field {0} can contain {1}Characters length.")] 
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}",ApplyFormatInEditMode=false)]// Apicar duas casas decimal nos precos "c2=currency", manter a formatacao normal no modo de edicao 
        public decimal Price { get; set; }

        [Display(Name="Image")] // Data notation
        public Guid ImageId { get; set; } // Propriedade que vai guardar as imagens vindas do azure
        //public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public User User { get; set; }

        // Passo 44: Criar uma propriedade so de leitura para a api, para o utilizador conseguir ler a imagem
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://supershopsa.azurewebsites.net/images/noimage.png"
            : $"https://supershopsofia.blob.core.windows.net/products/{ImageId}";
         
        //{
            //get
            //{
            //    if (string.IsNullOrEmpty(ImageUrl))
            //    {
            //        return null;
            //    }
            //    return $"https://localhost:44380{ImageUrl.Substring(1)}"; // caminho da imagem
            //}
        //}

    }
}

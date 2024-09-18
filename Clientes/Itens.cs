using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
 
namespace Ecommerce.Clientes
{
    public class Item
    {
        [Key] // Define a propriedade como chave primária
        [JsonIgnore]
        public int ProdutoId { get; init; }
        public string Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }


 

        [JsonIgnore]
        public decimal  Total  { get; set; }

    

        // Construtor para inicializar a classe com valores
        public Item(string descricao, decimal quantidade, decimal precoUnitario , decimal total)
        {
            ProdutoId = IdGenerator.GenerateId();
            Descricao = descricao;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            Total = total;
            
        }
    }
}

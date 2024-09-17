namespace Ecommerce.Clientes;


//record ja cria uma estrutura com get e set
public record AddClienteRequest(string Nome, string Cpf, string Categoria, List<Item>Itens);
public record AddItemRequest(string Descricao, decimal Quantidade, decimal precoUnitario);
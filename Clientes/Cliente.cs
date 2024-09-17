namespace Ecommerce.Clientes;

public class Cliente
{
    //Guid é um Id que não se repete
    //Init depois que criado e setado alguma coisa dentro do id nada mais vai poder alterar essa propridade Id

    public Guid Identificador { get; init; }

    public DateTime dataVenda { get; init; }

    public Guid clienteId { get; init; }

    //private set = so consigo alterar esse cara dentro da minha classe, não consigo alterar pela instancia 
    public string Nome { get; private set; }

    public string Cpf { get; private set; }

    public string Categoria { get; private set; }

    public List<Item> Itens { get; set; } 



    //construtor
    public Cliente(string nome, string cpf, string categoria)
    {   
        //o id eu vou gerar pq se eu to fazendo um public um new nova venda eu estou gerando uma nova venda no sistema eu gero um novo id
        Identificador = Guid.NewGuid();
        dataVenda = DateTime.Now;   
        clienteId = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Categoria = categoria;  
        Itens = new List<Item>();
    }



}

 
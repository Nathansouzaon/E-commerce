using Ecommerce.data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;
namespace Ecommerce.Clientes;
using System.Text.Json;

//static por que nao ha necessidade de instanciar
public static class ClientesRotas
{
  
    public static void AddRotasClientes(this WebApplication app)
    {   
        //alem de eu ficar usando o app vou so usar a var rotasClientes
        var rotasClientes = app.MapGroup("Clientes");
        var rotaListar = app.MapGroup("/listar-Pedido");
        var rotaAlterarPedido = app.MapGroup("/Alterar-Pedido");
        //criar cliente e inserindo no banco de dados, o entity quando for fazer uma requisição ele vai olhar esse addClienteRequest e o body quando eu mandar um json com o formato desse addCliente ele automaticamente vai pegar os dados do cliente e vai setar aqui dentro pra mim
        rotasClientes.MapPost("", async (AddClienteRequest request, AppDbContext context, CancellationToken ct) =>
        {
            decimal soma = 0;
            decimal subTotal = 0;
            decimal descontos = 0;
            decimal valorTotal = 0;

        

            //acesso meu banco de dados se tiver algum cliente com o mesmo nome eu lanço um erro
            var jaExiste = await context.Cliente.AnyAsync(Cliente => Cliente.Nome == request.Nome, ct);

      
            if (jaExiste)
                return Results.Conflict("Nome De Usuário Já Existe.");

            //pegando do meu construtor
            var novoCliente = new Cliente(request.Nome, request.Cpf, request.Categoria);
            
            if (novoCliente.Categoria == "regular") {
                decimal desconto = 0.5m;
                foreach (var item in request.Itens)
                {
                    soma += item.Quantidade * item.PrecoUnitario;
                    if (soma > 500)
                    {
                        desconto += desconto * soma;
                        item.Total += soma - desconto;
                    }
                }

              
         }else if(novoCliente.Categoria == "premium"){
                decimal descontoPremium = 0.10m;
                foreach (var item in request.Itens)
                {
                    soma += item.Quantidade * item.PrecoUnitario;
                    if (soma > 300)
                    {
                        descontoPremium = descontoPremium * soma;
                        item.Total += soma - descontoPremium;
                    }
                }
              
            }else if(novoCliente.Categoria == "vip")
            {
                decimal descontoVip = 0.15m;
                foreach(var item in request.Itens)
                {
                    soma += item.Quantidade * item.PrecoUnitario;
                    descontoVip = descontoVip * soma;
                    item.Total += soma - descontoVip;
                }

            }

         
           

            foreach (var item in request.Itens)
            {

                novoCliente.Itens.Add(item);
            }
            //o entity ele simula como se eu estivesse uma lista de clientes
            await context.Cliente.AddAsync(novoCliente, ct);
                
            //o entity não salva nada ele não executa nenhuma operação no banco enquanto eu não chamo essa função quando damos um save o entity ele vai salvar somente o que mudou
            await context.SaveChangesAsync(ct);


            /*
             O objeto resumo consolidará as seguintes informações:

Identificador do Cliente (identificador)
Subtotal antes dos descontos (subTotal)
Valor total dos descontos (descontos)
Valor total após descontos (valorTotal)
Detalhes dos itens do cliente (itens), incluindo quantidade e preço unitário.
             */
            //criação de um obj anonimo
            var resumo = new
            {
                identificador = novoCliente.Identificador.ToString(),
                subTotal = soma,
                descontos = soma - novoCliente.Itens.Sum(x => x.Total),
                valorTotal = novoCliente.Itens.Sum(x => x.Total),
                itens = novoCliente.Itens.Select(i => new
                {
                    quantidade = i.Quantidade,
                    precoUnitario = i.PrecoUnitario,
                }).ToList()
            };


            Console.WriteLine(resumo);

            //enviando o sumário
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("email", "nathanjau2018@outlook.com");


                var jsonContent = JsonSerializer.Serialize(resumo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://sti3-faturamento.azurewebsites.net/api/vendas", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Sumário enviado com sucesso");    
                }else
                {
                    
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Falha ao enviar o sumário. Código de status: {response.StatusCode}. Mensagem: {errorContent}");
                }
            }




   

            //se tudo der certo
            return Results.Ok(novoCliente);
        });



        rotaListar.MapGet("", async (AppDbContext context) =>
        {           
               
                 var buscarPedido = context.Itens.AsNoTracking().ToList();
          
                  return buscarPedido;
        });



        rotaAlterarPedido.MapPatch("/Item/{id}", async (int id,Item itens,AppDbContext context) =>
        {
            var ItemExiste = await context.Itens.FindAsync(id);
      
            decimal soma = 0;
     
     
            if (ItemExiste is null)
            {
                return Results.NotFound();
            }

            ItemExiste.Descricao = itens.Descricao;
            ItemExiste.Quantidade = itens.Quantidade;
            ItemExiste.PrecoUnitario = itens.PrecoUnitario;
            ItemExiste.Total = itens.Total;

          
                decimal desconto = 0.5m;
                soma += ItemExiste.PrecoUnitario * ItemExiste.Quantidade;
               if(soma > 500)
                {
                    desconto = soma * desconto;
                    ItemExiste.Total += soma - desconto;
                }else if(soma > 300)
                {
                    decimal descontoPremium = 0.10m;
                    descontoPremium = soma * descontoPremium;
                    ItemExiste.Total += soma - descontoPremium;
                }else
            {
                    decimal somaVip = 0;
                    decimal descontoVip = 0.15m;
                    somaVip += ItemExiste.PrecoUnitario * ItemExiste.Quantidade;
                    descontoVip = somaVip * descontoVip;
                    ItemExiste.Total += somaVip - descontoVip;
                }
        
            

            await context.SaveChangesAsync();
            return Results.Ok(ItemExiste);
        });
    }   
}
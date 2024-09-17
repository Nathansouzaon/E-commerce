using Ecommerce.data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;
namespace Ecommerce.Clientes;

using System.Globalization;
using System.Text.Json;

//static por que nao ha necessidade de instanciar
public static class ClientesRotas
{



    public static void AddRotasClientes(this WebApplication app)
    {
        //alem de eu ficar usando o app vou so usar a var rotasClientes
        var rotasClientes = app.MapGroup("Clientes");
        var rotaListar = app.MapGroup("/listar-Pedidos");
        var obterPedido = app.MapGroup("/Obter-Pedido");
        var rotaAlterarPedido = app.MapGroup("/Alterar-Pedido");
        //criar cliente e inserindo no banco de dados, o entity quando for fazer uma requisi��o ele vai olhar esse addClienteRequest e o body quando eu mandar um json com o formato desse addCliente ele automaticamente vai pegar os dados do cliente e vai setar aqui dentro pra mim
        rotasClientes.MapPost("", async (AddClienteRequest request, AppDbContext context, CancellationToken ct) =>
        {
            decimal soma = Convert.ToDecimal("1,41293", new CultureInfo("pt-BR"));
            decimal subTotal = Convert.ToDecimal("1,41293", new CultureInfo("pt-BR"));
            decimal descontos = Convert.ToDecimal("1,41293", new CultureInfo("pt-BR"));
            decimal valorTotal = Convert.ToDecimal("1,41293", new CultureInfo("pt-BR")); ;



            //acesso meu banco de dados se tiver algum cliente com o mesmo nome eu lan�o um erro
            var jaExiste = await context.Cliente.AnyAsync(Cliente => Cliente.Nome == request.Nome, ct);


            if (jaExiste)
                return Results.Conflict("Nome De Usu�rio J� Existe.");

            //pegando do meu construtor
            var novoCliente = new Cliente(request.Nome, request.Cpf, request.Categoria);

            if (novoCliente.Categoria == "regular")
            {
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


            }
            else if (novoCliente.Categoria == "premium")
            {
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

            }
            else if (novoCliente.Categoria == "vip")
            {
                decimal descontoVip = 0.15m;
                foreach (var item in request.Itens)
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

            //o entity n�o salva nada ele n�o executa nenhuma opera��o no banco enquanto eu n�o chamo essa fun��o quando damos um save o entity ele vai salvar somente o que mudou
            await context.SaveChangesAsync(ct);


            var httpClient = new HttpClient();
            var url = "https://sti3-faturamento.azurewebsites.net/api/vendas";
            var email = "nathanjau2018@outlook.com";

            var sumarioService = new SumarioService(httpClient, url, email);


            /*
             O objeto resumo consolidar� as seguintes informa��es:

Identificador do Cliente (identificador)
Subtotal antes dos descontos (subTotal)
Valor total dos descontos (descontos)
Valor total ap�s descontos (valorTotal)
Detalhes dos itens do cliente (itens), incluindo quantidade e pre�o unit�rio.
             */
            //cria��o de um obj anonimo
            var resumo = new
            {
                identificador = novoCliente.Identificador.ToString(),
                subTotal = Math.Round(novoCliente.Itens.Sum(x => x.Quantidade * x.PrecoUnitario), 2),
                descontos = Math.Round(soma - novoCliente.Itens.Sum(x => x.Total), 2),
                valorTotal = Math.Round(novoCliente.Itens.Sum(x => x.Total - descontos), 2),
                itens = novoCliente.Itens.Select(i => new
                {
                    quantidade = i.Quantidade,
                    precoUnitario = Math.Round(i.PrecoUnitario, 2),
                }).ToList()
            };


            Console.WriteLine(resumo);
            await sumarioService.EnviarSumarioAsync(resumo);
            






            //se tudo der certo
            return Results.Ok(novoCliente);
        });



        rotaListar.MapGet("", async (AppDbContext context) =>
        {

            var buscarPedido = context.Itens.ToList();

            return buscarPedido;
        });

        obterPedido.MapGet("/pedido/{id}", async (int id, AppDbContext context) =>
        {
            var obterPedido = context.Itens.FindAsync(id);

            return obterPedido;
        });



        rotaAlterarPedido.MapPatch("/Item/{id}", async (int id, Item itens, AppDbContext context) =>
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
            if (soma > 500)
            {
                desconto = soma * desconto;
                ItemExiste.Total += soma - desconto;
            }
            else if (soma > 300)
            {
                decimal descontoPremium = 0.10m;
                descontoPremium = soma * descontoPremium;
                ItemExiste.Total += soma - descontoPremium;
            }
            else
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
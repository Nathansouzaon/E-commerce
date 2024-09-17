using Ecommerce.Clientes;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class FaturamentoService
{
    private readonly HttpClient _httpClient;

    public FaturamentoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task EnviarFaturamentoAsync( double subTotal, double descontos, double valorTotal, List<Item> itens)
    {
        string url = "https://sti3-faturamento.azurewebsites.net/api/vendas";

        var requestBody = new
        {
            identificador = Guid.NewGuid(),
            subTotal = subTotal,
            descontos = descontos,
            valorTotal = valorTotal,
            Itens = new List<Item>()
    };

        string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
        HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Add("email", "seu-email@exemplo.com");

        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Processar a resposta se necessário
        }
        catch (HttpRequestException e)
        {
            // Tratar o erro
            Console.WriteLine("Erro na solicitação: " + e.Message);
        }
    }
}
 

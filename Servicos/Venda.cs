using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class SumarioService
{
    private readonly HttpClient _httpClient;
    private readonly string _url;
    private readonly string _email;

    public SumarioService(HttpClient httpClient, string url, string email)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _url = url ?? throw new ArgumentNullException(nameof(url));
        _email = email ?? throw new ArgumentNullException(nameof(email));

        _httpClient.DefaultRequestHeaders.Add("email", _email);
    }


    public async Task EnviarSumarioAsync(object resumo)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(resumo);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_url, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Sumário enviado com sucesso");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Falha ao enviar o sumário. Código de status: {response.StatusCode}. Mensagem: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao enviar o sumário: {ex.Message}");
        }
    }
}
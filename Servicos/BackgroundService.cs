//using Ecommerce.Clientes;

//internal interface IScopedProcessingService
//{
//    Task DoWork(CancellationToken stoppingToken);
//}

//internal class ScopedProcessingService : IScopedProcessingService
//{
//    private int executionCount = 0;
//    private readonly ILogger _logger;

//    public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
//    {
//        _logger = logger;
//    }

//    public async Task DoWork(CancellationToken stoppingToken)
//    {   


//        while (!stoppingToken.IsCancellationRequested)
//        {
//            executionCount++;
//            decimal soma = 0;
//            decimal descontos = 0;
//            var novoCliente = new Cliente("nome", "cpf", "categoria");
//            var resumo = new
//            {
//                identificador = novoCliente.Identificador.ToString(),
//                subTotal = Math.Round(novoCliente.Itens.Sum(x => x.Quantidade * x.PrecoUnitario), 2),
//                descontos = Math.Round(soma - novoCliente.Itens.Sum(x => x.Total), 2),
//                valorTotal = Math.Round(novoCliente.Itens.Sum(x => x.Total - descontos), 2),
//                itens = novoCliente.Itens.Select(i => new
//                {
//                    quantidade = i.Quantidade,
//                    precoUnitario = Math.Round(i.PrecoUnitario, 2),
//                }).ToList()
//            };


            

//            Console.WriteLine(resumo);
//            await sumarioService.EnviarSumarioAsync(resumo);
//            _logger.LogInformation(
//                "Scoped Processing Service is working. Count: {Count}", executionCount);

//            await Task.Delay(10000, stoppingToken);
//        }
//    }
//}
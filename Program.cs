using Ecommerce.data;
using Ecommerce.Clientes;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();//pra cada requisi��o ele vai gerar uma instancia pra mim desse appdbcontext como o proprio dotnet gero essa instancia pra mim ent�o para mim recuperar e s� eu chamar 
builder.Services.AddHttpClient<FaturamentoService>();
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//gerando endpoint - lambda () => arrow funct
//app.MapGet("Hello-World", () => "Hello World");
//VendasEndPoint.AddEndPointVendas(app);
//como usei o this na minha func agora posso chamar minha class assim
app.AddRotasClientes();

app.Run();
 



//migration e quando o entity framework compara o meu codigo com o jeito que est� meu banco a diferen�a ele coloca na migration para que ela seja aplicada
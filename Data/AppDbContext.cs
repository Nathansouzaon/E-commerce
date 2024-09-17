using Ecommerce.Clientes;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.data;


//nome padrão utilizado para representar um conjunto de tabelas
//ou todo o banco de dados utiliza o context toda vez que a gente for manipular o banco de dados vamos fazer isso pela nossa classe



//pra mim conseguir usar meu dbcontext preciso usar a injeção de dependencia do sistema e um mecanismo que gera as instancia para mim eu não preciso gerar manualmente
public class AppDbContext : DbContext
{
    //preciso falar pro entity que minha classe cliente vai ser uma tabela
   public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Item> Itens { get; set; }

    //preciso configurar ele dizer como ele vai conectar ao bd, essa linha eu estou dizendo como que eu quero que meu entity vai se comunicar com meu sql

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar o relacionamento entre Cliente e Item
        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Itens)
            .WithOne() // Se `Item` não tiver uma referência de volta para `Cliente`
            .OnDelete(DeleteBehavior.Cascade);

        // Outras configurações do modelo
    }   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //dentro do meu sqlite vou Colocar qual e o connection string dele  
        optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
        base.OnConfiguring(optionsBuilder); 
    }



}
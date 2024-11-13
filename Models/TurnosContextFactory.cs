using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Turnos.Models
{
    public class TurnosContextFactory : IDesignTimeDbContextFactory<TurnosContext>
    {
        public TurnosContext CreateDbContext(string[] args)
        {

            // Configuración de la ruta de la cadena de conexión

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Creación de las opciones de DBContext usando la cadena de conexión

            var optionBuilder = new DbContextOptionsBuilder<TurnosContext>();
            var connectionString = configuration.GetConnectionString("TurnosContext");
            optionBuilder.UseSqlServer(connectionString);

            return new TurnosContext(optionBuilder.Options);

        }

    }

}


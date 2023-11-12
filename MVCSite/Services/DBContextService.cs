using Microsoft.EntityFrameworkCore;
using ProjetosViniciusVieiraMota.Models;

namespace ProjetosViniciusVieiraMota.Services
{
    public class DBContextService : Microsoft.EntityFrameworkCore.DbContext
    {
        public DBContextService(DbContextOptions<DBContextService> options) : base(options)
        {
        }

        public DbSet<EmailModel> emails { get; set; }

    }
}

using manage.core.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.infra.context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<JobHistory> JobHistory { get; set; }
        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=database.db");

        //    // Aplicar migrações pendentes ou criar o banco se ele ainda não existir
        //    Database.Migrate();
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento 1:N entre Usuário e Projetos
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId);

            // Relacionamento 1:N entre Projeto e Tarefas
            modelBuilder.Entity<Job>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Jobs)
                .HasForeignKey(t => t.ProjectId);
        }
    }
}

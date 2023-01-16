using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_JWt_Clains.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_JWt_Clains.Data
{
    public class MinimalContextDb : DbContext
    {
        public MinimalContextDb(DbContextOptions<MinimalContextDb> options) : base(options){}

        public DbSet<Fornecedor> Fornecedores {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fornecedor>()
            .HasKey(p =>p.Id);

            modelBuilder.Entity<Fornecedor>()
            .Property(p=>p.Nome).IsRequired().HasColumnType("Varchar(200)");

            modelBuilder.Entity<Fornecedor>()
            .Property(p=>p.Documentacao).IsRequired().HasColumnType("varchar(14)");
            modelBuilder.Entity<Fornecedor>()
            .ToTable("Fornecedores");
            base.OnModelCreating(modelBuilder);

        }

    }
}
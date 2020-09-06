using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public class ParserContext : DbContext
    {
        public ParserContext()
        {
            //Database.EnsureCreated();
        }

        DbSet<Advertisement> Advertisements { get; set; }
        DbSet<House> Houses { get; set; }
        DbSet<Owner> Owners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=FlatParser;Trusted_Connection=True;");
        }
    }
}

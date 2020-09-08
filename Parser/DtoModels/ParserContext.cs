using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parser.Models;
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

        public DbSet<DtoAdvertisement> Advertisements { get; set; }
        public DbSet<DtoHouse> Houses { get; set; }
        public DbSet<DtoOwner> Owners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=FlatParser;Trusted_Connection=True;");

            //var config = new MapperConfiguration(cfg => cfg.CreateMap<DtoOwner, Owner>());
            //var mapper = new Mapper(config);
        }


    }
}

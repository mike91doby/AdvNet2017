﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SE407_Dobachesky
{
    public class InspectionDBContext : DbContext
    {
        // DB Context properties
        public IConfigurationRoot Configuration { get; set; }
        public DbSet<Inspection> Inspections { get; set; }

        // DB Context constructor
        public InspectionDBContext()
        {
            var Builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = Builder.Build();
        }

        // DB Context OnConfiguring override for connection string
        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            OptionsBuilder.UseSqlServer(Configuration.GetConnectionString("MSSQLDB"));
            base.OnConfiguring(OptionsBuilder);
        }
    }
}
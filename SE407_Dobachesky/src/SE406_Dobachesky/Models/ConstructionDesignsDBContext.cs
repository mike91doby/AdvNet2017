﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SE406_Dobachesky
{
    public class ConstructionDesignsDBContext : DbContext
    {
        // DB Context properties
        public IConfigurationRoot Configuration { get; set; }
        public DbSet<ConstructionDesigns> ConstructionDesigns { get; set; }

        // DB Context constructor
        public ConstructionDesignsDBContext()
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

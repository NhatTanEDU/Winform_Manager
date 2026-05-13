using System.IO;
using RMA.Server.Entities;
using System;
using System.Linq;

namespace RMA.Server.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Always create base categories if they don't exist
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Laptop" },
                    new Category { Name = "Desktop" },
                    new Category { Name = "Monitor" },
                    new Category { Name = "Accessories" }
                );
                context.SaveChanges();
            }

            // Always create base statuses if they don't exist
            if (!context.StatusMasters.Any())
            {
                context.StatusMasters.AddRange(
                    new StatusMaster { StatusName = "New", ColorCode = "#0000FF" },
                    new StatusMaster { StatusName = "In Progress", ColorCode = "#FFA500" },
                    new StatusMaster { StatusName = "Completed", ColorCode = "#008000" }
                );
                context.SaveChanges();
            }

            // Import/Update from CSV
            string csvPath = Path.Combine(AppContext.BaseDirectory, "Data", "sheet2026.csv");
            
            // During development, BaseDirectory might be bin/Debug/..., so let's fallback to relative path from project root if needed
            if (!File.Exists(csvPath))
            {
                csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "sheet2026.csv");
            }

            if (File.Exists(csvPath))
            {
                CsvImporter.ImportData(context, csvPath);
            }
        }
    }
}

using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect_WebApp.Workers
{
    public class DbData
    {
        public static void InitializeCategories(IServiceProvider serviceProvider)
        {
            try
            {
                var context = serviceProvider.GetRequiredService<Agri_Energy_Connect_WebAppContext>();

                context.Database.EnsureCreated();

                if (!context.Category.Any())
                {
                    context.Category.AddRange(
                        new Category { Description = "Fruits/Vegetables" },
                        new Category { Description = "Dairy" },
                        new Category { Description = "Meat" },
                        new Category { Description = "Beverage" },
                        new Category { Description = "Seed" },
                        new Category { Description = "Other" }
                    );

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }



        }
    }
}

using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect_WebApp.Workers
{
    public class DbData
    {
        /// <summary>
        /// Method to add dummy data to the category table
        /// </summary>
        /// <param name="serviceProvider"></param>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to add dummy data to the Employees table
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeEmployees(IServiceProvider serviceProvider)
        {
            try
            {
                var context = serviceProvider.GetRequiredService<Agri_Energy_Connect_WebAppContext>();

                context.Database.EnsureCreated();

                if (!context.Employee.Any())
                {
                    context.Employee.AddRange(
                        new Employee { EmployeeId = "E001", Name = "John", Surname = "Doe", Username = "JohnDoe"},
                        new Employee { EmployeeId = "E002", Name = "Jane", Surname = "Smith", Username = "JaneSmith"},
                        new Employee { EmployeeId = "E003", Name = "Bob", Surname = "Johnson", Username = "BobJohnson"}
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to add dummy data to the Farmers Table
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeFarmers(IServiceProvider serviceProvider)
        {
            try
            {
                var context = serviceProvider.GetRequiredService<Agri_Energy_Connect_WebAppContext>();

                context.Database.EnsureCreated();

                if (!context.Farmer.Any())
                {
                    context.Farmer.AddRange(
                        new Farmer { Name = "Alice", Surname = "Wonderland", Username = "AliceWonderland"},
                        new Farmer { Name = "Bob", Surname = "Builder", Username = "BobBuilder"},
                        new Farmer { Name = "Charlie", Surname = "Chocolate", Username = "CharlieChocolate"}
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to add dummy data to the Products table
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeProducts(IServiceProvider serviceProvider)
        {
            try
            {
                var context = serviceProvider.GetRequiredService<Agri_Energy_Connect_WebAppContext>();

                context.Database.EnsureCreated();

                if (!context.Product.Any())
                {
                    context.Product.AddRange(
                        new Product
                        {
                            Name = "Apple",
                            Description = "Fresh Red Apples",
                            ProductionDate = new DateTime(2023, 5, 1),
                            CategoryId = 1,
                            FarmerId = 1
                        },
                        new Product
                        {
                            Name = "Milk",
                            Description = "Fresh Whole Milk",
                            ProductionDate = new DateTime(2023, 4, 15),
                            CategoryId = 2,
                            FarmerId = 2
                        },
                        new Product
                        {
                            Name = "Beef",
                            Description = "Organic,Grass-fed Beef",
                            ProductionDate = new DateTime(2023, 3, 10),
                            CategoryId = 3,
                            FarmerId = 3
                        },
                        new Product
                        {
                            Name = "Banana",
                            Description = "Fresh Large Banana",
                            ProductionDate = new DateTime(2023, 2, 20),
                            CategoryId = 1,
                            FarmerId = 1
                        },
                        new Product
                        {
                            Name = "Chicken",
                            Description = "Organic, Free Range Chicken",
                            ProductionDate = new DateTime(2023, 7, 17),
                            CategoryId = 3,
                            FarmerId = 3
                        },
                        new Product
                        {
                            Name = "Leather",
                            Description = "Fresh Cow Leather",
                            ProductionDate = new DateTime(2023, 9, 30),
                            CategoryId = 6,
                            FarmerId = 2
                        }
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
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------End of Code------------------------------------------------------------
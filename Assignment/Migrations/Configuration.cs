namespace Assignment.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Assignment.Models.PlantDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Assignment.Models.PlantDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.Plants.AddOrUpdate(
                p => p.Description,
                new Models.Plant
                {
                    Description = "NewPLantitis",
                    Watered_Status = 1,
                    Last_Watered = DateTime.Now,

                },
                new Models.Plant
                {
                    Description = "SecondPlant",
                    Watered_Status = 0,
                    Last_Watered = DateTime.Now,

                }
                );
        }
    }
}

namespace DewaldReview.Migrations
{
    using DewaldReview.DataBase;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DewaldReview.DataBase.ReviewContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DewaldReview.DataBase.ReviewContext context)
        {
            //  This method will be called after migrating to the latest version.
            Seed oSeed = new Seed();
            oSeed.SeedData(context);
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}

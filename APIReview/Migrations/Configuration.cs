namespace APIReview.Migrations
{
    using APIReview.DataBase;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<APIReview.DataBase.ReviewContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(APIReview.DataBase.ReviewContext context)
        {
            //  This method will be called after migrating to the latest version.
            Seed oSeed = new Seed();
            oSeed.SeedData(context);
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}

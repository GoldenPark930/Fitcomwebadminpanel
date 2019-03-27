namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LinksMediaCorpDataAccessLayer;
    internal  class Configuration : DbMigrationsConfiguration<LinksMediaCorpDataAccessLayer.LinksMediaContext>
    {
        private Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

      
    }
}

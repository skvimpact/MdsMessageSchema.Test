using MdsMessageSchema.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StringTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace MdsMessageSchema.Test
{
    public class MessageSchemaCheck
    {
        public ISchemaRepo repo;
        public MessageSchemaCheck()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<SchemaDbContext>();
            optionsBuilder.UseSqlServer(config["ConnectionStrings:SchemaConnection"]);
                SchemaDbContext db = new SchemaDbContext(optionsBuilder.Options);
            repo = new SchemaRepoEF(db);
        }

        [Fact]
        public void ModelClassesCheck()
        {
            IEnumerable<SchemaItem> items = repo.SchemaItems.ToArray();
            foreach (var i in MessageSchema.Analyze(items).ModelClasses(new string[] { "A", "B", "C"}))
            {
                Debug.WriteLine(i.Content);
                Debug.WriteLine(i.Name);
                Console.WriteLine(i.Content);
                Console.WriteLine(i.Name);
            }
        }
        [Fact]
        public void ModelClassesCheck2()
        {
            IEnumerable<SchemaItem> items = repo.SchemaItems.ToArray();
            foreach (var i in MessageSchema.Analyze(items).ModelClasses(new string[] { "A", "B", "C" }).Take(1))
            {
                Debug.WriteLine(i.Content
                    //.InjectBefore("class", "\tShiny new string !!!")
                    //.InjectBefore("namespace", "// beautiful")
                    .Inject(InjectType.AfterLast, "}", "// world")
                    .Inject(InjectType.BeforeLast, "}", "// zozo")
                    .Inject(InjectType.ReplaceFirst, "class", "\tpublic class u : new()")
                    );
                //Debug.WriteLine(i.Content.InjectBefore("class", "\tShiny new string !!!"));
            }
        }
    }
}

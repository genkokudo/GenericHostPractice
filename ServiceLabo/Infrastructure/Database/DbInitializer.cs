using ServiceLabo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLabo.Infrastructure.Database
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Tests.Any())
            {
                return;   // すでに作成済み 
            }

            context.Tests.AddRange(TestEntity.InitialData());
            context.SaveChanges();
        }
    }
}

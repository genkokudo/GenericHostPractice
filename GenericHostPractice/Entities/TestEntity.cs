using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GenericHostPractice.Entities
{
    public class TestEntity : EntityBase
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public static List<TestEntity> InitialData()
        {
            var result = new List<TestEntity>
            {
                new TestEntity { Name = "Ginpay Iwatobi" },
                new TestEntity { Name = "Fujiko Kuwase" }
            };

            return result;
        }
    }

}

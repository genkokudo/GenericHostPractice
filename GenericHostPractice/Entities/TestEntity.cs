using System.ComponentModel.DataAnnotations;

namespace GenericHostPractice.Entities
{
    public class TestEntity : EntityBase
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}

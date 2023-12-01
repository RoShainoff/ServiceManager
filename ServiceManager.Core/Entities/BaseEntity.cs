using System.ComponentModel.DataAnnotations;

namespace ServiceManager.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
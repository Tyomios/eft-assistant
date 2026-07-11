using System.ComponentModel.DataAnnotations.Schema;

namespace TarkovAssistant.Backend.Database.Entities;

[NotMapped]
internal abstract class Entity
{
    internal Guid Id { get; set; } = Guid.NewGuid();
}

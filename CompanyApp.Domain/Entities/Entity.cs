using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyApp.Domain.Entities
{
    /// <summary>
    /// Абстрактный базовый класс для всех сущностей, предоставляющий уникальный идентификатор.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}

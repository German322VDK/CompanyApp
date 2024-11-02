using System.ComponentModel.DataAnnotations;

namespace CompanyApp.Domain.Entities
{
    /// <summary>
    /// Сотрудник организации
    /// </summary>
    public class Employee : Entity
    {
        /// <summary>
        /// Имя сотрудника.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество сотрудника.
        /// </summary>
        [Required]
        public string Patronymic { get; set; }

        /// <summary>
        /// Фамилия сотрудника.
        /// </summary>
        [Required]
        public string SurName { get; set; }

        /// <summary>
        /// Должность сотрудника в организации. 
        /// </summary>
        [Required]
        public Position JobTitle { get; set; }

        /// <summary>
        /// Указывает, является ли сотрудник действующим.
        /// </summary>
        public bool IsEmployed { get; set; } = true;

        /// <summary>
        /// Идентификатор непосредственного руководителя сотрудника.
        /// Значение может быть null, если у сотрудника нет руководителя.
        /// </summary>
        public int? LeaderId { get; set; }

        /// <summary>
        /// Непосредственный руководитель сотрудника.
        /// </summary>
        public virtual Employee? Leader { get; set; }

        /// <summary>
        /// Список подчинённых сотрудников.
        /// </summary>
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    }
}

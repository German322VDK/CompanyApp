namespace CompanyApp.Infrastructure.Initializers
{
    /// <summary>
    /// Интерфейс инициализации бд.
    /// </summary>
    public interface IDbInitializer
    {
        /// <summary>
        /// Инициализирует базу данных, применяя все необходимые миграции и заполняя её начальными данными.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию инициализации.</returns>
        public Task InitializeAsync();
    }
}

using CompanyApp.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CompanyApp.Infrastructure.Initializers
{
    /// <summary>
    /// Класс инициализации бд
    /// </summary>
    public class CompanyAppDbInitializer : IDbInitializer
    {
        private readonly CompanyAppDbContext _dbContext;
        private readonly ILogger<CompanyAppDbInitializer> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CompanyAppDbInitializer"/>.
        /// </summary>
        /// <param name="dbContext">Экземпляр <see cref="CompanyAppDbContext"/>, представляющий контекст базы данных для инициализации данных.</param>
        /// <param name="logger">Экземпляр <see cref="ILogger{TCategoryName}"/> для ведения логирования процесса инициализации данных.</param>
        public CompanyAppDbInitializer(CompanyAppDbContext dbContext, ILogger<CompanyAppDbInitializer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            Stopwatch timer = Stopwatch.StartNew();

            _logger.LogInformation("Инициализация базы данных...");

            DatabaseFacade dbFacade = _dbContext.Database;

            if (dbFacade.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Выполнение миграций...");

                await dbFacade.MigrateAsync();

                _logger.LogInformation("Выполнение миграций выполнено успешно");
            }
            else
            {
                _logger.LogInformation($"База данных находится в актуальной версии ({timer.Elapsed.TotalSeconds:0.0###} c)");
            }

            _logger.LogInformation($"Инициализация БД выполнена успешно {timer.Elapsed.TotalSeconds}");
            timer.Stop();
        }
    }
}

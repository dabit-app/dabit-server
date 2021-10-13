using Domain.Habits;
using Domain.Habits.Events;
using Domain.Habits.Projections;
using Domain.SeedWork;
using Infrastructure.Events;
using Infrastructure.Projections;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependenciesInjection
    {
        public static void AddInfrastructureDependenciesInjection(this IServiceCollection services) {
            // repositories
            services.AddScoped<IEventStoreRepository<Habit>, EventStoreRepository<Habit>>();
            services.AddMongoRepository<HabitProjection>(
                "habits",
                async collection => { await collection.AddGuidIndex(habit => habit.UserId); }
            );
            
            // projections
            services.Project<NewHabitCreated, HabitProjection>();
            services.Project<HabitNameChanged, HabitProjection>();
            services.Project<HabitScheduleDefined, HabitProjection>();
            services.Project<HabitEventCompleted, HabitProjection>();
            services.Project<HabitEventUncompleted, HabitProjection>();
            services.Project<HabitDeleted, HabitProjection>();
            
            // miscellaneous
            AddAggregateMapper(services);
        }
        
        
        private static void AddAggregateMapper(IServiceCollection services) {
            services.AddSingleton<IAggregateMapper>(_ =>
            {
                return new AggregateMapper(new[]
                {
                    typeof(Habit)
                });
            });
        }
    }
}
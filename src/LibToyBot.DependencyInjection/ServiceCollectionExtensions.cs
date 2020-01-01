using System.Collections.Generic;
using LibToyBot.Commands;
using LibToyBot.Movement;
using LibToyBot.Reporting;
using LibToyBot.Spatial;
using Microsoft.Extensions.DependencyInjection;

namespace LibToyBot.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceCollection AddRobot(this ServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<Robot>()
                .AddScoped<ICommandExecutor, CommandExecutor>()
                .AddScoped<IPositionTracker, PositionTracker>()
                .AddScoped<IPositionReporter, PositionReporter>()
                .AddScoped<IMovementProcessor, MovementProcessor>()
                .AddScoped<IBoundsEvaluator, BoundsEvaluator>()
                .AddSingleton<Stack<Call>>()
                .AddSingleton<PlaceCommand>()
                .AddSingleton<MoveCommand>()
                .AddSingleton<TurnCommand>()
                .AddSingleton<ReportCommand>()
                .AddSingleton<Table>();

            return serviceCollection;
        }
    }
}

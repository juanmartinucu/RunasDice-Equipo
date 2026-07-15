//------------------------------------------------------------------------------
// <copyright file="BotLoader.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ucu.Poo.RunasDices.Discord
{
    /// <summary>
    /// Esta clase ejecuta el bot de Discord hasta que en la terminal donde se
    /// ejecuta el bot se oprime la tecla 'Q'.
    /// </summary>
    public static class BotLoader
    {
        /// <summary>
        /// Ejecuta el bot.
        /// </summary>
        /// <returns>Una tarea representando la ejecución del bot.</returns>
        public static async Task LoadAsync()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(options =>
                {
                    options.ClearProviders();
                    options.AddConsole();
                })
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<Action<string>>(_ => message => { })
                .AddScoped<IBot, Bot>()
                .BuildServiceProvider();

            try
            {
                IBot bot = serviceProvider.GetRequiredService<IBot>();

                await bot.StartAsync(serviceProvider).ConfigureAwait(false);

                Console.WriteLine(
                    "Conectado a Discord. Presione 'q' para salir...");

                do
                {
                    var keyInfo = Console.ReadKey();

                    if (keyInfo.Key != ConsoleKey.Q) continue;

                    Console.WriteLine("\nFinalizado");
                    await bot.StopAsync().ConfigureAwait(false);

                    return;
                } while (true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Environment.Exit(-1);
            }
        }
    }
}

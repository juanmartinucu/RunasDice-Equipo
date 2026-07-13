//------------------------------------------------------------------------------
// <copyright file="Bot.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Ucu.Poo.RunasDices.Discord
{
    /// <summary>
    /// Esta clase implementa el bot de Discord.
    /// </summary>
    public sealed class Bot : IBot, IDisposable
    {
        private ServiceProvider serviceProvider;
        private readonly ILogger<Bot> logger;
        private readonly IConfiguration configuration;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private bool disposed;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Bot"/>
        /// </summary>
        /// <param name="logger">Un objeto para logging.</param>
        /// <param name="configuration">Un objeto para configuración.</param>
        public Bot(ILogger<Bot> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;

            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                AlwaysDownloadUsers = true,
                GatewayIntents =
                    GatewayIntents.AllUnprivileged
                    | GatewayIntents.MessageContent
            };

            client = new DiscordSocketClient(config);
            commands = new CommandService();

            // client.Ready += OnReadyAsync;
            // client.Connected += OnConnectedAsync;
            // client.Disconnected += OnDisconnectedAsync;
        }

        /// <summary>
        /// Inicia el bot.
        /// </summary>
        /// <param name="services">Una colección de servicios.</param>
        /// <returns>
        /// Una tarea asíncrona para representar la ejecución del bot.
        /// </returns>
        /// <exception cref="InvalidDataException">Cuando no se encuentre el
        /// token para iniciar el bot.</exception>
        public async Task StartAsync(ServiceProvider services)
        {
            string discordToken = configuration["DiscordToken"];
            if (discordToken == null)
            {
                throw new InvalidDataException("Falta el token");
            }

            logger.LogInformation("Iniciando bot de Discord");

            serviceProvider = services;

            await commands.AddModulesAsync(Assembly.GetExecutingAssembly(),
                serviceProvider).ConfigureAwait(false);

            await client.LoginAsync(TokenType.Bot, discordToken).ConfigureAwait(false);
            await client.StartAsync().ConfigureAwait(false);

            client.MessageReceived += HandleCommandAsync;
        }

        /// <summary>
        /// Detiene el bot.
        /// </summary>
        /// <returns>
        /// Una tarea asíncrona para representar la detención del bot.
        /// </returns>
        public async Task StopAsync()
        {
            logger.LogInformation("Finalizando");
            await client.LogoutAsync().ConfigureAwait(false);
            await client.StopAsync().ConfigureAwait(false);
        }

        // private Task OnConnectedAsync()
        // {
        //     logger.LogInformation("Conexión establecida con el gateway de Discord");
        //     return Task.CompletedTask;
        // }

        // private async Task OnReadyAsync()
        // {
        //     await client.SetStatusAsync(UserStatus.Online);
        //     await client.SetGameAsync("!who");

        //     logger.LogInformation(
        //         $"Bot listo como {client.CurrentUser?.Username}#{client.CurrentUser?.Discriminator}");
        //     logger.LogInformation("Estado publicado en Discord como Online");
        // }

        // private Task OnDisconnectedAsync(Exception exception)
        // {
        //     if (exception == null)
        //     {
        //         logger.LogWarning("Bot desconectado de Discord");
        //     }
        //     else
        //     {
        //         logger.LogWarning(exception, "Bot desconectado de Discord");
        //     }

        //     return Task.CompletedTask;
        // }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot)
            {
                return;
            }

            logger.LogInformation("Mensaje recibido de {Usuario}: {Mensaje}",
                message.Author.Username, message.Content);

            int position = 0;
            bool messageIsCommand = message.HasCharPrefix('!', ref position);

            if (messageIsCommand)
            {
                IResult result = await commands.ExecuteAsync(
                        new SocketCommandContext(client, message),
                        position,
                        serviceProvider)
                    .ConfigureAwait(false);

                if (!result.IsSuccess)
                {
                    logger.LogWarning("Error ejecutando comando: {Error}", result.ErrorReason);
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            client.MessageReceived -= HandleCommandAsync;

            client.Dispose();
            (commands as IDisposable)?.Dispose();
            serviceProvider?.Dispose();
        }

    }
}

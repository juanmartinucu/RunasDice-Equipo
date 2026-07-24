//------------------------------------------------------------------------------
// <copyright file="Bot.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ucu.Poo.RunasDices.Discord
{
    /// <summary>
    /// Esta clase implementa el bot de Discord.
    /// </summary>
    public sealed class Bot : IBot, IDisposable
    {
        private readonly ILogger<Bot> logger;
        private readonly IConfiguration configuration;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private ServiceProvider serviceProvider;
        private bool disposed;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Bot"/>.
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
                    | GatewayIntents.MessageContent,
            };

            this.client = new DiscordSocketClient(config);
            this.commands = new CommandService();

            // client.Ready += OnReadyAsync;
            // client.Connected += OnConnectedAsync;
            // client.Disconnected += OnDisconnectedAsync;
        }

        /// <summary>
        /// Inicia el bot.
        /// </summary>
        /// <param name="services">Una colección de servicios.</param>
        /// <returns>Una tarea asíncrona para representar la ejecución del
        /// bot.</returns>
        /// <exception cref="InvalidDataException">Cuando no se encuentre el
        /// token para iniciar el bot.</exception>
        public async Task StartAsync(ServiceProvider services)
        {
            string discordToken = this.configuration["DiscordToken"];
            if (discordToken == null)
            {
                throw new InvalidDataException("Falta el token");
            }

            this.logger.LogInformation("Iniciando bot de Discord");

            this.serviceProvider = services;

            HashSet<Assembly> assembliesToScan = new HashSet<Assembly>();
            assembliesToScan.Add(Assembly.GetExecutingAssembly());

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                assembliesToScan.Add(entryAssembly);

                foreach (AssemblyName referencedAssemblyName in entryAssembly.GetReferencedAssemblies())
                {
                    try
                    {
                        Assembly referencedAssembly = Assembly.Load(referencedAssemblyName);
                        assembliesToScan.Add(referencedAssembly);
                    }
                    catch
                    {
                        // Ignora ensamblados que no puedan cargarse en tiempo de ejecución.
                    }
                }
            }

            foreach (Assembly assembly in assembliesToScan)
            {
                await this.commands.AddModulesAsync(assembly, this.serviceProvider)
                    .ConfigureAwait(false);
            }

            await this.client.LoginAsync(TokenType.Bot, discordToken).ConfigureAwait(false);
            await this.client.StartAsync().ConfigureAwait(false);

            this.client.MessageReceived += this.HandleCommandAsync;
        }

        /// <summary>
        /// Detiene el bot.
        /// </summary>
        /// <returns>
        /// Una tarea asíncrona para representar la detención del bot.
        /// </returns>
        public async Task StopAsync()
        {
            this.logger.LogInformation("Finalizando");
            await this.client.LogoutAsync().ConfigureAwait(false);
            await this.client.StopAsync().ConfigureAwait(false);
        }

        /*
        private Task OnConnectedAsync()
        {
            logger.LogInformation("Conexión establecida con el gateway de Discord");
            return Task.CompletedTask;
        }

        private async Task OnReadyAsync()
        {
            await client.SetStatusAsync(UserStatus.Online);
            await client.SetGameAsync("!who");

            logger.LogInformation(
                $"Bot listo como {client.CurrentUser?.Username}#{client.CurrentUser?.Discriminator}");
            logger.LogInformation("Estado publicado en Discord como Online");
        }

        private Task OnDisconnectedAsync(Exception exception)
        {
            if (exception == null)
            {
                logger.LogWarning("Bot desconectado de Discord");
            }
            else
            {
                logger.LogWarning(exception, "Bot desconectado de Discord");
            }

            return Task.CompletedTask;
        }
        */

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            this.client.MessageReceived -= this.HandleCommandAsync;

            this.client.Dispose();
            (this.commands as IDisposable)?.Dispose();
            this.serviceProvider?.Dispose();
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot)
            {
                return;
            }

            int position = 0;
            bool messageIsCommand = message.HasCharPrefix('!', ref position);

            if (messageIsCommand)
            {
                IResult result = await this.commands.ExecuteAsync(
                        new SocketCommandContext(this.client, message),
                        position,
                        this.serviceProvider)
                    .ConfigureAwait(false);

                if (!result.IsSuccess)
                {
                    this.logger.LogWarning("Error ejecutando comando: {Error}", result.ErrorReason);
                }
            }
        }
    }
}

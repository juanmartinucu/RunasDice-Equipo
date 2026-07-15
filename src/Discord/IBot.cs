//------------------------------------------------------------------------------
// <copyright file="IBot.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ucu.Poo.RunasDices.Discord
{
    /// <summary>
    /// La interfaz del Bot de Discord para usar con inyección de dependencias.
    /// </summary>
    internal interface IBot
    {
        /// <summary>
        /// Inicia el bot.
        /// </summary>
        /// <param name="services">Una colección de servicios.</param>
        /// <returns>Una tarea asíncrona para representar el inicio del bot.</returns>
        Task StartAsync(ServiceProvider services);

        /// <summary>
        /// Detiene el bot.
        /// </summary>
        /// <returns>Una tarea asíncrona para representar la detención del bot.</returns>
        Task StopAsync();
    }
}

//------------------------------------------------------------------------------
// <copyright file="UserInfoCommandMessages.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.RunasDices.Commands
{
    /// <summary>
    /// Esta clase contiene todos los mensajes retornados por la clase <see
    /// cref="UserInfoCommand"/>.
    /// </summary>
    public static class UserInfoCommandMessages
    {
        /// <summary>Obtiene ayuda del comando.</summary>
        public static string CommandUsage =>
            "Usa `!who` o `!who <usuario>`.";

        /// <summary>Usuario agregado a la lista de espera.</summary>
        public static string UserNotFound(string displayName) =>
            $"No encuentro el usuario '{displayName}' en esta aplicación";

        /// <summary>Error genérico.</summary>
        public static string Error(string message) =>
            $"Error: {message}.";
    }
}

//------------------------------------------------------------------------------
// <copyright file="ParametersMessages.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.RunasDices.Discord
{
    /// <summary>
    /// Esta clase contiene todos los mensajes retornados por la clase <see
    /// cref="Parameters"/>.
    /// </summary>
    public static class ParametersMessages
    {
        /// <summary>Hay más de un alias.</summary>
        public const string MultipleAliases = "Hay más de un alias y sólo puede haber uno";

        /// <summary>El alias no es válido.</summary>
        public const string InvalidAlias = "El alias no es válido; escribe `as:<alias>`";
    }
}

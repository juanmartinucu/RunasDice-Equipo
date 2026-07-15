//------------------------------------------------------------------------------
// <copyright file="Parameters.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Ucu.Poo.RunasDices.Discord
{
    /// <summary>
    /// Esta clase permite manipular los parámetros de un <see
    /// cref="CommandBase"/>, en particular el soporte a los alias: un comando
    /// que incluya como parámetro <c>as:user</c> se debe procesar como si fuera
    /// enviado por el usuario <c>user</c> y no por el usuario que realmente
    /// envía el mensaje. Esto debería permitir que un jugador pueda jugar
    /// contra él mismo.
    /// </summary>
    public class Parameters
    {
        private IList<string> items = new List<string>();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Parameters"/>
        /// a partir del texto recibido como parámetro. Asume que los parámetros
        /// están separados en este texto por espacios.
        /// </summary>
        /// <param name="parameters">La lista de parámetros separados por
        /// espacios.</param>
        public Parameters(string parameters)
        {
            this.ExtractItems(parameters);
        }

        /// <summary>
        /// Obtiene un valor que indica si entre los parámetros se incluye un
        /// alias.
        /// </summary>
        public bool AliasIncluded { get; private set; }

        /// <summary>
        /// Obtiene el alias indicado como parámetro o <c>null</c> si no se
        /// indicó un alias.
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// Obtiene una lista de todos los parámetros, excluyendo el alias.
        /// </summary>
        public IReadOnlyList<string> Items { get { return this.items.AsReadOnly(); } }

        /// <summary>
        /// Obtiene la cantidad de parámetros, excluyendo el alias.
        /// </summary>
        public int Count { get { return this.items.Count; } }

        /// <summary>
        /// Obtiene un valor que indica si hay parámetros o no, excluyendo el
        /// alias.
        /// </summary>
        /// <returns><c>true</c> si hay parámetros, <c>false</c> en caso
        /// contrario.</returns>
        public bool IsEmpty { get { return this.items.Count == 0; } }

        /// <summary>
        /// Obtiene el parámetro indicado.
        /// </summary>
        /// <param name="index">El parámetro a obtener; el primero es 0.</param>
        /// <returns>El parámetro indicado.</returns>
        public string this[int index]
        {
            get { return this.items[index]; }
        }

        // Extrae los parámetros de la lista recibida como argumento.
        private void ExtractItems(string parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                string[] parts = parameters.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (string part in parts)
                {
                    if (part.StartsWith("as:", StringComparison.OrdinalIgnoreCase))
                    {
                        if (this.AliasIncluded)
                        {
                            throw new InvalidOperationException(
                                ParametersMessages.MultipleAliases);
                        }

                        this.Alias = part.Substring("as:".Length);
                        if (string.IsNullOrWhiteSpace(this.Alias))
                        {
                            this.Alias = null;
                            throw new InvalidOperationException(ParametersMessages.InvalidAlias);
                        }

                        this.AliasIncluded = true;
                    }
                    else
                    {
                        this.items.Add(part);
                    }
                }
            }
        }
    }
}

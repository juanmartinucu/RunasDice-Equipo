//------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using Ucu.Poo.RunasDices.Discord;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Program
{
    /// <summary>
    /// Un programa que implementa un bot de Discord.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada al programa.
        /// </summary>
        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                DemoFacade(args);
            }
            else
            {
                DemoBot();
            }
        }

        private static void DemoFacade(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine(Facade.Instance.GetUserInfo(args[0]));
            }
        }

        private static void DemoBot()
        {
            BotLoader.LoadAsync().GetAwaiter().GetResult();
        }
    }
}

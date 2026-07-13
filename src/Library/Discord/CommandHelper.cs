// //------------------------------------------------------------------------------
// // <copyright file="CommandHelper.cs" company="Universidad Católica del Uruguay">
// //     Copyright (c) Programación II. Derechos reservados.
// // </copyright>
// //------------------------------------------------------------------------------

// using System;
// using Discord.Commands;
// using Discord.WebSocket;

// namespace Ucu.Poo.RunasDices.Discord
// {
//     /// <summary>
//     /// Esta clase permite obtener información del usuario de Discord en el
//     /// contexto de un comando.
//     /// </summary>
//     internal static class CommandHelper
//     {
//         /// <summary>
//         /// Retorna el nombre visible del usuario de Discord que envía un
//         /// comando que se recibe como parámetro. En el contexto de ese comando
//         /// el nombre visible del usuario retornado es un usuario válido en el
//         /// servidor actual.
//         /// </summary>
//         /// <param name="command">El comando que se está ejecutando.</param>
//         /// <param name="name">El nombre de usuario a obtener.</param>
//         /// <returns>
//         /// Cuando no se provee un nombre, retorna el nombre visible del usuario
//         /// que envía el comando en el servidor de Discord del contexto
//         /// provisto. Cuando se provee un nombre, asume que ese nombre puede ser
//         /// el nombre visible, el nickname, o el nombre de usuario global de
//         /// Discord, pero retorna el nombre visible de usuario. Esto permite
//         /// usar de forma consistente el nombre visible del usuario
//         /// independiente de cómo se obtenga.
//         /// </returns>
//         public static string GetDisplayName(
//             //SocketCommandContext context,
//             CommandBase command,
//             string name = null)
//         {
//             ArgumentNullException.ThrowIfNull(command);

//             if (name == null)
//             {
//                 name = command.Context.Message.Author.Username;
//             }

//             foreach (SocketGuildUser user in command.Context.Guild.Users)
//             {
//                 if (user.Username == name
//                     || user.DisplayName == name
//                     || user.Nickname == name
//                     || user.GlobalName == name)
//                 {
//                     return user.DisplayName;
//                 }
//             }

//             return name;
//         }

//         /// <summary>
//         /// Determina si un usuario existe en el contexto en un comando que se
//         /// recibe como parámetro.
//         /// </summary>
//         /// <param name="command">El contexto del comando</param>
//         /// <param name="name">El nombre del usuario a buscar.</param>
//         /// <returns>
//         /// Busca un usuario cuyo nombre visible, nickname, o nombre de usuario
//         /// global de Discord coincida con el nombre provisto como parámetro.
//         /// Retorna <c>true</c> si existe y <c>false</c> en caso contrario.
//         /// </returns>
//         public static bool UserExists(
//             CommandBase command,
//             string name)
//         {
//              ArgumentNullException.ThrowIfNull(command);

//             if (name == null)
//             {
//                 return false;
//             }

//             foreach (SocketGuildUser user in command.Context.Guild.Users)
//             {
//                 if (user.Username == name
//                     || user.DisplayName == name
//                     || user.Nickname == name
//                     || user.GlobalName == name)
//                 {
//                     return true;
//                 }
//             }

//             return false;
//         }
//     }
// }

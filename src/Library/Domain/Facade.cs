//------------------------------------------------------------------------------
// <copyright file="Facade.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>
    /// Esta clase recibe las peticiones del usuario y devuelve los resultados
    /// que permiten implementar las historias de usuario. Otras clases
    /// necesarias para el funcionamiento del bot de Discord usan esta clase
    /// pero no conocen el resto de las clases del dominio.
    ///
    /// Esta clase es un singleton.
    ///
    /// Los métodos de esta clase reciben <c>string</c> como parámetro y
    /// retornan <see cref="Result"/> como resultado. De esta forma es posible
    /// determinar si el resultado de una operación es exitoso o no; en caso de
    /// que sea exitoso y corresponda un valor, también es posible determinar el
    /// valor; en caso de que sea fallido, se incluye el mensaje explicando el
    /// error. Esto permite que la clase pueda ser usada por el bot sin que
    /// conozca el resto de las clases del dominio.
    /// </summary>
    public class Facade
    {
        #region Singleton

        private static Facade instance;

        // Este constructor privado impide que otras clases puedan crear instancias
        // de esta.
        private Facade()
        {
            this.usersRepository = new UsersRepository();
        }

        /// <summary>
        /// Obtiene la única instancia de la clase <see cref="Facade"/>.
        /// </summary>
        public static Facade Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Facade();
                }

                return instance;
            }
        }

        /// <summary>
        /// Inicializa este singleton. Es necesario solo en los tests.
        /// </summary>
        public static void Reset()
        {
            instance = null;
        }

        #endregion

        private IUsersRepository usersRepository;
        private List<User> waitingList = new List<User>();

        /// <summary>
        /// Devuelve información del usuario cuyo nombre de usuario se recibe
        /// como parámetro.
        /// </summary>
        /// <param name="userName">El nombre de usuario del usuario.
        /// </param>
        /// <returns>Un texto con la información relevante del usuario. Este
        /// método está previsto que pueda ser extendido por los estudiantes
        /// para que devuelva información adicional.
        /// </returns>
        /// <exception cref="ArgumentException">Cuando <paramref name="userName"/>
        /// es <c>null</c>, vacío o contiene solo espacios en blanco.</exception>
        public Result<string> GetUserInfo(string userName)
        {
            string result;

            bool created = this.usersRepository.Find(userName) == null;
            User user = this.FindOrCreateUser(userName);
            if (created)
            {
                result = FacadeMessages.UserIsNew(userName);
            }
            else if (this.InternalUserIsWaiting(userName))
            {
                result = FacadeMessages.UserIsWaiting(userName);
            }
            else
            {
                result = FacadeMessages.UserCanPlay(userName);
            }

            return Result.Success<string>(result);
        }

        // Crea un usuario si no existe y lo retorna; en caso contrario retorna
        // el usuario existente.
        private User FindOrCreateUser(string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);
            ArgumentException.ThrowIfNullOrWhiteSpace(userName);

            User userFound = this.usersRepository.Find(userName);
            if (userFound == null)
            {
                userFound = this.usersRepository.Add(userName);
            }

            return userFound;
        }

        /// <summary>
        /// Agrega el usuario cuyo nombre de usuario se recibe como parámetro a
        /// la lista de espera de jugadores esperando por un oponente para
        /// jugar.
        /// </summary>
        /// <param name="userName">El nombre del usuario a agregar a la
        /// lista.</param>
        /// <exception cref="ArgumentException">Cuando <paramref
        /// name="userName"/> es <c>null</c>, vacío o contiene solo espacios en
        /// blanco.</exception>
        /// <exception cref="InvalidOperationException">Cuando el usuario ya
        /// está en la lista.</exception>
        public Result AddUserToWaitingList(string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);
            ArgumentException.ThrowIfNullOrWhiteSpace(userName);

            User user = this.FindOrCreateUser(userName);

            if (this.waitingList.Contains(user))
            {
                return Result.Failure(
                    FacadeMessages.UserAlreadyWaiting(user.UserName));
            }

            this.waitingList.Add(user);

            return Result.Success();
        }

        // Retorna true si el usuario está esperando para jugar y false el caso
        // contrario.
        private bool InternalUserIsWaiting(string userName)
        {
            return this.waitingList.Any(
                user => user.UserName.Equals(
                    userName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determina si el usuario cuyo nombre de usuario se recibe como
        /// parámetro se ha inscrito en la lista de espera de jugadores
        /// esperando por un oponente para jugar.
        /// </summary>
        /// <param name="userName">El nombre del usuario a buscar.</param>
        /// <returns>Retorna un <see cref="Result{T}"/> de éxito cuyo valor
        /// <c>true</c>si el usuario está en la lista, <c>false</c> en caso
        /// contrario.</returns>
        /// <exception cref="ArgumentException">Cuando <paramref name="userName"/>
        /// es <c>null</c>, vacío o contiene solo espacios en blanco.</exception>
        public Result<bool> UserIsWaiting(string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);
            ArgumentException.ThrowIfNullOrWhiteSpace(userName);

            bool result = this.InternalUserIsWaiting(userName);

            return Result<bool>.Success(result);
        }

        /// <summary>
        /// Obtiene la lista actual de usuarios esperando por un oponente.
        /// </summary>
        /// <returns>Retorna un <see cref="Result{T}"/> de éxito cuyo valor es
        /// una colección de nombres de usuarios esperando por un oponente para
        /// jugar.</returns>
        public Result<IReadOnlyList<string>> GetUsersWaitingForOpponent()
        {
            return Result.Success<IReadOnlyList<string>>(
                this.waitingList
                    .Select(user => user.UserName)
                    .ToList()
                    .AsReadOnly());
        }

        /// <summary>
        /// Inicia una partida entre dos usuarios cuyos nombres de usuario se
        /// reciben como parámetro: el usuario que envía el mensaje y un
        /// oponente. El oponente debe estar en la lista de espera de jugadores
        /// esperando por un oponente para jugar. Quita el oponente de la lista
        /// de espera y también al usuario que envía el mensaje si estaba en esa
        /// lista.
        /// </summary>
        /// <param name="userName">El nombre de usuario de uno de los
        /// jugadores.</param>
        /// <param name="opponentName">El nombre de usuario jugador
        /// oponente.</param>
        /// <exception cref="ArgumentNullException">Cuando
        /// <paramref name="userName"/> o <paramref name="opponentName"/>
        /// es <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Cuando
        /// <paramref name="userName"/> o <paramref name="opponentName"/>
        /// es vacío o contiene solo espacios en blanco.</exception>
        public Result<Game> StartGame(string userName, string opponentName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userName);
            ArgumentException.ThrowIfNullOrWhiteSpace(opponentName);

            bool opponentIsWaiting = this.InternalUserIsWaiting(opponentName);

            if (!opponentIsWaiting)
            {
                return Result.Failure<Game>(FacadeMessages.OpponentIsNotWaiting(opponentName));
            }

            User user = this.FindOrCreateUser(userName);
            User opponent = this.FindOrCreateUser(opponentName);

            Game game = new Game(new Player(user), new Player(opponent));

            this.waitingList.Remove(user);
            this.waitingList.Remove(opponent);

            return Result.Success<Game>(game);
        }
    }

    /// <summary>
    /// Esta clase contiene todos los mensajes retornados por <see
    /// cref="Facade"/>.
    /// </summary>
    public static class FacadeMessages
    {
        /// <summary>El usuario ya está esperando.</summary>
        public static string UserAlreadyWaiting(string userName) =>
            $"El usuario '{userName}' ya está esperando para jugar.";

        /// <summary>El oponente no está esperando.</summary>
        public static string OpponentIsNotWaiting(string opponentName) =>
            $"El usuario '{opponentName}' no está esperando para jugar.";

        /// <summary>El usuario es nuevo.</summary>
        public static string UserIsNew(string userName) =>
            $"El usuario '{userName}' es nuevo.";

        /// <summary>El usuario está esperando para jugar.</summary>
        public static string UserIsWaiting(string userName) =>
            $"El usuario '{userName}' e⁄stá esperando para jugar.";

        /// <summary>El usuario puede jugar.</summary>
        public static string UserCanPlay(string userName) =>
            $"El usuario '{userName}' puede jugar.";
    }
}

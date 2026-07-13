//------------------------------------------------------------------------------
// <copyright file="Result.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>
    /// Esta clase representa un resultado de la <see cref="Facade"/>. El
    /// resultado puede ser exitoso o fallido, según <see
    /// cref="Result.IsSuccess"/> sea <c>true</c> o <c>false</c>
    /// respectivamente; <see cref="Result.IsFailure"/> tiene el valor
    /// contrario, <c>true</c> si es fallido o <c>false</c> en caso contrario.
    ///
    /// En el caso de que el resultado sea fallido, la propiedad <see
    /// cref="Result.Errors"/> tiene la lista de errores; en caso contrario, es
    /// <c>null</c>.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Obtiene el valor del resultado: <c>true</c> si es de éxito y
        /// <c>false</c> en caso contrario.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Obtiene el valor del resultado: <c>true</c> si es fallido y
        /// <c>false</c> en caso contrario.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Obtiene el o los mensajes de errores si <see
        /// cref="Result.IsFailure"/> es <c>true</c>. Es <c>null</c> en caso
        /// contrario.
        /// </summary>
        public string Errors { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Result"/> con
        /// el o los mensajes de error en caso de resultado fallido.
        /// </summary>
        /// <param name="isSuccess">Indica si es un resultado exitoso o de
        /// fracaso.</param>
        /// <param name="errors">El o los mensajes de error.</param>
        protected Result(bool isSuccess, string errors = null)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        /// <summary>
        /// Crea una instancia de la clase <see cref="Result"/> para representar
        /// un resultado exitoso. Los resultados exitosos no tienen errores
        /// asociados.
        /// </summary>
        /// <returns>
        /// Una nueva instancia de <see cref="Result"/> que representa un
        /// resultado exitoso.
        /// </returns>
        public static Result Success()
        {
            return new Result(true);
        }

        /// <summary>
        /// Crea una instancia de la clase <see cref="Result"/> para representar
        /// un resultado fallido con un solo mensaje de error.
        /// </summary>
        /// <param name="error">El mensaje de error.</param>
        /// <returns>
        /// Una nueva instancia de <see cref="Result"/> que representa un
        /// resultado fallido.
        /// </returns>
        public static Result Failure(string error)
        {
            return new Result(false, error);
        }

        /// <summary>
        /// Crea un resultado exitoso con un valor asociado.
        /// </summary>
        /// <typeparam name="T">El tipo del valor asociado al resultado.</typeparam>
        /// <param name="value">El valor asociado al resultado exitoso.</param>
        /// <returns>
        /// Una nueva instancia de <see cref="Result{T}"/> que representa un
        /// resultado exitoso con valor.
        /// </returns>
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value);
        }

        /// <summary>
        /// Crea un resultado fallido con uno o más mensajes de error y sin
        /// valor asociado.
        /// </summary>
        /// <typeparam name="T">El tipo del valor asociado en caso de éxito.</typeparam>
        /// <param name="error">El mensaje de error.</param>
        /// <returns>
        /// Una nueva instancia de <see cref="Result{T}"/> que representa un
        /// resultado fallido sin valor.
        /// </returns>
        public static Result<T> Failure<T>(string error)
        {
            return new Result<T>(error);
        }
    }

    /// <summary>
    /// Esta clase representa un resultado, al igual que en <see
    /// cref="Result"/>, pero en el que hay un valor asociado en
    /// caso de éxito.
    /// </summary>
    /// <typeparam name="T">El valor asociado al resultado exitoso.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Obtiene el valor asociado al resultado en caso de éxito; en caso de
        /// error será <c>default</c> para el tipo <typeparamref name="T"/>.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Result{T}"/> que
        /// representa un resultado exitoso con valor.
        /// </summary>
        /// <param name="value">El valor asociado al resultado exitoso.</param>
        public Result(T value)
            : base(true)
        {
            this.Value = value;
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Result{T}"/> que
        /// representa un resultado fallido con una colección de errores. En
        /// este caso, el valor asociado al resultado exitoso <see
        /// cref="Result{T}.Value"/> es <c>default</c> para el tipo
        /// <typeparamref name="T"/>
        /// </summary>
        /// <param name="errors">El o los mensajes de error.</param>
        public Result(string errors)
            : base(false, errors)
        {
            this.Value = default(T);
        }
    }
}

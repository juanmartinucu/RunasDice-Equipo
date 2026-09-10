using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Define los datos comunes de cualquier carta.</summary>
    public interface ICard
    {
        string Name { get; }

        int Cost { get; }

        string Description { get; }
    }
}

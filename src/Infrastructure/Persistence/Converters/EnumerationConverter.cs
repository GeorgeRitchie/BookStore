using Domain.Primitives;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters
{
    /// <summary>
    /// Converts custom enumeration types implemented from <see cref="Enumeration{TEnum}"/> to and from strings for database storage.
    /// </summary>
    /// <typeparam name="T">The enumeration type that implements <see cref="Enumeration{TEnum}"/>.</typeparam>
    public sealed class EnumerationConverter<T> : ValueConverter<T, string> where T : Enumeration<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationConverter{T}"/>.
        /// </summary>
        public EnumerationConverter() : base(v => v.ToString(),
                                             v => (T)typeof(T).GetMethod("FromName")!.Invoke(null, new object[] { v })!)
        { }
    }
}

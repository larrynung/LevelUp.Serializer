using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelUp.Serializer
{
    public static class EnumExtension
    {
        #region Methods

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        /// <exception cref="InvalidOperationException">This member belongs to a type that is loaded into the reflection-only context. See How to: Load Assemblies into the Reflection-Only Context.</exception>
        /// <exception cref="NotSupportedException">This <see cref="T:System.Type" /> object is a <see cref="T:System.Reflection.Emit.TypeBuilder" /> whose <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> method has not yet been called. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is null. </exception>
        public static T GetCustomAttribute<T>(this Enum e)
        {
            return GetCustomAttributes<T>(e).FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is null. </exception>
        /// <exception cref="NotSupportedException">This <see cref="T:System.Type" /> object is a <see cref="T:System.Reflection.Emit.TypeBuilder" /> whose <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> method has not yet been called. </exception>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        /// <exception cref="InvalidOperationException">This member belongs to a type that is loaded into the reflection-only context. See How to: Load Assemblies into the Reflection-Only Context.</exception>
        public static IEnumerable<T> GetCustomAttributes<T>(this Enum e)
        {
            return
                e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(T), false).Cast<T>();
        }

        #endregion Methods
    }
}
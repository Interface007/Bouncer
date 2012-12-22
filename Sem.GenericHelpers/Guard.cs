namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Helps to streamline data validation.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> in case of <paramref name="value"/> being <c>null</c>.
        /// </summary>
        /// <param name="value"> The value to be checked. </param>
        /// <param name="name"> The name of the PARAMETER this value represents. </param>
        /// <typeparam name="T"> The type of <paramref name="value"/>. </typeparam>
        /// <exception cref="ArgumentNullException"> In case of <paramref name="value"/> being <c>null</c>. </exception>
        public static void ThrowIfParameterNull<T>([ValidatedNotNull] this T value, string name)
            where T : class
        {
            if (value != null)
            {
                return;
            }

            var stackTrace = new StackTrace();
            var methodName = stackTrace.GetFrame(1).GetMethod().Name;
            throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "The parameter {0} (type: {1}) of method {2} must not be NULL.", name, typeof(T).Name, methodName));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> in case of <paramref name="value"/> being <c>null</c>.
        /// </summary>
        /// <param name="value"> The <see cref="IEnumerable{T}"/> to be checked. </param>
        /// <param name="name"> The name of the PARAMETER this <see cref="IEnumerable{T}"/> represents. </param>
        /// <typeparam name="T"> The generic type of <paramref name="value"/>. </typeparam>
        /// <exception cref="ArgumentNullException"> In case of <paramref name="value"/> being <c>null</c>. </exception>
        public static void ThrowIfParameterNull<T>([ValidatedNotNull] ref IEnumerable<T> value, string name)
            where T : class
        {
            if (value != null)
            {
                return;
            }

            var stackTrace = new StackTrace();
            var methodName = stackTrace.GetFrame(1).GetMethod().Name;
            throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "The parameter {0} (type: IEnumerable<{1}>) of method {2} must not be NULL.", name, typeof(T).Name, methodName));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> in case of <paramref name="value"/> being <c>null</c>.
        /// </summary>
        /// <param name="value"> The value to be checked. </param>
        /// <param name="name"> The name of the PARAMETER this value represents. </param>
        /// <param name="exceptionMsg"> The string-formattable message of the exception to be thrown. </param>
        /// <param name="parameters"> The string-format-parameters for the message of the exception to be thrown. </param>
        /// <typeparam name="T"> The type of <paramref name="value"/>. </typeparam>
        /// <exception cref="ArgumentNullException"> In case of <paramref name="value"/> being <c>null</c>. </exception>
        public static void ThrowIfParameterNull<T>([ValidatedNotNull] this T value, string name, string exceptionMsg, params object[] parameters)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, exceptionMsg, parameters));
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> in case of <paramref name="value"/> being <c>null</c>.
        /// Use this for checks of NON-Parameter values.
        /// </summary>
        /// <param name="value"> The <see cref="IEnumerable{T}"/> to be checked. </param>
        /// <param name="exceptionMsg"> The string-formattable message of the exception to be thrown. </param>
        /// <param name="parameters"> The string-format-parameters for the message of the exception to be thrown. </param>
        /// <typeparam name="T"> The generic type of <paramref name="value"/>. </typeparam>
        /// <exception cref="ArgumentNullException"> In case of <paramref name="value"/> being <c>null</c>. </exception>
        public static void ThrowIfIsNull<T>([ValidatedNotNull] this T value, string exceptionMsg, params object[] parameters)
            where T : class
        {
            if (value == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, exceptionMsg, parameters));
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> in case of <paramref name="value"/> being NOT EQUAL to <paramref name="otherValue"/>.
        /// </summary>
        /// <param name="value"> The value to be checked. </param>
        /// <param name="otherValue"> The value to compare to. </param>
        /// <param name="name"> The name of the PARAMETER this value represents or belongs to. </param>
        /// <param name="exceptionMsg"> The string-formattable message of the exception to be thrown. </param>
        /// <param name="parameters"> The string-format-parameters for the message of the exception to be thrown. </param>
        /// <typeparam name="T"> The generic type of <paramref name="value"/>. </typeparam>
        /// <exception cref="ArgumentNullException"> In case of <paramref name="value"/> being <c>null</c>. </exception>
        public static void EnsureNotEquals<T>([ValidatedNotNull] this T value, T otherValue, string name, string exceptionMsg, params object[] parameters)
        {
            if (Equals(value, otherValue))
            {
                throw new ArgumentOutOfRangeException(name, string.Format(CultureInfo.InvariantCulture, exceptionMsg, parameters));
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> in case of <paramref name="value"/> being EQUAL to <paramref name="otherValue"/>.
        /// </summary>
        /// <param name="value"> The value to be checked. </param>
        /// <param name="otherValue"> The value to compare to. </param>
        /// <param name="name"> The name of the PARAMETER this value represents or belongs to. </param>
        /// <param name="exceptionMsg"> The string-formattable message of the exception to be thrown. </param>
        /// <param name="parameters"> The string-format-parameters for the message of the exception to be thrown. </param>
        /// <typeparam name="T"> The generic type of <paramref name="value"/>. </typeparam>
        /// <exception cref="ArgumentNullException"> In case of <paramref name="value"/> being <c>null</c>. </exception>
        public static void EnsureEquals<T>([ValidatedNotNull] this T value, T otherValue, string name, string exceptionMsg, params object[] parameters)
        {
            if (!Equals(value, otherValue))
            {
                throw new ArgumentOutOfRangeException(name, string.Format(CultureInfo.InvariantCulture, exceptionMsg, parameters));
            }
        }
    }
}
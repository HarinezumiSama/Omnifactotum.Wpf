using Omnifactotum.Annotations;

//// ReSharper disable once CheckNamespace :: Namespace is intentionally named so in order to simplify usage of extension methods
namespace System;

internal static class LocalTypeExtensions
{
#if NET40
    public static bool IsNullableValueType([NotNull] this Type type) => type.IsNullable();
#endif
}
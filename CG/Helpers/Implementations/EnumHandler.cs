using CG.Exceptions;

namespace CG.Helpers.Implementations
{
    public static class EnumHandler
    {
        public static bool IsValidMember<EnumType>(string? value) where EnumType : struct, Enum
        {
            return Enum.TryParse<EnumType>(value, true, out var parsed)
                && Enum.IsDefined(typeof(EnumType), parsed);
        }

        public static EnumType Parse<EnumType>(string value) where EnumType : struct, Enum
        {
            if (Enum.TryParse(value, true, out EnumType parsed))
                return parsed;

            throw new BadRequestException($"{nameof(EnumType)} is invalid.");
        }
    }
}
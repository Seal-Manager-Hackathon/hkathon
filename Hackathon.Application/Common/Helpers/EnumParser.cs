using Hackathon.Application.Exceptions;

namespace Hackathon.Application.Common.Helpers;

public static class EnumParser
{
    public static TEnum ParseOrThrow<TEnum>(string value, string fieldName)
        where TEnum : struct, Enum
    {
        if (!Enum.TryParse<TEnum>(value, ignoreCase: true, out var result)
            || !Enum.IsDefined(typeof(TEnum), result))
        {
            throw new BadRequestException(
                $"Value '{value}' Is Invalid For Field {fieldName}");
        }

        return result;
    }
}

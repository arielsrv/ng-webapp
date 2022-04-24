using Core.Shared.Errors;

namespace System;

public static class StringExtensions
{
    public static IEnumerable<long> ToEnumerable(this string values)
    {
        IEnumerable<long> result = values
            .Split(',')
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => !long.TryParse(value, out long number)
                ? throw new ApiBadRequestException("Invalid format for ids. e.g: ...&ids=1,2,3")
                : number)
            .Where(value => value != 0)
            .Distinct()
            .ToList();

        const int maxConcurrent = 10;
        if (result.Count() > maxConcurrent)
        {
            throw new ApiBadRequestException($"Limit for multi-get is {maxConcurrent}");
        }

        return result;
    }
}
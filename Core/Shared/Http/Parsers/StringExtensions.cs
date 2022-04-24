using Core.Shared.Errors;

namespace System;

public static class StringExtensions
{
    public static IEnumerable<long> ToEnumerable(this string values)
    {
        const int maxConcurrent = 10;
        IEnumerable<long> result = values
            .Split(',')
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select((value, index) =>
            {
                if (!long.TryParse(value, out long number))
                {
                    throw new ApiBadRequestException("Invalid format for ids. e.g: ...&ids=1,2,3");
                }

                if (index > maxConcurrent - 1)
                {
                    throw new ApiBadRequestException($"Limit for multi-get is {maxConcurrent}");
                }

                return number;
            })
            .Distinct()
            .ToList();

        return result;
    }
}
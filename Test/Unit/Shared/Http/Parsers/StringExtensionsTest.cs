using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core.Shared.Errors;
using Xunit;

namespace Test.Unit.Shared.Http.Parsers;

public class StringExtensionsTest
{
    [Fact]
    public void String_Values_To_Long_List()
    {
        const string values = "0, 1, 1,, , 2, 3";

        IEnumerable<long> actual = values.ToEnumerable().ToList();

        Assert.NotEmpty(actual);
        Assert.Equal(3, actual.Count());
        Assert.Contains(actual, value => value == 1);
        Assert.Contains(actual, value => value == 2);
        Assert.Contains(actual, value => value == 3);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public void String_Values_To_Long_List_Throws_Error()
    {
        const string values = "1, 2, a";
        Assert.Throws<ApiBadRequestException>(() => { values.ToEnumerable().ToList(); });
    }
}
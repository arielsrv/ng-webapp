using System;
using System.Collections.Generic;
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
        Assert.Equal(4, actual.Count());
        Assert.Contains(actual, value => value == 1);
        Assert.Contains(actual, value => value == 2);
        Assert.Contains(actual, value => value == 3);
    }

    [Fact]
    public void String_Values_To_Long_List_Throws_Error()
    {
        const string values = "1, 2, a";
        Assert.Throws<ApiBadRequestException>(() => { values.ToEnumerable().ToList(); });
    }

    [Fact]
    public void String_Max_Values_To_Long_List_Throws_Error()
    {
        string values = string.Join(",", Enumerable.Range(1, 11).ToArray());
        Assert.Throws<ApiBadRequestException>(() => { values.ToEnumerable().ToList(); });
    }
}
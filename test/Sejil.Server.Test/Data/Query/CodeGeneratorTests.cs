﻿using System;
using Sejil.Data.Query.Internal;
using Xunit;

namespace Sejil.Test.Data.Query
{
    // Currently covering non tested cases from sql provider.
    // TODO: full coverage

    public class CodeGeneratorTests
    {
        [Fact]
        public void Generate_throws_when_left_side_is_non_var()
        {
            var ex = Assert.Throws<QueryEngineException>(() => new CodeGenerator().Generate(new Parser(new Scanner("'v'=p").Scan(), Array.Empty<string>()).Parse()));
            Assert.Equal("Error at position '2': Left side of comparison can only be a property/non property name.", ex.Message);
        }

        [Fact]
        public void Generate_throws_when_right_side_is_non_literal()
        {
            var ex = Assert.Throws<QueryEngineException>(() => new CodeGenerator().Generate(new Parser(new Scanner("p=p").Scan(), Array.Empty<string>()).Parse()));
            Assert.Equal("Error at position '3': Right side of comparison can only be a value.", ex.Message);
        }

        [Theory]
        [InlineData("p like 5", 8)]
        [InlineData("p not like 5", 12)]
        public void Generate_throws_when_like_notLike_op_used_with_non_string_value(string source, int pos)
        {
            var ex = Assert.Throws<QueryEngineException>(() => new CodeGenerator().Generate(new Parser(new Scanner(source).Scan(), Array.Empty<string>()).Parse()));
            Assert.Equal($"Error at position '{pos}': 'like'/'not like' keywords can only be used with strings.", ex.Message);
        }
    }
}

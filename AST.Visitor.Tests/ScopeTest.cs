using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Statements.Loops;
using Moq;
using NUnit.Framework;
using AST.Expressions.Function;
using TestHelpers;

namespace AST.Visitor.Tests
{
    namespace ScopeTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void TakesNoArguments()
            {
                new Scope();
            }
        }

        [TestFixture]
        public class DefiningIdentifiersTests
        {
            [TestCase]
            public void DefinedIdentifiersAreAbleToBeFound()
            {
                var scope = new Scope();

                scope.DefineIdentifier("TEST", new Value(1));

                var actual = scope.FindIdentifier("TEST");

                Assert.AreEqual(true, actual.IsDefined);
            }
        }
    }
}

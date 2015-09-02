using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Statements;
using AST.Visitor.Exceptions;
using NUnit.Framework;

namespace AST.Visitor.Tests
{
    namespace ScopeTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void TakesNoArguments()
            {
                new Scope();
            }
        }

        [TestFixture]
        public class DefiningIdentifiersTests
        {
            [Test]
            public void DefinedIdentifiersAreAbleToBeFound()
            {
                var scope = new Scope();

                scope.DefineIdentifier("TEST", new Value(1));

                var actual = scope.FindIdentifier("TEST");

                Assert.AreEqual(true, actual.IsDefined);
            }

            [Test]
            public void UndefinedIdentifiersHaveIsDefinedSetToFalse()
            {
                var scope = new Scope();

                var actual = scope.FindIdentifier("TEST");

                Assert.IsFalse(actual.IsDefined);
            }

            [Test]
            public void NestedScopesCanStillFindLocallyDefinedIdentifier()
            {
                var scope = new Scope();
                var identifier = "A";

                scope.DefineIdentifier(identifier, new Value(1));

                using (scope.PushScope())
                {
                    scope.DefineIdentifier(identifier, new Value(2));

                    var actual = scope.FindIdentifier(identifier);

                    Assert.AreEqual(true, actual.IsDefined);
                }
            }

            [Test]
            public void PopScopeDefinesANewMoreDeeplyNestedScope()
            {
                var scope = new Scope();
                var identifier = "A";
                var expected = 1234;

                scope.DefineIdentifier(identifier, new Value(expected));

                scope.PushScope();

                scope.DefineIdentifier(identifier, new Value(1));

                scope.PopScope();

                var actual = scope.FindIdentifier(identifier);

                Assert.AreEqual(expected, actual.Value.ToObject());
            }

            [Test]
            public void PushScopeReturnsAnIDisposableWhichPopsScopeAutomatically()
            {
                var scope = new Scope();
                var identifier = "A";
                var expected = 1234;

                scope.DefineIdentifier(identifier, new Value(expected));

                using (scope.PushScope())
                {
                    scope.DefineIdentifier(identifier, new Value(1));
                }

                var actual = scope.FindIdentifier(identifier);

                Assert.AreEqual(expected, actual.Value.ToObject());
            }

            [Test]
            public void PushScopeReturnsAnIDisposableWhichPopsScopeAutomaticallyAndWorksWithMultipleLevels()
            {
                var scope = new Scope();
                var identifier = "A";
                var expected = 1234;

                scope.DefineIdentifier(identifier, new Value(expected));

                using (scope.PushScope())
                {
                    scope.DefineIdentifier(identifier, new Value(1));
                    using (scope.PushScope())
                    {
                        scope.DefineIdentifier(identifier, new Value(2));
                        using (scope.PushScope())
                        {
                            scope.DefineIdentifier(identifier, new Value(3));
                            using (scope.PushScope())
                            {
                                scope.DefineIdentifier(identifier, new Value(4));
                            }
                        }
                    }
                }

                var actual = scope.FindIdentifier(identifier);

                Assert.AreEqual(expected, actual.Value.ToObject());
            }

            [Test]
            public void NestedScopesReturnsTheMostDeeplyDefinedInstanceOfAnIdentifier()
            {
                var scope = new Scope();
                var identifier = "A";
                var expected = 2;

                scope.DefineIdentifier(identifier, new Value(1));

                using (scope.PushScope())
                {
                    scope.DefineIdentifier(identifier, new Value(expected));

                    var actual = scope.FindIdentifier(identifier);

                    Assert.AreEqual(expected, actual.Value.ToObject());
                }
            }

            [Test]
            public void NestedScopesReturnsIdentifierFromParentIfNotFoundLocally()
            {
                var scope = new Scope();
                var identifier = "A";
                var expected = 2;

                scope.DefineIdentifier(identifier, new Value(expected));

                using (scope.PushScope())
                {
                    scope.DefineIdentifier("B", new Value(1));

                    var actual = scope.FindIdentifier(identifier);

                    Assert.AreEqual(expected, actual.Value.ToObject());
                }
            }

            [Test]
            public void NestedScopesReturnsIdentifierFromAncestorIfNotFoundLocally()
            {
                var scope = new Scope();
                var identifier = "A";
                var expected = 2;

                scope.DefineIdentifier(identifier, new Value(expected));

                using (scope.PushScope())
                {
                    scope.DefineIdentifier("B", new Value(3));
                    using (scope.PushScope())
                    {
                        scope.DefineIdentifier("C", new Value(4));
                        using (scope.PushScope())
                        {
                            scope.DefineIdentifier("D", new Value(5));
                            using (scope.PushScope())
                            {
                                scope.DefineIdentifier("E", new Value(6));
                                {
                                    scope.DefineIdentifier("F", new Value(7));

                                    var actual = scope.FindIdentifier(identifier);

                                    Assert.AreEqual(expected, actual.Value.ToObject());
                                }
                            }
                        }
                    }
                }
            }

            [Test]
            public void PushArgumentsIsAShortWayToAddIdentifiersIntoANewScopeQuickly()
            {
                var scope = new Scope();

                // colliding identifier in parent scpoe
                scope.DefineIdentifier("A", new Value(1345645));

                var arguments = new[]
                {
                    new VarDefinitionStmt(new IdentifierExpr("A"), new IdentifierExpr("int"), false, new ConstantExpr(1)),
                    new VarDefinitionStmt(new IdentifierExpr("B"), new IdentifierExpr("string"), false, new ConstantExpr("hello")),
                    new VarDefinitionStmt(new IdentifierExpr("C"), new IdentifierExpr("bool"), false, new ConstantExpr(true))
                };

                var values = new Value[]
                {
                    new Value(2), 
                    new Value("world"), 
                    new Value(false)
                };

                using (scope.PushArguments(arguments, values))
                {
                    for (var i = 0; i < arguments.Length; i++)
                    {
                        var argument = arguments[i];
                        var value = values[i];

                        var actual = scope.FindIdentifier(argument.Name.Name);

                        Assert.AreEqual(true, actual.IsDefined);
                        Assert.AreEqual(value.ToObject(), actual.Value.ToObject());
                    }
                }                
            }

            [Test]
            public void PushArgumentsRemovesTheIdentifiersWithAnIDisposable()
            {
                var scope = new Scope();

                var arguments = new[]
                {
                    new VarDefinitionStmt(new IdentifierExpr("A"), new IdentifierExpr("int"), false, new ConstantExpr(1)),
                    new VarDefinitionStmt(new IdentifierExpr("B"), new IdentifierExpr("string"), false, new ConstantExpr("hello")),
                    new VarDefinitionStmt(new IdentifierExpr("C"), new IdentifierExpr("bool"), false, new ConstantExpr(true))
                };

                var values = new Value[]
                {
                    new Value(2), 
                    new Value("world"), 
                    new Value(false)
                };

                using (scope.PushArguments(arguments, values))
                {
                    foreach (var argument in arguments)
                    {                                            
                        var actual = scope.FindIdentifier(argument.Name.Name);

                        Assert.AreEqual(true, actual.IsDefined);                    
                    }
                }

                // arguments are no longer available in parent scope.
                foreach (var argument in arguments)
                {
                    var actual = scope.FindIdentifier(argument.Name.Name);

                    Assert.AreEqual(false, actual.IsDefined);
                }
            }

            [TestCase(ExpectedException=typeof(IdentifierAlreadyDefinedException))]
            public void DucplicateIdentifiersAtTheSameLevelAreDisallowed()
            {
                var scope = new Scope();
                var identifier = "A";
                
                scope.DefineIdentifier(identifier, new Value(1));
                scope.DefineIdentifier(identifier, new Value(1));
            }
        }
    }
}

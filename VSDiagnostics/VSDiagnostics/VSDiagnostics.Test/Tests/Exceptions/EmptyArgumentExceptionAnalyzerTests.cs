﻿using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;
using VSDiagnostics.Diagnostics.Exceptions.EmptyArgumentExceptionAnalyzer;

namespace VSDiagnostics.Test.Tests.Exceptions
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {
        [TestMethod]
        public void EmptyArgumentExceptionAnalyzer_WithEmptyArgument_InvokesWarning()
        {
            var test = @"
    using System;
    using System.Text;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            void Method(string input)
            {
                throw new ArgumentException();
            }
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = EmptyArgumentExceptionAnalyzer.DiagnosticId,
                Message = EmptyArgumentExceptionAnalyzer.MessageFormat,
                Severity = EmptyArgumentExceptionAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 11, 23)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void EmptyArgumentExceptionAnalyzer_WithArgument_DoesNotInvokeWarning()
        {
            var test = @"
    using System;
    using System.Text;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            void Method(string input)
            {
                throw new ArgumentException(input);
            }
        }
    }";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void EmptyArgumentExceptionAnyalzer_WithDumbRethrowStatement_DoesNotInvokeWarning()
        {
            var test = @"
    using System;
    using System.Text;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            void Method(string input)
            {
                try { }
                catch (Exception e) { throw e; }
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void EmptyArgumentExceptionAnyalzer_WithRethrowStatement_DoesNotInvokeWarning()
        {
            var test = @"
    using System;
    using System.Text;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            void Method(string input)
            {
                try { }
                catch (Exception e) { throw; }
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new EmptyArgumentExceptionAnalyzer();
        }
    }
}
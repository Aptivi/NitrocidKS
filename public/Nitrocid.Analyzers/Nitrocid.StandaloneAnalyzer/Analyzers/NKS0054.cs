﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.MiscWriters;

namespace Nitrocid.StandaloneAnalyzer.Analyzers
{
    internal class NKS0054 : IAnalyzer
    {
        public bool Analyze(Document document)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            bool found = false;
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not InvocationExpressionSyntax exp)
                    continue;

                var args = exp.ArgumentList.Arguments;
                if (args.Count != 1)
                    continue;
                if (args[0].Expression is LiteralExpressionSyntax les)
                    if (les.Token.ValueText != "\r")
                        continue;
                if (exp.Expression is not MemberAccessExpressionSyntax maes)
                    continue;
                if (maes.Name is IdentifierNameSyntax identifier)
                {
                    var location = syntaxNode.GetLocation();

                    // Let's see if the caller tries to access .Split.
                    var idName = identifier.Identifier.Text;
                    if (idName == nameof(string.Split))
                    {
                        var lineSpan = location.GetLineSpan();
                        TextWriterColor.Write($"{GetType().Name}: {document.FilePath} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): Caller uses .Split(\"\\r\") instead of TextTools.SplitNewLines()", true, ConsoleColors.Yellow);
                        if (!string.IsNullOrEmpty(document.FilePath))
                            LineHandleRangedWriter.PrintLineWithHandle(document.FilePath, lineSpan.StartLinePosition.Line + 1, lineSpan.StartLinePosition.Character + 1, lineSpan.EndLinePosition.Character);
                        found = true;
                    }
                }
            }
            return found;
        }

        public async Task SuggestAsync(Document document, CancellationToken cancellationToken = default)
        {
            var tree = document.GetSyntaxTreeAsync(cancellationToken).Result;
            var syntaxNodeNodes = tree.GetRoot(cancellationToken).DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not InvocationExpressionSyntax exp)
                    continue;
                var maes = (MemberAccessExpressionSyntax)exp.Expression;
                if (maes.Name is IdentifierNameSyntax idName)
                {
                    // Get the method
                    if (idName.Identifier.Text != nameof(string.Split))
                        continue;

                    // We need to have a syntax that calls myvar.SplitNewLines
                    var varName = maes.Expression;
                    var classSyntax = varName;
                    var methodSyntax = SyntaxFactory.IdentifierName("SplitNewLines");
                    var maesSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                    var argsSyntax = SyntaxFactory.ArgumentList();
                    var resultSyntax = SyntaxFactory.InvocationExpression(maesSyntax, argsSyntax);
                    var replacedSyntax = resultSyntax
                        .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                        .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                    // Actually replace
                    var node = await document.GetSyntaxRootAsync(cancellationToken);
                    var finalNode = node.ReplaceNode(exp, replacedSyntax);
                    TextWriterColor.Write("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                    TextWriterColor.Write($"  - {exp}", true, ConsoleColors.Red);
                    TextWriterColor.Write($"  + {replacedSyntax.ToFullString()}", true, ConsoleColors.Green);

                    // Check the imports
                    var compilation = finalNode as CompilationUnitSyntax;
                    if (compilation?.Usings.Any(u => u.Name.ToString() == "Nitrocid.Misc.Text") == false)
                    {
                        var name = SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.IdentifierName("Nitrocid"),
                                SyntaxFactory.IdentifierName("Misc")),
                            SyntaxFactory.IdentifierName("Text"));
                        var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                        TextWriterColor.Write("Additionally, the suggested fix will add the following using statement:", true, ConsoleColors.Yellow);
                        TextWriterColor.Write($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                    }
                }
            }
        }
    }
}

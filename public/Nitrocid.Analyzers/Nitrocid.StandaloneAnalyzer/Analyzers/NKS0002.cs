﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.StandaloneAnalyzer.Analyzers
{
    internal class NKS0002 : IAnalyzer
    {
        public void Analyze(Document document)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not MemberAccessExpressionSyntax exp)
                    continue;
                if (exp.Expression is IdentifierNameSyntax identifier)
                {
                    var location = syntaxNode.GetLocation();
                    if (identifier.Identifier.Text == nameof(Console))
                    {
                        // Before making the diagnostic, we need to first check for any existing methods from the ConsoleWrapper class.
                        // For contributors: Update the below array when new functions get added to ConsoleWrapper or changed.
                        string[] wrapperFunctions = new[]
                        {
                            nameof(Console.Out),
                            nameof(Console.CursorLeft),
                            nameof(Console.CursorTop),
                            nameof(Console.WindowWidth),
                            nameof(Console.WindowHeight),
                            nameof(Console.WindowTop),
                            nameof(Console.BufferWidth),
                            nameof(Console.BufferHeight),
                            nameof(Console.ForegroundColor),
                            nameof(Console.BackgroundColor),
                            nameof(Console.CursorVisible),
                            nameof(Console.OutputEncoding),
                            nameof(Console.InputEncoding),
                            nameof(Console.KeyAvailable),
                            nameof(Console.Clear),
                            nameof(Console.SetCursorPosition),
                            nameof(Console.ResetColor),
                            nameof(Console.OpenStandardInput),
                            nameof(Console.OpenStandardOutput),
                            nameof(Console.OpenStandardError),
                            nameof(Console.SetOut),
                            nameof(Console.Beep),
                            nameof(Console.ReadKey),
                            nameof(Console.Write),
                            nameof(Console.WriteLine),
                        };

                        // Let's see if the caller tries to access one of the above Console functions and/or properties.
                        var name = (IdentifierNameSyntax)exp.Name;
                        var idName = name.Identifier.Text;
                        if (wrapperFunctions.Contains(idName))
                        {
                            var lineSpan = location.GetLineSpan();
                            TextWriterColor.Write($"{GetType().Name}: {document.FilePath} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): Caller uses Console instead of ConsoleWrapper", true, ConsoleColors.Yellow);
                        }
                    }
                }
            }
        }
    }
}

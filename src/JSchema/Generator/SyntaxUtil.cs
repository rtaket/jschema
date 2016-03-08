﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.JSchema.Generator
{
    internal static class SyntaxUtil
    {
        private const string DocCommentSummaryFormat =
@"/// <summary>
/// {0}
/// </summary>
";
        private const string DocCommentParamFormat =
@"/// <param name=""{0}"">
/// {1}
/// </param>
";

        private const string DocCommentReturnsFormat =
@"/// <returns>
/// {0}
/// </returns>
";
        private const string DocCommentExceptionFormat =
@"/// <exception cref=""{0}"">
/// {1}
/// </exception>
";


        internal static SyntaxTriviaList MakeDocComment(
            string summary,
            string returns = null,
            Dictionary<string, string> paramDescriptionDictionary = null,
            Dictionary<string, string> exceptionDictionary = null)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(summary))
            {
                sb.AppendFormat(
                    CultureInfo.CurrentCulture,
                    DocCommentSummaryFormat,
                    summary);
            }

            if (paramDescriptionDictionary != null)
            {
                foreach (KeyValuePair<string, string> kvp in paramDescriptionDictionary)
                {
                    sb.AppendFormat(
                        CultureInfo.CurrentCulture,
                        DocCommentParamFormat,
                        kvp.Key,
                        kvp.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(returns))
            {
                sb.AppendFormat(
                    CultureInfo.CurrentCulture,
                    DocCommentReturnsFormat,
                    returns);
            }

            if (exceptionDictionary != null)
            {
                foreach (KeyValuePair<string, string> kvp in exceptionDictionary)
                {
                    sb.AppendFormat(
                        CultureInfo.CurrentCulture,
                        DocCommentExceptionFormat,
                        kvp.Key,
                        kvp.Value);
                }
            }

            return SyntaxFactory.ParseLeadingTrivia(sb.ToString());
        }

        internal static SyntaxTriviaList MakeCopyrightComment(string copyrightNotice)
        {
            var trivia = new SyntaxTriviaList();
            if (!string.IsNullOrWhiteSpace(copyrightNotice))
            {
                trivia = trivia.AddRange(new SyntaxTrivia[]
                {
                    SyntaxFactory.Comment(copyrightNotice),
                    SyntaxFactory.Whitespace(Environment.NewLine)
                });
            }

            return trivia;
        }

        internal static AccessorDeclarationSyntax MakeGetAccessor(BlockSyntax body = null)
        {
            return MakeAccessor(SyntaxKind.GetAccessorDeclaration, body);
        }

        internal static AccessorDeclarationSyntax MakeSetAccessor(BlockSyntax body = null)
        {
            return MakeAccessor(SyntaxKind.SetAccessorDeclaration, body);
        }

        private static AccessorDeclarationSyntax MakeAccessor(SyntaxKind getOrSet, BlockSyntax body)
        {
            return SyntaxFactory.AccessorDeclaration(
                getOrSet,
                default(SyntaxList<AttributeListSyntax>),
                default(SyntaxTokenList),
                getOrSet == SyntaxKind.GetAccessorDeclaration
                    ? SyntaxFactory.Token(SyntaxKind.GetKeyword)
                    : SyntaxFactory.Token(SyntaxKind.SetKeyword),
                body,
                body == null ? SyntaxFactory.Token(SyntaxKind.SemicolonToken) : default(SyntaxToken));
        }
    }
}
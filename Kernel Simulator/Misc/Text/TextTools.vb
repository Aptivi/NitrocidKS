
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Namespace Misc.Text
    Public Module TextTools
        Private ReadOnly regexMatchEnclosedStrings As String = "(""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+"

        ''' <summary>
        ''' Splits the string enclosed in double quotes delimited by spaces using regular expression formula
        ''' </summary>
        ''' <param name="target">Target string</param>
        <Extension()>
        Public Function SplitEncloseDoubleQuotes(target As String) As String()
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))

            Return Regex.Matches(target, regexMatchEnclosedStrings).Cast(Of Match).Select(Function(m) m.Value.ReleaseDoubleQuotes()).ToArray()
        End Function

        ''' <summary>
        ''' Splits the string enclosed in double quotes delimited by spaces using regular expression formula without releasing double quotes
        ''' </summary>
        ''' <param name="target">Target string</param>
        <Extension()>
        Public Function SplitEncloseDoubleQuotesNoRelease(target As String) As String()
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))

            Return Regex.Matches(target, regexMatchEnclosedStrings).Cast(Of Match).Select(Function(m) m.Value).ToArray()
        End Function

        ''' <summary>
        ''' Releases a string from double quotations
        ''' </summary>
        ''' <param name="target">Target string</param>
        ''' <returns>A string that doesn't contain double quotation marks at the start and at the end of the string</returns>
        <Extension()>
        Public Function ReleaseDoubleQuotes(target As String) As String
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))

            Dim ReleasedString = target
            If target.StartsWith("""") AndAlso target.EndsWith("""") AndAlso Not Equals(target, """") OrElse
               target.StartsWith("'") AndAlso target.EndsWith("'") AndAlso Not Equals(target, "'") OrElse
               target.StartsWith("`") AndAlso target.EndsWith("`") AndAlso Not Equals(target, "`") Then
                ReleasedString = ReleasedString.Remove(0, 1)
                ReleasedString = ReleasedString.Remove(ReleasedString.Length - 1)
            End If
            Return ReleasedString
        End Function

        ''' <summary>
        ''' Gets the enclosed double quotes type from the string
        ''' </summary>
        ''' <param name="target">Target string to query</param>
        ''' <returns><seecref="EnclosedDoubleQuotesType"/> containing information about the current string enclosure</returns>
        <Extension()>
        Public Function GetEnclosedDoubleQuotesType(target As String) As EnclosedDoubleQuotesType
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))

            Dim type = EnclosedDoubleQuotesType.None
            If target.StartsWith("""") AndAlso target.EndsWith("""") AndAlso Not Equals(target, """") Then
                type = EnclosedDoubleQuotesType.DoubleQuotes
            ElseIf target.StartsWith("'") AndAlso target.EndsWith("'") AndAlso Not Equals(target, "'") Then
                type = EnclosedDoubleQuotesType.SingleQuotes
            ElseIf target.StartsWith("`") AndAlso target.EndsWith("`") AndAlso Not Equals(target, "`") Then
                type = EnclosedDoubleQuotesType.Backticks
            End If
            Return type
        End Function

        ''' <summary>
        ''' Truncates the string if the string is larger than the threshold, otherwise, returns an unchanged string
        ''' </summary>
        ''' <param name="target">Source string to truncate</param>
        ''' <param name="threshold">Max number of string characters</param>
        ''' <returns>Truncated string</returns>
        <Extension()>
        Public Function Truncate(target As String, threshold As Integer) As String
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))

            ' Try to truncate string. If the string length is bigger than the threshold, it'll be truncated to the length of
            ' the threshold, putting three dots next to it. We don't use ellipsis marks here because we're dealing with the
            ' terminal, and some terminals and some monospace fonts may not support that character, so we mimick it by putting
            ' the three dots.
            If target.Length > threshold Then
                Return target.Substring(0, threshold - 1) + "..."
            Else
                Return target
            End If
        End Function

        ''' <summary>
        ''' Makes a string array with new line as delimiter
        ''' </summary>
        ''' <param name="target">Target string</param>
        ''' <returns>List of words that are separated by the new lines</returns>
        <Extension()>
        Public Function SplitNewLines(target As String) As String()
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))

            Return target.Replace(Convert.ToChar(13).ToString(), "").Split(Convert.ToChar(10))
        End Function

        ''' <summary>
        ''' Checks to see if the string starts with any of the values
        ''' </summary>
        ''' <param name="target">Target string</param>
        ''' <param name="values">Values</param>
        ''' <returns>True if the string starts with any of the values specified in the array. Otherwise, false.</returns>
        <Extension()>
        Public Function StartsWithAnyOf(target As String, values As String()) As Boolean
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))

            Dim started = False
            For Each value In values
                If target.StartsWith(value) Then started = True
            Next
            Return started
        End Function

        ''' <summary>
        ''' Checks to see if the string contains any of the target strings.
        ''' </summary>
        ''' <param name="source">Source string</param>
        ''' <param name="targets">Target strings</param>
        ''' <returns>True if one of them is found; otherwise, false.</returns>
        <Extension()>
        Public Function ContainsAnyOf(source As String, targets As String()) As Boolean
            If source Is Nothing Then Throw New Exception(DoTranslation("The source may not be null"))

            For Each target In targets
                If source.Contains(target) Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' Replaces all the instances of strings with a string
        ''' </summary>
        ''' <param name="target">Target string</param>
        ''' <param name="toBeReplaced">Strings to be replaced</param>
        ''' <param name="toReplace">String to replace with</param>
        ''' <returns>Modified string</returns>
        ''' <exceptioncref="ArgumentNullException"></exception>
        <Extension()>
        Public Function ReplaceAll(target As String, toBeReplaced As String(), toReplace As String) As String
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))
            If toBeReplaced Is Nothing OrElse toBeReplaced.Length = 0 Then Throw New Exception(DoTranslation("Array of to be replaced strings may not be null"))

            For Each ReplaceTarget In toBeReplaced
                target = target.Replace(ReplaceTarget, toReplace)
            Next
            Return target
        End Function

        ''' <summary>
        ''' Replaces all the instances of strings with a string assigned to each entry
        ''' </summary>
        ''' <param name="target">Target string</param>
        ''' <param name="toBeReplaced">Strings to be replaced</param>
        ''' <param name="toReplace">Strings to replace with</param>
        ''' <returns>Modified string</returns>
        ''' <exceptioncref="ArgumentNullException"></exception>
        ''' <exceptioncref="ArgumentException"></exception>
        <Extension()>
        Public Function ReplaceAllRange(target As String, toBeReplaced As String(), toReplace As String()) As String
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))
            If toBeReplaced Is Nothing OrElse toBeReplaced.Length = 0 Then Throw New Exception(DoTranslation("Array of to be replaced strings may not be null"))
            If toReplace Is Nothing OrElse toReplace.Length = 0 Then Throw New Exception(DoTranslation("Array of to be replacement strings may not be null"))
            If toBeReplaced.Length <> toReplace.Length Then Throw New Exception(DoTranslation("Array length of which strings to be replaced doesn't equal the array length of which strings to replace."))

            Dim i = 0, loopTo = toBeReplaced.Length - 1

            While i <= loopTo
                target = target.Replace(toBeReplaced(i), toReplace(i))
                i += 1
            End While
            Return target
        End Function

        ''' <summary>
        ''' Replaces last occurrence of a text in source string with the replacement
        ''' </summary>
        ''' <param name="source">A string which has the specified text to replace</param>
        ''' <param name="searchText">A string to be replaced</param>
        ''' <param name="replace">A string to replace</param>
        ''' <returns>String that has its last occurrence of text replaced</returns>
        <Extension()>
        Public Function ReplaceLastOccurrence(source As String, searchText As String, replace As String) As String
            If source Is Nothing Then Throw New Exception(DoTranslation("The source may not be null"))
            If searchText Is Nothing Then Throw New Exception(DoTranslation("The search text may not be null"))

            Dim position = source.LastIndexOf(searchText)
            If position = -1 Then Return source
            Dim result = source.Remove(position, searchText.Length).Insert(position, replace)
            Return result
        End Function

        ''' <summary>
        ''' Get all indexes of a value in string
        ''' </summary>
        ''' <param name="target">Source string</param>
        ''' <param name="value">A value</param>
        ''' <returns>Indexes of strings</returns>
        <Extension()>
        Public Iterator Function AllIndexesOf(target As String, value As String) As IEnumerable(Of Integer)
            If target Is Nothing Then Throw New Exception(DoTranslation("The target may not be null"))
            If String.IsNullOrEmpty(value) Then Throw New Exception(DoTranslation("Empty string value specified"))

            Dim index = 0
            While True
                index = target.IndexOf(value, index)
                If index = -1 Then Exit While
                Yield index
                index += value.Length
            End While
        End Function

        ''' <summary>
        ''' Replaces last occurrence of a text in source string with the replacement
        ''' </summary>
        ''' <param name="source">A string which has the specified text to replace</param>
        ''' <returns>String that has its last occurrence of text replaced</returns>
        <Extension()>
        Public Function Repeat(source As String, times As Integer) As String
            Dim result As String = ""
            For time As Integer = 1 To times
                result += source
            Next
            Return result
        End Function

        ''' <summary>
        ''' Formats the string
        ''' </summary>
        ''' <param name="Format">The string to format</param>
        ''' <param name="Vars">The variables used</param>
        ''' <returns>A formatted string if successful, or the unformatted one if failed.</returns>
        <Extension()>
        Public Function FormatString(Format As String, ParamArray Vars As Object()) As String
            If Format Is Nothing Then Throw New Exception(DoTranslation("The target format may not be null"))

            Dim FormattedString = Format
            Try
                If Vars.Length > 0 Then FormattedString = String.Format(Format, Vars)
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to format string: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return FormattedString
        End Function

        ''' <summary>
        ''' Is the string numeric?
        ''' </summary>
        ''' <param name="Expression">The expression</param>
        Public Function IsStringNumeric(Expression As String) As Boolean
            If Expression Is Nothing Then Throw New Exception(DoTranslation("The target expression may not be null"))

            Dim __ As Double = Nothing
            Return Double.TryParse(Expression, __)
        End Function
    End Module
End Namespace

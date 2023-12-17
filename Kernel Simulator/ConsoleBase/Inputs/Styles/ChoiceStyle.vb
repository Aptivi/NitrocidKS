
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

Imports Terminaux.Inputs.Styles.Choice
Imports TermChoiceStyle = Terminaux.Inputs.Styles.Choice.ChoiceStyle

Namespace ConsoleBase.Inputs.Styles
    Public Module ChoiceStyle

        Public DefaultChoiceOutputType As ChoiceOutputType = ChoiceOutputType.Modern

        ''' <summary>
        ''' Prompts user for choice
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="OutputType">Output type of choices</param>
        ''' <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        Public Function PromptChoice(Question As String, AnswersStr As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False) As String
            Return PromptChoice(Question, AnswersStr, Array.Empty(Of String)(), OutputType, PressEnter)
        End Function

        ''' <summary>
        ''' Prompts user for choice
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        ''' <param name="OutputType">Output type of choices</param>
        ''' <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        Public Function PromptChoice(Question As String, AnswersStr As String, AnswersTitles() As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False) As String
            Return TermChoiceStyle.PromptChoice(Question, AnswersStr, AnswersTitles, OutputType, PressEnter)
        End Function

    End Module
End Namespace

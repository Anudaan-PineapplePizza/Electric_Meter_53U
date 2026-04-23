Option Strict On
Option Explicit On

Module Class_ComboBox

    'Link une ComboBox TEXT avec une ENUM pour eviter les (select / case)
    Public Class ComboItem(Of T)
        Public Property Text As String
        Public Property Value As T
        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class

End Module

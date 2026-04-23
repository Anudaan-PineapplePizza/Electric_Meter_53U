Option Strict Off
Option Explicit On

Imports System.IO.Ports
Imports System.Management
Imports System.Text.RegularExpressions
Imports EasyModbus

'=========================================================================
' READ COM PORT LIST (WINDOWS)
'=========================================================================

'=========================================================================
' COM PORT INFO CLASS 
'=========================================================================
Public Class ComPortInfo
    Public Property Port As String
    Public Property Caption As String

    Public Sub New(port As String, caption As String)
        Me.Port = port
        Me.Caption = caption
    End Sub

    Public Overrides Function ToString() As String
        Dim friendly As String = Caption
        If Not String.IsNullOrEmpty(friendly) Then
            friendly = friendly.Replace("(" & Port & ")", "").Trim()
            Return $"{Port}  -  {friendly}"
        End If
        Return Port
    End Function

End Class

Public Module ComPortMaster

    '=========================================================================
    ' CREATE FULL NAME COM PORT
    '=========================================================================
    Public Function GetFriendlyNamesByPort() As Dictionary(Of String, String)
        Dim map As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Try
            Using searcher As New ManagementObjectSearcher("SELECT Name FROM Win32_PnPEntity WHERE Name LIKE '%(COM%'")
                For Each obj As ManagementObject In searcher.Get()
                    Dim name As String = TryCast(obj("Name"), String)
                    If String.IsNullOrEmpty(name) Then Continue For

                    Dim m As Match = Regex.Match(name, "\((COM\d+)\)", RegexOptions.IgnoreCase)
                    If m.Success Then
                        Dim port As String = m.Groups(1).Value.ToUpperInvariant()
                        map(port) = name
                    End If
                Next
            End Using
        Catch
        End Try

        Return map
    End Function

    'Si plusieurs ports COM : 
    Public Function GetComPortInfoList() As List(Of ComPortInfo)
        Dim list As New List(Of ComPortInfo)()
        Dim friendly = GetFriendlyNamesByPort()
        Dim ports = SerialPort.GetPortNames()

        For Each p In ports
            Dim caption As String = Nothing
            friendly.TryGetValue(p, caption)
            list.Add(New ComPortInfo(p, If(caption, p)))
        Next

        list.Sort(Function(a, b)
                      Dim na As Integer = 0
                      Dim nb As Integer = 0
                      Integer.TryParse(Regex.Replace(a.Port, "[^\d]", ""), na)
                      Integer.TryParse(Regex.Replace(b.Port, "[^\d]", ""), nb)
                      Return na.CompareTo(nb)
                  End Function)

        Return list
    End Function

    '=========================================================================
    ' REFRESH COM PORT
    '=========================================================================
    Public Sub RefreshComPorts()
        ConnectionWindow.ComboBox_COMPort.Items.Clear()

        Dim items = GetComPortInfoList()

        For Each info In items
            ConnectionWindow.ComboBox_COMPort.Items.Add(info)
        Next

        If ConnectionWindow.ComboBox_COMPort.Items.Count > 0 Then   'Auto Select index 0
            ConnectionWindow.ComboBox_COMPort.SelectedIndex = 0
        End If
    End Sub

End Module
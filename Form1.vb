Imports System.Net.NetworkInformation
Imports System.Threading

' From Bing AI

Public Class Form1
    Private threads As New List(Of Thread)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        TextBox2.Clear()

        Dim ipList As String() = TextBox1.Lines

        For Each ip In ipList
            Dim t As New Thread(AddressOf PingAndReport)
            t.Start(ip)
            threads.Add(t)
        Next

        Dim monitorThread As New Thread(AddressOf MonitorThreads)
        monitorThread.Start()
    End Sub

    Sub PingAndReport(ip As Object)
        Dim pinger As New Ping()
        Dim result As PingReply = pinger.Send(ip.ToString())

        If result.Status = IPStatus.Success Then
            AppendText(ip.ToString())
        End If
    End Sub

    Delegate Sub AppendTextCallback([text] As String)

    Private Sub AppendText([text] As String)
        If TextBox2.InvokeRequired Then
            Dim d As New AppendTextCallback(AddressOf AppendText)
            Me.Invoke(d, New Object() {[text]})
        Else
            TextBox2.AppendText([text] & Environment.NewLine)
        End If
    End Sub

    Sub MonitorThreads()
        For Each t In threads
            t.Join()
        Next

        Button1.Enabled = True
        MessageBox.Show("Finished pinging all IPs.", Application.ProductName)
    End Sub
End Class
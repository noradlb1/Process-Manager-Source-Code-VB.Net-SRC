
Imports System.Diagnostics
Imports System
Imports System.Runtime.InteropServices



Public Class FrmMain
    Private t_PerformanceCounter As New PerformanceCounter("Processor", "% Processor Time", "_Total")
    Dim myCounter As System.Diagnostics.PerformanceCounter = New System.Diagnostics.PerformanceCounter()
    Private Declare Function OpenProcess Lib "kernel32" Alias "OpenProcess" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal dwProcessId As Integer) As Integer
    Private Declare Function CloseHandle Lib "kernel32" Alias "CloseHandle" (ByVal hObject As Integer) As Integer
    Private Const PROCESS_TERMINATE = &H1
    Private Const SYNCHRONIZE = &H100000
    Private Const PROCESS_ALL_ACCESS = &H1F0FFF&

    Private Declare Function NtTerminateProcess Lib "ntdll.dll" (ByVal ProcessHandle As Integer, ByVal Exitstatus As Integer) As Integer
#Region "Start"
    Sub Myprocess()
        Timer1.Enabled = True
        Dim Process As New Process()
        Dim Count As Integer = 0
        ListView1.Items.Clear()
        For Each Myproc In Process.GetProcesses(My.Computer.Name)

            On Error Resume Next

            With ListView1
                .Items.Add(Myproc.ProcessName)
                .Items(Count).SubItems.Add(Myproc.MainModule.FileName.ToString)
                .Items(Count).SubItems.Add(FormatNumber(Math.Round(Myproc.PrivateMemorySize64 / 1024), 0))
                .Items(Count).SubItems.Add(Myproc.Id)
                .Items(Count).SubItems.Add(Myproc.Handle)
                .Items(Count).SubItems.Add(Myproc.StartTime)
                Count += 1
            End With
        Next
        ToolStripStatusLabel3.Text = "Computer Name : " & My.Computer.Name
        ToolStripStatusLabel1.Text = "|" & " Current Processes : " & ListView1.Items.Count & " "
    End Sub
#End Region



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Timer3.Start()

        Call Myprocess()

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim type As String = CInt(t_PerformanceCounter.NextValue()) & "%"
        Dim Count As Integer = 4
        ToolStripStatusLabel2.Text = "|" & " CPU Usage : " & type.ToString() & " "
        Threading.Thread.Sleep(20)

    End Sub

    Private Sub ListView1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseUp
        If e.Button = MouseButtons.Right Then
            Me.ContextMenuStrip1.Show(ListView1, e.Location)
        End If

    End Sub


    Private Sub NoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NoToolStripMenuItem.Click
        For Each Myproc As ListViewItem In ListView1.SelectedItems

            Dim x As String = MsgBox("Are you sure you want to kill : " & ListView1.SelectedItems(0).Text, vbOKCancel, "Kill process")
            If x = vbOK Then
                Try
                    Process.GetProcessById(Myproc.SubItems(3).Text).Kill()
                    Myproc.SubItems(3).Text.Remove(0)
                    Call Myprocess()
                Catch ex As Exception
                    MessageBox.Show("You can't kill this process..", "Kill process")
                    Exit Sub
                End Try
            Else
                Exit Sub
            End If
        Next
    End Sub

    Private Sub Form2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Form2ToolStripMenuItem.Click
        For Each items As ListViewItem In ListView1.SelectedItems

            Process.Start("http://www.google.com/search?q=" & items.SubItems(0).Text)
        Next

    End Sub

    Private Sub SelectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectToolStripMenuItem.Click
        Call Myprocess()
    End Sub

    Private Sub RestartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestartToolStripMenuItem.Click
        On Error Resume Next
        For Each MyProc As ListViewItem In ListView1.SelectedItems
            Dim protxt As New TextBox
            protxt.Text = MyProc.SubItems(1).Text
            Process.GetProcessById(MyProc.SubItems(3).Text).Kill()
            Process.Start(protxt.Text)

        Next

        Call Myprocess()
    End Sub




    Private Sub SearchOnlineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchOnlineToolStripMenuItem.Click
        Form2ToolStripMenuItem.PerformClick()
    End Sub

    Private Sub SearchOnlineToolStripMenuItem_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchOnlineToolStripMenuItem.MouseEnter

    End Sub

    Private Sub KillProcessToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KillProcessToolStripMenuItem.Click
        NoToolStripMenuItem.PerformClick()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        SelectToolStripMenuItem.PerformClick()
    End Sub

    Private Sub RestartProcessToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestartProcessToolStripMenuItem.Click
        RestartToolStripMenuItem.PerformClick()
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        Form2ToolStripMenuItem1.PerformClick()
    End Sub

    Private Sub CdcdToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CdcdToolStripMenuItem.Click


        If MessageBox.Show("Are You Sure You Want to Exit..", "Exit", MessageBoxButtons.OKCancel, _
                    Nothing, MessageBoxDefaultButton.Button1) = DialogResult.OK Then

            Me.Close()
        Else
            Exit Sub
        End If


    End Sub

    Private Sub SoftwareInstallToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles About.Click
        MessageBox.Show("All Rights Reserved : Dev-PoinT TeaM " & vbCrLf & "Copyright © 2012-2013 " & vbCrLf & "‏MAӇMƠƲƊ.10" & vbCrLf & "AnTiviRus7" & vbCrLf & "http://www.dev-point.com/vb", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        If ListView1.SelectedItems.Count <> Nothing Then

            myCounter.CategoryName = "Processor"
            myCounter.CounterName = "% Processor Time"
            myCounter.InstanceName = "_Total"
            Label1.Text = "CPU Usage : " & myCounter.NextValue.ToString()
            Label2.Text = ToolStripStatusLabel3.Text

        End If

        For Each Process As ListViewItem In ListView1.SelectedItems
            Try
                Label3.Text = "Process : " & Process.SubItems(0).Text
                Label4.Text = "Path    : " & Process.SubItems(1).Text
                Label5.Text = "Memory  : " & Process.SubItems(2).Text
                Label6.Text = "ID      : " & Process.SubItems(3).Text
                Label7.Text = "Start Time : " & Process.SubItems(5).Text
                If Process.SubItems(3).Text = Nothing And Process.SubItems(5).Text = Nothing Then
                    Label6.Text = "ID      : Not Aviable "
                    Label7.Text = "Start Time   : Not Aviable "

                End If

                Dim ico As Icon = Icon.ExtractAssociatedIcon(Process.SubItems(1).Text)

                PictureBox1.Image = ico.ToBitmap



            Catch ex As Exception
                Label6.Text = "ID      : Nothing"
                Label7.Text = "Start Time : Nothing"

            End Try

        Next

    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Timer2.Start()

    End Sub
#Region "COPY"
    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

        If CheckBox1.Checked = True Then
            My.Computer.Clipboard.SetText(Label1.Text)
            MessageBox.Show("Copied...!", "OK..:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

        End If


    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        If CheckBox1.Checked = True Then
            My.Computer.Clipboard.SetText(Label2.Text)
            MessageBox.Show("Copied...!", "OK..:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

        End If
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        If CheckBox1.Checked = True Then
            My.Computer.Clipboard.SetText(Label3.Text)
            MessageBox.Show("Copied...!", "OK..:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

        End If
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click
        If CheckBox1.Checked = True Then
            My.Computer.Clipboard.SetText(Label4.Text)
            MessageBox.Show("Copied...!", "OK..:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

        End If
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click
        If CheckBox1.Checked = True Then
            My.Computer.Clipboard.SetText(Label5.Text)
            MessageBox.Show("Copied...!", "OK..:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

        End If
    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click
        If CheckBox1.Checked = True Then
            My.Computer.Clipboard.SetText(Label6.Text)
            MessageBox.Show("Copied...!", "OK..:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

        End If
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
        If CheckBox1.Checked = True Then
            My.Computer.Clipboard.SetText(Label7.Text)
            MessageBox.Show("Copied...!", "OK..:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

        End If
    End Sub
#End Region
   

    Private Sub TerminateProcessToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TerminateProcessToolStripMenuItem.Click
        Dim pid As Integer = CInt(ListView1.SelectedItems(0).SubItems(3).Text)
        Dim target_process_id, target_process_handle As Integer

        target_process_id = pid

        ' Open the process.
        target_process_handle = OpenProcess( _
            SYNCHRONIZE Or PROCESS_TERMINATE Or PROCESS_ALL_ACCESS, _
            0, target_process_id)

        If (target_process_handle = 0) Then
            MessageBox.Show("Error finding target process " & _
                "handle")
            Exit Sub
        End If

        ' Terminate the process.
        If (NtTerminateProcess(target_process_handle, 0) = 0) Then

            ''''''' WHEN U USE NT if The Result = 0 THEN RESULT IS STATUS_SUCCESS

            MessageBox.Show("Process terminated Successfuly", "OK..", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Error terminating process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        ' Close the process.
        CloseHandle(target_process_handle)
    End Sub


    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Dim Process As New Process()
        Dim Count As Integer = 0
        ListView2.Items.Clear()
        For Each Myproc In Process.GetProcesses(My.Computer.Name)

            On Error Resume Next

            With ListView2
                .Items.Add(Myproc.ProcessName)
                .Items(Count).SubItems.Add(Myproc.MainModule.FileName.ToString)
                .Items(Count).SubItems.Add(FormatNumber(Math.Round(Myproc.PrivateMemorySize64 / 1024), 0))
                .Items(Count).SubItems.Add(Myproc.Id)
                .Items(Count).SubItems.Add(Myproc.Handle)
                .Items(Count).SubItems.Add(Myproc.StartTime)
                Count += 1
            End With
        Next
        ToolStripStatusLabel3.Text = "Computer Name : " & My.Computer.Name
        ToolStripStatusLabel1.Text = "|" & " Current Processes : " & ListView2.Items.Count & " "
        Timer4.Start()
        Timer3.Stop()
        If ListView1.Items.Count = ListView2.Items.Count Then

        Else
            Myprocess()

        End If
    End Sub

    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        Dim Process As New Process()
        Dim Count As Integer = 0
        ListView2.Items.Clear()
        For Each Myproc In Process.GetProcesses(My.Computer.Name)

            On Error Resume Next

            With ListView2
                .Items.Add(Myproc.ProcessName)
                .Items(Count).SubItems.Add(Myproc.MainModule.FileName.ToString)
                .Items(Count).SubItems.Add(FormatNumber(Math.Round(Myproc.PrivateMemorySize64 / 1024), 0))
                .Items(Count).SubItems.Add(Myproc.Id)
                .Items(Count).SubItems.Add(Myproc.Handle)
                .Items(Count).SubItems.Add(Myproc.StartTime)
                Count += 1
            End With
        Next
        ToolStripStatusLabel3.Text = "Computer Name : " & My.Computer.Name
        ToolStripStatusLabel1.Text = "|" & " Current Processes : " & ListView2.Items.Count & " "
        Timer3.Start()
        Timer4.Stop()
        If ListView1.Items.Count = ListView2.Items.Count Then

        Else
            Myprocess()

        End If
    End Sub
End Class

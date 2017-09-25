Imports System.IO
Public Class NG_FORM


    Private Sub NG_FORM_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

        Control.Show()
        'Control.TextSerial.Clear()
    End Sub

    Private Sub NG_FORM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GroupBox3.Visible = False
    End Sub


End Class

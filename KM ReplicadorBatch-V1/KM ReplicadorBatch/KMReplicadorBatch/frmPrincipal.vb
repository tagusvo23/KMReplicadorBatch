Imports System.Data.OracleClient
Imports Microsoft.Win32

Public Class frmPrincipal
    Private Sub ParámetrosGeneralesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ParámetrosGeneralesToolStripMenuItem.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Refresh()
        frmConfig.Show()
        Me.Cursor = Cursors.Default
        Me.Refresh()
    End Sub
    Private Sub frmPrincipal_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Terminar()
    End Sub
    Private Sub AsignacionesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        Me.Refresh()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub SalirToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SalirToolStripMenuItem.Click
        End
        Application.Exit()
    End Sub
    Private Sub TablaTTToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TablaTTToolStripMenuItem.Click
        CargaLayout()
        frmProcesaTT.ShowDialog()
    End Sub
End Class

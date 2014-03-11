<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrincipal
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mnuPrincipal = New System.Windows.Forms.MenuStrip()
        Me.ArchivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProcesosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TablaTTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ParametrosGeneralesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ParámetrosGeneralesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AyudaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.barEstatus = New System.Windows.Forms.StatusStrip()
        Me.Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Panel2 = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DatosDelClienteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AsignaciónDeFiltrosPorClienteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPrincipal.SuspendLayout()
        Me.barEstatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuPrincipal
        '
        Me.mnuPrincipal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuPrincipal.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem, Me.ProcesosToolStripMenuItem, Me.ParametrosGeneralesToolStripMenuItem, Me.AyudaToolStripMenuItem})
        Me.mnuPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.mnuPrincipal.Name = "mnuPrincipal"
        Me.mnuPrincipal.Size = New System.Drawing.Size(1134, 24)
        Me.mnuPrincipal.TabIndex = 0
        Me.mnuPrincipal.Text = "MenuStrip1"
        '
        'ArchivoToolStripMenuItem
        '
        Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SalirToolStripMenuItem})
        Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
        Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(58, 20)
        Me.ArchivoToolStripMenuItem.Text = "Archivo"
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
        Me.SalirToolStripMenuItem.Text = "Salir"
        '
        'ProcesosToolStripMenuItem
        '
        Me.ProcesosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TablaTTToolStripMenuItem})
        Me.ProcesosToolStripMenuItem.Name = "ProcesosToolStripMenuItem"
        Me.ProcesosToolStripMenuItem.Size = New System.Drawing.Size(70, 20)
        Me.ProcesosToolStripMenuItem.Text = "Procesos"
        '
        'TablaTTToolStripMenuItem
        '
        Me.TablaTTToolStripMenuItem.Name = "TablaTTToolStripMenuItem"
        Me.TablaTTToolStripMenuItem.Size = New System.Drawing.Size(230, 22)
        Me.TablaTTToolStripMenuItem.Text = "Replica Tabla Transaccional"
        '
        'ParametrosGeneralesToolStripMenuItem
        '
        Me.ParametrosGeneralesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ParámetrosGeneralesToolStripMenuItem})
        Me.ParametrosGeneralesToolStripMenuItem.Name = "ParametrosGeneralesToolStripMenuItem"
        Me.ParametrosGeneralesToolStripMenuItem.Size = New System.Drawing.Size(143, 20)
        Me.ParametrosGeneralesToolStripMenuItem.Text = "Parámetros Generales"
        '
        'ParámetrosGeneralesToolStripMenuItem
        '
        Me.ParámetrosGeneralesToolStripMenuItem.Name = "ParámetrosGeneralesToolStripMenuItem"
        Me.ParámetrosGeneralesToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.ParámetrosGeneralesToolStripMenuItem.Text = "Parámetros Generales"
        '
        'AyudaToolStripMenuItem
        '
        Me.AyudaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator2})
        Me.AyudaToolStripMenuItem.Name = "AyudaToolStripMenuItem"
        Me.AyudaToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.AyudaToolStripMenuItem.Text = "Ayuda"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(57, 6)
        '
        'barEstatus
        '
        Me.barEstatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Panel1, Me.Panel2})
        Me.barEstatus.Location = New System.Drawing.Point(0, 592)
        Me.barEstatus.Name = "barEstatus"
        Me.barEstatus.Size = New System.Drawing.Size(1134, 22)
        Me.barEstatus.TabIndex = 1
        Me.barEstatus.Text = "StatusStrip1"
        '
        'Panel1
        '
        Me.Panel1.AutoSize = False
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(400, 17)
        Me.Panel1.Text = "Listo   |"
        Me.Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(100, 16)
        Me.Panel2.Visible = False
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(245, 22)
        Me.ToolStripMenuItem2.Text = "Catalogo de Clientes"
        '
        'DatosDelClienteToolStripMenuItem
        '
        Me.DatosDelClienteToolStripMenuItem.Name = "DatosDelClienteToolStripMenuItem"
        Me.DatosDelClienteToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.DatosDelClienteToolStripMenuItem.Text = "Asignaciòn de Subformatos Clientes"
        '
        'AsignaciónDeFiltrosPorClienteToolStripMenuItem
        '
        Me.AsignaciónDeFiltrosPorClienteToolStripMenuItem.Name = "AsignaciónDeFiltrosPorClienteToolStripMenuItem"
        Me.AsignaciónDeFiltrosPorClienteToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.AsignaciónDeFiltrosPorClienteToolStripMenuItem.Text = "Asignación de Filtros Por Cliente"
        '
        'frmPrincipal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1134, 614)
        Me.Controls.Add(Me.barEstatus)
        Me.Controls.Add(Me.mnuPrincipal)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.mnuPrincipal
        Me.MinimumSize = New System.Drawing.Size(1100, 650)
        Me.Name = "frmPrincipal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "KM Replicador Batch"
        Me.mnuPrincipal.ResumeLayout(False)
        Me.mnuPrincipal.PerformLayout()
        Me.barEstatus.ResumeLayout(False)
        Me.barEstatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnuPrincipal As System.Windows.Forms.MenuStrip
    Friend WithEvents barEstatus As System.Windows.Forms.StatusStrip
    Friend WithEvents Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents AyudaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DatosDelClienteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AsignaciónDeFiltrosPorClienteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ParametrosGeneralesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ParámetrosGeneralesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel2 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ArchivoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SalirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProcesosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TablaTTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class

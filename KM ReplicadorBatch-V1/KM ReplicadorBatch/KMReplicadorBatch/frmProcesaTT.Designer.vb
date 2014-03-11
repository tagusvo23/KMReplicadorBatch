<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProcesaTT
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProcesaTT))
        Me.dlgArchivo = New System.Windows.Forms.OpenFileDialog()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CboLayout = New System.Windows.Forms.ComboBox()
        Me.CboFormatos = New System.Windows.Forms.ComboBox()
        Me.CboTipoRegistro = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtTotalLeidos = New System.Windows.Forms.TextBox()
        Me.TxtTotalProcesados = New System.Windows.Forms.TextBox()
        Me.TxtTotalErroneos = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtTotalEnBlanco = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TxtArchivoSeleccionado = New System.Windows.Forms.TextBox()
        Me.PanelProcesaTT = New System.Windows.Forms.Panel()
        Me.PicBoxEjecutar = New System.Windows.Forms.PictureBox()
        Me.PicBoxCargaArchivos = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.PanelProcesaTT.SuspendLayout()
        CType(Me.PicBoxEjecutar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicBoxCargaArchivos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(18, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(223, 18)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Layout"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(259, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(223, 18)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "Formato"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(497, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(223, 18)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Tipo de Registro"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CboLayout
        '
        Me.CboLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboLayout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CboLayout.FormattingEnabled = True
        Me.CboLayout.Location = New System.Drawing.Point(18, 55)
        Me.CboLayout.Name = "CboLayout"
        Me.CboLayout.Size = New System.Drawing.Size(223, 21)
        Me.CboLayout.TabIndex = 0
        '
        'CboFormatos
        '
        Me.CboFormatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboFormatos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CboFormatos.FormattingEnabled = True
        Me.CboFormatos.Location = New System.Drawing.Point(259, 55)
        Me.CboFormatos.Name = "CboFormatos"
        Me.CboFormatos.Size = New System.Drawing.Size(223, 21)
        Me.CboFormatos.TabIndex = 1
        '
        'CboTipoRegistro
        '
        Me.CboTipoRegistro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboTipoRegistro.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CboTipoRegistro.FormattingEnabled = True
        Me.CboTipoRegistro.Items.AddRange(New Object() {"1.- Cheques", "2.- Tarjetas"})
        Me.CboTipoRegistro.Location = New System.Drawing.Point(497, 55)
        Me.CboTipoRegistro.Name = "CboTipoRegistro"
        Me.CboTipoRegistro.Size = New System.Drawing.Size(223, 21)
        Me.CboTipoRegistro.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(254, 230)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(186, 20)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "T o t a l e s"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(251, 264)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 18)
        Me.Label5.TabIndex = 31
        Me.Label5.Text = "Leidos :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(251, 299)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 18)
        Me.Label6.TabIndex = 32
        Me.Label6.Text = "Procesados :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(251, 334)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 18)
        Me.Label7.TabIndex = 33
        Me.Label7.Text = "Con Error :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtTotalLeidos
        '
        Me.TxtTotalLeidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTotalLeidos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTotalLeidos.Location = New System.Drawing.Point(340, 264)
        Me.TxtTotalLeidos.Name = "TxtTotalLeidos"
        Me.TxtTotalLeidos.ReadOnly = True
        Me.TxtTotalLeidos.Size = New System.Drawing.Size(100, 21)
        Me.TxtTotalLeidos.TabIndex = 60
        '
        'TxtTotalProcesados
        '
        Me.TxtTotalProcesados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTotalProcesados.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTotalProcesados.Location = New System.Drawing.Point(340, 299)
        Me.TxtTotalProcesados.Name = "TxtTotalProcesados"
        Me.TxtTotalProcesados.ReadOnly = True
        Me.TxtTotalProcesados.Size = New System.Drawing.Size(100, 21)
        Me.TxtTotalProcesados.TabIndex = 70
        '
        'TxtTotalErroneos
        '
        Me.TxtTotalErroneos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTotalErroneos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTotalErroneos.Location = New System.Drawing.Point(340, 334)
        Me.TxtTotalErroneos.Name = "TxtTotalErroneos"
        Me.TxtTotalErroneos.ReadOnly = True
        Me.TxtTotalErroneos.Size = New System.Drawing.Size(100, 21)
        Me.TxtTotalErroneos.TabIndex = 80
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(251, 369)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 18)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "En Blanco :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtTotalEnBlanco
        '
        Me.TxtTotalEnBlanco.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTotalEnBlanco.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTotalEnBlanco.Location = New System.Drawing.Point(340, 369)
        Me.TxtTotalEnBlanco.Name = "TxtTotalEnBlanco"
        Me.TxtTotalEnBlanco.ReadOnly = True
        Me.TxtTotalEnBlanco.Size = New System.Drawing.Size(100, 21)
        Me.TxtTotalEnBlanco.TabIndex = 90
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(276, 93)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(196, 18)
        Me.Label9.TabIndex = 39
        Me.Label9.Text = "Archivo de Texto a Procesar"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TxtArchivoSeleccionado
        '
        Me.TxtArchivoSeleccionado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtArchivoSeleccionado.Location = New System.Drawing.Point(100, 115)
        Me.TxtArchivoSeleccionado.Name = "TxtArchivoSeleccionado"
        Me.TxtArchivoSeleccionado.ReadOnly = True
        Me.TxtArchivoSeleccionado.Size = New System.Drawing.Size(542, 20)
        Me.TxtArchivoSeleccionado.TabIndex = 5
        '
        'PanelProcesaTT
        '
        Me.PanelProcesaTT.BackColor = System.Drawing.Color.LightGray
        Me.PanelProcesaTT.Controls.Add(Me.BtnSalir)
        Me.PanelProcesaTT.Controls.Add(Me.Label10)
        Me.PanelProcesaTT.Controls.Add(Me.PicBoxEjecutar)
        Me.PanelProcesaTT.Controls.Add(Me.PicBoxCargaArchivos)
        Me.PanelProcesaTT.Controls.Add(Me.Label1)
        Me.PanelProcesaTT.Controls.Add(Me.TxtArchivoSeleccionado)
        Me.PanelProcesaTT.Controls.Add(Me.CboLayout)
        Me.PanelProcesaTT.Controls.Add(Me.Label9)
        Me.PanelProcesaTT.Controls.Add(Me.Label2)
        Me.PanelProcesaTT.Controls.Add(Me.CboFormatos)
        Me.PanelProcesaTT.Controls.Add(Me.Label3)
        Me.PanelProcesaTT.Controls.Add(Me.CboTipoRegistro)
        Me.PanelProcesaTT.Location = New System.Drawing.Point(-1, 1)
        Me.PanelProcesaTT.Name = "PanelProcesaTT"
        Me.PanelProcesaTT.Size = New System.Drawing.Size(745, 219)
        Me.PanelProcesaTT.TabIndex = 0
        '
        'PicBoxEjecutar
        '
        Me.PicBoxEjecutar.BackColor = System.Drawing.Color.Silver
        Me.PicBoxEjecutar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PicBoxEjecutar.Image = CType(resources.GetObject("PicBoxEjecutar.Image"), System.Drawing.Image)
        Me.PicBoxEjecutar.Location = New System.Drawing.Point(497, 150)
        Me.PicBoxEjecutar.Name = "PicBoxEjecutar"
        Me.PicBoxEjecutar.Size = New System.Drawing.Size(50, 50)
        Me.PicBoxEjecutar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PicBoxEjecutar.TabIndex = 41
        Me.PicBoxEjecutar.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PicBoxEjecutar, "Ejecutar Proceso")
        '
        'PicBoxCargaArchivos
        '
        Me.PicBoxCargaArchivos.BackColor = System.Drawing.Color.DarkGray
        Me.PicBoxCargaArchivos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PicBoxCargaArchivos.Image = CType(resources.GetObject("PicBoxCargaArchivos.Image"), System.Drawing.Image)
        Me.PicBoxCargaArchivos.Location = New System.Drawing.Point(181, 150)
        Me.PicBoxCargaArchivos.Name = "PicBoxCargaArchivos"
        Me.PicBoxCargaArchivos.Size = New System.Drawing.Size(50, 50)
        Me.PicBoxCargaArchivos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicBoxCargaArchivos.TabIndex = 40
        Me.PicBoxCargaArchivos.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PicBoxCargaArchivos, "Seleccionar Archivo TXT")
        '
        'ProgressBar1
        '
        Me.ProgressBar1.BackColor = System.Drawing.Color.Gainsboro
        Me.ProgressBar1.ForeColor = System.Drawing.Color.LightSteelBlue
        Me.ProgressBar1.Location = New System.Drawing.Point(155, 418)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(428, 23)
        Me.ProgressBar1.TabIndex = 91
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(-1, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(745, 20)
        Me.Label10.TabIndex = 42
        Me.Label10.Text = "Replica Transaccional"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnSalir
        '
        Me.BtnSalir.BackColor = System.Drawing.Color.Silver
        Me.BtnSalir.Location = New System.Drawing.Point(318, 166)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(100, 23)
        Me.BtnSalir.TabIndex = 92
        Me.BtnSalir.Text = "Salir"
        Me.BtnSalir.UseVisualStyleBackColor = False
        '
        'frmProcesaTT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(743, 463)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.PanelProcesaTT)
        Me.Controls.Add(Me.TxtTotalEnBlanco)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TxtTotalErroneos)
        Me.Controls.Add(Me.TxtTotalProcesados)
        Me.Controls.Add(Me.TxtTotalLeidos)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.ForeColor = System.Drawing.Color.DarkBlue
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProcesaTT"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.PanelProcesaTT.ResumeLayout(False)
        Me.PanelProcesaTT.PerformLayout()
        CType(Me.PicBoxEjecutar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicBoxCargaArchivos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dlgArchivo As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CboLayout As System.Windows.Forms.ComboBox
    Friend WithEvents CboFormatos As System.Windows.Forms.ComboBox
    Friend WithEvents CboTipoRegistro As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TxtTotalLeidos As System.Windows.Forms.TextBox
    Friend WithEvents TxtTotalProcesados As System.Windows.Forms.TextBox
    Friend WithEvents TxtTotalErroneos As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtTotalEnBlanco As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TxtArchivoSeleccionado As System.Windows.Forms.TextBox
    Friend WithEvents PanelProcesaTT As System.Windows.Forms.Panel
    Friend WithEvents PicBoxEjecutar As System.Windows.Forms.PictureBox
    Friend WithEvents PicBoxCargaArchivos As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents BtnSalir As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class

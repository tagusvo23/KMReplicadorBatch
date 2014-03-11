Imports system.Data.Odbc
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Data.OracleClient
Imports System.Data.SqlClient
Imports System.Threading
Public Class frmConfig
    Dim WithEvents VMtcpCliente As Cliente_TCP
    Dim VMclsConfiguracion As ClsConfiguracion
    Private Sub frmConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If VGblnAutentif = True Then
            cmdReiniciar.Visible = True
            cmdSalir.Visible = True
        Else
            cmdReiniciar.Visible = True
            cmdReiniciar.Enabled = True
            cmdSalir.Visible = False
        End If

        Dim VLstrRuta As String = ValidaArchivoXML() & "\ConfiguracionesKM.xml"
        picRes4.Image = Nothing
        picRes3.Image = Nothing
        picRes2.Image = Nothing
        picRes1.Image = Nothing
        '--------------Configuracion lee autentifica
        Leer(VLstrRuta, VGstrNombreIDConfig)
        txtIP.Text = VGstrucAutentifica.Servidor
        txtPuerto.Text = VGstrucAutentifica.Puerto
        '--------------Configuracion lee central
        Try
            Select Case VGstrucBaseCentral.Tipo
                Case TipoBase.Oracle
                    cboTipoCentral.Text = "Oracle"
                Case TipoBase.MySQL
                    cboTipoCentral.Text = "MySQL"
                Case TipoBase.SQLServer
                    cboTipoCentral.Text = "SQL Server"
            End Select

            chkSICentral.Checked = VGstrucBaseCentral.Seguridad_Integrada

            txtServidorCentral.Text = VGstrucBaseCentral.Servidor
            txtPuertoCentral.Text = VGstrucBaseCentral.Puerto
            txtBaseCentral.Text = VGstrucBaseCentral.Base

            If Not (VGstrucBaseCentral.Tipo = TipoBase.SQLServer And chkSICentral.Checked) Then
                txtUsuarioCentral.Text = VGstrucBaseCentral.Usuario
                txtContrasenaCentral.Text = VGstrucBaseCentral.Contraseña
            End If
            txtAliasCentral.Text = VGstrucBaseCentral.Instancia

        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch-03001 --> : " & ex.Message)
            txtRes2.Text = txtRes2.Text & " Ocurrio un error al leer archivo .XML para obtener datos de la base de datos central" & ex.Message
        End Try

        '--------------Configuracion lee Transaccional
        Try
            Select Case VGstrucTransaccional.Tipo
                Case TipoBase.Oracle
                    cboTipoTransaccional.Text = "Oracle"
                Case TipoBase.MySQL
                    cboTipoTransaccional.Text = "MySQL"
                Case TipoBase.SQLServer
                    cboTipoTransaccional.Text = "SQL Server"
            End Select

            chkSITransaccional.Checked = VGstrucTransaccional.Seguridad_Integrada

            txtServidorTransaccional.Text = VGstrucTransaccional.Servidor
            txtPuertoTransaccional.Text = VGstrucTransaccional.Puerto
            txtBaseTransaccional.Text = VGstrucTransaccional.Base

            If Not (VGstrucTransaccional.Tipo = TipoBase.SQLServer And chkSITransaccional.Checked) Then
                txtUsuarioTransaccional.Text = VGstrucTransaccional.Usuario
                txtContrasenaTransaccional.Text = VGstrucTransaccional.Contraseña
            End If
            txtAliasTransaccional.Text = VGstrucTransaccional.Instancia

        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch-03003 --> : " & ex.Message)
            txtRes3.Text = txtRes3.Text & " Ocurrio un error al leer archivo .XML para obtener datos de la base de datos transaccional" & ex.Message
        End Try
        '--------------Configuracion lee Bitácora
        Try
            Select Case VGstrucBaseBitacora.Tipo
                Case TipoBase.Oracle
                    cboTipoBitacora.Text = "Oracle"
                Case TipoBase.MySQL
                    cboTipoBitacora.Text = "MySQL"
                Case TipoBase.SQLServer
                    cboTipoBitacora.Text = "SQL Server"
            End Select

            chkSIBitacora.Checked = VGstrucBaseBitacora.Seguridad_Integrada

            txtServidorBitacora.Text = VGstrucBaseBitacora.Servidor
            txtPuertoBitacora.Text = VGstrucBaseBitacora.Puerto
            txtBaseBitacora.Text = VGstrucBaseBitacora.Base

            If Not (VGstrucBaseBitacora.Tipo = TipoBase.SQLServer And chkSIBitacora.Checked) Then
                txtUsuarioBitacora.Text = VGstrucBaseBitacora.Usuario
                txtContrasenaBitacora.Text = VGstrucBaseBitacora.Contraseña
            End If
            txtAliasBitacora.Text = VGstrucBaseBitacora.Instancia

        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch-03005 --> : " & ex.Message)
            txtRes4.Text = txtRes4.Text & " Ocurrio un error al leer archivo .XML para obtener datos de la base de datos bitacora" & ex.Message
        End Try
        tabConfig.SelectedIndex = 0

    End Sub
    Private Sub cmdP1_Click(sender As System.Object, e As System.EventArgs) Handles cmdP1.Click
        cmdP1.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Me.Refresh()
        If Validar1() Then
            Me.Cursor = Cursors.WaitCursor
            txtRes1.Text = "Conectando con servidor Autenticación" & vbNewLine & _
                           "Servidor: " & txtIP.Text & vbNewLine & _
                           "Puerto: " & txtPuerto.Text
            txtRes1.Refresh()
            Try
                VMtcpCliente = New Cliente_TCP
                VMtcpCliente.Conectar(txtIP.Text, txtPuerto.Text)
                Do
                    If VMtcpCliente.Estado = Cliente_TCP.EstadoConexion.EnError Then
                        Exit Do
                    End If
                    Threading.Thread.Sleep(100)
                Loop Until VMtcpCliente.Conectado
                Threading.Thread.Sleep(100)

                If VMtcpCliente.Estado = Cliente_TCP.EstadoConexion.Conectado Then
                    picRes1.Image = imgImagenes.Images(0)
                    txtRes1.AppendText(vbNewLine & "Se conectó exitosamente al servidor autentifica")
                Else
                    picRes1.Image = imgImagenes.Images(1)
                    txtRes1.AppendText(vbNewLine & "Error al conectarse con el servidor autentifica. ")
                End If
                VMtcpCliente.Cerrar()

            Catch ex As Exception
                Me.picRes1.Image = imgImagenes.Images(1)
                VGobjDepuracion.ArchivoPlano("KMRBatch-03007 --> : " & ex.Message)
                txtRes1.Text = txtRes1.Text & " Ocurrio un error al intentarse conectar con el servidor de autentificación " & ex.Message
                VMtcpCliente.Cerrar()
            End Try

        End If
        cmdP1.Enabled = True
        Me.Cursor = Cursors.Default

    End Sub
    Private Sub cmdP2_Click(sender As System.Object, e As System.EventArgs) Handles cmdP2.Click
        'CENTRAL
        cmdP2.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        If Validar2() Then
            txtRes2.Text = "Probando conexión con base de datos Central..."
            txtRes2.Refresh()
            Select Case cboTipoCentral.Text
                Case "Oracle"
                    ConexionC()
                Case Else
                    If Trim(txtBaseCentral.Text) <> "" Then
                        ConexionC()
                    Else
                        txtRes2.Text = txtRes2.Text & vbNewLine & "Error en la conexión" & _
                         vbNewLine & "Descripción: " & " No se escribió el nombre de la base"
                        txtRes2.Refresh()
                        Me.picRes2.Image = imgImagenes.Images(1)
                    End If
            End Select
        End If

        cmdP2.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub cmdP3_Click(sender As System.Object, e As System.EventArgs) Handles cmdP3.Click
        '-----------------------
        'TRANSACCIONAL
        cmdP3.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        If Validar3() Then
            txtRes3.Text = "Probando conexión con base de datos Transaccional..."
            txtRes3.Refresh()
            Select Case cboTipoTransaccional.Text
                Case "Oracle"
                    ConexionT()
                Case Else
                    If Trim(txtBaseTransaccional.Text) <> "" Then
                        ConexionT()
                    Else
                        txtRes3.Text = txtRes3.Text & vbNewLine & "Error en la conexión" & _
                        vbNewLine & "Descripción: " & " No se escribió el nombre de la base"
                        txtRes3.Refresh()
                        Me.picRes3.Image = imgImagenes.Images(1)
                    End If
            End Select
        End If
        cmdP3.Enabled = True
        Me.Cursor = Cursors.Default

    End Sub
    Private Sub cmdP4_Click(sender As System.Object, e As System.EventArgs) Handles cmdP4.Click
        'BITACORA
        cmdP4.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        If Validar4() Then
            txtRes4.Text = "Probando conexión con base de datos Bitácora..."
            txtRes4.Refresh()
            Select Case cboTipoBitacora.Text
                Case "Oracle"
                    ConexionB()
                Case Else
                    If Trim(txtBaseBitacora.Text) <> "" Then
                        ConexionB()
                    Else
                        txtRes4.Text = txtRes4.Text & vbNewLine & "Error en la conexión" & _
                        vbNewLine & "Descripción: " & " No se escribió el nombre de la base"
                        txtRes4.Refresh()
                        Me.picRes4.Image = imgImagenes.Images(1)
                    End If
            End Select
        End If
        cmdP4.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub cmdG1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdG1.Click
        Dim VLstrRuta As String = ValidaArchivoXML() & "\ConfiguracionesKM.xml"

        If Validar1() Then
            cmdG1.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            '--------------Configuracion guarda datos de la pestaña de (GENERAL)
            VMclsConfiguracion = Nothing
            VMclsConfiguracion = New ClsConfiguracion

            VMclsConfiguracion.Configuracion = VGstrNombreIDConfig
            VMclsConfiguracion.ArchivoXML = VLstrRuta
            VMclsConfiguracion.RutaNodo = "/ConfiguracionesKM/Configuracion"
            VMclsConfiguracion.TipoResultado = ClsConfiguracion.TipoConfiguracion.ServidorAutentifica

            VGstrucAutentifica.Servidor = Me.txtIP.Text
            VGstrucAutentifica.Puerto = txtPuerto.Text

            If VMclsConfiguracion.GuardaServidor(VGstrucAutentifica) Then
                guardaBitacora("Cambio en la configuración general ", 7)
                MsgBox("Los datos de configuración general se guardaron correctamente" & vbNewLine & "NOTA: Estos cambios se realizaran la próxima vez que entre", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
            End If

        End If
        Me.Cursor = Cursors.Default
        cmdG1.Enabled = True

    End Sub
    Private Sub cmdG2_Click(sender As System.Object, e As System.EventArgs) Handles cmdG2.Click
        '----------------------
        Dim VLstrRuta As String = ValidaArchivoXML() & "\ConfiguracionesKM.xml"

        If Validar2() Then
            cmdG2.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            '--------------Configuracion guarda datos de la pestaña de (CENTRAL)
            If chkSICentral.CheckState = CheckState.Checked Then
                VGstrucBaseCentral.Seguridad_Integrada = 1
                VGstrucBaseCentral.Usuario = ""
                VGstrucBaseCentral.Contraseña = ""
            Else
                VGstrucBaseCentral.Seguridad_Integrada = 0
                VGstrucBaseCentral.Usuario = txtUsuarioCentral.Text
                VGstrucBaseCentral.Contraseña = IIf(txtContrasenaCentral.Text.Trim.Length = 0, "", encriptaValor(txtContrasenaCentral.Text))
            End If
            VGstrucBaseCentral.Servidor = txtServidorCentral.Text
            VGstrucBaseCentral.Puerto = txtPuertoCentral.Text
            VGstrucBaseCentral.Base = txtBaseCentral.Text
            VGstrucBaseCentral.Instancia = txtAliasCentral.Text

            VMclsConfiguracion = Nothing
            VMclsConfiguracion = New ClsConfiguracion

            VMclsConfiguracion.Configuracion = VGstrNombreIDConfig
            VMclsConfiguracion.ArchivoXML = VLstrRuta
            VMclsConfiguracion.RutaNodo = "/ConfiguracionesKM/Configuracion"
            VMclsConfiguracion.Atributo = "Central"
            VMclsConfiguracion.TipoResultado = ClsConfiguracion.TipoConfiguracion.BaseDatos

            Select Case cboTipoCentral.Text
                Case "Oracle"
                    VGstrucBaseCentral.Tipo = 2
                Case "SQL Server"
                    VGstrucBaseCentral.Tipo = 3
                Case "MySQL"
                    VGstrucBaseCentral.Tipo = 5
            End Select

            If VMclsConfiguracion.GuardaBD(VGstrucBaseCentral) Then
                guardaBitacora("Cambio en la configuración de la base de datos central ", 7)
                MsgBox("Los datos de la base central se guardaron  correctamente", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
            Else
                cboTipoCentral.Focus()
                cmdG2.Enabled = True
                Me.Cursor = Cursors.Default
                Exit Sub
            End If

            cmdG2.Enabled = True
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub cmdG3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdG3.Click
        Dim VLstrRuta As String = ValidaArchivoXML() & "\ConfiguracionesKM.xml"

        If Validar3() Then
            cmdG3.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            '--------------Configuracion guarda datos de la pestaña de (TRANSACCIONAL)
            If chkSITransaccional.CheckState = CheckState.Checked Then
                VGstrucTransaccional.Seguridad_Integrada = 1
                VGstrucTransaccional.Usuario = ""
                VGstrucTransaccional.Contraseña = ""
            Else
                VGstrucTransaccional.Seguridad_Integrada = 0
                VGstrucTransaccional.Usuario = txtUsuarioTransaccional.Text
                VGstrucTransaccional.Contraseña = IIf(txtContrasenaTransaccional.Text.Trim.Length = 0, "", encriptaValor(txtContrasenaTransaccional.Text))
            End If
            VGstrucTransaccional.Servidor = txtServidorTransaccional.Text
            VGstrucTransaccional.Puerto = txtPuertoTransaccional.Text
            VGstrucTransaccional.Base = txtBaseTransaccional.Text
            VGstrucTransaccional.Instancia = txtAliasTransaccional.Text

            VMclsConfiguracion = Nothing
            VMclsConfiguracion = New ClsConfiguracion

            VMclsConfiguracion.Configuracion = VGstrNombreIDConfig
            VMclsConfiguracion.ArchivoXML = VLstrRuta
            VMclsConfiguracion.RutaNodo = "/ConfiguracionesKM/Configuracion"
            VMclsConfiguracion.Atributo = "Transaccional"
            VMclsConfiguracion.TipoResultado = ClsConfiguracion.TipoConfiguracion.BaseDatos

            Select Case cboTipoTransaccional.Text
                Case "Oracle"
                    VGstrucTransaccional.Tipo = 2
                Case "SQL Server"
                    VGstrucTransaccional.Tipo = 3
                Case "MySQL"
                    VGstrucTransaccional.Tipo = 5
            End Select

            If VMclsConfiguracion.GuardaBD(VGstrucTransaccional) Then
                guardaBitacora("Cambio en la configuración de la base de datos Transaccional ", 7)
                MsgBox("Los datos de la base Transaccional se guardaron  correctamente", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
            Else
                cboTipoTransaccional.Focus()
                cmdG3.Enabled = True
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            cmdG3.Enabled = True
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub cmdG4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdG4.Click
        Dim VLstrRuta As String = ValidaArchivoXML() & "\ConfiguracionesKM.xml"

        If Validar4() Then
            cmdG4.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            '--------------Configuracion guarda datos de la pestaña de (BITACORA)

            If chkSIBitacora.CheckState = CheckState.Checked Then
                VGstrucBaseBitacora.Seguridad_Integrada = 1
                VGstrucBaseBitacora.Usuario = ""
                VGstrucBaseBitacora.Contraseña = ""
            Else
                VGstrucBaseBitacora.Seguridad_Integrada = 0
                VGstrucBaseBitacora.Usuario = txtUsuarioBitacora.Text
                VGstrucBaseBitacora.Contraseña = IIf(txtContrasenaBitacora.Text.Trim.Length = 0, "", encriptaValor(txtContrasenaBitacora.Text))
            End If

            VGstrucBaseBitacora.Servidor = txtServidorBitacora.Text
            VGstrucBaseBitacora.Puerto = txtPuertoBitacora.Text
            VGstrucBaseBitacora.Base = txtBaseBitacora.Text
            VGstrucBaseBitacora.Instancia = txtAliasBitacora.Text

            VMclsConfiguracion = Nothing
            VMclsConfiguracion = New ClsConfiguracion

            VMclsConfiguracion.Configuracion = VGstrNombreIDConfig
            VMclsConfiguracion.ArchivoXML = VLstrRuta
            VMclsConfiguracion.RutaNodo = "/ConfiguracionesKM/Configuracion"
            VMclsConfiguracion.Atributo = "Bitacora"
            VMclsConfiguracion.TipoResultado = ClsConfiguracion.TipoConfiguracion.BaseDatos

            Select Case cboTipoBitacora.Text
                Case "Oracle"
                    VGstrucBaseBitacora.Tipo = 2
                Case "SQL Server"
                    VGstrucBaseBitacora.Tipo = 3
                Case "MySQL"
                    VGstrucBaseBitacora.Tipo = 5
            End Select

            If VMclsConfiguracion.GuardaBD(VGstrucBaseBitacora) Then
                guardaBitacora("Cambio en la configuración de la base de datos bitácora ", 7)
                MsgBox("Los datos de la base bitácora se guardaron  correctamente", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
            Else
                cboTipoBitacora.Focus()
                cmdG4.Enabled = True
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            cmdG4.Enabled = True
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub VMtcpCliente_Conexion(ByVal Estado As Cliente_TCP.EstadoConexion, ByVal Mensaje As String)
        Try
            Select Case VMtcpCliente.Estado
                Case Cliente_TCP.EstadoConexion.Conectado
                    txtRes1.Text = txtRes1.Text & vbNewLine & "Conexión exitosa"
                    Me.Cursor = Cursors.Default
                    Me.picRes1.Image = imgImagenes.Images(0)
                    VMtcpCliente.Cerrar()
                Case Cliente_TCP.EstadoConexion.Conectando
                    txtRes1.Text = txtRes1.Text & "Servidor remoto válido, conectando..."
                    txtRes1.Refresh()
                Case Cliente_TCP.EstadoConexion.EnError
                    txtRes1.Text = txtRes1.Text & vbNewLine & Mensaje
                    Me.Cursor = Cursors.Default
                    Me.picRes1.Image = imgImagenes.Images(1)
                Case Cliente_TCP.EstadoConexion.ResolviendoHost
                    txtRes1.Text = txtRes1.Text & vbNewLine & "Resolviendo dirección servidor remoto..."
                    txtRes1.Refresh()
                Case Cliente_TCP.EstadoConexion.Desconectado
                    VMtcpCliente = Nothing
            End Select
        Catch ex As Exception
            MsgBox("KMRBatch 02003: No se pudo establecer una conexión con el servidor." & vbNewLine & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        End Try

    End Sub

    Private Sub ConexionC()
        Dim VLdbsBase As Object = Nothing

        VGstrucBaseCentral.Base = txtBaseCentral.Text.Trim()
        VGstrucBaseCentral.Seguridad_Integrada = CBool(chkSICentral.CheckState)
        VGstrucBaseCentral.Servidor = txtServidorCentral.Text.Trim
        VGstrucBaseCentral.Usuario = txtUsuarioCentral.Text.Trim
        VGstrucBaseCentral.Contraseña = txtContrasenaCentral.Text.Trim
        VGstrucBaseCentral.Puerto = txtPuertoCentral.Text.Trim
        VGstrucBaseCentral.Instancia = txtAliasCentral.Text.Trim

        Select Case VGstrucBaseCentral.Tipo
            Case TipoBase.MySQL
                VLdbsBase = New MySqlConnection
            Case TipoBase.SQLServer
                VLdbsBase = New SqlConnection
            Case TipoBase.Oracle
                VLdbsBase = New OracleConnection
        End Select
        VLdbsBase.ConnectionString = BaseDatos.CadenaConexion(VGstrucBaseCentral)
        Try
            VLdbsBase.Open()
            txtRes2.Text = txtRes2.Text & vbNewLine & "Conexión exitosa"
            txtRes2.Refresh()
            Me.picRes2.Image = imgImagenes.Images(0)
            VLdbsBase.Close()
        Catch ex As MySqlException
            txtRes2.Text = txtRes2.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción:  " & obtenerErrorBD(ex)
            txtRes2.Refresh()
            Me.picRes2.Image = imgImagenes.Images(1)
        Catch ex As OracleException
            txtRes2.Text = txtRes2.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción:  " & obtenerErrorBD(ex)
            txtRes2.Refresh()
            Me.picRes2.Image = imgImagenes.Images(1)
        Catch ex As Exception
            txtRes2.Text = txtRes2.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción: " & ex.Message
            txtRes2.Refresh()
            Me.picRes2.Image = imgImagenes.Images(1)
        End Try
    End Sub

    Private Sub ConexionT()
        Dim VLdbsBase As Object = Nothing

        VGstrucTransaccional.Base = txtBaseTransaccional.Text.Trim()
        VGstrucTransaccional.Seguridad_Integrada = CBool(chkSITransaccional.CheckState)
        VGstrucTransaccional.Servidor = txtServidorTransaccional.Text.Trim
        VGstrucTransaccional.Usuario = txtUsuarioTransaccional.Text.Trim
        VGstrucTransaccional.Contraseña = txtContrasenaTransaccional.Text.Trim
        VGstrucTransaccional.Puerto = txtPuertoTransaccional.Text.Trim
        VGstrucTransaccional.Instancia = txtAliasTransaccional.Text.Trim

        Select Case VGstrucTransaccional.Tipo
            Case TipoBase.MySQL
                VLdbsBase = New MySqlConnection
            Case TipoBase.SQLServer
                VLdbsBase = New SqlConnection
            Case TipoBase.Oracle
                VLdbsBase = New OracleConnection
        End Select
        VLdbsBase.ConnectionString = BaseDatos.CadenaConexion(VGstrucTransaccional)

        Try
            VLdbsBase.Open()
            txtRes3.Text = txtRes3.Text & vbNewLine & "Conexión exitosa"
            txtRes3.Refresh()
            Me.picRes3.Image = imgImagenes.Images(0)
            VLdbsBase.Close()

        Catch ex As MySqlException
            txtRes3.Text = txtRes3.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción:  " & obtenerErrorBD(ex)
            txtRes3.Refresh()
            Me.picRes3.Image = imgImagenes.Images(1)
        Catch ex As OracleException
            txtRes3.Text = txtRes3.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción:  " & obtenerErrorBD(ex)
            txtRes3.Refresh()
            Me.picRes3.Image = imgImagenes.Images(1)
        Catch ex As Exception
            txtRes3.Text = txtRes3.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción: " & ex.Message
            txtRes3.Refresh()
            Me.picRes3.Image = imgImagenes.Images(1)
        End Try
    End Sub

    Private Sub ConexionB()
        Dim VLdbsBase As Object = Nothing

        VGstrucBaseBitacora.Base = txtBaseBitacora.Text.Trim
        VGstrucBaseBitacora.Seguridad_Integrada = CBool(chkSIBitacora.CheckState)
        VGstrucBaseBitacora.Servidor = txtServidorBitacora.Text.Trim
        VGstrucBaseBitacora.Usuario = txtUsuarioBitacora.Text.Trim
        VGstrucBaseBitacora.Contraseña = txtContrasenaBitacora.Text.Trim
        VGstrucBaseBitacora.Puerto = txtPuertoBitacora.Text.Trim
        VGstrucBaseBitacora.Instancia = txtAliasBitacora.Text.Trim

        Select Case VGstrucBaseBitacora.Tipo
            Case TipoBase.MySQL
                VLdbsBase = New MySqlConnection
            Case TipoBase.SQLServer
                VLdbsBase = New SqlConnection
            Case TipoBase.Oracle
                VLdbsBase = New OracleConnection
        End Select

        VLdbsBase.ConnectionString = BaseDatos.CadenaConexion(VGstrucBaseBitacora)
        Try
            VLdbsBase.Open()
            txtRes4.Text = txtRes4.Text & vbNewLine & "Conexión exitosa"
            txtRes4.Refresh()
            Me.picRes4.Image = imgImagenes.Images(0)
            VLdbsBase.Close()
        Catch ex As MySqlException
            txtRes4.Text = txtRes4.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción: " & obtenerErrorBD(ex)
            txtRes4.Refresh()
            Me.picRes4.Image = imgImagenes.Images(1)
        Catch ex As OracleException
            txtRes4.Text = txtRes4.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción: " & obtenerErrorBD(ex)
            txtRes4.Refresh()
            Me.picRes4.Image = imgImagenes.Images(1)
        Catch ex As SqlException
            txtRes4.Text = txtRes4.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción: " & obtenerErrorBD(ex)
            txtRes4.Refresh()
            Me.picRes4.Image = imgImagenes.Images(1)
        Catch ex As Exception
            txtRes4.Text = txtRes4.Text & vbNewLine & "Error en la conexión" & vbNewLine & "Descripción: " & ex.Message
            txtRes4.Refresh()
            Me.picRes4.Image = imgImagenes.Images(1)
        End Try
    End Sub

    Private Sub cmdSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSalir.Click
        cmdReiniciar.Enabled = False
        Me.Close()
    End Sub
    Private Sub cmdReiniciar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReiniciar.Click
        VMtcpCliente = Nothing
        Application.Restart()
    End Sub
    Private Sub txtPuerto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPuerto.GotFocus
        txtPuerto.SelectAll()
    End Sub
    Private Sub txtPuerto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPuerto.KeyPress

        If Convert.ToByte(e.KeyChar) = Keys.Back Then
            e.Handled = False
        ElseIf Convert.ToByte(e.KeyChar) < Keys.D0 Or Convert.ToByte(e.KeyChar) > Keys.D9 Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmConfig_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        frmPrincipal.Cursor = Cursors.Default
        txtIP.Focus()

    End Sub
    Private Sub frmConfig_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If VGblnAutentif = False Then
                If cmdReiniciar.Enabled = True Then
                    MsgBox("Debe reiniciar la aplicación. Ya que no se ha identificado", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
                    e.Cancel = True
                End If
            End If
        End If
    End Sub
    Private Sub cboTipoCentral_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboTipoCentral.SelectedIndexChanged
        Me.picRes2.Image = Nothing
        LimpaCamposCentral()

        If cboTipoCentral.Text = "SQL Server" Then
            chkSICentral.Enabled = True
            Me.txtUsuarioCentral.Text = ""
            Me.txtContrasenaCentral.Text = ""
        Else
            chkSICentral.Enabled = False
            chkSICentral.CheckState = CheckState.Unchecked
        End If
        Select Case cboTipoCentral.Text
            Case "Oracle"
                VGstrucBaseCentral.Tipo = TipoBase.Oracle
                LblPuertoCentral.Text = "Predeterminado: 1521"
            Case "MySQL"
                VGstrucBaseCentral.Tipo = TipoBase.MySQL
                LblPuertoCentral.Text = "Predeterminado: 3306"
            Case "SQL Server"
                VGstrucBaseCentral.Tipo = TipoBase.SQLServer
                LblPuertoCentral.Text = "Predeterminado: 1433"
        End Select
    End Sub
    Private Sub cboTipoTransaccional_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTipoTransaccional.SelectedIndexChanged
        Me.picRes3.Image = Nothing
        LimpaCamposTransaccional()

        If cboTipoTransaccional.Text = "SQL Server" Then
            chkSITransaccional.Enabled = True
            Me.txtUsuarioTransaccional.Text = ""
            Me.txtContrasenaTransaccional.Text = ""
        Else
            chkSITransaccional.Enabled = False
            chkSITransaccional.CheckState = CheckState.Unchecked
        End If
        Select Case cboTipoTransaccional.Text
            Case "Oracle"
                VGstrucTransaccional.Tipo = TipoBase.Oracle
                LblPuertoTransaccional.Text = "Predeterminado: 1521"
            Case "MySQL"
                VGstrucTransaccional.Tipo = TipoBase.MySQL
                LblPuertoTransaccional.Text = "Predeterminado: 3306"
            Case "SQL Server"
                VGstrucTransaccional.Tipo = TipoBase.SQLServer
                LblPuertoTransaccional.Text = "Predeterminado: 1433"
        End Select
    End Sub
    Private Sub cboTipoBitacora_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTipoBitacora.SelectedIndexChanged
        Me.picRes4.Image = Nothing
        LimpaCamposBitacora()

        If cboTipoBitacora.Text = "SQL Server" Then
            chkSIBitacora.Enabled = True
            txtUsuarioBitacora.Text = ""
            txtContrasenaBitacora.Text = ""
        Else
            chkSIBitacora.Enabled = False
            chkSIBitacora.CheckState = CheckState.Unchecked
        End If

        Select Case cboTipoBitacora.Text
            Case "Oracle"
                VGstrucBaseBitacora.Tipo = TipoBase.Oracle
                LblPuertoBitacora.Text = "Predeterminado: 1521"
            Case "MySQL"
                VGstrucBaseBitacora.Tipo = TipoBase.MySQL
                LblPuertoBitacora.Text = "Predeterminado: 3306"
            Case "SQL Server"
                VGstrucBaseBitacora.Tipo = TipoBase.SQLServer
                LblPuertoBitacora.Text = "Predeterminado: 1433"
        End Select

    End Sub

    Private Function Validar1() As Boolean
        'Valida que existan los datos en las cajas de texto autentica
        Validar1 = False
        If txtPuerto.Text.Trim = "" Or Not IsNumeric(txtPuerto.Text.Trim) Then
            MsgBox("Debe indicar un número de puerto de Autenticación", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtPuerto.Focus()
        ElseIf txtIP.Text.Trim = "" Then
            MsgBox("Debe indicar una IP de Autenticación", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtIP.Focus()
        Else
            Validar1 = True
        End If
    End Function

    Private Function Validar2() As Boolean
        'valida datos de la base central
        Validar2 = False
        If cboTipoCentral.Text.Trim = "" Then
            MsgBox("No se encontro un origen de datos para la base central", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            cboTipoCentral.Focus()
        ElseIf txtServidorCentral.Text.Trim = "" Then
            MsgBox("Debe capturar la IP o Nombre del servidor para la base central", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtServidorCentral.Focus()
        ElseIf txtPuertoCentral.Text.Trim = "" Or Not IsNumeric(txtPuertoCentral.Text.Trim) Then
            MsgBox("Debe capturar el No. de puerto de datos para la base central", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtPuertoCentral.Focus()
        ElseIf txtBaseCentral.Text.Trim = "" Then
            MsgBox("Debe capturar el nombre de la base central", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtBaseCentral.Focus()
        ElseIf txtUsuarioCentral.Text.Trim = "" Then
            MsgBox("Debe capturar el usuario de la base central", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtUsuarioCentral.Focus()
        ElseIf txtContrasenaCentral.Text.Trim = "" Then
            MsgBox("Debe capturar la contraseña de la base central", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtContrasenaCentral.Focus()
        Else
            Validar2 = True
        End If
    End Function

    Private Function Validar3() As Boolean
        Dim VLblnRes As Boolean
        VLblnRes = False
        If cboTipoTransaccional.Text.Trim = "" Then
            MsgBox("No se encontro un origen de datos para la base Transaccional", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            cboTipoTransaccional.Focus()
        ElseIf txtServidorTransaccional.Text.Trim = "" Then
            MsgBox("Debe capturar la IP o Nombre del servidor para la base Transaccional", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtServidorTransaccional.Focus()
        ElseIf txtPuertoTransaccional.Text.Trim = "" Then
            MsgBox("Debe capturar el No. de puerto de datos para la base Transaccional", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtPuertoTransaccional.Focus()
        ElseIf txtBaseTransaccional.Text.Trim = "" Then
            MsgBox("Debe capturar el nombre de la base Transaccional", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtBaseTransaccional.Focus()
        ElseIf txtUsuarioTransaccional.Text.Trim = "" Then
            MsgBox("Debe capturar el usuario de la base Transaccional", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtUsuarioTransaccional.Focus()
        ElseIf txtContrasenaTransaccional.Text.Trim = "" Then
            MsgBox("Debe capturar la contraseña de la base Transaccional", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtContrasenaTransaccional.Focus()
        Else
            VLblnRes = True
        End If
        Validar3 = VLblnRes
    End Function

    Private Function Validar4() As Boolean
        Dim VLblnRes As Boolean
        VLblnRes = False

        If cboTipoBitacora.Text.Trim = "" Then
            MsgBox("No se encontro un origen de datos para la base bitacora", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            cboTipoBitacora.Focus()
        ElseIf txtServidorBitacora.Text.Trim = "" Then
            MsgBox("Debe capturar la IP o Nombre del servidor para la base bitacora", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtServidorBitacora.Focus()
        ElseIf txtPuertoBitacora.Text.Trim = "" Or Not IsNumeric(txtPuertoBitacora.Text.Trim) Then
            MsgBox("Debe capturar el No. de puerto de datos para la base bitacora", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtPuertoBitacora.Focus()
        ElseIf txtBaseBitacora.Text.Trim = "" Then
            MsgBox("Debe capturar el nombre de la base bitacora", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtBaseBitacora.Focus()
        ElseIf txtUsuarioBitacora.Text.Trim = "" Then
            MsgBox("Debe capturar el usuario de la base bitacora", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtUsuarioBitacora.Focus()
        ElseIf txtContrasenaBitacora.Text.Trim = "" Then
            MsgBox("Debe capturar la contraseña de la base bitacora", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtContrasenaBitacora.Focus()
        Else
            VLblnRes = True
        End If

        Validar4 = VLblnRes
    End Function

    Sub LimpaCamposCentral()
        chkSICentral.Checked = False
        txtServidorTransaccional.Text = ""
        txtPuertoCentral.Text = ""
        txtBaseCentral.Text = ""
        txtUsuarioCentral.Text = ""
        txtContrasenaCentral.Text = ""
        txtAliasCentral.Text = ""

    End Sub

    Sub LimpaCamposTransaccional()
        chkSITransaccional.Checked = False
        txtServidorTransaccional.Text = ""
        txtPuertoTransaccional.Text = ""
        txtBaseTransaccional.Text = ""
        txtUsuarioTransaccional.Text = ""
        txtContrasenaTransaccional.Text = ""
        txtAliasTransaccional.Text = ""

    End Sub
    Sub LimpaCamposBitacora()
        chkSIBitacora.Checked = False
        txtServidorBitacora.Text = ""
        txtPuertoBitacora.Text = ""
        txtBaseBitacora.Text = ""
        txtUsuarioBitacora.Text = ""
        txtContrasenaBitacora.Text = ""
        txtAliasBitacora.Text = ""
    End Sub

    Private Sub txtServidorCentral_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtServidorCentral.TextChanged
        cambiaIMg2()
    End Sub
    Private Sub txtPuertoCentral_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPuertoCentral.TextChanged
        cambiaIMg2()
    End Sub
    Private Sub txtBaseCentral_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBaseCentral.TextChanged
        cambiaIMg2()
    End Sub
    Private Sub txtUsuarioCentral_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtUsuarioCentral.TextChanged
        cambiaIMg2()
    End Sub
    Private Sub txtContrasenaCentral_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtContrasenaCentral.TextChanged
        cambiaIMg2()
    End Sub
    Private Sub txtAliasCentral_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAliasCentral.TextChanged
        cambiaIMg2()
    End Sub
    '--------------------------
    Private Sub txtServidorTransaccional_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtServidorTransaccional.TextChanged
        cambiaIMg3()
    End Sub
    Private Sub txtBaseTransaccional_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBaseTransaccional.TextChanged
        cambiaIMg3()
    End Sub
    Private Sub txtUsuarioTransaccional_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsuarioTransaccional.TextChanged
        cambiaIMg3()
    End Sub
    Private Sub txtContrasenaTransaccional_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtContrasenaTransaccional.TextChanged
        cambiaIMg3()
    End Sub
    Private Sub txtAliasTransaccional_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAliasTransaccional.TextChanged
        cambiaIMg3()
    End Sub
    Private Sub txtPuertoTransaccional_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPuertoTransaccional.TextChanged
        cambiaIMg3()
    End Sub
    '-----------------------
    Private Sub txtServidorBitacora_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtServidorBitacora.TextChanged
        cambiaIMg4()
    End Sub
    Private Sub txtBaseBitacora_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBaseBitacora.TextChanged
        cambiaIMg4()
    End Sub
    Private Sub txtUsuarioBitacora_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsuarioBitacora.TextChanged
        cambiaIMg4()
    End Sub
    Private Sub txtContrasenaBitacora_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtContrasenaBitacora.TextChanged
        cambiaIMg4()
    End Sub
    Private Sub txtAliasBitacora_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAliasBitacora.TextChanged
        cambiaIMg4()
    End Sub
    Private Sub txtPuertoBitacora_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPuertoBitacora.TextChanged
        cambiaIMg4()
    End Sub
    '--------------------
    Private Sub txtIP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP.TextChanged
        Me.picRes1.Image = Nothing
        txtRes1.Text = ""
    End Sub
    Private Sub txtPuerto_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPuerto.TextChanged
        Me.picRes1.Image = Nothing
    End Sub

    Private Sub cambiaIMg2()
        picRes2.Image = Nothing
        txtRes2.Text = ""
    End Sub
    Private Sub cambiaIMg3()
        picRes3.Image = Nothing
        txtRes3.Text = ""
    End Sub
    Private Sub cambiaIMg4()
        picRes4.Image = Nothing
        txtRes4.Text = ""
    End Sub

End Class

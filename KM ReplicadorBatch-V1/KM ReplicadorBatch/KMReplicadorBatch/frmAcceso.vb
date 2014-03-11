Imports Microsoft.Win32
Imports System.Data.Odbc
Imports System.Threading
Imports System.Net
Imports System.Net.Dns
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Public Class frmAcceso
    Public WithEvents VMtcpAcceso As Cliente_TCP
    Private VMblnSigue As Boolean
    Private VMblnAceptado As Boolean
    Dim VMobjRc4 As New RC4
    Public Const FS As String = ChrW(28)
    Dim VMclsConfiguracion As New ClsConfiguracion
    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAceptar.Click
        cmdAceptar.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Me.Refresh()

        VGblnAutentificando = True
        lblAccion.Text = "Validando usuario..."
        lblAccion.Refresh()

        If Not IsNumeric(txtUsuario.Text) And txtUsuario.Text.Length < 8 Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-01001 --> El usuario debe tener al menos 8 caracteres <-- ")
            MsgBox("El usuario debe tener al menos 8 caracteres", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            cmdAceptar.Enabled = True
            Me.Cursor = Cursors.Default
            lblAccion.Text = ""
            txtUsuario.Focus()
            Exit Sub
        End If

        If txtNuevaContrasena.Text.Length > 0 And txtNuevaContrasena.Text.Length < 8 Then
            'VGobjDepuracion.AgregarMensaje("KMRBatch-00001 --> La nueva contraseña debe tener al menos 8 caracteres <-- ")
            VGobjDepuracion.ArchivoPlano("KMRBatch-01003 --> La nueva contraseña debe tener al menos 8 caracteres <--  ")

            MsgBox("La nueva contraseña debe tener al menos 8 caracteres", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtNuevaContrasena.Focus()
            cmdAceptar.Enabled = True
            lblAccion.Text = ""
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        If txtNuevaContrasena.Text.Length >= 8 And (txtNuevaContrasena.Text <> txtConfirmar.Text) Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-01005 --> La nueva contraseña y la confirmación no coinciden <-- ")
            MsgBox("La nueva contraseña y la confirmación no coinciden", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtConfirmar.Text = ""
            txtConfirmar.Enabled = True
            lblConfirmar.Enabled = True
            txtNuevaContrasena.Focus()
            lblAccion.Text = ""
            cmdAceptar.Enabled = True
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        If txtContrasena.Text.Trim.Length < 8 And txtNuevaContrasena.Text.Length = 0 Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-01007 --> La contraseña debe tener al menos 8 caracteres <-- ")
            MsgBox("La contraseña debe tener al menos 8 caracteres", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            txtContrasena.Focus()
            lblAccion.Text = ""
            cmdAceptar.Enabled = True
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        If CboConfiguraciones.Text.Length = 0 Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-01009 --> Debe elegir una configuración <-- ")
            MsgBox("Debe elegir una configuración, verifique por favor ", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            CboConfiguraciones.Focus()
            cmdAceptar.Enabled = True
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        lblAccion.Text = "Validando datos..."
        lblAccion.Refresh()

        VGstrNombreIDConfig = CboConfiguraciones.Text

        Inicio()

    End Sub
    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click
        End
    End Sub
    Private Sub Inicio()
        Dim VLstrMensaje As String
        Dim VLobjControl As Control
        Dim VLobjEncripta As New RC4
        Dim VLstrIP As String = ""
        Dim VLlngTimer As Long
        Dim VLstrRuta As String = ""
        Dim VLintPuerto As Integer = 0


        lblAccion.Text = "Cargando información del servidor de autenticación..."
        lblAccion.Refresh()
        '--------------Configuracion - lee para obtener todos los datos DE autentificacion y bases de datos.
        VLstrRuta = ValidaArchivoXML() & "\ConfiguracionesKM.xml"

        Leer(VLstrRuta, VGstrNombreIDConfig)

        If VGstrucAutentifica.Servidor = Nothing Then
            VGobjDepuracion.ArchivoPlano("KMRBatch--01011 --> Configuración de autentificación incompleta  <-- ")
            MsgBox("KMRBatch--01011: Configuración de autentificación incompleta, verifique por favor ", MsgBoxStyle.Information)
            frmConfig.tabConfig.SelectedIndex = 0
            frmConfig.txtIP.Focus()
            cargaConfig()
            Exit Sub
        End If

        VLstrIP = VGstrucAutentifica.Servidor
        VLintPuerto = VGstrucAutentifica.Puerto
        '------------------
        VGstrUsuario = txtUsuario.Text.Trim
        lblAccion.Text = "Autentificando al usuario..."
        lblAccion.Refresh()

        VLlngTimer = Now.AddSeconds(40).Ticks
        VMtcpAcceso = New Cliente_TCP
        VMblnSigue = False

        ' Codigo en el cual obtenemos la ip del equipo, para maquinas con windows vista
        Dim VLipEquipo As IPHostEntry = GetHostEntry(My.Computer.Name)
        Dim VLipListaIPs As IPAddress() = VLipEquipo.AddressList()
        Dim VLobjIPLocal As Object

        For Each VLobjIPLocal In VLipListaIPs
            If VLobjIPLocal.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                VLstrIP = VLobjIPLocal.ToString
            End If
        Next

        VMtcpAcceso.Conectar(VGstrucAutentifica.Servidor, VGstrucAutentifica.Puerto)

        Do
            Thread.Sleep(500)
        Loop While Not VMblnSigue And Now.Ticks < VLlngTimer

        If VMtcpAcceso.Estado = Cliente_TCP.EstadoConexion.Conectado Then
            VMblnSigue = False
            VLstrMensaje = ksAutentifica(1, VLobjEncripta.RC4Encrypt(txtContrasena.Text, "%KeY*MoNiTor%V30"), VLobjEncripta.RC4Encrypt(txtNuevaContrasena.Text, "%KeY*MoNiTor%V30"), VLstrIP)

            VMblnAceptado = False
            VLlngTimer = Nothing
            VLlngTimer = Now.AddSeconds(40).Ticks
            VMtcpAcceso.EnviarDatos(VLstrMensaje)
            Do
                Thread.Sleep(500)
            Loop While Not VMblnSigue And Now.Ticks < VLlngTimer
            'Valida el resultado
            If VMblnAceptado = True Then
                VGblnAutentif = True
                VGblnAutentificando = False
                frmPrincipal.mnuPrincipal.Enabled = True
                VMtcpAcceso.Cerrar()

                'inhabilita los controles
                For Each VLobjControl In Me.Controls
                    If VLobjControl.Name <> "lblAccion" Then
                        VLobjControl.Enabled = False
                    End If
                Next

                '--------------Configuracion - valida  EL TIPO DE BASE DE DATOS (CENTRAL).
                If VGstrucBaseCentral.Tipo = 0 Then
                    VGobjDepuracion.ArchivoPlano("KMRBatch-01013 --> Configuración de base de datos central incompleta  <-- ")
                    MsgBox("KMRBatch-01013: Configuración de base de datos central incompleta, verifique por favor ", MsgBoxStyle.Information)
                    frmConfig.tabConfig.SelectedIndex = 1
                    frmConfig.cboTipoCentral.Focus()
                    cargaConfig()
                    Exit Sub
                End If

                '--------------Configuracion - valida  EL TIPO DE BASE DE DATOS (TRANSACCIONAL).
                If VGstrucTransaccional.Tipo = 0 Then
                    VGobjDepuracion.ArchivoPlano("KMRBatch-01017 --> Configuración de base de datos TRANSACCIONAL incompleta  <-- ")
                    MsgBox("KMRBatch-01017: Configuración de base de datos TRANSACCIONAL incompleta, verifique por favor ", MsgBoxStyle.Information)
                    frmConfig.tabConfig.SelectedIndex = 1
                    frmConfig.cboTipoCentral.Focus()
                    cargaConfig()
                    Exit Sub
                End If

                '--------------Configuracion - valida  EL TIPO DE BASE DE DATOS (BITACORA).
                If VGstrucBaseBitacora.Tipo = 0 Then
                    VGobjDepuracion.ArchivoPlano("KMRBatch--01021 --> Configuración de base de datos BITACORA incompleta  <-- ")
                    MsgBox("KMRBatch--01021: Configuración de base de datos BITACORA incompleta, verifique por favor ", MsgBoxStyle.Information)
                    frmConfig.tabConfig.SelectedIndex = 2
                    frmConfig.cboTipoBitacora.Focus()
                    cargaConfig()
                    Exit Sub
                End If

            Else
                VGobjDepuracion.ArchivoPlano("KMRBatch--01025 --> Usuario sin autentificación <-- ")
                MsgBox("KMRBatch--01025: Usuario sin autentificación, verifique por favor ", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)

                VGblnAutentif = False
                Me.cmdAceptar.Enabled = True
                Me.txtContrasena.Text = ""
                Me.lblAccion.Text = ""
                Me.Cursor = Cursors.Default
                Me.txtContrasena.Focus()
                Exit Sub
            End If
            obtenerFormatoFecha()
            guardaBitacora("Inicio de sesión", 1)
            cargaNormal()
        Else
            VGblnAutentif = False
            Me.cmdAceptar.Enabled = True
            Me.txtContrasena.Text = ""
            Me.lblAccion.Text = ""
            Me.Cursor = Cursors.Default
            Me.txtContrasena.Focus()
        End If

    End Sub

    Private Sub obtenerFormatoFecha()
        Dim VLdbsBaseC As Object = Nothing
        Dim VLcmdConsulta As Object = Nothing
        Dim VLstrSQL As String = ""
        Dim VLrcsDatos As Object = Nothing

        VGstrFechaArchivo = Format(Now, "yyyyMMdd")

        If VGstrucBaseCentral.Tipo = TipoBase.SQLServer Then
            Try
                VLdbsBaseC = New SqlClient.SqlConnection
                VLcmdConsulta = New SqlCommand(VLstrSQL, VLdbsBaseC)

                VLdbsBaseC.ConnectionString = BaseDatos.CadenaConexion(VGstrucBaseCentral)
                VLdbsBaseC.Open()

                VLcmdConsulta.CommandText = "SELECT @@LANGUAGE as Idioma"
                VLcmdConsulta.Connection = VLdbsBaseC
                VLcmdConsulta.CommandTimeout = 200

                VLrcsDatos = VLcmdConsulta.ExecuteReader

                With VLrcsDatos
                    If .HasRows Then
                        While .Read
                            If (!Idioma) = "Español" Then
                                VGstrFechaFormato = "dd/MM/yyyy HH:mm:ss.fff"
                            Else
                                VGstrFechaFormato = "yyyy/MM/dd HH:mm:ss.fff"
                            End If
                        End While
                    Else
                        VGstrFechaFormato = "yyyy/MM/dd HH:mm:ss.fff"
                    End If
                    VLrcsDatos.Close()
                    VLcmdConsulta.Cancel()
                End With
                VLdbsBaseC.Close()

            Catch ex As Exception
                VGstrFechaFormato = "yyyy/MM/dd HH:mm:ss.fff"
                MsgBox("KMRBatch 01011: Falla al obtener formato de fecha " & vbNewLine & ex.Message)
            End Try
        Else
            VGstrFechaFormato = "yyyy/MM/dd hh:mm:ss"
        End If
    End Sub

    Private Sub Acceso_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control And e.KeyValue = Keys.S Then
            If CboConfiguraciones.Text.Length = 0 Then
                VGobjDepuracion.ArchivoPlano("KMRBatch-01027 --> Debe elegir una configuración <-- ")
                MsgBox("Para cambiar parámetros debe elegir una Configuración, verifique por favor ", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
                CboConfiguraciones.Focus()
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            Me.Hide()
            If MsgBox("¿Desea cambiar los parámetros de configuración?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                VGstrUsuario = "-9999"
                VGstrNombreIDConfig = CboConfiguraciones.Text
                cargaConfig()
            Else
                Me.Show()
            End If
        End If
    End Sub
    Private Sub Acceso_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VGblnAutentificando = False
        VGobjDepuracion = Depuracion.Instancia
        VGobjDepuracion.Depuracion = Depuracion.TipoDepuracion.Simple
    End Sub
    Private Sub cargaConfig()
        frmConfig.ShowDialog()

        If VGblnAutentif = True Then
            frmConfig.cmdReiniciar.Visible = True
            frmConfig.cmdReiniciar.Enabled = True
            frmConfig.cmdSalir.Visible = True
        Else
            frmConfig.cmdReiniciar.Visible = True
            frmConfig.cmdSalir.Enabled = True
            frmConfig.cmdSalir.Visible = True
        End If

    End Sub

    Private Sub VMtcpAcceso_Conexion(ByVal Estado As Cliente_TCP.EstadoConexion, ByVal Mensaje As String)
        If Estado = Cliente_TCP.EstadoConexion.Conectado Then
            lblAccion.Text = "Conectado con servidor remoto"
            lblAccion.Refresh()
            VMblnSigue = True
        ElseIf Estado = Cliente_TCP.EstadoConexion.EnError Then
            lblAccion.Text = "Error en servidor remoto"
            lblAccion.Refresh()
            If VGblnAutentif = False Then
                MsgBox("KMRBatch-01029: No se pudo conectar con el servidor remoto por el siguiente motivo" & vbNewLine & Mensaje, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
                VMblnSigue = True
            End If
        End If
    End Sub

    Private Sub VMtcpAcceso_ConexionEstablecida() Handles VMtcpAcceso.ConexionEstablecida
        VMblnSigue = True
    End Sub
    Public Sub VMtcpAcceso_DatosRecibidos(ByVal Mensaje As String) Handles VMtcpAcceso.DatosRecibidos
        If Mensaje.Substring(16, 8).ToUpper = "SINLICEN" Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-01031 --> Esta petición excedió el limite de licencias, el programa se cerrará <-- ")
            MsgBox("Esta petición excedió el limite de licencias, el programa se cerrará", vbExclamation)
            Terminar()
        ElseIf Mensaje.Substring(16, 8).ToUpper = "SFCLVUSU" And Mensaje.Substring(66, 14).ToUpper.Trim = "LOGIN" Then
            If Not Mensaje.Substring(85, 8).ToUpper.Trim = "ACEPTADO" Or Mid(Mensaje.ToUpper.Trim, 86, 17) = "ACEPTADOYCAMBIADO" Then
                Select Case Mid(Mensaje, 96, 1)
                    Case 1 ' RepitaOperacion
                        VGobjDepuracion.ArchivoPlano("KMRBatch-01033 -->: <Mensaje del administrador central>" & Mensaje)
                        MsgBox("KMRBatch 01033: <Mensaje del administrador central>" & Chr(13) & Chr(10) & Mensaje.Substring(96).ToUpper, vbInformation)
                        VMblnAceptado = False
                        VMblnSigue = True
                    Case 3 ' Salir
                        VGobjDepuracion.ArchivoPlano("KMRBatch-01035 -->: <Mensaje del administrador central>" & Mensaje)
                        MsgBox("KMRBatch 01035: <Mensaje del administrador central>" & Chr(13) & Chr(10) & Mensaje.Substring(96).ToUpper, vbCritical)
                        Terminar()
                End Select
            ElseIf Mensaje.Substring(85, 8).ToUpper.Trim = "ACEPTADO" Or Mid(Mensaje.ToUpper.Trim, 86, 17) = "ACEPTADOYCAMBIADO" Then
                Select Case Mid(Mensaje, 96, 1)
                    Case 2
                        VGobjDepuracion.ArchivoPlano("KMRBatch-01037 -->: <Mensaje del administrador central>" & Mensaje)
                        MsgBox("KMRBatch-01037: <Mensaje del administrador central>" & Chr(13) & Chr(10) & Mensaje.Substring(96).ToUpper, MsgBoxStyle.Information)
                End Select
                VMblnAceptado = True
                VMblnSigue = True
            Else
                VMblnSigue = True
                VMblnAceptado = False
            End If
        ElseIf Mensaje.Substring(16, 8).ToUpper = "AUTENTIF" Then
            VMtcpAcceso.Cerrar()
        Else
            VGobjDepuracion.ArchivoPlano("KMRBatch-01039 --> Respuesta incorrecta, intente de nuevo <-- ")
            MsgBox("KMAN-01039: Respuesta incorrecta, intente de nuevo", vbOKOnly + vbExclamation)
            VMblnSigue = True
            VMblnAceptado = False
        End If
    End Sub
    Private Sub txtUsuario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsuario.GotFocus
        txtUsuario.SelectAll()
    End Sub
    Private Sub txtUsuario_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUsuario.KeyPress
        If Convert.ToByte(e.KeyChar) = Keys.Back Then
            e.Handled = False
        ElseIf Char.IsLetterOrDigit(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub
    Private Sub txtUsuario_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsuario.TextChanged
        lblAccion.Text = ""
    End Sub
    Private Sub txtContrasena_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtContrasena.GotFocus
        txtContrasena.SelectAll()
    End Sub
    Private Sub txtContrasena_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtContrasena.TextChanged
        lblAccion.Text = ""
    End Sub
    Private Sub txtNuevaContrasena_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNuevaContrasena.GotFocus
        txtNuevaContrasena.SelectAll()
    End Sub
    Private Sub txtNuevaContrasena_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNuevaContrasena.TextChanged
        lblAccion.Text = ""
        txtConfirmar.Enabled = True
        lblConfirmar.Enabled = True
        lblConfirmar.Enabled = Convert.ToBoolean(txtNuevaContrasena.Text.Trim.Length)
        txtConfirmar.Enabled = lblConfirmar.Enabled
    End Sub
    Private Sub txtConfirmar_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConfirmar.GotFocus
        txtConfirmar.SelectAll()
    End Sub
    Private Sub txtConfirmar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConfirmar.TextChanged
        lblAccion.Text = ""
    End Sub
    Private Sub cargaNormal()
        '********************************************************************************
        'Función para abrir la pantalla principal
        '********************************************************************************
        Me.Close()
        Me.Dispose(True)
        frmPrincipal.ShowDialog()
    End Sub

    Private Sub VMtcpAcceso_ErrorConexion(ByVal Mensaje As String) Handles VMtcpAcceso.ErrorConexion
        VMblnSigue = True
    End Sub

    Private Sub InicioParaConfiguracion()
        Dim VLstrRuta As String = ""
        '--------------Configuracion - lee para obtener todos los datos DE autentificacion y bases de datos.
        VLstrRuta = ValidaArchivoXML() & "\ConfiguracionesKM.xml"
        Leer(VLstrRuta, VGstrNombreIDConfig)
    End Sub

End Class

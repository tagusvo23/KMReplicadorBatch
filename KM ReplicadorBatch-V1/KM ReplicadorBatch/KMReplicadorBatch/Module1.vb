Imports Microsoft.Win32
Imports System.Data.Odbc
Imports System.Threading
Imports System.Net
Imports System.Net.Dns
Module Module1
    Public WithEvents VMtcpAcceso As Cliente_TCP
    Dim VMclsConfiguracion As New ClsConfiguracion
    Public VGstrNombreIDConfig As String
    Public VGstrUsuario As String

    Sub Main(ByVal args() As String)
        '------------------
        Dim VLstrRuta As String = ""
        Try
            '--------------Configuracion - lee para llenar cambo de configuraciones.
            VLstrRuta = ValidaArchivoXML() & "\ConfiguracionesKM.xml"

            Leer(VLstrRuta, VGstrNombreIDConfig)
            '------------------

            If args.Length = 0 Then
                frmAcceso.ShowDialog()
            Else
                VGstrNombreIDConfig = args(2)
                VGstrUsuario = args(0)
                Inicio()
            End If

        Catch ex As Exception
            MsgBox("KMRBatch-05001 La aplicación no pudo ser iniciada por el siguiente motivo: " & vbNewLine & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "KM Administrador")
            End
        End Try
    End Sub
    Private Sub Inicio()
        Dim VLstrIP As String = ""
        Dim VLintPuerto As Integer = 0
        Dim VLstrRuta As String = ""

        VGobjDepuracion = Depuracion.Instancia
        VGobjDepuracion.Depuracion = Depuracion.TipoDepuracion.Simple
        '-----
        VGblnAutentif = True
        VGblnAutentificando = False
        frmPrincipal.mnuPrincipal.Enabled = True

        '--------------Configuracion - lee para obtener todos los datos autentificacion y bases de datos.
        VLstrRuta = ValidaArchivoXML() & "\ConfiguracionesKM.xml"
        Leer(VLstrRuta, VGstrNombreIDConfig)

        If VGstrucAutentifica.Servidor = Nothing Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-05003 --> Configuración de autentificación incompleta  <-- ")
            MsgBox("KMRBatch-05003: Configuración de autentificación incompleta, verifique por favor ", MsgBoxStyle.Information)
            frmConfig.tabConfig.SelectedIndex = 0
            frmConfig.txtIP.Focus()
            cargaConfig()
            Exit Sub
        End If

        VLstrIP = VGstrucAutentifica.Servidor
        VLintPuerto = VGstrucAutentifica.Puerto
        '------------------
        ' Codigo en el cual obtenemos la ip del equipo, para maquinas con windows vista
        Dim VLipEquipo As IPHostEntry = GetHostEntry(My.Computer.Name)
        Dim VLipListaIPs As IPAddress() = VLipEquipo.AddressList()
        Dim VLobjIPLocal As Object

        For Each VLobjIPLocal In VLipListaIPs
            If VLobjIPLocal.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                VLstrIP = VLobjIPLocal.ToString
            End If
        Next
        '--------------Configuracion - valida  EL TIPO DE BASE DE DATOS (CENTRAL).

        If VGstrucBaseCentral.Tipo = 0 Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-05005 --> Configuración de base de datos central incompleta  <-- ")
            MsgBox("KMRBatch-05005: Configuración de base de datos central incompleta, verifique por favor ", MsgBoxStyle.Information)
            frmConfig.tabConfig.SelectedIndex = 1
            frmConfig.cboTipoCentral.Focus()
            cargaConfig()
            Exit Sub
        End If
        '--------------Configuracion - recupera datos para  EL TIPO DE BASE DE DATOS (TRANSACCIONAL).
        If VGstrucTransaccional.Tipo = 0 Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-05007 --> Configuración de base de datos TRANSACCIONAL incompleta  <-- ")
            MsgBox("KMRBatch-05007: Configuración de base de datos TRANSACCIONAL incompleta, verifique por favor ", MsgBoxStyle.Information)
            frmConfig.tabConfig.SelectedIndex = 1
            frmConfig.cboTipoCentral.Focus()
            cargaConfig()
            Exit Sub
        End If
        '--------------Configuracion - recupera datos para  EL TIPO DE BASE DE DATOS (BITACORA).
        If VGstrucBaseBitacora.Tipo = 0 Then
            VGobjDepuracion.ArchivoPlano("KMRBatch-05009 --> Configuración de base de datos BITACORA incompleta  <-- ")
            MsgBox("KMRBatch-05009: Configuración de base de datos BITACORA incompleta, verifique por favor ", MsgBoxStyle.Information)
            frmConfig.tabConfig.SelectedIndex = 2
            frmConfig.cboTipoBitacora.Focus()
            cargaConfig()
            Exit Sub
        End If
        '--------------Configuracion - recupera datos de parametros para la configuracion (KMAConfig).
        guardaBitacora("Inicio de sesión", 1)
        cargaNormal()
    End Sub
    Private Sub cargaConfig()
        '********************************************************************************
        'Función para abrir la pantalla de configuración
        '********************************************************************************
        frmPrincipal.Show()
        frmConfig.ShowDialog()
    End Sub
    Private Sub cargaNormal()
        '********************************************************************************
        'Función para abrir la pantalla principal
        '********************************************************************************
        frmPrincipal.ShowDialog()
    End Sub
End Module

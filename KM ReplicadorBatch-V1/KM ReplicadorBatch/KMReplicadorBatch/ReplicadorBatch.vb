Imports System.Data.SqlClient
Imports System.Data.OracleClient
Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Imports System.IO
Imports System.Xml

Module ReplicadorBatch
#Region "Servidor"
    Public Structure tipSvrRemoto
        Dim IP As String
        Dim Puerto As Integer
    End Structure
#End Region
#Region "Bases"
    Public Enum TipoBase As Byte
        Access = 1
        Oracle = 2
        SQLServer = 3
        Informix = 4
        MySQL = 5
        Sybase = 6
    End Enum
    Public Structure tipoBD
        Dim Servidor As String
        Dim Usuario As String
        Dim Base As String
        Dim Contraseña As String
        Dim Seguridad_Integrada As Boolean
        Dim Instancia As String
        Dim Tipo As TipoBase
        Dim Puerto As String
    End Structure
#End Region
#Region "Estructuras"
    Public Structure tipEncComunicaciones
        Dim Pathway() As Char
        Dim Servidor() As Char
        Dim Longitud() As Char
    End Structure
    Public Structure tipEncDatos
        Dim CodigoServ() As Char
        Dim NumOcur() As Char
        Dim MasInf() As Char
        Dim TotRegs() As Char
        Dim UltimoInd() As Char
        Dim NumUsuario() As Char
        Dim EstacTrab() As Char
    End Structure
    Public Structure tipXclavusu
        Dim Usuario() As Char
        Dim Clave() As Char
        Dim NuevaClave() As Char
        Dim PCNombre() As Char
        Dim IP() As Char
        Dim IdAplicacion() As Char
    End Structure
    Public Structure tipUsuario
        Dim EncComunicaciones As tipEncComunicaciones
        Dim EncDatos As tipEncDatos
        Dim DatUsuario As tipXclavusu
    End Structure
#End Region

    Public VGblnAutentif As Boolean                 'Indica si el usuario se autentificó correctamente 
    Public VGstrFechaFormato As String = ""         'para guardar el formato de fechas
    Public VGtipUsuario As tipUsuario               'Estructura para la autenticación del usuario
    Public VGstrRegistro As String                  'Guarda la ruta del registro de windows de la configuración
    Public Const CGbytAplicacion As Byte = 77       'Valor de la aplicación
    Public VGblnAutentificando As Boolean           'Si es true no se ejecuta código fuera de la forma frmAcceso
    Public VGstrDatos() As String                   'arreglo para los parametros de configuracion "Opciones de Ejecuación"
    Public VGobjDepuracion As Depuracion

    Public VGstrucAutentifica As ClsConfiguracion.Servidor = Nothing 'guarda datos de autentificacion
    Public VGstrucBaseCentral As BaseDatos.Base = Nothing ' guarda datos de base de datos central
    Public VGstrucBaseBitacora As BaseDatos.Base = Nothing ' guarda datos de base bitacora
    Public VGstrucTransaccional As BaseDatos.Base = Nothing ' guarda datos de base Transaccional
    Public VGstrFechaArchivo As String = ""         'para obtener la fecha del sistema y utilizarla en la tabla TT a guardar.
    Public VGstrNombreTablaTT As String = ""        'para armar el nombre de la tabla TT a procesar.
    Public VGstrucDatosTxt As frmProcesaTT.DatosTxt  'para guardar datos del registro de tarjetas archivo txt.

    Public Sub Terminar()
        If VGblnAutentif = True Then
            guardaBitacora("Fin de sesión", 2, )
            End
        Else
            End
        End If

    End Sub
    Public Function ksAutentifica(ByVal VPbytOpcion As Byte, ByVal VPstrContrasena As String, ByVal VPstrNuevaContrasena As String, ByVal VPstrIP As String) As String
        '****************************************************************************************************************
        'Carga el mensaje que se enviará al servidor de autenticación                                                   '
        '---PARAMETROS                                                                                                  ' 
        '   1. VPbytOpcion: Indica si es logeo (1), validación de licencia (2) ó aviso de liberación de licencia (3)    '
        '   2. VPstrContrasena: Contraseña encriptada                                                                   '
        '   3. VPstrNuevaContrasena: Nueva contraseña encriptada                                                        '
        '   4. VPstrIP: Dirección IP del equipo que se autentica                                                        '
        '---VALORES DEVUELTOS                                                                                           '
        '   Mensaje armado para el logeo                                                                                '
        '****************************************************************************************************************
        Dim VLstrMensaje As String
        Dim VLstrLong As String

        With VGtipUsuario
            .EncComunicaciones.Pathway = Space(16)
            'Arma el mensaje
            Select Case VPbytOpcion
                Case 1
                    .EncComunicaciones.Servidor = "SFCLVUSU"
                Case 2
                    .EncComunicaciones.Servidor = "AUTENTIF"
                Case 3
                    .EncComunicaciones.Servidor = "CERRANDO"
            End Select
            .EncComunicaciones.Servidor = .EncComunicaciones.Servidor & Space(15 - .EncComunicaciones.Servidor.Length)
            .EncDatos.CodigoServ = "00099"
            .EncDatos.NumOcur = "00001"
            .EncDatos.MasInf = "00000"
            .EncDatos.TotRegs = "00001"
            .EncDatos.UltimoInd = Format(CGbytAplicacion, "00000")
            If VGstrUsuario.Trim.Length < 5 Then
                .EncDatos.NumUsuario = VGstrUsuario & Space(5 - VGstrUsuario.Trim.Length)
            Else
                .EncDatos.NumUsuario = VGstrUsuario.Substring(0, 5) ' .EncDatos.NumUsuario = VGstrUsuario.Substring(1, 5)
            End If
            .EncDatos.EstacTrab = Space(14)

            .DatUsuario.Usuario = VGstrUsuario.Trim & Space(12 - VGstrUsuario.Trim.Length)
            .DatUsuario.Clave = VPstrContrasena.Trim & Space(60 - VPstrContrasena.Trim.Length)
            .DatUsuario.NuevaClave = VPstrNuevaContrasena.Trim & Space(60 - VPstrNuevaContrasena.Trim.Length)

            If My.Computer.Name.Length > 14 Then
                .DatUsuario.PCNombre = My.Computer.Name.Substring(0, 14)
            Else
                .DatUsuario.PCNombre = My.Computer.Name & Space(14 - My.Computer.Name.Length)
            End If
            .DatUsuario.IP = VPstrIP & Space(15 - VPstrIP.Length)
            .DatUsuario.IdAplicacion = Format(CGbytAplicacion, "00")

            'Arma la cadena
            VLstrMensaje = .EncDatos.CodigoServ & .EncDatos.NumOcur & .EncDatos.MasInf & _
                           .EncDatos.TotRegs & .EncDatos.UltimoInd & .EncDatos.NumUsuario _
            & .EncDatos.EstacTrab & .DatUsuario.Usuario & .DatUsuario.Clave _
            & .DatUsuario.NuevaClave & .DatUsuario.PCNombre & _
            .DatUsuario.IP & .DatUsuario.IdAplicacion

            VLstrLong = Format(.EncComunicaciones.Servidor.Length + .EncComunicaciones.Pathway.Length + 5 + VLstrMensaje.Length, "00000")
            VLstrMensaje = .EncComunicaciones.Pathway & .EncComunicaciones.Servidor _
                           & VLstrLong & VLstrMensaje
        End With
        ksAutentifica = VLstrMensaje
    End Function
    Function guardaBitacora(ByVal VPstrMensaje As String, ByVal VPintRegistro As Integer, Optional ByVal VPintStatus _
                          As Integer = 0, Optional ByVal VPstrDescFiltro As String = "", Optional ByVal VPstrEscala As String = "" _
                          , Optional ByVal VpstrLimite As String = "", Optional ByVal VPstrTiempo As String = "", Optional _
                          ByVal VPstrReferencia As String = "", Optional ByVal VPintFiltro As Integer = 0, Optional _
                          ByVal VPstrCondicion As String = "") As Boolean

        Dim VLstrSQL As String
        Dim VLdbsBase As Object
        Dim VLcmdComando As Object
        Dim VLrcsDatos As Object
        Dim KGintaPP As Integer
        Dim VGstrLocalHostName As String
        Dim VGstrLocalIPAddress As String
        VLdbsBase = Nothing
        VLcmdComando = Nothing
        Try
            Select Case VGstrucBaseBitacora.Tipo
                Case TipoBase.MySQL
                    VLdbsBase = New MySqlConnection
                Case TipoBase.Oracle
                    VLdbsBase = New OracleConnection
                Case TipoBase.SQLServer
                    VLdbsBase = New SqlClient.SqlConnection
            End Select
            VLdbsBase.ConnectionString = BaseDatos.CadenaConexion(VGstrucBaseBitacora)

            VLdbsBase.open()
            KGintaPP = 77
            VGstrLocalHostName = VGtipUsuario.DatUsuario.PCNombre
            VGstrLocalIPAddress = VGtipUsuario.DatUsuario.IP

            If VPstrDescFiltro.Length > 50 Then
                VPstrDescFiltro = Strings.Left(VPstrDescFiltro, 50)
            End If

            VLstrSQL = ""

            Select Case VGstrucBaseBitacora.Tipo
                Case TipoBase.MySQL
                    VLstrSQL = "INSERT INTO TBITALARMA (ID_USUARIO, ID_LAY,ID_FORMATO,ID_APLICACION,ID_TIPOREG,IP_ADRESS,PC_NAME,FECHA_HORA,ID_FILTRO,DESCRIPCION_FILTRO,ESCALA,MENSAJE,LIMITE,TIEMPO,REFERENCIA,STATUS) " & _
                               " VALUES ('" & VGstrUsuario & "'," & 0 & "," & 0 & "," & KGintaPP & "," & VPintRegistro & ",'" & VGstrLocalIPAddress & "','" & VGstrLocalHostName & "',NOW()," & VPintFiltro & ",'" & VPstrDescFiltro & "','" & VPstrEscala & "','" & Replace(VPstrMensaje, "'", "") & "','" & VpstrLimite & "','" & VPstrTiempo & "','" & VPstrReferencia & "'," & VPintStatus & ")"
                    VLcmdComando = New MySqlCommand(VLstrSQL, VLdbsBase)
                Case TipoBase.Oracle
                    VLstrSQL = "INSERT INTO TBITALARMA (ID_USUARIO, ID_LAY,ID_FORMATO,ID_APLICACION,ID_TIPOREG,IP_ADRESS,PC_NAME,FECHA_HORA,ID_FILTRO,DESCRIPCION_FILTRO,ESCALA,MENSAJE,LIMITE,TIEMPO,REFERENCIA,STATUS) " & _
                               " VALUES ('" & VGstrUsuario & "'," & 0 & "," & 0 & "," & KGintaPP & "," & VPintRegistro & ",'" & VGstrLocalIPAddress & "','" & VGstrLocalHostName & "',to_date(TO_char(SYSDATE,'YYYY-MM-DD HH24:MI:SS:SSSSS'),'YYYY-MM-DD HH24:MI:SS:SSSSS')," & VPintFiltro & ",'" & VPstrDescFiltro & "','" & VPstrEscala & "','" & Replace(VPstrMensaje, "'", "") & "','" & VpstrLimite & "','" & VPstrTiempo & "','" & VPstrReferencia & "'," & VPintStatus & ")"
                    VLcmdComando = New OracleCommand(VLstrSQL, VLdbsBase)
                Case TipoBase.SQLServer
                    VLstrSQL = "INSERT INTO TBITALARMA (ID_USUARIO, ID_LAY,ID_FORMATO,ID_APLICACION,ID_TIPOREG,IP_ADRESS,PC_NAME,FECHA_HORA,ID_FILTRO,DESCRIPCION_FILTRO,ESCALA,MENSAJE,LIMITE,TIEMPO,REFERENCIA,STATUS) " & _
                               " VALUES ('" & VGstrUsuario & "'," & 0 & "," & 0 & "," & KGintaPP & "," & VPintRegistro & ",'" & VGstrLocalIPAddress & "','" & VGstrLocalHostName & "',GETDATE()," & VPintFiltro & ",'" & VPstrDescFiltro & "','" & VPstrEscala & "','" & Replace(VPstrMensaje, "'", "") & "','" & VpstrLimite & "','" & VPstrTiempo & "','" & VPstrReferencia & "'," & VPintStatus & ")"
                    VLcmdComando = New SqlCommand(VLstrSQL, VLdbsBase)

            End Select

            Try
                VLrcsDatos = VLcmdComando.ExecuteReader
                VLrcsDatos.Close()
                VLcmdComando.Dispose()
            Catch ex As MySqlException
                VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & " Error TBitalarma:" & obtenerErrorBD(ex) & " " & VLstrSQL)
            Catch ex As OracleException
                VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & " Error TBitalarma:" & obtenerErrorBD(ex) & " " & VLstrSQL)
            Catch ex As SqlException
                VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & " Error TBitalarma:" & obtenerErrorBD(ex) & " " & VLstrSQL)
            Catch ex As Exception
                VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & " Error TBitalarma:" & ex.Message)
            End Try
            VLdbsBase.close()
        Catch ex As MySqlException
            VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & "Error TBitalarma:" & obtenerErrorBD(ex))
        Catch ex As OracleException
            VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & "Error TBitalarma:" & obtenerErrorBD(ex))
        Catch ex As SqlException
            VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & "Error TBitalarma:" & obtenerErrorBD(ex))
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano(VPstrMensaje & " " & VPstrDescFiltro & "Error TBitalarma:" & ex.Message)
        End Try

    End Function

    Public Function claveRutaUnica() As String
        '********************************************************************************
        'Obtiene un identificador único para el nombre y ruta del archivo ejecutable    '
        '---VALORES DEVUELTOS                                                           '
        '   Cadena con el identificador único                                           '
        '********************************************************************************
        Dim VLintX As Integer
        Dim VLlngLlave As Double
        Dim VLstrRuta As String
        VLstrRuta = Application.ExecutablePath
        For VLintX = 1 To VLstrRuta.Length - 1
            VLlngLlave = VLlngLlave + (Asc(VLstrRuta.Substring(VLintX - 1, 1)) * VLintX)
        Next VLintX
        claveRutaUnica = "?" & VLlngLlave.ToString.Trim & "?"
    End Function

    Public Sub ReporteTexto(ByVal VPstrMensaje As String)
        Dim VLstrTemporal As String
        Dim VLkeyRegistro As RegistryKey
        Dim VLioArchivo As System.IO.StreamWriter

        Try
            VLkeyRegistro = Registry.LocalMachine.OpenSubKey(VGstrRegistro & "General")
            VLstrTemporal = VLkeyRegistro.GetValue("Reporte").ToString()
            VLioArchivo = My.Computer.FileSystem.OpenTextFileWriter(VLstrTemporal, True)
            VLioArchivo.WriteLine(VPstrMensaje)
            VLioArchivo.Close()
        Catch ex As Exception
            MsgBox("Error al escribir en el archivo de Reporte. " & vbNewLine & "Verifique la ruta donde se almacena el archivo de Reporte. " & vbNewLine & ex.Message)
        End Try
    End Sub
    Public Function ValidaArchivoXML() As String

        Dim VLstrDirectorio As String
        Dim VLstrDir() As String
        Dim VLstrRuta As String

        VLstrDirectorio = My.Application.Info.DirectoryPath

        VLstrDir = Split(VLstrDirectorio, "\")
        ReDim Preserve VLstrDir(VLstrDir.Length - 2)
        VLstrRuta = Join(VLstrDir, "\")

        If Not My.Computer.FileSystem.FileExists(VLstrRuta & "\ConfiguracionesKM.xml") Then
            'VGobjDepuracion.ArchivoPlano("KMRBatch-06001 --> No existe archivo ConfiguracionesKM.xml ")
            MsgBox("KMRBatch-06001: No existe el archivo de configuraciones  ***ConfiguracionesKM.xml***, Verifique por favor ", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            End
        Else
            Return VLstrRuta
        End If

    End Function

    Public Sub Leer(ByVal VPstrRuta As String, ByVal VPstrNombreIDConfig As String)
        Dim VLobjXmlTReader As XmlTextReader = New XmlTextReader(VPstrRuta)
        Dim VLstrConfig As String

        VGstrucAutentifica = Nothing
        VGstrucBaseCentral = Nothing
        VGstrucBaseBitacora = Nothing

        frmAcceso.CboConfiguraciones.Items.Clear()

        VLobjXmlTReader.WhitespaceHandling = WhitespaceHandling.None
        Try
            VLobjXmlTReader.Read()
            While (VLobjXmlTReader.Read())
                Select Case (VLobjXmlTReader.NodeType)
                    Case XmlNodeType.Element
                        Select Case (VLobjXmlTReader.Name)
                            Case "Configuracion"
                                VLstrConfig = VLobjXmlTReader.GetAttribute("ID")
                                frmAcceso.CboConfiguraciones.Items.Add(VLobjXmlTReader.GetAttribute("ID"))
                                If VLstrConfig = VPstrNombreIDConfig Then
                                    While VLobjXmlTReader.Read
                                        If VLobjXmlTReader.Name = "Configuracion" Then
                                            VLobjXmlTReader.Close()
                                            Exit Sub
                                        End If

                                        Select Case VLobjXmlTReader.Name
                                            Case "Base"
                                                Dim VLstrNombre As String = VLobjXmlTReader.GetAttribute("ID")
                                                Select Case VLstrNombre
                                                    Case "Central"
                                                        VLobjXmlTReader.Read()
                                                        VGstrucBaseCentral.Tipo = VLobjXmlTReader.ReadElementString("Tipo")
                                                        VGstrucBaseCentral.Servidor = VLobjXmlTReader.ReadElementString("IP")
                                                        VGstrucBaseCentral.Puerto = VLobjXmlTReader.ReadElementString("Puerto")
                                                        VGstrucBaseCentral.Base = VLobjXmlTReader.ReadElementString("NombreBase")
                                                        VGstrucBaseCentral.Instancia = VLobjXmlTReader.ReadElementString("Instancia")
                                                        VGstrucBaseCentral.Usuario = VLobjXmlTReader.ReadElementString("Usuario")
                                                        VGstrucBaseCentral.Contraseña = desencriptaValor(VLobjXmlTReader.ReadElementString("Clave"))
                                                        VGstrucBaseCentral.Seguridad_Integrada = VLobjXmlTReader.ReadElementString("SI")
                                                    Case "Transaccional"
                                                        VLobjXmlTReader.Read()
                                                        VGstrucTransaccional.Tipo = VLobjXmlTReader.ReadElementString("Tipo")
                                                        VGstrucTransaccional.Servidor = VLobjXmlTReader.ReadElementString("IP")
                                                        VGstrucTransaccional.Puerto = VLobjXmlTReader.ReadElementString("Puerto")
                                                        VGstrucTransaccional.Base = VLobjXmlTReader.ReadElementString("NombreBase")
                                                        VGstrucTransaccional.Instancia = VLobjXmlTReader.ReadElementString("Instancia")
                                                        VGstrucTransaccional.Usuario = VLobjXmlTReader.ReadElementString("Usuario")
                                                        VGstrucTransaccional.Contraseña = desencriptaValor(VLobjXmlTReader.ReadElementString("Clave"))
                                                        VGstrucTransaccional.Seguridad_Integrada = VLobjXmlTReader.ReadElementString("SI")
                                                    Case "Bitacora"
                                                        VLobjXmlTReader.Read()
                                                        VGstrucBaseBitacora.Tipo = VLobjXmlTReader.ReadElementString("Tipo")
                                                        VGstrucBaseBitacora.Servidor = VLobjXmlTReader.ReadElementString("IP")
                                                        VGstrucBaseBitacora.Puerto = VLobjXmlTReader.ReadElementString("Puerto")
                                                        VGstrucBaseBitacora.Base = VLobjXmlTReader.ReadElementString("NombreBase")
                                                        VGstrucBaseBitacora.Instancia = VLobjXmlTReader.ReadElementString("Instancia")
                                                        VGstrucBaseBitacora.Usuario = VLobjXmlTReader.ReadElementString("Usuario")
                                                        VGstrucBaseBitacora.Contraseña = desencriptaValor(VLobjXmlTReader.ReadElementString("Clave"))
                                                        VGstrucBaseBitacora.Seguridad_Integrada = VLobjXmlTReader.ReadElementString("SI")
                                                End Select
                                            Case "ServidorAuten"
                                                VLobjXmlTReader.Read()
                                                VGstrucAutentifica.Servidor = VLobjXmlTReader.ReadElementString("IP")
                                                VGstrucAutentifica.Puerto = VLobjXmlTReader.ReadElementString("Puerto")
                                            Case "ReplicadorBatch"
                                                VLobjXmlTReader.Read()
                                        End Select
                                    End While
                                    VLobjXmlTReader.Close()
                                End If
                        End Select
                End Select
            End While
            VLobjXmlTReader.Close()
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch--06003 --> Error al leer arhivo configuracion.xml <-- ")
            MsgBox("KMRBatch--06003:" & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            VLobjXmlTReader.Close()
            End
        End Try
    End Sub
    Public Sub CargaLayout()
        '*************************************************************************
        'Descripcion:  carga de Layouts en el combo
        '*************************************************************************
        Dim VLdbsBaseC As Object = Nothing
        Dim VLrcsDatos As Object = Nothing
        Dim VLcmdConsulta As Object = Nothing
        Dim VLstrSQL As String = ""

        VLstrSQL = "SELECT * FROM tlaylogt"
        frmProcesaTT.CboLayout.Items.Clear()

        Select Case VGstrucBaseCentral.Tipo
            Case TipoBase.MySQL
                VLdbsBaseC = New MySqlConnection
                VLcmdConsulta = New MySqlCommand(VLstrSQL, VLdbsBaseC)
            Case TipoBase.Oracle
                VLdbsBaseC = New OracleConnection
                VLcmdConsulta = New OracleCommand(VLstrSQL, VLdbsBaseC)
            Case TipoBase.SQLServer
                VLdbsBaseC = New SqlClient.SqlConnection
                VLcmdConsulta = New SqlCommand(VLstrSQL, VLdbsBaseC)
        End Select
        VLdbsBaseC.connectionstring = BaseDatos.CadenaConexion(VGstrucBaseCentral)

        Try
            VLdbsBaseC.open()

            Try
                VLrcsDatos = VLcmdConsulta.ExecuteReader
                If VLrcsDatos.HasRows Then
                    While VLrcsDatos.Read
                        frmProcesaTT.CboLayout.Items.Add(VLrcsDatos!Id_Lay & "- " & VLrcsDatos!Descripcion)
                    End While
                Else
                    MsgBox("No existe ningun Layout en la base", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
                End If
                VLrcsDatos.Close()
                VLcmdConsulta.Dispose()
                VLcmdConsulta = Nothing
            Catch ex As OracleException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06005 --> : " & ex.Message)
                MsgBox("KMRBatch-06005:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Catch ex As SqlClient.SqlException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06005 --> : " & ex.Message)
                MsgBox("KMRBatch-06005:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Catch ex As MySqlException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06005 --> : " & ex.Message)
                MsgBox("KMRBatch-06005:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Catch ex As Exception
                VGobjDepuracion.ArchivoPlano("KMRBatch-06005 --> : " & ex.Message)
                MsgBox("KMRBatch-06005:" & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            End Try
            VLdbsBaseC.close()
        Catch ex As OracleException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06007 --> : " & ex.Message)
            MsgBox("KMRBatch-06007: Ocurrió el siguiente error al cargar los Layout" & vbNewLine & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As SqlClient.SqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06007 --> : " & ex.Message)
            MsgBox("KMRBatch-06007: Ocurrió el siguiente error al cargar los Layout" & vbNewLine & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As MySqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06007 --> : " & ex.Message)
            MsgBox("KMRBatch-06007: Ocurrió el siguiente error al cargar los Layout" & vbNewLine & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch-06007 --> : " & ex.Message)
            MsgBox("KMRBatch-06007:" & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Public Sub cargaFormatos(VPintlay As Integer)
        '*************************************************************************
        'Descripcion:carga los formatos en los combos
        '*************************************************************************
        Dim VLdbsBaseC As Object = Nothing
        Dim VLrcsDatos As Object = Nothing
        Dim VLcmdConsulta As Object = Nothing
        Dim VLstrSQL As String = ""

        VLstrSQL = "SELECT * FROM tlaymsgt where id_lay = " & VPintlay
        frmProcesaTT.CboFormatos.Items.Clear()

        Select Case VGstrucBaseCentral.Tipo
            Case TipoBase.MySQL
                VLdbsBaseC = New MySqlConnection
                VLcmdConsulta = New MySqlCommand(VLstrSQL, VLdbsBaseC)
            Case TipoBase.Oracle
                VLdbsBaseC = New OracleConnection
                VLcmdConsulta = New OracleCommand(VLstrSQL, VLdbsBaseC)
            Case TipoBase.SQLServer
                VLdbsBaseC = New SqlClient.SqlConnection
                VLcmdConsulta = New SqlCommand(VLstrSQL, VLdbsBaseC)
        End Select
        VLdbsBaseC.connectionstring = BaseDatos.CadenaConexion(VGstrucBaseCentral)
        Try
            VLdbsBaseC.open()

            Try
                VLrcsDatos = VLcmdConsulta.ExecuteReader
                If VLrcsDatos.HasRows Then
                    While VLrcsDatos.Read
                        frmProcesaTT.CboFormatos.Items.Add(VLrcsDatos!id_formato & "- " & VLrcsDatos!Descripcion)
                    End While
                Else
                    MsgBox("No existe ningun Formato en la base", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
                End If
                VLrcsDatos.Close()
                VLcmdConsulta.Dispose()
                VLcmdConsulta = Nothing
            Catch ex As OracleException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06009 --> : " & ex.Message)
                MsgBox("KMRBatch-06009:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Catch ex As SqlClient.SqlException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06009 --> : " & ex.Message)
                MsgBox("KMRBatch-06009:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Catch ex As MySqlException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06009 --> : " & ex.Message)
                MsgBox("KMRBatch-06009:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Catch ex As Exception
                VGobjDepuracion.ArchivoPlano("KMRBatch-06009 --> : " & ex.Message)
                MsgBox("KMRBatch-06009:" & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            End Try
            VLdbsBaseC.close()
        Catch ex As OracleException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06011 --> : " & ex.Message)
            MsgBox("KMRBatch-06011:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As SqlClient.SqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06011 --> : " & ex.Message)
            MsgBox("KMRBatch-06011:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As MySqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06011 --> : " & ex.Message)
            MsgBox("KMRBatch-06011:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch-06011 --> : " & ex.Message)
            MsgBox("KMRBatch-06011:" & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Public Function ValidaTablaTT() As Boolean
        '*************************************************************************
        'Descripcion:valida que exista la tabla TT correspomdiente al dia de proceso.
        '*************************************************************************
        Dim VLdbsBaseT As Object = Nothing
        Dim VLrcsDatos As Object = Nothing
        Dim VLcmdConsulta As Object = Nothing
        Dim VLstrSQL As String = ""


        ValidaTablaTT = False

        Select Case VGstrucTransaccional.Tipo
            Case TipoBase.MySQL
                VLstrSQL = "SELECT * FROM " & VGstrNombreTablaTT & " LIMIT 1 "
                VLdbsBaseT = New MySqlConnection
                VLcmdConsulta = New MySqlCommand(VLstrSQL, VLdbsBaseT)
            Case TipoBase.Oracle
                VLstrSQL = "SELECT * FROM " & VGstrNombreTablaTT & " where ROWNUM < 2 "
                VLdbsBaseT = New OracleConnection
                VLcmdConsulta = New OracleCommand(VLstrSQL, VLdbsBaseT)
            Case TipoBase.SQLServer
                VLstrSQL = "SELECT TOP 1 * FROM " & VGstrNombreTablaTT
                VLdbsBaseT = New SqlClient.SqlConnection
                VLcmdConsulta = New SqlCommand(VLstrSQL, VLdbsBaseT)
        End Select
        VLdbsBaseT.connectionstring = BaseDatos.CadenaConexion(VGstrucTransaccional)
        Try
            VLdbsBaseT.open()

            Try
                VLrcsDatos = VLcmdConsulta.ExecuteReader
                ValidaTablaTT = True

                'If VLrcsDatos.HasRows Then
                '    While VLrcsDatos.Read
                '        ValidaTablaTT = True
                '    End While
                'End If

                VLrcsDatos.Close()
                VLcmdConsulta.Dispose()
                VLcmdConsulta = Nothing
            Catch ex As OracleException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06013 --> : " & ex.Message)
            Catch ex As SqlClient.SqlException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06013 --> : " & ex.Message)
            Catch ex As MySqlException
                VGobjDepuracion.ArchivoPlano("KMRBatch-06013 --> : " & ex.Message)
            Catch ex As Exception
                VGobjDepuracion.ArchivoPlano("KMRBatch-06013 --> : " & ex.Message)
                MsgBox("KMRBatch-06009:" & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            End Try
            VLdbsBaseT.close()
        Catch ex As OracleException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06015 --> : " & ex.Message)
        Catch ex As SqlClient.SqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06015 --> : " & ex.Message)
        Catch ex As MySqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-06015 --> : " & ex.Message)
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch-06015 --> : " & ex.Message)
        End Try
    End Function
End Module


'Dim VLstrValor As String = ""
'If dlgArchivo.ShowDialog() = Windows.Forms.DialogResult.OK Then
'    Try
'        If dlgArchivo.FileName.Length > 0 Then
'            VMioArchivo = New StreamReader(dlgArchivo.FileName)
'            Me.Cursor = Cursors.WaitCursor
'            While Not VMioArchivo.EndOfStream
'                VLstrValor = VMioArchivo.ReadLine
'                If Not IsNothing(VLstrValor) Then
'                    'CboValorF.Items.Add(VLstrValor)
'                End If
'            End While
'            VMioArchivo.Close()
'            Me.Cursor = Cursors.Default
'        End If
'    Catch ex As Exception
'        VGobjDepuracion.ArchivoPlano("KMRBatch-18135 --> : " & ex.Message)
'        MsgBox("KMRBatch-18135 Conflicto al leer archivo", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "KM Replicador Batch")
'    End Try
'End If
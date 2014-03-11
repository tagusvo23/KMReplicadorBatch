Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Data.OracleClient
Imports System.Data.SqlClient
''' <remarks>Contiene todos los componentes necesarios para realizar la conexion con una base de datos</remarks>
Public Class BaseDatos

#Region "Estructuras"
    ''' <remarks>Contiene todos los datos de conexion</remarks>
    Public Structure Base
        ''' <summary>
        ''' IP donde se encuentra la base de datos
        ''' </summary>
        Dim Servidor As String
        ''' <summary>
        ''' Usuario de acceso a la base de datos
        ''' </summary>
        Dim Usuario As String
        ''' <summary>
        ''' Base de datos de conexion
        ''' </summary>
        Dim Base As String
        ''' <summary>
        ''' Contraseña de la base de datos
        ''' </summary>
        Dim Contraseña As String
        ''' <summary>
        ''' La autentificacion contiene seguridad integarda
        ''' </summary>
        Dim Seguridad_Integrada As Boolean
        ''' <summary>
        ''' Instancia de la base
        ''' </summary>
        Dim Instancia As String
        ''' <summary>
        ''' Tipo de base de datos
        ''' </summary>
        Dim Tipo As TipoBase
        ''' <summary>
        ''' Puerto de conexion a la base
        ''' </summary>
        Dim Puerto As String
    End Structure

    ''' <remarks>Tipos de base de datos que existen</remarks>
    Public Enum TipoBase As Byte
        Access = 1
        Oracle = 2
        SQLServer = 3
        Informix = 4
        MySQL = 5
        Sybase = 6
    End Enum

    ''' <remarks>Errores que se han documentado en SLQ Server</remarks>
    Public Enum errorSQLServer
        TablNoExiste = 208
        TablaYaExistente = 2714
        ServidorYaNoDisponibe = 64
        ErrorNivelTransporte = 10054
        EspacioLleno = 1105
        LogTransaccionesLleno = 9002
        PermisoDenegado = 262
        InicioSesionError = 4060
        ErrorConexion = 18456
        TablaIndiceNoExiste = 1088
        SintaxisIncorrecta = 102
        IndiceExistente = 1913
        CampoIndiceNoExiste = 1911
        ValorDuplicado = 2601
        ErrorConversionDatos = 8114
        ServidorNoEncontrado = 10060
    End Enum

    ''' <remarks>Errores que se han documentado de Oracle</remarks>
    Public Enum errorOracle
        TablaNoExiste = 942
        TablaYaExiste = 955
        LogTransaccionesLleno
        PermisoDenegado = 1031
        InicioSesionError = 1017
        IndiceExistente = 955
        CampoIndiceNoExiste = 904
        ValorDuplicado = 1
        ServidorNoEncontrado = 12541
        AliasInstanciaNoExiste = 12514
        UsuarioBloqueado = 28000
        ErrorProtocolo = 12560
        ErrorAdaptadorProtocolo = 12538
        InicioReinicioEnProgreso = 1033
        TimeOutConexion = 12170
    End Enum

    ''' <remarks>Error que se han documentado de MySQL</remarks>
    Public Enum errorMySQL
        ServidorNoDisponible = 0
        ServidorNoEncontrado = 1024
        DiscoLleno = 1021
        ClaveDuplicada = 1022
    End Enum

#End Region

    ''' <summary>
    ''' Devuelve la cadena de conexion dependiendo el tipo de base de datos
    ''' </summary>
    ''' <param name="DatosBase">Informacion de la base de datos</param>
    Public Shared Function CadenaConexion(ByVal DatosBase As Base) As String
        '***********************************************************************************
        '       Devuelve la cadena de conexion dependiendo del tipo de base
        '***********************************************************************************
        Dim VLstrConexion As String 'Guarda la cadena de conexion de la base de datos

        Select Case DatosBase.Tipo
            Case TipoBase.MySQL
                VLstrConexion = "Server = " & DatosBase.Servidor & _
                                ";database = " & DatosBase.Base & _
                                ";uid = " & DatosBase.Usuario & _
                                ";pwd = " & DatosBase.Contraseña & _
                                ";Port = " & DatosBase.Puerto

            Case TipoBase.Oracle

                VLstrConexion = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" & DatosBase.Servidor & ") " & _
                                "(PORT=" & DatosBase.Puerto & "))(CONNECT_DATA=(SERVICE_NAME=" & DatosBase.Instancia & ")))" & _
                                ";uid=" & DatosBase.Usuario & ";pwd=" & DatosBase.Contraseña & ";"
            Case TipoBase.SQLServer
                If DatosBase.Seguridad_Integrada Then
                    VLstrConexion = "Data Source=" & DatosBase.Servidor & "," & DatosBase.Puerto & _
                                                 ";Initial Catalog=" & DatosBase.Base & _
                                                 ";Integrated Security=SSPI;"
                Else
                    VLstrConexion = "Server = " & DatosBase.Servidor & "," & DatosBase.Puerto & _
                         ";Initial Catalog = " & DatosBase.Base & _
                         ";uid = " & DatosBase.Usuario & _
                         ";pwd = " & DatosBase.Contraseña
                End If
            Case Else
                Throw New Exception("No se reconoce el tipo de la base de datos")
        End Select
        CadenaConexion = VLstrConexion
    End Function

    ''' <summary>
    ''' Crea una conexion con la base de datos
    ''' </summary>
    ''' <param name="DatosBase">Informacion de la base de datos</param>
    Public Shared Function ConectarBase(ByVal DatosBase As Base) As Object
        Dim VLdbsBase As Object
        Dim VLintIntentos As Integer

        Select Case DatosBase.Tipo
            Case TipoBase.MySQL
                VLdbsBase = New MySqlConnection
            Case TipoBase.SQLServer
                VLdbsBase = New SqlConnection
            Case TipoBase.Oracle
                VLdbsBase = New OracleConnection
            Case Else
                Throw New Exception("No se reconoce el tipo de la base de datos")
        End Select
        Try
Conectar:   VLdbsBase.ConnectionString = CadenaConexion(DatosBase)
            VLdbsBase.Open()
        Catch ex As MySqlException
            Select Case CType(ex.Number, errorMySQL)
                Case errorMySQL.ServidorNoDisponible Or errorMySQL.ServidorNoEncontrado
                    If VLintIntentos <= 3 Then
                        Threading.Thread.Sleep(100)
                        VLintIntentos += 1
                        GoTo Conectar
                    Else
                        Throw New Exception("Se realizaron 3 intentos de conexion a la base de datos y esta no responde. Intente despues")
                    End If
                Case Else
                    Throw New Exception("Error al abrir la base de datos." & vbNewLine & "Mensaje: " & ex.Message & vbNewLine & "Error: " & ex.Number)
            End Select
        Catch ex As SqlException
            Select Case CType(ex.Number, errorSQLServer)
                Case errorSQLServer.ErrorConexion Or errorSQLServer.ServidorNoEncontrado Or errorSQLServer.ServidorYaNoDisponibe
                    If VLintIntentos <= 3 Then
                        Threading.Thread.Sleep(100)
                        VLintIntentos += 1
                        GoTo Conectar
                    Else
                        Throw New Exception("Se realizaron 3 intentos de conexion a la base de datos y esta no responde. Intente despues")
                    End If
                Case Else
                    Throw New Exception("Error al abrir la base de datos." & vbNewLine & "Mensaje: " & ex.Message & vbNewLine & "Error: " & ex.Number)
            End Select
        Catch ex As OracleException
            Select Case CType(ex.Code, errorMySQL)
                Case errorMySQL.ServidorNoDisponible Or errorMySQL.ServidorNoEncontrado
                    If VLintIntentos <= 3 Then
                        Threading.Thread.Sleep(100)
                        VLintIntentos += 1
                        GoTo Conectar
                    Else
                        Throw New Exception("Se realizaron 3 intentos de conexion a la base de datos y esta no responde. Intente despues")
                    End If
                Case Else
                    Throw New Exception("Error al abrir la base de datos." & vbNewLine & "Mensaje: " & ex.Message & vbNewLine & "Error: " & ex.Code)
            End Select
        Catch ex As Exception
            Throw New Exception("Error al abrir la base de datos." & vbNewLine & "Mensaje: " & ex.Message)
        End Try
        Return VLdbsBase
    End Function
End Class
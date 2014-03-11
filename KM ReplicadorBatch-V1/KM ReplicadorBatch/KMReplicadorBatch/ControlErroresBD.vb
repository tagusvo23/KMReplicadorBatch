Imports MySql.Data.MySqlClient
Imports System.Data.OracleClient
Imports System.Data.SqlClient

Public Module ControlErroresBD
    '**************************************************
    ' ÚlTiMa MoDiFiCaCiÓn: 17 de Septiembre de 2009 SRD
    '**************************************************
#Region "Catálogo Errores"
    Public Enum errorSQLServer
        TablaNoExiste = 208
        TablaYaExiste = 2714
        ServidorYaNoDisponible = 64
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
        DemasiadosValores = 213
    End Enum
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
        PerdidaConexion = 3113 'La conexion del servidor y el cliente se ha perdido 
        IDCnxEspecificado = 12154 ' tns: no se ha podido resolver el identificador de conexion especificado
        DemasiadosValores = 913 'Indica que hay mas valores en el values... la tabla no tiene tantos campos...
        DeshabilitaFire = 12571 '12571: TNS:fallo en el escritor del paquete. No se puede tener acceso por el firewall del antivirus
        RecuperaCred = 12638 '12638 fallo en recueperacion de credenciales SE DEBE A QUE SE PERDIO LA CONEXION CON EL SERVIDOR
        FaltaExpresion = 936 'La cadena SQL esta incompleta o le falta un elemento 
    End Enum
    Public Enum errorMySQL
        TablaNoExiste = 1146
        TablaNoSePuedeCrear = 1005
        TablaYaExiste = 1050
        ServidorNoDisponible = 1042
        BaseInexistente = 1049
        'InicioSesionError
        NoAutorizado = 1130 'El equipo no está autorizado a conectarse a ese servidor MySQL
        AccesoDenegadoBD = 1044 'Access denied for user to database 
        TipoDatoIncorrecto = 1366 'tipo de dato incorrecto
        ColumnaNoExiste = 1054 '1054 la columna no existe en la tabla
        ErrorSintaxis = 1064 '1064 error de sintaxis en la consulta 
    End Enum
#End Region
    Public Function obtenerErrorBD(ByVal ex As SqlException, Optional ByVal VPstrTabla As String = "") As String

        '****************************************************************************
        'Interpreta los errores de SQL Server para mejor entendimiento del usuario  '
        '---PARAMETROS                                                              '
        '   1. VPobjError: La excepción que contiene la información del error       '
        '---VALORES DEVUELTOS                                                       '
        '   Cadena con el error interpretado                                        '
        '****************************************************************************
        Dim VLstrError As String

        Select Case CType(ex.Number, errorSQLServer)
            Case errorSQLServer.TablaNoExiste
                VLstrError = "La tabla no existe en la base de datos." & vbNewLine & ex.Message
            Case errorSQLServer.TablaYaExiste
                VLstrError = "Error al crear la tabla, porque ya existe." & vbNewLine & ex.Message
            Case errorSQLServer.ServidorYaNoDisponible
                VLstrError = "El servidor no responde o no se encuentra disponible. " & vbNewLine & ex.Message
            Case errorSQLServer.ErrorNivelTransporte
                VLstrError = "Se perdió la conexión con la base de datos bitácora. " & vbNewLine & ex.Message
            Case errorSQLServer.EspacioLleno
                VLstrError = "El espacio de la base de datos se encuentra lleno." & vbNewLine & ex.Message
            Case errorSQLServer.LogTransaccionesLleno
                VLstrError = "El log de de transacciones de la base de datos se encuentra lleno." & vbNewLine & ex.Message
            Case errorSQLServer.PermisoDenegado
                VLstrError = "No tiene los permisos necesarios para acceder a la base de datos." & vbNewLine & ex.Message
            Case errorSQLServer.InicioSesionError
                VLstrError = "Error al iniciar sesión con el servidor de base de datos." & vbNewLine & ex.Message
            Case errorSQLServer.ErrorConexion
                VLstrError = "Se perdió la conexión con la base de datos bitácora. " & vbNewLine & ex.Message
            Case errorSQLServer.TablaIndiceNoExiste
                VLstrError = "La tabla no existe." & vbNewLine & ex.Message
            Case errorSQLServer.SintaxisIncorrecta
                VLstrError = "Sintáxis incorrecta en la cadena SQL." & vbNewLine & ex.Message
            Case errorSQLServer.IndiceExistente
                VLstrError = ""
            Case errorSQLServer.CampoIndiceNoExiste
                VLstrError = "El campo no existe en la base de datos." & vbNewLine & ex.Message
            Case errorSQLServer.ValorDuplicado
                VLstrError = "El registro ya ha sido insertado en la base de datos." & vbNewLine & ex.Message
            Case errorSQLServer.ErrorConversionDatos
                VLstrError = "Error al convertir los datos." & vbNewLine & ex.Message
            Case errorSQLServer.ServidorNoEncontrado
                VLstrError = "El servidor no se encuentra o no esta disponible." & vbNewLine & ex.Message
            Case errorSQLServer.DemasiadosValores
                VLstrError = "Se está tratando de insertar más valores que los que contiene la tabla." & vbNewLine & ex.Message
            Case Else
                VLstrError = "Error al extraer información." & vbNewLine & ex.Number & ": " & ex.Message
        End Select

        obtenerErrorBD = VLstrError

    End Function
    Public Function obtenerErrorBD(ByVal ex As OracleException, Optional ByVal VPstrTabla As String = "") As String
        '************************************************************************
        'Interpreta los errores de Oracle para mejor entendimiento del usuario  '
        '---PARAMETROS                                                          '
        '   1. VPobjError: La excepción que contiene la información del error   '
        '---VALORES DEVUELTOS                                                   '
        '   Cadena con el error interpretado                                    '
        '************************************************************************
        Dim VLstrError As String

        Select Case CType(ex.Code, errorOracle)
            Case errorOracle.TablaNoExiste
                VLstrError = "La tabla no existe en la base de datos." & vbNewLine & ex.Message
            Case errorOracle.TablaYaExiste
                VLstrError = "Error al crear la tabla, porque ya existe." & vbNewLine & ex.Message
            Case errorOracle.LogTransaccionesLleno
                VLstrError = "El log de de transacciones de la base de datos se encuentra lleno." & vbNewLine & ex.Message
            Case errorOracle.PermisoDenegado
                VLstrError = "No tiene los permisos necesarios para acceder a la base de datos." & vbNewLine & ex.Message
            Case errorOracle.InicioSesionError
                VLstrError = "Error al iniciar sesión con el servidor de base de datos." & vbNewLine & ex.Message
            Case errorOracle.IndiceExistente
                VLstrError = ""
            Case errorOracle.ValorDuplicado
                VLstrError = "El registro ya ha sido insertado en la base de datos." & vbNewLine & ex.Message
            Case errorOracle.ServidorNoEncontrado
                VLstrError = "El servidor no se encuentra o no esta disponible." & vbNewLine & ex.Message
            Case errorOracle.AliasInstanciaNoExiste
                VLstrError = "El alias que esta utilizando no existe." & vbNewLine & ex.Message
            Case errorOracle.UsuarioBloqueado
                VLstrError = "El usuario esta bloqueado y no tiene acceso a la base de datos." & vbNewLine & ex.Message
            Case errorOracle.ErrorProtocolo
                VLstrError = "Error en el protocolo, revisar los parámetros de configuración." & vbNewLine & ex.Message
            Case errorOracle.ErrorAdaptadorProtocolo
                VLstrError = "Error en el protocolo, revisar los parámetros de configuración." & vbNewLine & ex.Message
            Case errorOracle.InicioReinicioEnProgreso
                VLstrError = "No se puede tener acceso a la base de datos, la base de datos está reiniciandose." & vbNewLine & ex.Message
            Case errorOracle.IDCnxEspecificado
                VLstrError = "Esta omitiendo un valor en los parámetros de configuración." & vbNewLine & ex.Message
            Case errorOracle.TimeOutConexion
                VLstrError = "El servidor tarda mucho en responder. " & vbNewLine & ex.Message
            Case errorOracle.PerdidaConexion
                VLstrError = "La conexión entre el servdor y el cliente se ha perdido." & vbNewLine & ex.Message
            Case errorOracle.DemasiadosValores
                VLstrError = "Se está tratando de insertar más valores que los que contiene la tabla." & vbNewLine & ex.Message
            Case errorOracle.RecuperaCred
                VLstrError = "Se ha perdido comunicación con el servidor de la base de datos." & vbNewLine & ex.Message
            Case errorOracle.DeshabilitaFire
                VLstrError = "Se necesita habilitar el puerto de oracle en el firewall del antivirus para establecer la conexión. " & vbNewLine & ex.Message
            Case errorOracle.FaltaExpresion
                VLstrError = "Error en la cadena SQL, compruebe la sintaxis de la instrucción." & vbNewLine & ex.Message
            Case Else
                VLstrError = "Error al extraer información." & vbNewLine & ex.Code & ": " & ex.Message
        End Select

        obtenerErrorBD = VLstrError
    End Function
    Public Function obtenerErrorBD(ByVal ex As MySqlException, Optional ByVal VPstrTabla As String = "") As String
        '***********************************************************************
        'Interpreta los errores de MySQL para mejor entendimiento del usuario  '
        '---PARAMETROS                                                         '
        '   1. VPobjError: La excepción que contiene la información del error  '
        '---VALORES DEVUELTOS                                                  '
        '   Cadena con el error interpretado                                   '
        '***********************************************************************
        Dim VLstrError As String

        Select Case CType(ex.Number, errorMySQL)
            Case errorMySQL.TablaNoExiste
                VLstrError = "La tabla no existe o el usuario no tiene permisos  para verla." & vbNewLine & ex.Message
            Case errorMySQL.TablaNoSePuedeCrear
                VLstrError = "La tabla no se puede crear." & vbNewLine & ex.Message
            Case errorMySQL.TablaYaExiste
                VLstrError = "Error al crear la tabla, porque ya existe." & vbNewLine & ex.Message
            Case errorMySQL.ServidorNoDisponible
                VLstrError = "No se pudo conectar con el host remoto, no se encuentra o no está disponible." & vbNewLine & ex.Message
            Case errorMySQL.ColumnaNoExiste
                VLstrError = "La columna no existe en la tabla." & vbNewLine & ex.Message
            Case errorMySQL.BaseInexistente
                VLstrError = "Error al establecer conexión con la base de datos, ya que la base no existe en el servidor." & vbNewLine & ex.Message
            Case errorMySQL.NoAutorizado
                VLstrError = "Acceso denegado para el usuario a la base de datos." & vbNewLine & ex.Message
            Case errorMySQL.AccesoDenegadoBD
                VLstrError = "El equipo no está autorizado para conectarse a ese servidor MySQL." & vbNewLine & ex.Message
            Case errorMySQL.ErrorSintaxis
                VLstrError = "Error en el código al crear la cadena SQL." & vbNewLine & ex.Message
            Case errorMySQL.TipoDatoIncorrecto
                VLstrError = "El tipo de dato que esta ingresando es incorrecto." & vbNewLine & ex.Message
            Case Else
                VLstrError = "Error al extraer información. " & vbNewLine & ex.Number & ": " & ex.Message
        End Select

        obtenerErrorBD = VLstrError
    End Function

End Module

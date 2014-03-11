Imports System.Net
Imports System.Net.Dns
Imports System.Data.SqlClient
Imports System.Data.OracleClient
Imports MySql.Data.MySqlClient

''' <remarks>Permite escribir en la bitacora</remarks>
Public Class Bitacora

#Region "Enumeraciones"
    ''' <remarks>Tipos de registro que existen actualmente</remarks>
    Public Enum TipoRegistro As Byte
        Logon = 1
        Logoff = 2
        Alertas = 3
        ProblemaAplicacion = 4
        BorrarRegistrosBD = 5
        Tickets = 6
        CambioConfiguracion = 7
        Avisos = 8
    End Enum

    ''' <remarks>Estados probables de la alerta</remarks>
    Public Enum EstadoxAlerta As Byte
        NoAplica = 0
        NoAtendido = 1
        Pendiente = 2
        Atendido = 3
    End Enum

    ''' <remarks>Muestra todas las aplicaciones que existen actualmente en perfiles</remarks>
    Public Enum AplicacionesPerfiles As Byte
        ConsolaAdministrador = 0
        Replicador = 1
        Receptor = 2
        Ruteador = 3
        Consola = 4
        Autenttifica = 5
        ControlBD = 6
        Informa = 7
        Administrador = 8
        Previo = 9
        ServicioPrevio = 10
        Escalamientos = 11
    End Enum

    ''' <remarks>Muestra todas las aplicaciones que existen actualmente en Key Monitor</remarks>
    Public Enum AplicacionesKM As Byte
        Administrador = 1
        AdministradorTransaccional = 2
        Analizador = 3
        Receptor = 4
        Replicador = 5
        Estadístico = 6
        Informa = 7
        ControlBD = 8
        GuíaHTTP = 10
        Autentifica = 11
        GuíaHTTPOffLine = 12
        ReplicadorOffLine = 13
        GuíaFiltros = 14
        ReceptorPorcentual = 18
        ReceptorCentral = 44
        Consola = 50
    End Enum

    Public Enum Producto As Byte
        KeyMonitor = 1
        Perfiles = 2
    End Enum
#End Region

#Region "Variables"
    Private Shared VMcolaSQL As New Queue
    Private Shared VMcolaInsertar As New Queue
    Private Shared VMhiloSQL As New Threading.Thread(AddressOf Agregar)
    Private Shared VMhiloInsertar As New Threading.Thread(AddressOf InsertarSegundo)
    'Variables de la clase
    Private Shared VMstrUsuario As String
    Private Shared VMintLayout As Integer
    Private Shared VMintFormato As Integer
    Private Shared VMobjAplicacionKM As AplicacionesKM
    Private Shared VMobjAplicacionPerfiles As AplicacionesPerfiles
    Private Shared VMbaseBitacora As BaseDatos.Base
    Private Shared VMobjProducto As Producto
    Private Shared VMstrPCName As String
    Private Shared VMstrIP As String
    'Variables para la bitácora
    Private _Usuario As String
    Private _Layout As Integer
    Private _Formato As Integer
    Private _Grupo As Integer
    Private _Perfil As Integer
    Private _Alerta As Integer
    Private _IdFiltro As Integer
    Private _TipoRegistro As TipoRegistro
    Private _Fecha As String
    Private _Hora As String
    Private _IdTran As String
    Private _Comparacion As String
    Private _Escalamiento As Integer
    Private _Mensaje As String
    Private _Limite As Integer
    Private _Tiempo As Integer
    Private _Referencia As String
    Private _Condicion As String
    Private _Estado As EstadoxAlerta
    Private _Prioridad As Integer
    Private _FalsoPositivo As Boolean
    Private _FactorRiesgo As Integer
    'Variables para las observaciones
    Private _Id As Integer
    Private _Observaciones As String
    'Variables que permiten el comportamiento
    Private _SegundoPlano As Boolean
    Private _IP As String
    Private _NombrePc As String
#End Region

#Region "Propiedades Shared"
    ''' <summary>
    ''' El usuario que genero el registro
    ''' </summary>
    Public Shared WriteOnly Property Usuario() As String
        Set(ByVal value As String)
            VMstrUsuario = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la aplicacion de Key Monitor que va a generar el registro
    ''' </summary>
    Public Shared WriteOnly Property AplicacionKM() As AplicacionesKM
        Set(ByVal value As AplicacionesKM)
            VMobjAplicacionKM = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la aplicacion de Perfiles que va a generar el registro
    ''' </summary>
    Public Shared WriteOnly Property AplicacionPerfiles() As AplicacionesPerfiles
        Set(ByVal value As AplicacionesPerfiles)
            VMobjAplicacionPerfiles = value
        End Set
    End Property

    ''' <summary>
    ''' Contiene todos los datos de conexion para la base bitacora
    ''' </summary>
    Public Shared WriteOnly Property BaseBitacora() As BaseDatos.Base
        Set(ByVal value As BaseDatos.Base)
            VMbaseBitacora = value
        End Set
    End Property

    ''' <summary>
    ''' Layout con el cual se esta trabajando
    ''' </summary>
    Public Shared WriteOnly Property Layout() As Integer
        Set(ByVal value As Integer)
            VMintLayout = value
        End Set
    End Property

    ''' <summary>
    ''' Formato del cual se esta trabajando
    ''' </summary>
    Public Shared WriteOnly Property Formato() As Integer
        Set(ByVal value As Integer)
            VMintFormato = value
        End Set
    End Property

    ''' <summary>
    ''' Producto que se esta utilizando, Key Monitor o Perfiles
    ''' </summary>
    Public Shared WriteOnly Property Produto() As Producto
        Set(ByVal value As Producto)
            VMobjProducto = value
        End Set
    End Property

    ''' <summary>
    ''' Ip del equipo actual
    ''' </summary>
    Public Shared ReadOnly Property IP() As String
        Get
            If VMstrIP Is Nothing Then
                VMstrIP = DeterminarIP()
            End If
            Return VMstrIP
        End Get
    End Property

    ''' <summary>
    ''' Nombre del Equipo
    ''' </summary>
    Public Shared ReadOnly Property NombrePC() As String
        Get
            If VMstrPCName Is Nothing Then
                VMstrPCName = My.Computer.Name
            End If
            Return VMstrPCName
        End Get
    End Property
#End Region

#Region "Propiedades"
    ''' <summary>
    ''' Usuario con el que se va a agregar el registro
    ''' </summary>
    Public WriteOnly Property UsuarioRegistro() As String
        Set(ByVal value As String)
            _Usuario = value
        End Set
    End Property

    ''' <summary>
    ''' Layout con el que se va a agregar el registro
    ''' </summary>
    Public WriteOnly Property LayoutRegistro() As Integer
        Set(ByVal value As Integer)
            _Layout = value
        End Set
    End Property

    ''' <summary>
    ''' Formato con el que se va a agregar el registro
    ''' </summary>
    Public WriteOnly Property FormatoRegistro() As Integer
        Set(ByVal value As Integer)
            _Formato = value
        End Set
    End Property

    ''' <summary>
    ''' Grupo con el cual se esta trabajando
    ''' </summary>
    Public WriteOnly Property Grupo() As Integer
        Set(ByVal value As Integer)
            _Grupo = value
        End Set
    End Property

    ''' <summary>
    ''' Perfil con el que se esta trabajando
    ''' </summary>
    Public WriteOnly Property Perfil() As Integer
        Set(ByVal value As Integer)
            _Perfil = value
        End Set
    End Property

    ''' <summary>
    ''' Es el identificador del alertamiento
    ''' </summary>
    Public WriteOnly Property Alerta() As String
        Set(ByVal value As String)
            _Alerta = value
        End Set
    End Property

    ''' <summary>
    ''' Identificador del filtro
    ''' </summary>
    Public WriteOnly Property IdFiltro() As Integer
        Set(ByVal value As Integer)
            _IdFiltro = value
        End Set
    End Property

    ''' <summary>
    ''' Tipo de informacion a almacenar
    ''' </summary>
    Public WriteOnly Property Registro() As TipoRegistro
        Set(ByVal value As TipoRegistro)
            _TipoRegistro = value
        End Set
    End Property

    ''' <summary>
    ''' IP con la que se va a registrar la alerta
    ''' </summary>
    Public WriteOnly Property IPRegistro() As String
        Set(ByVal value As String)
            _IP = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre de la PC con la que se va a registrar la alerta
    ''' </summary>
    Public WriteOnly Property NombrePCRegistro() As String
        Set(ByVal value As String)
            _NombrePc = value
        End Set
    End Property

    ''' <summary>
    ''' La fecha en la que se registra este registro
    ''' </summary>
    Public WriteOnly Property Fecha() As String
        Set(ByVal value As String)
            _Fecha = value
        End Set
    End Property

    ''' <summary>
    ''' La hora en la que se regitra este registro
    ''' </summary>
    Public WriteOnly Property Hora() As String
        Set(ByVal value As String)
            _Hora = value
        End Set
    End Property

    ''' <summary>
    ''' Indicador de la transaccion
    ''' </summary>
    Public WriteOnly Property IdTran() As String
        Set(ByVal value As String)
            _IdTran = value
        End Set
    End Property

    ''' <summary>
    ''' Campo que nos indica la comparacion que se realizo para la alerta
    ''' </summary>
    Public WriteOnly Property Comparacion() As String
        Set(ByVal value As String)
            _Comparacion = value
        End Set
    End Property

    ''' <summary>
    ''' Indicador de cuantos escalamientos se han mandado
    ''' </summary>
    Public WriteOnly Property Escalamiento() As Integer
        Set(ByVal value As Integer)
            _Escalamiento = value
        End Set
    End Property

    ''' <summary>
    ''' Mensaje del registro
    ''' </summary>
    Public WriteOnly Property Mensaje() As String
        Set(ByVal value As String)
            _Mensaje = value
        End Set
    End Property

    ''' <summary>
    ''' Numero de transacciones que disparan la alerta
    ''' </summary>
    Public WriteOnly Property Limite() As Integer
        Set(ByVal value As Integer)
            _Limite = value
        End Set
    End Property

    ''' <summary>
    ''' Tiempo necesario para generar una alerta
    ''' </summary>
    Public WriteOnly Property Tiempo() As Integer
        Set(ByVal value As Integer)
            _Tiempo = value
        End Set
    End Property

    ''' <summary>
    ''' Condicion que se debe de cumplir
    ''' </summary>
    Public WriteOnly Property Condicion() As String
        Set(ByVal value As String)
            _Condicion = value
        End Set
    End Property

    ''' <summary>
    ''' La referncia de la alerta generada
    ''' </summary>
    Public WriteOnly Property Referencia() As String
        Set(ByVal value As String)
            _Referencia = value
        End Set
    End Property

    ''' <summary>
    ''' Indicador del estado que se encuentra la alerta
    ''' </summary>
    Public WriteOnly Property Estado() As EstadoxAlerta
        Set(ByVal value As EstadoxAlerta)
            _Estado = value
        End Set
    End Property

    ''' <summary>
    ''' La prioridad de la alerta
    ''' </summary>
    Public WriteOnly Property Prioridad() As Integer
        Set(ByVal value As Integer)
            _Prioridad = value
        End Set
    End Property

    ''' <summary>
    ''' Indicador si la alerta es fraude o no
    ''' </summary>
    Public WriteOnly Property FalsoPositivo() As Boolean
        Set(ByVal value As Boolean)
            _FalsoPositivo = value
        End Set
    End Property

    ''' <summary>
    ''' Indicador del valor del Factor de Riesgo
    ''' </summary>
    Public WriteOnly Property FactorRiesgo() As Integer
        Set(ByVal value As Integer)
            _FactorRiesgo = value
        End Set
    End Property

    ''' <summary>
    ''' ID de alerta
    ''' </summary>
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

    ''' <summary>
    ''' Observaciones a agregar
    ''' </summary>
    Public WriteOnly Property Observaciones() As String
        Set(ByVal value As String)
            _Observaciones = value
        End Set
    End Property

    ''' <summary>
    ''' Permite la ejecucion del los comandos en segundo plano
    ''' </summary>
    Public WriteOnly Property SegundoPlano() As Boolean
        Set(ByVal value As Boolean)
            _SegundoPlano = value
        End Set
    End Property
#End Region

#Region "Métodos Públicos"

    Public Sub New()
        Limpiar()
    End Sub

    ''' <summary>
    ''' Guarda la informacion en la bitacora
    ''' </summary>
    Public Sub GuardarBitacora()
        Dim VLstrSQL As String = ""

        If Not _Usuario Is Nothing Then
            If _Usuario.Trim.Length = 0 Then
                Throw New System.Exception("Se tiene que agregar un usuario")
            End If
        Else
            Throw New System.Exception("Se tiene que agregar un usuario")
        End If

        If _IP Is Nothing Then
            _IP = DeterminarIP()
        End If

        If _NombrePc Is Nothing Then
            _NombrePc = My.Computer.Name
        End If

        If VMobjProducto = 0 Then
            Throw New System.Exception("Se tiene que establecer un producto (Key Monitor o Perfiles)")
        End If

        If VMobjProducto = Producto.KeyMonitor And VMobjAplicacionKM = 0 Then
            Throw New System.Exception("Se tiene que establecer una aplicacion")
        End If

        Try
            Select Case VMbaseBitacora.Tipo
                Case BaseDatos.TipoBase.Oracle
                    VLstrSQL = "INSERT INTO tbitacora (Id,IdProducto,IdUsuario,IdLay,IdFormato," & _
                    "IdGrupo,IdPerfil,IdAlerta,IdFiltro,IdAplicacion,IdTipoReg,IP,NombrePC,Fecha,Hora," & _
                    "IdTran,Comparacion,Escalamiento,Mensaje,Limite,Tiempo,Referencia,Condicion,IdEstado," & _
                    "IdPrioridad,FalsoPositivo,FactorRiesgo) VALUES (IdConsecutivo.nextval, "
                Case Else
                    VLstrSQL = "INSERT INTO tbitacora (IdProducto,IdUsuario,IdLay,IdFormato,IdGrupo," & _
                    "IdPerfil,IdAlerta,IdFiltro,IdAplicacion,IdTipoReg,IP,NombrePC,Fecha,Hora,IdTran," & _
                    "Comparacion,Escalamiento,Mensaje,Limite,Tiempo,Referencia,Condicion,IdEstado," & _
                    "IdPrioridad,FalsoPositivo,FactorRiesgo) VALUES ("
            End Select

            _Fecha = IIf(_Fecha.Trim.Length = 0, Format(Date.Now, "yyyy-MM-dd"), _Fecha)
            _Hora = IIf(_Hora.Trim.Length = 0, Format(Date.Now, "HH:mm:ss.fffff"), _Hora)
            _Mensaje = _Mensaje.Replace("'", "")
            _Referencia = _Referencia.Replace("'", "")
            _IdTran = _IdTran.Replace("'", "")
            _Comparacion = _Comparacion.Replace("'", "")

            VLstrSQL &= VMobjProducto.GetHashCode & ","
            VLstrSQL &= "'" & _Usuario & "', "
            VLstrSQL &= VMintLayout & ", "
            VLstrSQL &= VMintFormato & ", "
            VLstrSQL &= _Grupo & ", "
            VLstrSQL &= _Perfil & ", "
            VLstrSQL &= _Alerta & ", "
            VLstrSQL &= _IdFiltro & ", "
            If VMobjProducto = Producto.KeyMonitor Then
                VLstrSQL &= VMobjAplicacionKM.GetHashCode & ", "
            Else
                VLstrSQL &= VMobjAplicacionPerfiles.GetHashCode & ", "
            End If
            VLstrSQL &= _TipoRegistro.GetHashCode & ", "
            VLstrSQL &= "'" & _IP & "', "
            If VMstrPCName.Length > 15 Then
                VLstrSQL &= "'" & _NombrePc.Substring(0, 15) & "', "
            Else
                VLstrSQL &= "'" & _NombrePc & "',"
            End If
            VLstrSQL &= "'" & _Fecha & "', "
            VLstrSQL &= "'" & _Hora & "', "
            If _IdTran.Length > 80 Then
                VLstrSQL &= "'" & _IdTran.Substring(0, 80) & "', "
            Else
                VLstrSQL &= "'" & _IdTran & "', "
            End If
            If _Comparacion.Length > 1000 Then
                VLstrSQL &= "'" & _Comparacion.Substring(0, 1000) & "', "
            Else
                VLstrSQL &= "'" & _Comparacion & "', "
            End If
            VLstrSQL &= _Escalamiento & ", "
            If _Mensaje.Length > 1000 Then
                VLstrSQL &= "'" & _Mensaje.Substring(0, 1000) & "', "
            Else
                VLstrSQL &= "'" & _Mensaje & "', "
            End If
            VLstrSQL &= _Limite & ","
            VLstrSQL &= _Tiempo & ","
            If _Referencia.Length > 1000 Then
                VLstrSQL &= "'" & _Referencia.Substring(0, 1000) & "', "
            Else
                VLstrSQL &= "'" & _Referencia & "', "
            End If
            If _Condicion.Length > 1000 Then
                VLstrSQL &= "'" & _Condicion.Substring(0, 1000) & "', "
            Else
                VLstrSQL &= "'" & _Condicion & "', "
            End If
            VLstrSQL &= _Estado.GetHashCode & ", "
            VLstrSQL &= _Prioridad & ", "
            VLstrSQL &= IIf(_FalsoPositivo, 1, 0) & ", "
            VLstrSQL &= _FactorRiesgo & ")"
            If _SegundoPlano Then
                SyncLock VMcolaInsertar
                    VMcolaInsertar.Enqueue(VLstrSQL)
                End SyncLock
                If Not VMhiloInsertar.IsAlive Then
                    VMhiloInsertar = New Threading.Thread(AddressOf InsertarSegundo)
                    VMhiloInsertar.Start()
                End If
            Else
                Insertar(VLstrSQL)
            End If
        Catch ex As Exception
            Agregar(VLstrSQL)
            Throw New Exception("Problema al determinar el query: " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Guarda la informacion en la observaciones
    ''' </summary>
    Public Sub GuardarObservaciones()
        Dim VLstrSQL As String = ""

        If _Id = 0 Then
            Throw New Exception("El identificador de la alerta no puede ser 0")
        End If

        Try
            _Fecha = IIf(_Fecha.Trim.Length = 0, Format(Date.Now, "yyyy-MM-dd"), _Fecha)
            _Hora = IIf(_Hora.Trim.Length = 0, Format(Date.Now, "HH:mm:ss.fffff"), _Hora)
            _Observaciones = _Observaciones.Replace("'", "")

            'VLstrSQL = "INSERT INTO tbitobse Values (" & _Id & ",'" & _Fecha & "','" & _Hora & "','" & _
            '            IIf(_Observaciones.Length > 1000, _Observaciones.Substring(0, 1000), _Observaciones) & "'," & _
            '           _Estado & ",'" & _Usuario & "')"

            VLstrSQL = "INSERT INTO tbitaobse Values (" & _Id & ",'" & _Fecha & "','" & _Hora & "','" & _
                        _Observaciones & "'," & _
                      _Estado & ",'" & _Usuario & "')"

            SyncLock VMcolaInsertar
                VMcolaInsertar.Enqueue(VLstrSQL)
            End SyncLock
            If Not VMhiloInsertar.IsAlive Then
                VMhiloInsertar = New Threading.Thread(AddressOf InsertarSegundo)
                VMhiloInsertar.Start()
            End If
        Catch ex As OracleException
            Agregar(VLstrSQL)
            Throw New Exception("Error al agregar la observación" & vbNewLine & "No. Error: " & ex.Code & vbNewLine & "Error: " & ex.Message)
        Catch ex As SqlException
            Agregar(VLstrSQL)
            Throw New Exception("Error al agregar la observación" & vbNewLine & "No. Error: " & ex.Number & vbNewLine & "Error: " & ex.Message)
        Catch ex As MySqlException
            Agregar(VLstrSQL)
            Throw New Exception("Error al agregar la observación" & vbNewLine & "No. Error: " & ex.Number & vbNewLine & "Error: " & ex.Message)
        Catch ex As Exception
            Agregar(VLstrSQL)
            Throw New Exception("Error al agregar la observación: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Limpiar las variables para que se pueda generar otra consulta
    ''' </summary>
    Public Sub Limpiar()
        _Usuario = VMstrUsuario
        _Layout = VMintLayout
        _Formato = VMintFormato
        _Grupo = 0
        _Perfil = 0
        _Alerta = 0
        _IdFiltro = 0
        _TipoRegistro = TipoRegistro.Logon
        _Fecha = ""
        _Hora = ""
        _IdTran = ""
        _Comparacion = ""
        _Escalamiento = 0
        _Mensaje = ""
        _Limite = 0
        _Tiempo = 0
        _Referencia = ""
        _Estado = EstadoxAlerta.NoAplica
        _Prioridad = 0
        _FalsoPositivo = False
        _FactorRiesgo = 0
        _Id = 0
        _Observaciones = ""
        _Condicion = ""
        _IP = Bitacora.IP
        _NombrePc = Bitacora.NombrePC
        _SegundoPlano = True
    End Sub
#End Region

#Region "Métodos Privados"
    Private Sub Insertar(ByVal VPstrSQL As String)
        Dim VLdbsBase As Object
        Dim VLcmdComando As Object
        Dim VLrcsDatos As Object = Nothing
        Try
            VLdbsBase = BaseDatos.ConectarBase(VMbaseBitacora)
            Try
                VLcmdComando = VLdbsBase.CreateCommand
                VLcmdComando.CommandText = VPstrSQL
                VLcmdComando.ExecuteNonQuery()
                VLcmdComando.Cancel()
                Try
                    VPstrSQL = "SELECT Id FROM tbitacora WHERE Fecha = '" & _Fecha & "' AND Hora = '" & _Hora & "' AND IdUsuario = '" & _
                                _Usuario & "' AND IdProducto = " & VMobjProducto.GetHashCode & " AND IdTipoReg = " & _TipoRegistro
                    VLcmdComando = VLdbsBase.CreateCommand
                    VLcmdComando.CommandText = VPstrSQL
                    VLrcsDatos = VLcmdComando.ExecuteReader
                    If VLrcsDatos.HasRows Then
                        If VLrcsDatos.Read Then
                            _Id = VLrcsDatos!Id
                        End If
                    End If
                    VLrcsDatos.Close()
                    VLcmdComando.Cancel()
                Catch ex As OracleException
                    Agregar(VPstrSQL)
                    Throw New Exception("Problema al determinar identificador de la alerta" & vbNewLine & "No. Problema: " & ex.Code & vbNewLine & "Problema: " & ex.Message)
                Catch ex As SqlException
                    Agregar(VPstrSQL)
                    Throw New Exception("Problema al determinar identificador de la alerta" & vbNewLine & "No. Problema: " & ex.Number & vbNewLine & "Problema: " & ex.Message)
                Catch ex As MySqlException
                    Agregar(VPstrSQL)
                    Throw New Exception("Problema al determinar identificador de la alerta" & vbNewLine & "No. Problema: " & ex.Number & vbNewLine & "Problema: " & ex.Message)
                Catch ex As Exception
                    Agregar(VPstrSQL)
                    Throw New Exception("Problema al determinar identificador de la alerta: " & ex.Message)
                End Try
            Catch ex As OracleException
                Agregar(VPstrSQL)
                Throw New Exception("Problema al agregar el mensaje en la bitácora" & vbNewLine & "No. Problema: " & ex.Code & vbNewLine & "Problema: " & ex.Message)
            Catch ex As SqlException
                Agregar(VPstrSQL)
                Throw New Exception("Problema al agregar el mensaje en la bitácora" & vbNewLine & "No. Problema: " & ex.Number & vbNewLine & "Problema: " & ex.Message)
            Catch ex As MySqlException
                Agregar(VPstrSQL)
                Throw New Exception("Problema al agregar el mensaje en la bitácora" & vbNewLine & "No. Problema: " & ex.Number & vbNewLine & "Problema: " & ex.Message)
            Catch ex As Exception
                Agregar(VPstrSQL)
                Throw New Exception("Problema al agregar el mensaje en la bitácora: " & ex.Message)
            End Try
            VLdbsBase.Close()
        Catch ex As Exception
            Throw New Exception("Poblema al abrir la base bitácora: " & ex.Message)
        End Try
    End Sub
    Private Shared Sub InsertarSegundo()
        Dim VLdbsBase As Object
        Dim VLcmdComando As Object
        Dim VLstrSQL As String = ""

        Try
            VLdbsBase = BaseDatos.ConectarBase(VMbaseBitacora)
            Do
                SyncLock VMcolaInsertar
                    VLstrSQL = VMcolaInsertar.Dequeue
                End SyncLock
                Try
                    VLcmdComando = VLdbsBase.CreateCommand
                    VLcmdComando.CommandText = VLstrSQL
                    VLcmdComando.ExecuteNonQuery()
                    VLcmdComando.Cancel()
                Catch ex As OracleException
                    Agregar(VLstrSQL)
                Catch ex As SqlException
                    Agregar(VLstrSQL)
                Catch ex As MySqlException
                    Agregar(VLstrSQL)
                Catch ex As Exception
                    Agregar(VLstrSQL)
                End Try
            Loop While VMcolaInsertar.Count > 0
            VLdbsBase.Close()
        Catch ex As Exception
            SyncLock VMcolaInsertar
                VLstrSQL = VMcolaInsertar.Dequeue
            End SyncLock
            Agregar(VLstrSQL)


        End Try
    End Sub
    ''' <summary>
    ''' Agrega el query que no se puede insertar a una cola
    ''' </summary>
    ''' <param name="VPstrSQL">Query a guardar</param>
    Private Shared Sub Agregar(ByVal VPstrSQL As String)
        Try
            SyncLock VMcolaSQL
                VMcolaSQL.Enqueue(VPstrSQL)
            End SyncLock
            If Not VMhiloSQL.IsAlive Then
                VMhiloSQL = New Threading.Thread(AddressOf Guardar)
                VMhiloSQL.IsBackground = True
                VMhiloSQL.Name = "Guardar Bitácora en archivo"
                VMhiloSQL.Priority = Threading.ThreadPriority.Lowest
                VMhiloSQL.Start()
            End If
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Toma los query de la cola para ser guardados en un archivos de texto
    ''' </summary>
    Private Shared Sub Guardar()
        Dim VLioArchivo As System.IO.StreamWriter
        Dim VLstrPath As String
        Dim VLstrSQL As String = ""
        Try
            VLstrPath = My.Application.Info.DirectoryPath & "\SQL" & Format(Date.Now, "yyyyMMdd") & ".sql"
            VLioArchivo = New System.IO.StreamWriter(VLstrPath, True, System.Text.Encoding.Default)
            Do
                SyncLock VMcolaSQL
                    VLstrSQL = VMcolaSQL.Dequeue
                End SyncLock
                VLioArchivo.WriteLine(VLstrSQL & ";")
            Loop While VMcolaSQL.Count > 0
            VLioArchivo.Flush()
            VLioArchivo.Close()
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Determina la IP del equipo donde se corriendo la aplicacion
    ''' </summary>
    Private Shared Function DeterminarIP() As String
        Dim VLipEquipo As IPHostEntry
        Dim VLipListaIPs As IPAddress()
        Dim VLobjIPLocal As Object
        Dim VLstrIP As String = ""

        VLipEquipo = GetHostEntry(My.Computer.Name)
        VLipListaIPs = VLipEquipo.AddressList()
        For Each VLobjIPLocal In VLipListaIPs
            If VLobjIPLocal.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                VLstrIP = VLobjIPLocal.ToString
            End If
        Next
        Return VLstrIP
    End Function
#End Region
End Class

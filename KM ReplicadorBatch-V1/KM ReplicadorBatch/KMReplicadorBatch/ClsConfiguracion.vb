Imports System.IO
Imports System.Xml

Public Class ClsConfiguracion
#Region "Estructuras y Enumeraciones"

    ''' <remarks>Contiene todos los datos de configuración para conexión de Tipo Servidor</remarks>
    Public Structure Servidor
        ''' <summary>
        ''' IP del Servidor
        ''' </summary>
        Dim Servidor As String
        ''' <summary>
        ''' Puerto de conexion con el Servidor
        ''' </summary>
        Dim Puerto As String
    End Structure

    ''' <remarks>Contiene los datos de configuración para el envio de Correo electronico</remarks>
    Public Structure Correo
        ''' <summary>
        ''' IP del Servidor de correo
        ''' </summary>
        Dim Servidor As String
        ''' <summary>
        ''' Cuenta para la salida de correo
        ''' </summary>
        Dim Correo As String
        ''' <summary>
        ''' Usuario para la conexión al servidor de correo en caso de utilizar Autentificación
        ''' </summary>
        Dim Usuario As String
        ''' <summary>
        ''' Contraseña de la cuenta de correo en caso de utilizar Autentificación
        ''' </summary>
        Dim Contrasena As String
        ''' <summary>
        ''' Indica si se enviara correo utilizando Autentificación
        ''' </summary>
        Dim Autentificacion As Boolean

        ''' <summary>
        ''' Puerto de Salida para el servidor de correo por SMTP
        ''' </summary>
        Dim Puerto As String

        ''' <summary>
        ''' Indica si el correo será enviado en formato de texto plano
        ''' </summary>
        Dim FmtTxtCorreo As Boolean  'Indica si el mensaje de correo se enviara en formato de Texto o XML

    End Structure
    ''' <remarks>Contiene los datos de configuración para el envio de Alertas por medio un CMD en vez de correo</remarks>
    Public Structure CMD
        Dim RutaShell As String
        Dim Prefijo As String
        Dim Posfijo As String
    End Structure
    ''' <summary>
    ''' Contiene la configuración para conexión con el correo Express
    ''' </summary>
    Public Structure CorreoExpress
        ''' <summary>
        ''' Contiene la información para la conexión con el servicio KSMAil
        ''' </summary>
        Dim Activo As Boolean
        Dim ServicioMail As Servidor
        Dim Para As String
        Dim CC As String
        Dim CCO As String
    End Structure
    ''' <summary>
    ''' Almacena los datos de Configuración necesarios para el funcionamiento del Receptor
    ''' </summary>
    Public Structure Receptor
        ''' <summary>
        ''' Directorio de la Base de Datos Local
        ''' </summary>
        Dim BaseLocal As String
        ''' <summary>
        ''' Directorio para almacenar la bitacora de filtros
        ''' </summary>
        ''' <remarks></remarks>
        Dim DirBitTran As String

        Dim LayOut As Integer
        Dim Formato As Integer
        Dim Pathway As String
        Dim IDMonto As Integer
        Dim IDMascara As Integer
        Dim MaximoAlertas As Integer

        Dim ECHO As Boolean
        Dim TiempoECHO As Integer

        Dim GeneraLogTX As Boolean
        Dim HrInicio As String
        Dim HrFin As String

        Dim ContGeneral As Boolean
        Dim ContSesiones As Boolean
        Dim HrContadores As String

        Dim IdToken As Integer
        Dim TipoCambio As Boolean
        Dim MontoToken As Integer
        Dim MonedaToken As Integer
        Dim VerToken As Boolean
        Dim FuenteTCambio As Integer

        Dim TipoCorreo As Integer
    End Structure
    ''' <summary>
    ''' Almacena los datos de Configuración necesarios para el funcionamiento del Receptor
    ''' </summary>
    Public Structure KMRConfig

        Dim LongEncabezado As Integer
        Dim Tokens As Boolean
        Dim Informa As Boolean
        Dim Patrol As Boolean
        Dim NPuertos As Integer
        Dim EchoAute As Boolean
        Dim EchoTran As Boolean
        Dim NvaBita As Boolean

    End Structure
    ''' <summary>
    ''' Almacena los datos de Configuración necesarios para el funcionamiento del Administrador
    ''' </summary>
    Public Structure KMAConfig

        Dim Tokens As Boolean
        Dim Patrol As Boolean
        Dim Categorias As Boolean
        Dim TiposUsuario As Boolean '2012-06-20
        Dim PermisosCtes As Integer 'PermisosCtes es : 0 = Resto, 1 = HSBC y 2 = BANAMEX.
        Dim FiltrosCorporativos As Boolean
    End Structure
    ''' <remarks>Contiene todos los datos de configuración para conexión de Tipo ServidorTandem</remarks>
    Public Structure ServidorTandem
        ''' <summary>
        ''' Nombre de Pathway
        ''' </summary>
        Dim Pathway As String
        ''' <summary>
        ''' IP del Servidor Tandem
        ''' </summary>
        Dim ServidorTandem As String
        ''' <summary>
        ''' Puerto de conexion con el Servidor Tandem
        ''' </summary>
        Dim PuertoTandem As String
    End Structure
    Public Enum TipoConfiguracion As Integer
        BaseDatos = 1
        ServidorAutentifica = 2
        ServidorTransaccional = 3
        ServidorAdministracion = 4
        Correo = 5
        CorreoExpress = 6
        CMD = 7
        Receptor = 8
        KMRConfig = 9
        Administrador = 10
        KMAConfig = 11
    End Enum

#End Region

#Region "Variables"
    Private _ArchivoXML As String 'Almacena la ruta del archivo XML con los datos de Configuración
    Private _TipoConfiguracion As Integer 'Almacena el tipo de configuración que se va a leer
    Private _RutaNodo As String 'Almacena la Ruta del Nodo que se Quiere Leer
    Private _Configuracion As String 'Almacena el nombre de la configuración que se va a Leer
    Private _Atributo As String 'Almacena el valor del Atributo a Leer en caso de ser Necesario

    Private _Base As BaseDatos
    Private _Servidor As Servidor
    Private _Correo As Correo
    Private _CorreoExpress As Correo
    Private _Receptor As Receptor
    Private _KMRConfig As KMRConfig
#End Region

#Region "Propiedades"
    Public WriteOnly Property ArchivoXML() As String
        Set(ByVal value As String)
            _ArchivoXML = value
        End Set
    End Property

    Public WriteOnly Property TipoResultado() As TipoConfiguracion
        Set(ByVal value As TipoConfiguracion)
            _TipoConfiguracion = value
        End Set
    End Property

    Public WriteOnly Property RutaNodo() As String
        Set(ByVal value As String)
            _RutaNodo = value
        End Set
    End Property

    Public WriteOnly Property Configuracion() As String
        Set(ByVal value As String)
            _Configuracion = value
        End Set
    End Property

    Public WriteOnly Property Atributo() As String
        Set(ByVal value As String)
            _Atributo = value
        End Set
    End Property
#End Region

#Region "Funciones Publicas"
    Public Sub New()
        _ArchivoXML = ""
        _TipoConfiguracion = 0
        _RutaNodo = ""
        _Configuracion = ""
        _Atributo = ""
    End Sub
    Public Function LeeConfiguracion() As Object
        Try
            If Trim(_ArchivoXML) = "" Then
                Throw New Exception("Es necesario especificar la Ruta del Archivo XML")
            ElseIf Trim(_RutaNodo) = "" Then
                Throw New Exception("Es necesario especificar la Ruta del Nodo de Configuración")
                'ElseIf Trim(_Configuracion) = "" Then
                '    Throw New Exception("Es necesario especificar el Nombre de la Configuración")
            End If

            If _TipoConfiguracion = TipoConfiguracion.BaseDatos Then
                If Trim(_Atributo) = "" Then
                    Throw New Exception("Es necesario especificar la descripción de la Base de Datos")
                End If
            End If

            If Not My.Computer.FileSystem.FileExists(Trim(_ArchivoXML)) Then
                Throw New Exception("No existe el archivo de configuración[" & _ArchivoXML & "]")
            End If

            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode
            VLxmlDocumento = New XmlDocument()
            VLxmlDocumento.Load(_ArchivoXML)

            '---------OBTIENE EL NODO PRINCIPAL------
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)
            Select Case _TipoConfiguracion
                Case TipoConfiguracion.BaseDatos
                    Dim VLobjBase As New BaseDatos.Base
                    '_RutaNodo = _RutaNodo '& "/Base"
                    'Obtiene la Configuración de la Base de Datos
                    VLobjBase = ConfiguracionBD(VLxmlNodo)
                    Return VLobjBase
                Case TipoConfiguracion.ServidorAdministracion, TipoConfiguracion.ServidorAutentifica, TipoConfiguracion.ServidorTransaccional
                    Dim VLobjServidor As Servidor
                    'Obtiene el nodo Correspondiente
                    Select Case _TipoConfiguracion
                        Case TipoConfiguracion.ServidorAdministracion
                            'VLxmlNodo = ObtenerNodo(VLxmlDocumento, "", "", "ServicioAdmin")
                            Dim VLobjBase As New Servidor
                            '_RutaNodo = _RutaNodo '& "/ServicioAdmin"
                            'Obtiene la Configuración de la Base de Datos
                            VLobjBase = ConfiguracionServidor(VLxmlNodo)
                            Return VLobjBase
                        Case TipoConfiguracion.ServidorAutentifica
                            Dim VLobjBase As New Servidor
                            '_RutaNodo = _RutaNodo '& "/ServidorAuten"
                            'Obtiene la Configuración de la Base de Datos
                            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, "", "ServicioAdmin")

                            VLobjBase = ConfiguracionServidor(VLxmlDocumento)
                            Return VLobjBase
                        Case TipoConfiguracion.ServidorTransaccional
                            'VLxmlNodo = ObtenerNodo(VLxmlNodo, "", "", "ServidorTran")
                            Dim VLobjBase As New Servidor
                            _RutaNodo = _RutaNodo & "/ServidorTran"
                            'Obtiene la Configuración de la Base de Datos
                            VLobjBase = ConfiguracionServidor(VLxmlDocumento)
                            Return VLobjBase
                    End Select

                    VLobjServidor = ConfiguracionServidor(VLxmlNodo)

                    Return VLobjServidor
                Case TipoConfiguracion.Correo
                    Dim VLobjCorreo As Correo

                    VLxmlNodo = ObtenerNodo(VLxmlNodo, "", "", "Correo")
                    VLobjCorreo = ConfiguracionCorreo(VLxmlNodo)

                    Return VLobjCorreo
                Case TipoConfiguracion.CorreoExpress
                    Dim VLobjCorreoExpress As CorreoExpress
                    VLxmlNodo = ObtenerNodo(VLxmlNodo, "", "", "CorreoExpress")

                    VLobjCorreoExpress = ConfiguracionCorreoExpress(VLxmlNodo)

                    Return VLobjCorreoExpress
                Case TipoConfiguracion.CMD
                    Dim VLobjCMD As CMD
                    VLxmlNodo = ObtenerNodo(VLxmlNodo, "", "", "CMD")
                    VLobjCMD = ConfiguracionCMD(VLxmlNodo)

                    Return VLobjCMD
                Case TipoConfiguracion.Receptor
                    Dim VLobjReceptor As Receptor
                    'VLxmlNodo = ObtenerNodo(VLxmlNodo, _RutaNodo, _Configuracion)
                    'VLxmlNodo = ObtenerNodo(VLxmlDocumento, "", "", "Receptor")

                    VLobjReceptor = ConfiguracionReceptor(VLxmlNodo)

                    Return VLobjReceptor
                Case TipoConfiguracion.KMRConfig

            End Select
            Return Nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Private Function ConfiguracionBD(ByVal VPxmlNodo As XmlNode) As BaseDatos.Base
        Dim VLstrucBase As BaseDatos.Base

        '---------Inicializa la Variable Base de Datos-------------
        VLstrucBase.Base = ""
        VLstrucBase.Contraseña = ""
        VLstrucBase.Instancia = ""
        VLstrucBase.Puerto = 0
        VLstrucBase.Seguridad_Integrada = 0
        VLstrucBase.Servidor = ""
        VLstrucBase.Tipo = 0
        VLstrucBase.Usuario = ""
        '---------Inicializa la Variable Base de Datos-------------

        Try
            Dim m_nodelist As XmlNodeList
            'Obtenemos la lista de los nodos "id"
            m_nodelist = VPxmlNodo.SelectNodes("/Base")

            If m_nodelist.Count() > 0 Then
                'Iniciamos el ciclo de lectura
                For Each m_node In m_nodelist
                    'Obtenemos el atributo del codigo
                    Dim mCodigo As String = m_node.Attributes.GetNamedItem("Id").Value
                    If mCodigo = _Atributo Then
                        Dim hijo As String
                        For a As Integer = 0 To m_node.ChildNodes.Count() - 1
                            hijo = m_node.ChildNodes.Item(a).Name()
                            Select Case hijo
                                Case "Tipo"
                                    VLstrucBase.Tipo = m_node.ChildNodes.Item(a).InnerText
                                Case "IP"
                                    VLstrucBase.Servidor = m_node.ChildNodes.Item(a).InnerText
                                Case "Puerto"
                                    VLstrucBase.Puerto = m_node.ChildNodes.Item(a).InnerText
                                Case "NombreBase"
                                    VLstrucBase.Base = m_node.ChildNodes.Item(a).InnerText
                                Case "Instancia"
                                    VLstrucBase.Instancia = m_node.ChildNodes.Item(a).InnerText
                                Case "Usuario"
                                    VLstrucBase.Usuario = m_node.ChildNodes.Item(a).InnerText
                                Case "Clave"
                                    VLstrucBase.Contraseña = m_node.ChildNodes.Item(a).InnerText
                                Case "SI"
                                    VLstrucBase.Seguridad_Integrada = m_node.ChildNodes.Item(a).InnerText
                            End Select
                        Next
                    End If
                Next
            End If
            Return VLstrucBase
        Catch ex As Exception
            Return VLstrucBase
        End Try
    End Function

    Private Function ConfiguracionServidor(ByVal VPxmlNodo As XmlDocument) As Servidor
        Dim VLstrucServidor As Servidor
        '---------Inicializa la Variable Servidor-------------
        VLstrucServidor.Servidor = ""
        VLstrucServidor.Puerto = ""
        '---------Inicializa la Variable Servidor-------------

        Try
            Dim m_nodelist As XmlNodeList
            'Obtenemos la lista de los nodos "id"
            m_nodelist = VPxmlNodo.SelectNodes(_RutaNodo)

            If m_nodelist.Count() > 0 Then
                'Iniciamos el ciclo de lectura
                For Each m_node In m_nodelist
                    'Obtenemos el atributo del codigo
                    Dim mCodigo As String = m_node.Attributes.GetNamedItem("Id").Value
                    If mCodigo = _Atributo Then
                        Dim hijo As String
                        For a As Integer = 0 To m_node.ChildNodes.Count() - 1
                            hijo = m_node.ChildNodes.Item(a).Name()
                            Select Case hijo
                                Case "IP"
                                    VLstrucServidor.Servidor = m_node.ChildNodes.Item(a).InnerText
                                Case "Puerto"
                                    VLstrucServidor.Puerto = m_node.ChildNodes.Item(a).InnerText
                            End Select
                        Next
                    End If
                Next
            End If
            Return VLstrucServidor
        Catch ex As Exception
            Return VLstrucServidor
        End Try
    End Function
    Private Function ConfiguracionCorreo(ByVal VPxmlNodo As XmlNode) As Correo
        Dim VLstrucCorreo As Correo

        '---------Inicializa la Variable Base de Datos-------------
        VLstrucCorreo.Autentificacion = False
        VLstrucCorreo.Contrasena = ""
        VLstrucCorreo.Correo = ""
        VLstrucCorreo.FmtTxtCorreo = False
        VLstrucCorreo.Puerto = 0
        VLstrucCorreo.Servidor = ""
        VLstrucCorreo.Usuario = ""

        '---------Inicializa la Variable Base de Datos-------------

        Try
            'Recorre los nodos encontrados


            For Each VLxmlNodoHijo As Xml.XmlNode In VPxmlNodo.ChildNodes

                'If Not (hijo.SelectSingleNode("Name") Is Nothing) Then
                Select Case VLxmlNodoHijo.Name
                    Case "Servidor"
                        VLstrucCorreo.Servidor = VLxmlNodoHijo.InnerText
                    Case "Correo"
                        VLstrucCorreo.Correo = VLxmlNodoHijo.InnerText
                    Case "Puerto"
                        VLstrucCorreo.Puerto = VLxmlNodoHijo.InnerText
                    Case "Autenticacion"
                        VLstrucCorreo.Autentificacion = CBool(VLxmlNodoHijo.InnerText)
                    Case "Usuario"
                        VLstrucCorreo.Usuario = VLxmlNodoHijo.InnerText
                    Case "Clave"
                        VLstrucCorreo.Contrasena = VLxmlNodoHijo.InnerText
                    Case "FmtTxtCorreo"
                        VLstrucCorreo.FmtTxtCorreo = CBool(VLxmlNodoHijo.InnerText)
                End Select

            Next
            Return VLstrucCorreo
        Catch ex As Exception
            Return VLstrucCorreo
        End Try
    End Function
    Private Function ConfiguracionCorreoExpress(ByVal VPxmlNodo As XmlNode) As CorreoExpress
        Dim VLstrucCorreoE As CorreoExpress

        '---------Inicializa la Variable Base de Datos-------------
        VLstrucCorreoE.Activo = False
        VLstrucCorreoE.ServicioMail.Servidor = ""
        VLstrucCorreoE.ServicioMail.Puerto = 0
        VLstrucCorreoE.Para = ""
        VLstrucCorreoE.CC = ""
        VLstrucCorreoE.CCO = ""
        '---------Inicializa la Variable Base de Datos-------------

        Try
            'Recorre los nodos encontrados
            For Each VLxmlNodohijo As XmlNode In VPxmlNodo.ChildNodes

                'If Not (hijo.SelectSingleNode("Name") Is Nothing) Then
                Select Case VLxmlNodohijo.Name
                    Case "Activo"
                        VLstrucCorreoE.Activo = CBool(VLxmlNodohijo.InnerText)
                    Case "IP"
                        VLstrucCorreoE.ServicioMail.Servidor = VLxmlNodohijo.InnerText
                    Case "Puerto"
                        VLstrucCorreoE.ServicioMail.Puerto = VLxmlNodohijo.InnerText
                    Case "Para"
                        VLstrucCorreoE.Para = VLxmlNodohijo.InnerText
                    Case "CC"
                        VLstrucCorreoE.CC = VLxmlNodohijo.InnerText
                    Case "CCO"
                        VLstrucCorreoE.CCO = VLxmlNodohijo.InnerText
                End Select
            Next
            Return VLstrucCorreoE
        Catch ex As Exception
            Return VLstrucCorreoE
        End Try
    End Function
    Private Function ConfiguracionCMD(ByVal VPxmlNodo As XmlNode) As CMD
        Dim VLstrucCMD As CMD
        '---------Inicializa la Variable CMD-------------
        VLstrucCMD.RutaShell = ""
        VLstrucCMD.Prefijo = ""
        VLstrucCMD.Posfijo = ""
        '---------Inicializa la Variable CMD-------------
        Try
            'Recorre los nodos encontrados
            For Each VLxmlNodoHijo As Xml.XmlNode In VPxmlNodo.ChildNodes
                'If Not (hijo.SelectSingleNode("Name") Is Nothing) Then
                Select Case VLxmlNodoHijo.Name
                    Case "Ruta"
                        VLstrucCMD.RutaShell = VLxmlNodoHijo.InnerText
                    Case "Prefijo"
                        VLstrucCMD.Prefijo = VLxmlNodoHijo.InnerText
                    Case "Posfijo"
                        VLstrucCMD.Posfijo = VLxmlNodoHijo.InnerText
                End Select
            Next
            Return VLstrucCMD
        Catch ex As Exception
            Return VLstrucCMD
        End Try
    End Function
    Private Function ConfiguracionReceptor(ByVal VPxmlNodo As XmlNode) As Receptor
        Dim VLstrucReceptor As Receptor

        '---------Inicializa la Variable Base de Datos-------------
        VLstrucReceptor.BaseLocal = ""
        VLstrucReceptor.DirBitTran = ""

        VLstrucReceptor.LayOut = 0
        VLstrucReceptor.Formato = 0
        VLstrucReceptor.Pathway = ""

        VLstrucReceptor.IDMascara = 0
        VLstrucReceptor.IDMonto = 0
        VLstrucReceptor.MaximoAlertas = 0

        VLstrucReceptor.ECHO = False
        VLstrucReceptor.TiempoECHO = 0

        VLstrucReceptor.GeneraLogTX = False
        VLstrucReceptor.HrInicio = ""
        VLstrucReceptor.HrFin = ""

        VLstrucReceptor.ContGeneral = False
        VLstrucReceptor.ContSesiones = False
        VLstrucReceptor.HrContadores = ""

        VLstrucReceptor.IdToken = 0
        VLstrucReceptor.TipoCambio = False
        VLstrucReceptor.MontoToken = 0
        VLstrucReceptor.MonedaToken = 0
        VLstrucReceptor.VerToken = False
        VLstrucReceptor.FuenteTCambio = 0

        VLstrucReceptor.TipoCorreo = 0
        '---------Inicializa la Variable Base de Datos-------------

        Try
            'Recorre los nodos encontrados
            For Each VLxmlNodoHijo As Xml.XmlNode In VPxmlNodo.ChildNodes
                'If Not (hijo.SelectSingleNode("Name") Is Nothing) Then
                Select Case VLxmlNodoHijo.Name
                    Case "BaseLocal"
                        VLstrucReceptor.BaseLocal = VLxmlNodoHijo.InnerText
                    Case "DirBitTran"
                        VLstrucReceptor.DirBitTran = VLxmlNodoHijo.InnerText
                    Case "LayOut"
                        VLstrucReceptor.LayOut = VLxmlNodoHijo.InnerText
                    Case "Formato"
                        VLstrucReceptor.Formato = VLxmlNodoHijo.InnerText
                    Case "Pathway"
                        VLstrucReceptor.Pathway = VLxmlNodoHijo.InnerText
                    Case "IdMascara"
                        VLstrucReceptor.IDMascara = VLxmlNodoHijo.InnerText
                    Case "IdMonto"
                        VLstrucReceptor.IDMonto = VLxmlNodoHijo.InnerText
                    Case "MaximoAlertas"
                        VLstrucReceptor.MaximoAlertas = VLxmlNodoHijo.InnerText
                    Case "ECHO"
                        VLstrucReceptor.ECHO = CBool(VLxmlNodoHijo.InnerText)
                    Case "TiempoECHO"
                        VLstrucReceptor.TiempoECHO = VLxmlNodoHijo.InnerText
                    Case "GeneralLogTX"
                        VLstrucReceptor.GeneraLogTX = CBool(VLxmlNodoHijo.InnerText)
                    Case "HrInicio"
                        VLstrucReceptor.HrInicio = VLxmlNodoHijo.InnerText
                    Case "HrFin"
                        VLstrucReceptor.HrFin = VLxmlNodoHijo.InnerText
                    Case "ContGeneral"
                        VLstrucReceptor.ContGeneral = CBool(VLxmlNodoHijo.InnerText)
                    Case "ContSesiones"
                        VLstrucReceptor.ContSesiones = CBool(VLxmlNodoHijo.InnerText)
                    Case "HrContadores"
                        VLstrucReceptor.HrContadores = VLxmlNodoHijo.InnerText
                    Case "IdToken"
                        VLstrucReceptor.IdToken = VLxmlNodoHijo.InnerText
                    Case "TipoCambio"
                        VLstrucReceptor.TipoCambio = CBool(VLxmlNodoHijo.InnerText)
                    Case "MontoToken"
                        VLstrucReceptor.MontoToken = VLxmlNodoHijo.InnerText
                    Case "MonedaToken"
                        VLstrucReceptor.MonedaToken = VLxmlNodoHijo.InnerText
                    Case "VerToken"
                        VLstrucReceptor.VerToken = CBool(VLxmlNodoHijo.InnerText)
                    Case "FuenteTCambio"
                        VLstrucReceptor.FuenteTCambio = VLxmlNodoHijo.InnerText
                    Case "TipoCorreo"
                        VLstrucReceptor.TipoCorreo = VLxmlNodoHijo.InnerText
                End Select
            Next
            Return VLstrucReceptor
        Catch ex As Exception
            Return VLstrucReceptor
        End Try
    End Function
    Private Function ObtenerNodo(ByVal VPxmlNodo As XmlNode, Optional ByVal VPstrRutaNodo As String = "", Optional ByVal VPstrAtributo As String = "", Optional ByVal VPstrNombre As String = "") As XmlNode

        Dim VLstrListaNodos As XmlNodeList
        Dim VLxmlNodo As XmlNode = Nothing
        Try
            'Recorre los nodos encontrados
            If VPxmlNodo.HasChildNodes Then
                'Obtiene la lista de nodos Base
                If Trim(VPstrRutaNodo) <> "" Then
                    VLstrListaNodos = VPxmlNodo.SelectNodes(VPstrRutaNodo)

                    If VLstrListaNodos.Count > 0 Then
                        For Each VLxmlNodo In VLstrListaNodos
                            'Obtiene el atributo ID
                            If VLxmlNodo.Attributes.Count > 0 Then
                                Dim IDAtributo = VLxmlNodo.Attributes.GetNamedItem("ID").Value
                                If IDAtributo.ToString = Trim(VPstrAtributo) Then
                                    Return VLxmlNodo
                                    Exit For
                                Else
                                    VLxmlNodo = Nothing
                                    'Return VLxmlNodo
                                End If
                                'Else
                                'Return VLxmlNodo
                                'Exit For
                            End If
                        Next
                    End If

                    ''Cargamos el archivo
                    'xmlConfig.Load(_ArchivoXML) '(My.Application.Info.DirectoryPath & "\ConfiguracionesKM.xml")

                    ''Obtenemos la lista de los nodos "id"
                    'm_nodelist = xmlConfig.SelectNodes(_RutaNodo & "/Base") '("/ConfiguracionesKM/ConfiguracionMaestro/Base")
                Else
                    For Each VLxmlNodo In VPxmlNodo.ChildNodes
                        'Obtiene el atributo ID
                        If VLxmlNodo.Name = Trim(VPstrNombre) Then
                            If VPstrAtributo.Trim <> "" Then
                                Dim IDAtributo = VLxmlNodo.Attributes.GetNamedItem("ID").Value
                                If IDAtributo.ToString = Trim(VPstrAtributo) Then
                                    Return VLxmlNodo
                                    Exit For
                                Else
                                    VLxmlNodo = Nothing
                                    'Exit For
                                End If
                            Else
                                Return VLxmlNodo
                                'Return VLxmlNodo
                                Exit For
                            End If
                        Else
                            VLxmlNodo = Nothing
                        End If
                    Next
                End If
            Else
                VLxmlNodo = Nothing
            End If

            Return VLxmlNodo
        Catch ex As Exception
            Return VLxmlNodo
        End Try

    End Function

#End Region

#Region "FuncionesGuardaConfig"
    Public Function ValidaConfiguracion() As Boolean

        If Trim(_ArchivoXML) = "" Then
            Throw New Exception("Es necesario especificar la Ruta del Archivo XML")
            Return False
        ElseIf Trim(_RutaNodo) = "" Then
            Throw New Exception("Es necesario especificar la Ruta del Nodo de Configuración")
            Return False
            'ElseIf Trim(_Configuracion) = "" Then
            '    Throw New Exception("Es necesario especificar el Nombre de la Configuración")
            '    Return False
        End If

        If _TipoConfiguracion = TipoConfiguracion.BaseDatos Then
            If Trim(_Atributo) = "" Then
                Throw New Exception("Es necesario especificar la descripción de la Base de Datos")
                Return False
            End If
        End If

        If Not My.Computer.FileSystem.FileExists(Trim(_ArchivoXML)) Then
            Throw New Exception("No existe el archivo de configuración[" & _ArchivoXML & "]")
            Return False
        End If

        Return True
    End Function
    Public Function GuardaBD(ByVal VPstrucBase As BaseDatos.Base) As Boolean
        Try
            If ValidaConfiguracion() = False Then
                Exit Function
            End If

            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then
                Select Case _TipoConfiguracion
                    Case TipoConfiguracion.BaseDatos

                        Dim VLxmlNodoHijo As XmlNode
                        'Obtiene el Nodo de la Base Solicitada
                        VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, "Base")

                        If Not IsNothing(VLxmlNodoHijo) Then
                            VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                            VLxmlDocumento.Save(_ArchivoXML)
                        End If

                        Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                        Dim VLstrTextoNodo As String
                        VLstrTextoNodo = "<Base ID='" & _Atributo & "'><Tipo>" & VPstrucBase.Tipo & "</Tipo><IP>" & VPstrucBase.Servidor & "</IP><Puerto>" & VPstrucBase.Puerto & "</Puerto><NombreBase>" & VPstrucBase.Base & "</NombreBase><Instancia>" & VPstrucBase.Instancia & "</Instancia><Usuario>" & VPstrucBase.Usuario & "</Usuario><Clave>" & VPstrucBase.Contraseña & "</Clave><SI>" & IIf(VPstrucBase.Seguridad_Integrada = True, 1, 0) & "</SI></Base>"
                        VLxmlNavegador.AppendChild(VLstrTextoNodo)

                        VLxmlDocumento.Save(_ArchivoXML)

                        Return True
                End Select
            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<Base ID='" & _Atributo & "'><Tipo>" & VPstrucBase.Tipo & "</Tipo><IP>" & VPstrucBase.Servidor & "</IP><Puerto>" & VPstrucBase.Puerto & "</Puerto><NombreBase>" & VPstrucBase.Base & "</NombreBase><Instancia>" & VPstrucBase.Instancia & "</Instancia><Usuario>" & VPstrucBase.Usuario & "</Usuario><Clave>" & VPstrucBase.Contraseña & "</Clave><SI>" & IIf(VPstrucBase.Seguridad_Integrada = True, 1, 0) & "</SI></Base>"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If

        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch--24005 --> : " & ex.Message)
            MsgBox("KMRBatch--24005 --> No se pudo guardar la informacíón por el siguiente motivo --> : " & vbNewLine & ex.Message & vbNewLine, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function
    Public Function GuardaServidor(ByVal VPstrucServidor As ClsConfiguracion.Servidor) As Boolean
        Try
            If ValidaConfiguracion() = False Then
                Exit Function
            End If

            Dim VLstrNombreNodo As String

            Select Case _TipoConfiguracion
                Case TipoConfiguracion.ServidorAdministracion
                    'Obtiene el Nodo de la Base Solicitada
                    VLstrNombreNodo = "ServicioAdmin"
                Case TipoConfiguracion.ServidorAutentifica
                    'Obtiene el Nodo de la Base Solicitada
                    VLstrNombreNodo = "ServidorAuten"
                Case TipoConfiguracion.ServidorTransaccional
                    'Obtiene el Nodo de la Base Solicitada
                    VLstrNombreNodo = "ServidorTran"
                Case Else
                    VLstrNombreNodo = ""
            End Select

            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then

                Dim VLxmlNodoHijo As XmlNode

                'Obtiene el nodo hijo especificado
                VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, VLstrNombreNodo)

                If Not IsNothing(VLxmlNodoHijo) Then
                    VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                    VLxmlDocumento.Save(_ArchivoXML)
                End If

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                Dim VLstrTextoNodo As String
                VLstrTextoNodo = "<" & VLstrNombreNodo & "><IP>" & VPstrucServidor.Servidor & "</IP><Puerto>" & VPstrucServidor.Puerto & "</Puerto></" & VLstrNombreNodo & ">"
                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)

                Return True

            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<" & VLstrNombreNodo & "><IP>" & VPstrucServidor.Servidor & "</IP><Puerto>" & VPstrucServidor.Puerto & "</Puerto></" & VLstrNombreNodo & ">"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch--24001 --> : " & ex.Message)
            MsgBox("KMRBatch--24001 --> No se pudo guardar la informacíón por el siguiente motivo --> : " & vbNewLine & ex.Message & vbNewLine, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)

            MsgBox("KMRBatch--24001 --> numero de error : " & Err.Number & Err.Description)
            Return False
        End Try
    End Function
    Public Function GuardaCorreo(ByVal VPstrucCorreo As Correo) As Boolean
        Try

            If ValidaConfiguracion() = False Then
                Exit Function
            End If


            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then

                Dim VLxmlNodoHijo As XmlNode

                'Obtiene el nodo hijo especificado
                VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, "Correo")

                If Not IsNothing(VLxmlNodoHijo) Then
                    VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                    VLxmlDocumento.Save(_ArchivoXML)
                End If

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                Dim VLstrTextoNodo As String
                VLstrTextoNodo = "<Correo>" & _
                                     "<Servidor>" & VPstrucCorreo.Servidor & "</Servidor>" & _
                                     "<Correo>" & VPstrucCorreo.Correo & "</Correo>" & _
                                     "<Puerto>" & VPstrucCorreo.Puerto & "</Puerto>" & _
                                     "<Autenticacion>" & IIf(VPstrucCorreo.Autentificacion = True, 1, 0) & "</Autenticacion>" & _
                                     "<Usuario>" & VPstrucCorreo.Usuario & "</Usuario>" & _
                                     "<Clave>" & VPstrucCorreo.Contrasena & "</Clave>" & _
                                     "<FmtTxtCorreo>" & IIf(VPstrucCorreo.FmtTxtCorreo = True, 1, 0) & "</FmtTxtCorreo>" & _
                                 "</Correo>"
                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)
                Return True

            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<Correo>" & _
                                     "<Servidor>" & VPstrucCorreo.Servidor & "</Servidor>" & _
                                     "<Correo>" & VPstrucCorreo.Correo & "</Correo>" & _
                                     "<Puerto>" & VPstrucCorreo.Puerto & "</Puerto>" & _
                                     "<Autenticacion>" & IIf(VPstrucCorreo.Autentificacion = True, 1, 0) & "</Autenticacion>" & _
                                     "<Usuario>" & VPstrucCorreo.Usuario & "</Usuario>" & _
                                     "<Clave>" & VPstrucCorreo.Contrasena & "</Clave>" & _
                                     "<FmtTxtCorreo>" & IIf(VPstrucCorreo.FmtTxtCorreo = True, 1, 0) & "</FmtTxtCorreo>" & _
                                 "</Correo>"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GuardaCorreoExpress(ByVal VPstrucCorreoExpress As CorreoExpress) As Boolean
        Try
            If ValidaConfiguracion() = False Then
                Exit Function
            End If

            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then

                Dim VLxmlNodoHijo As XmlNode

                'Obtiene el nodo hijo especificado
                VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, "CorreoExpress")

                If Not IsNothing(VLxmlNodoHijo) Then
                    VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                    VLxmlDocumento.Save(_ArchivoXML)
                End If

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                Dim VLstrTextoNodo As String
                VLstrTextoNodo = "<CorreoExpress>" & _
                                     "<Activo>" & IIf(VPstrucCorreoExpress.Activo = True, 1, 0) & "</Activo>" & _
                                     "<IP>" & VPstrucCorreoExpress.ServicioMail.Servidor & "</IP>" & _
                                     "<Puerto>" & VPstrucCorreoExpress.ServicioMail.Puerto & "</Puerto>" & _
                                     "<Para>" & VPstrucCorreoExpress.Para & "</Para>" & _
                                     "<CC>" & VPstrucCorreoExpress.CC & "</CC>" & _
                                     "<CCO>" & VPstrucCorreoExpress.CCO & "</CCO>" & _
                                 "</CorreoExpress>"
                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)
                Return True

            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<CorreoExpress>" & _
                                     "<Activo>" & IIf(VPstrucCorreoExpress.Activo = True, 1, 0) & "</Activo>" & _
                                     "<IP>" & VPstrucCorreoExpress.ServicioMail.Servidor & "</IP>" & _
                                     "<Puerto>" & VPstrucCorreoExpress.ServicioMail.Puerto & "</Puerto>" & _
                                     "<Para>" & VPstrucCorreoExpress.Para & "</Para>" & _
                                     "<CC>" & VPstrucCorreoExpress.CC & "</CC>" & _
                                     "<CCO>" & VPstrucCorreoExpress.CCO & "</CCO>" & _
                                 "</CorreoExpress>"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)
                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GuardaCMD(ByVal VPstrucCMD As CMD) As Boolean
        Try
            If ValidaConfiguracion() = False Then
                Exit Function
            End If
            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then

                Dim VLxmlNodoHijo As XmlNode

                'Obtiene el nodo hijo especificado
                VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, "CMD")

                If Not IsNothing(VLxmlNodoHijo) Then
                    VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                    VLxmlDocumento.Save(_ArchivoXML)
                End If

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                Dim VLstrTextoNodo As String
                VLstrTextoNodo = "<CMD>" & _
                                     "<Ruta>" & VPstrucCMD.RutaShell & "</Ruta>" & _
                                     "<Prefijo>" & VPstrucCMD.Prefijo & "</Prefijo>" & _
                                     "<Posfijo>" & VPstrucCMD.Posfijo & "</Posfijo>" & _
                                 "</CMD>"
                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)
                Return True

            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<CMD>" & _
                                     "<Ruta>" & VPstrucCMD.RutaShell & "</Ruta>" & _
                                     "<Prefijo>" & VPstrucCMD.Prefijo & "</Prefijo>" & _
                                     "<Posfijo>" & VPstrucCMD.Posfijo & "</Posfijo>" & _
                                 "</CMD>"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GuardaReceptor(ByVal VPstrucReceptor As Receptor) As Boolean
        Try

            If ValidaConfiguracion() = False Then
                Exit Function
            End If

            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then

                Dim VLxmlNodoHijo As XmlNode

                'Obtiene el nodo hijo especificado
                VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, "Receptor")

                If Not IsNothing(VLxmlNodoHijo) Then
                    VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                    VLxmlDocumento.Save(_ArchivoXML)
                End If

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                Dim VLstrTextoNodo As String
                VLstrTextoNodo = "<Receptor>" & _
                                     "<BaseLocal>" & VPstrucReceptor.BaseLocal & "</BaseLocal>" & _
                                     "<DirBitTran>" & VPstrucReceptor.DirBitTran & "</DirBitTran>" & _
                                     "<LayOut>" & VPstrucReceptor.LayOut & "</LayOut>" & _
                                     "<Formato>" & VPstrucReceptor.Formato & "</Formato>" & _
                                     "<Pathway>" & VPstrucReceptor.Pathway & "</Pathway>" & _
                                     "<IdMascara>" & VPstrucReceptor.IDMascara & "</IdMascara>" & _
                                     "<IdMonto>" & VPstrucReceptor.IDMonto & "</IdMonto>" & _
                                     "<MaximoAlertas>" & VPstrucReceptor.MaximoAlertas & "</MaximoAlertas>" & _
                                     "<ECHO>" & IIf(VPstrucReceptor.ECHO = True, 1, 0) & "</ECHO>" & _
                                     "<TiempoECHO>" & VPstrucReceptor.TiempoECHO & "</TiempoECHO>" & _
                                     "<GeneraLogTX>" & IIf(VPstrucReceptor.GeneraLogTX = True, 1, 0) & "</GeneraLogTX>" & _
                                     "<HrInicio>" & VPstrucReceptor.HrInicio & "</HrInicio>" & _
                                     "<HrFin>" & VPstrucReceptor.HrFin & "</HrFin>" & _
                                     "<ContGeneral>" & IIf(VPstrucReceptor.ContGeneral = True, 1, 0) & "</ContGeneral>" & _
                                     "<ContSesiones>" & IIf(VPstrucReceptor.ContSesiones = True, 1, 0) & "</ContSesiones>" & _
                                     "<HrContadores>" & VPstrucReceptor.HrContadores & "</HrContadores>" & _
                                     "<IdToken>" & VPstrucReceptor.IdToken & "</IdToken>" & _
                                     "<TipoCambio>" & IIf(VPstrucReceptor.TipoCambio = True, 1, 0) & "</TipoCambio>" & _
                                     "<MontoToken>" & VPstrucReceptor.MontoToken & "</MontoToken>" & _
                                     "<MonedaToken>" & VPstrucReceptor.MonedaToken & "</MonedaToken>" & _
                                     "<VerToken>" & IIf(VPstrucReceptor.VerToken = True, 1, 0) & "</VerToken>" & _
                                     "<FuenteTCambio>" & VPstrucReceptor.FuenteTCambio & "</FuenteTCambio>" & _
                                     "<TipoCorreo>" & VPstrucReceptor.TipoCorreo & "</TipoCorreo>" & _
                                 "</Receptor>"
                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)
                Return True

            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<Receptor>" & _
                                     "<BaseLocal>" & VPstrucReceptor.BaseLocal & "</BaseLocal>" & _
                                     "<DirBitTran>" & VPstrucReceptor.DirBitTran & "</DirBitTran>" & _
                                     "<LayOut>" & VPstrucReceptor.LayOut & "</LayOut>" & _
                                     "<Formato>" & VPstrucReceptor.Formato & "</Formato>" & _
                                     "<Pathway>" & VPstrucReceptor.Pathway & "</Pathway>" & _
                                     "<IdMascara>" & VPstrucReceptor.IDMascara & "</IdMascara>" & _
                                     "<IdMonto>" & VPstrucReceptor.IDMonto & "</IdMonto>" & _
                                     "<MaximoAlertas>" & VPstrucReceptor.MaximoAlertas & "</MaximoAlertas>" & _
                                     "<ECHO>" & IIf(VPstrucReceptor.ECHO = True, 1, 0) & "</ECHO>" & _
                                     "<TiempoECHO>" & VPstrucReceptor.TiempoECHO & "</TiempoECHO>" & _
                                     "<GeneraLogTX>" & IIf(VPstrucReceptor.GeneraLogTX = True, 1, 0) & "</GeneraLogTX>" & _
                                     "<HrInicio>" & VPstrucReceptor.HrInicio & "</HrInicio>" & _
                                     "<HrFin>" & VPstrucReceptor.HrFin & "</HrFin>" & _
                                     "<ContGeneral>" & IIf(VPstrucReceptor.ContGeneral = True, 1, 0) & "</ContGeneral>" & _
                                     "<ContSesiones>" & IIf(VPstrucReceptor.ContSesiones = True, 1, 0) & "</ContSesiones>" & _
                                     "<HrContadores>" & VPstrucReceptor.HrContadores & "</HrContadores>" & _
                                     "<IdToken>" & VPstrucReceptor.IdToken & "</IdToken>" & _
                                     "<TipoCambio>" & IIf(VPstrucReceptor.TipoCambio = True, 1, 0) & "</TipoCambio>" & _
                                     "<MontoToken>" & VPstrucReceptor.MontoToken & "</MontoToken>" & _
                                     "<MonedaToken>" & VPstrucReceptor.MonedaToken & "</MonedaToken>" & _
                                     "<VerToken>" & IIf(VPstrucReceptor.VerToken = True, 1, 0) & "</VerToken>" & _
                                     "<FuenteTCambio>" & VPstrucReceptor.FuenteTCambio & "</FuenteTCambio>" & _
                                     "<TipoCorreo>" & VPstrucReceptor.TipoCorreo & "</TipoCorreo>" & _
                                 "</Receptor>"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)
                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GuardaPathway(ByVal VPstrucServidorTandem As ClsConfiguracion.ServidorTandem) As Boolean
        Try
            If ValidaConfiguracion() = False Then
                Exit Function
            End If

            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then
                Dim VLxmlNodoHijo As XmlNode

                'Obtiene el nodo hijo especificado
                VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, "Administrador")

                If Not IsNothing(VLxmlNodoHijo) Then
                    VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                    VLxmlDocumento.Save(_ArchivoXML)
                End If

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                Dim VLstrTextoNodo As String
                VLstrTextoNodo = "<Administrador>" & _
                                     "<Pathway>" & VPstrucServidorTandem.Pathway & "</Pathway>" & _
                                 "</Administrador>"
                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)
                Return True

            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<Administrador>" & _
                                     "<Pathway>" & VPstrucServidorTandem.Pathway & "</Pathway>" & _
                                 "</Administrador>"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch--24003 --> : " & ex.Message)
            MsgBox("KMRBatch--24003 --> : No se pudo guardar PATHWAY por el siguiente motivo " & vbNewLine & ex.Message, vbNewLine & " Verifique por favor " & MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

    Public Function GuardaParametros(ByVal VPstrucparametros As ClsConfiguracion.KMAConfig) As Boolean
        Try
            If ValidaConfiguracion() = False Then
                Exit Function
            End If

            Dim VLxmlDocumento As XmlDocument
            Dim VLxmlNodo As XmlNode 'Almacena el Nodo de Configuración

            VLxmlDocumento = New XmlDocument
            VLxmlDocumento.Load(_ArchivoXML)

            'Obtiene el nodo de la Configuración Solicitada
            VLxmlNodo = ObtenerNodo(VLxmlDocumento, _RutaNodo, _Configuracion)

            If Not IsNothing(VLxmlNodo) Then
                Dim VLxmlNodoHijo As XmlNode

                'Obtiene el nodo hijo especificado
                VLxmlNodoHijo = ObtenerNodo(VLxmlNodo, "", _Atributo, "KMAConfig")

                If Not IsNothing(VLxmlNodoHijo) Then
                    VLxmlNodo.RemoveChild(VLxmlNodoHijo)
                    VLxmlDocumento.Save(_ArchivoXML)
                End If

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlNodo.CreateNavigator

                Dim VLstrTextoNodo As String
                VLstrTextoNodo = "<KMAConfig>" & _
                                     "<Tokens>" & VPstrucparametros.Tokens & "</Tokens>" & _
                                     "<Patrol>" & VPstrucparametros.Patrol & "</Patrol>" & _
                                     "<Categorias>" & VPstrucparametros.Categorias & "</Categorias>" & _
                                     "<TiposUsuario>" & VPstrucparametros.TiposUsuario & "</TiposUsuario>" & _
                                     "<PermisosCtes>" & VPstrucparametros.PermisosCtes & "</PermisosCtes>" & _
                                     "<FiltrosCorporativos>" & VPstrucparametros.FiltrosCorporativos & "</FiltrosCorporativos>" & _
                                 "</KMAConfig>"
                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)
                Return True

            Else 'Crea el Nodo desde Cero

                Dim VLxmlNavegador As Xml.XPath.XPathNavigator = VLxmlDocumento.CreateNavigator
                Dim VLstrTextoNodo As String

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo = "<ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo = "<Configuracion ID='" & _Configuracion & "'>"
                End If

                VLstrTextoNodo &= "<KMAConfig>" & _
                                     "<Tokens>" & VPstrucparametros.Tokens & "</Tokens>" & _
                                     "<Patrol>" & VPstrucparametros.Patrol & "</Patrol>" & _
                                     "<Categorias>" & VPstrucparametros.Categorias & "</Categorias>" & _
                                     "<TiposUsuario>" & VPstrucparametros.TiposUsuario & "</TiposUsuario>" & _
                                     "<PermisosCtes>" & VPstrucparametros.PermisosCtes & "</PermisosCtes>" & _
                                     "<FiltrosCorporativos>" & VPstrucparametros.FiltrosCorporativos & "</FiltrosCorporativos>" & _
                                 "</KMAConfig>"

                If _Configuracion.Trim = "" Then
                    VLstrTextoNodo &= "</ConfiguracionMaestro>"
                Else
                    VLstrTextoNodo &= "</Configuracion>"
                End If

                VLxmlNavegador.AppendChild(VLstrTextoNodo)

                VLxmlDocumento.Save(_ArchivoXML)

                Return True
            End If
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch--24007 --> : " & ex.Message)
            MsgBox("KMRBatch--24007 --> : No se pudo guardar PARAMETROS por el siguiente motivo " & vbNewLine & ex.Message, vbNewLine & " Verifique por favor " & MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function


#End Region

End Class

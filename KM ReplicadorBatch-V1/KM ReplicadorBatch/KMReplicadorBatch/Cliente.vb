Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text


Public Class ClienteTCP

    Public Enum typEstadoConexion As Byte
        Desconectado = 0
        Conectado = 1
        Conectando = 2
        Desconectando = 3
        EnError = 4
        ResolviendoHost = 5
    End Enum

    Private _Puerto As Integer
    Private _IPRemota As String
    Private Shared _IPLocal As String
    Private Shared _Error As String
    Private Shared _UltimoError As String
    Private Shared _Estado As typEstadoConexion

#Region "Propiedades"
    Public Property Puerto() As Integer
        Get
            Puerto = _Puerto
        End Get
        Set(ByVal value As Integer)
            _Puerto = value
        End Set
    End Property
    Public Property IPRemota() As String
        Get
            IPRemota = _IPRemota
        End Get
        Set(ByVal value As String)
            _IPRemota = value
        End Set
    End Property
    Public ReadOnly Property Estado() As typEstadoConexion
        Get
            Estado = _Estado
        End Get
    End Property
    Public ReadOnly Property IPLocal() As String
        Get
            IPLocal = _IPLocal
        End Get
    End Property
    Public ReadOnly Property UltimoError() As String
        Get
            UltimoError = _UltimoError
        End Get
    End Property
#End Region

    ' The port number for the remote device.
    Private VMtcpCliente As Socket
    Private VMhilRecibeDatos As Thread
    Private VMhilConectar As Thread

    ' ManualResetEvent instances signal completion.
    Private Shared connectDone As ManualResetEvent
    Private Shared sendDone As ManualResetEvent
    'Private Shared receiveDone As ManualResetEvent

    Public Event DatosRecibidos(ByVal Mensaje As String)
    Public Event Informacion(ByVal Estado As typEstadoConexion, ByVal Mensaje As String)
    Public Event Conexion(ByVal Estado As typEstadoConexion, ByVal Mensaje As String)

    ''' <summary>
    ''' Establece la conexión con un servidor remoto
    ''' </summary>
    Public Sub Conectar()
        ' Establish the remote endpoint for the socket.
        ' For this example use local machine.
        Dim VLobjIPInfo As IPHostEntry
        Dim VLobjDireccionIP As IPAddress
        Dim remoteEP As IPEndPoint

        _Estado = typEstadoConexion.Conectando
        System.Windows.Forms.Application.DoEvents()
        RaiseEvent Informacion(typEstadoConexion.ResolviendoHost, "Resolviendo nombre de servidor...")
        If _IPRemota.Trim = "" Then
            System.Windows.Forms.Application.DoEvents()
            RaiseEvent Conexion(typEstadoConexion.EnError, "Dirección de servidor remoto incorrecto")
            Exit Sub
        End If

        Try
            VLobjIPInfo = Dns.GetHostEntry(_IPRemota)
            VLobjDireccionIP = Nothing
            For Each VLobjDireccionIP In VLobjIPInfo.AddressList
                If VLobjDireccionIP.AddressFamily = AddressFamily.InterNetwork Then
                    Exit For
                End If
            Next
            RaiseEvent Informacion(typEstadoConexion.Conectando, "Dirección obtenida [" & VLobjDireccionIP.ToString & "]")

            remoteEP = New IPEndPoint(VLobjDireccionIP, _Puerto)

            ' Create a TCP/IP socket.
            System.Windows.Forms.Application.DoEvents()
            RaiseEvent Informacion(typEstadoConexion.Conectando, "Conectando con servidor remoto...")

            VMtcpCliente = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            VMtcpCliente.BeginConnect(remoteEP, New AsyncCallback(AddressOf ConnectCallback), VMtcpCliente)

            Try
                connectDone.WaitOne()
            Catch ex As Exception
                RaiseEvent Conexion(typEstadoConexion.EnError, ex.Message)
            End Try

            'Valida el estado de la conexión
            If _Estado = typEstadoConexion.Conectado Then
                'Levanta el hilo para recibir datos
                VMhilRecibeDatos = New Thread(AddressOf recibeDatos)
                VMhilRecibeDatos.IsBackground = False
                VMhilRecibeDatos.Start()

                System.Windows.Forms.Application.DoEvents()
                RaiseEvent Conexion(typEstadoConexion.Conectado, "")
            Else
                System.Windows.Forms.Application.DoEvents()
                RaiseEvent Conexion(typEstadoConexion.EnError, _Error)
            End If
        Catch ex As SocketException
            _Estado = typEstadoConexion.EnError
            System.Windows.Forms.Application.DoEvents()
            RaiseEvent Conexion(typEstadoConexion.EnError, ex.Message)
        End Try

        connectDone.Close()
    End Sub
    ''' <summary>
    ''' Establece la conexión con un servidor remoto
    ''' </summary>
    Public Sub Conectar(ByVal Puerto As Integer)
        _Puerto = Puerto
        Conectar()
    End Sub
    ''' <summary>
    ''' Establece la conexión con un servidor remoto
    ''' </summary>
    Public Sub Conectar(ByVal IPRemota As String, ByVal Puerto As Integer)
        _Puerto = Puerto
        _IPRemota = IPRemota
        Conectar()
    End Sub
    Public Sub LimpiaError()
        _UltimoError = ""
    End Sub
    Public Sub Enviar(ByVal VPstrCadena As String)
        Dim byteData As Byte() = Encoding.ASCII.GetBytes(VPstrCadena)
        If VMtcpCliente.Connected Then
            VMtcpCliente.Send(byteData)
        End If
        'Send(VMtcpCliente, VPstrCadena)
    End Sub
    ''' <summary>
    ''' Cierra la conexión con el servidor remoto
    ''' </summary>
    Public Sub Cerrar()
        If _Estado <> typEstadoConexion.Desconectado And _Estado <> typEstadoConexion.Desconectando Then
            _Estado = typEstadoConexion.Desconectando
            RaiseEvent Informacion(typEstadoConexion.Desconectando, "")
        End If
        'Detiene el hilo
        If _Estado = typEstadoConexion.Conectado Then
            VMhilRecibeDatos.Abort()
        End If
        If VMtcpCliente.Connected Then
            VMtcpCliente.Shutdown(SocketShutdown.Both)
            VMtcpCliente.Close()
        End If

        If _Estado <> typEstadoConexion.Desconectado Then
            _Estado = typEstadoConexion.Desconectado
            RaiseEvent Conexion(typEstadoConexion.Desconectado, "")
        End If
    End Sub
    Private Shared Sub ConnectCallback(ByVal ar As IAsyncResult)
        ' Retrieve the socket from the state object.
        Dim client As Socket = CType(ar.AsyncState, Socket)
        Dim VLipLocal As IPAddress
        Dim hostInfo As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(My.Computer.Name)

        ' Complete the connection.
        Try
            client.EndConnect(ar)
            _Estado = typEstadoConexion.Conectado
            Dim IP As IPAddress
            VLipLocal = Nothing
            For Each IP In hostInfo.AddressList
                If IP.AddressFamily = AddressFamily.InterNetwork Then
                    VLipLocal = IP
                    Exit For
                End If
            Next
            _IPLocal = VLipLocal.ToString
        Catch ex As Exception
            _Estado = typEstadoConexion.EnError
            _Error = ex.Message
            _UltimoError = _Error
        End Try

        ' Signal that the connection has been made.
        connectDone.Set()
    End Sub 'ConnectCallback
    Private Shared Sub Send(ByVal client As Socket, ByVal data As String)
        ' Convert the string data to byte data using ASCII encoding.
        Dim byteData As Byte() = Encoding.ASCII.GetBytes(data)

        ' Begin sending the data to the remote device.
        client.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), client)
    End Sub 'Send
    Private Shared Sub SendCallback(ByVal ar As IAsyncResult)
        ' Retrieve the socket from the state object.
        Dim client As Socket = CType(ar.AsyncState, Socket)

        ' Complete sending the data to the remote device.
        client.EndSend(ar)

        ' Signal that all bytes have been sent.
        sendDone.Set()
    End Sub 'SendCallback
    Public Sub New()
        _Puerto = 0
        _IPRemota = ""
        _IPLocal = "127.0.0.1"
        _Estado = typEstadoConexion.Desconectado
        connectDone = New ManualResetEvent(False)
    End Sub
    Private Sub recibeDatos()
        Dim Recibir() As Byte 'Array utilizado para recibir los datos que llegan 
        Dim VLstrAux As String 'Variable donde se almacena el mensaje recibido

        Dim Ret As Integer = 0

        While True
            If VMtcpCliente.Connected Then
                Try
                    'Me quedo esperando a que llegue un mensaje desde el cliente 
                    'Ret = .Socket.Receive(Recibir, Recibir.Length, SocketFlags.None)
                    Recibir = New Byte(0) {}
                    Ret = VMtcpCliente.Receive(Recibir, Recibir.Length, SocketFlags.None)
                    VLstrAux = Mid(Encoding.ASCII.GetString(Recibir), 1, Ret)
                    Ret = VMtcpCliente.Available
                    Ret = 300
                    If Ret > 0 Then
                        Recibir = New Byte(Ret - 1) {}
                        Ret = VMtcpCliente.Receive(Recibir, Recibir.Length, SocketFlags.None)
                    End If
                    'Verifica cuanta información está pendiente y la procesa
                    If Ret > 0 Or VLstrAux.Length > 0 Then
                        'Guardo el mensaje recibido 
                        If Ret > 0 Then
                            VLstrAux = VLstrAux & Mid(Encoding.ASCII.GetString(Recibir), 1, Ret)
                        End If
                        'Genero el evento de la recepcion del mensaje 
                        RaiseEvent DatosRecibidos(VLstrAux)
                    Else
                        'Genero el evento de la finalizacion de la conexion 
                        RaiseEvent Conexion(typEstadoConexion.Desconectado, "")
                        Exit While
                    End If
                Catch e As Exception
                    If Not VMtcpCliente.Connected Then
                        'Genero el evento de la finalizacion de la conexion 
                        RaiseEvent Conexion(typEstadoConexion.Desconectado, "")
                        Exit While
                    End If
                End Try
            End If
        End While
    End Sub
    Private Sub conectaSocket()
        Dim VLobjIPInfo As IPHostEntry
        Dim VLobjDireccionIP As IPAddress
        Dim remoteEP As IPEndPoint

        System.Windows.Forms.Application.DoEvents()
        RaiseEvent Conexion(typEstadoConexion.ResolviendoHost, "")
        If _IPRemota.Trim = "" Then
            System.Windows.Forms.Application.DoEvents()
            RaiseEvent Conexion(typEstadoConexion.EnError, "Dirección de servidor remoto incorrecto")
            Exit Sub
        End If

        Try
            VLobjIPInfo = Dns.GetHostEntry(_IPRemota)
            VLobjDireccionIP = VLobjIPInfo.AddressList(0)
            remoteEP = New IPEndPoint(VLobjDireccionIP, _Puerto)

            ' Create a TCP/IP socket.
            System.Windows.Forms.Application.DoEvents()
            RaiseEvent Conexion(typEstadoConexion.Conectando, "")

            VMtcpCliente = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            VMtcpCliente.Connect(remoteEP)
            If VMtcpCliente.Connected Then
                _Estado = typEstadoConexion.Conectado
            Else
                VMtcpCliente.Shutdown(SocketShutdown.Both)
                _Estado = typEstadoConexion.Desconectado
            End If

            RaiseEvent Conexion(typEstadoConexion.Conectado, "")


        Catch ex As Exception
            System.Windows.Forms.Application.DoEvents()
            RaiseEvent Conexion(typEstadoConexion.EnError, ex.Message)
            VMhilConectar.Abort()
        End Try
    End Sub
End Class

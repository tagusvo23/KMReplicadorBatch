Imports System.Data.OracleClient
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports System.Threading
Imports C1.Win.C1FlexGrid
Imports Microsoft.Win32
Imports System.Drawing.Printing
Imports System.Drawing
Imports System.IO 'para usar StreamReader y leer registros
Public Class frmProcesaTT
    Dim VMioArchivo As StreamReader      'para guardar el nombre del archivo a cargar
    Dim VMstrRegistroTxt As String = ""  'para recibir el registro TXT y procesarlo.
    Private _RutaDirectorio As String
    Dim VMintCtrlReg As Integer = 0        'para controlar que registro del archivo TXT se guarda y que contador se incrementa.
    Dim VMintCtrl As Integer = 0           'para controlar la ejecuación de eventos de algunos objetos
    Dim VMintTotalRegArchivo As Integer = 0 'total de registros del archivo TXT
    '---------------------------
#Region "Estructuras"
    ''' <remarks>Contiene todos los datos del registro del archivo de texto </remarks>
    Public Structure DatosTxt
        Dim FechaHoraKm As String 'es la fecha del servidor con longitud de 26
        'Dim IdTran As Integer
        Dim OrigenKM As Integer 'es un dato fijo con valor '04' para cheques y '05' para tarjetas y es numerico
        Dim Origen As Integer ' Origen es un dato fijo con valor '04'  para cheques y '05' para tarjetas y es numerico
        Dim FechaHora As String
        Dim CuentaCI As String
        Dim TipoCuenta As Integer
        Dim NumCliente As String
        Dim NombreCliente As String
        Dim TipoPago As Integer
        Dim TipoOperacion As String
        Dim DescOperacion As String
        Dim Sucursal As Integer
        Dim Cajero As String
        Dim Autorizador As String
        Dim Referencia As String
        Dim NumCheque As String
        Dim Moneda As String
        Dim Monto As Integer
        Dim SaldoAntOper As Integer
        Dim SaldoDespOper As Integer
        Dim Banco As String
        Dim CntaBeneficiario As String
        Dim ClienteBeneficiario As String
        Dim Respuesta As String
        Dim IPCliente As String
        Dim TipoCambio As Integer
        Dim NumTrajeta As Long
        Dim EntryMode As Integer
        Dim CapCode As Integer
        Dim MerchanType As Integer
        Dim MemberID As String
        Dim Terminal_ID As String
        Dim NombreComercio As String
        Dim Ciudad As String
        Dim Poblacion As String
        Dim AutNum As String
        Dim Afiliacion As String
    End Structure

#End Region

    Private Sub CboLayout_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CboLayout.SelectedIndexChanged
        If VMintCtrl = 0 Then
            cargaFormatos(Val(CboLayout.Text))
        End If
    End Sub
    Private Sub frmProcesaTT_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.CboLayout.Focus()
        LimpiaCampos()
    End Sub
    '---------------------------
    Private Sub PicBoxEjecutar_Click(sender As System.Object, e As System.EventArgs) Handles PicBoxEjecutar.Click

        Dim VLintTotalLeidos As Integer = 0
        Dim VLintTotalProcesados As Integer = 0
        Dim VLintTotalErroneos As Integer = 0
        Dim VLintTotalEnBlanco As Integer = 0

        Dim VLintConta As Integer = 0

        TxtTotalLeidos.Text = ""
        TxtTotalProcesados.Text = ""
        TxtTotalErroneos.Text = ""
        TxtTotalEnBlanco.Text = ""
        ProgressBar1.Value = 0

        If ValidarDatos() Then
            If Not ArmaNombreTablaTT() Then
                MsgBox("No pudo crear armar el nombre de la tabla a procesar, verifique por favor ", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "KM Replicador Batch")
                CboLayout.Focus()
                Exit Sub
            End If

            If Not ValidaTablaTT() Then
                MsgBox("La tabla : " & VGstrNombreTablaTT & " no existe, verifique por favor la fecha de proceso ", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
                PicBoxEjecutar.Focus()
                Exit Sub
            End If

            Try
                PanelProcesaTT.Enabled = False
                If Val(CboTipoRegistro.Text) = 1 Then ' condicion para leer registros de cheques
                    VMioArchivo = New StreamReader(dlgArchivo.FileName)
                    VMintTotalRegArchivo = File.ReadAllLines(dlgArchivo.FileName).Length

                    Me.Cursor = Cursors.WaitCursor
                    While Not VMioArchivo.EndOfStream
                        VMstrRegistroTxt = Nothing
                        VMintCtrlReg = 0
                        VMstrRegistroTxt = VMioArchivo.ReadLine

                        VLintTotalLeidos = VLintTotalLeidos + 1

                        If Not IsNothing(VMstrRegistroTxt) Then
                            '--------------------
                            If Mid(VMstrRegistroTxt, 1, 2) = "02" Then
                                ArmaRegistroCheques()
                                If VMintCtrlReg = 0 Then
                                    If InsertaRegistroTT(VGstrucDatosTxt) Then
                                        VLintTotalProcesados = VLintTotalProcesados + 1
                                    End If
                                ElseIf VMintCtrlReg = 1 Then
                                    ArchivoPlanoConError()
                                    VLintTotalErroneos = VLintTotalErroneos + 1
                                End If
                            End If
                            '--------------------
                        Else
                            VLintTotalEnBlanco = VLintTotalEnBlanco + 1
                        End If

                        llenabarra(VMintTotalRegArchivo, VLintTotalLeidos)
                    End While

                ElseIf Val(CboTipoRegistro.Text) = 2 Then ' condicion para leer registros de tarjetas
                    VMioArchivo = New StreamReader(dlgArchivo.FileName)
                    VMintTotalRegArchivo = File.ReadAllLines(dlgArchivo.FileName).Length
                    Me.Cursor = Cursors.WaitCursor

                    While Not VMioArchivo.EndOfStream

                        VMstrRegistroTxt = Nothing
                        VMintCtrlReg = 0
                        VMstrRegistroTxt = VMioArchivo.ReadLine
                        VLintConta = VLintConta + 1

                        If VLintConta > 6 And (VLintConta < VMintTotalRegArchivo) Then
                            VLintTotalLeidos = VLintTotalLeidos + 1
                            If Not IsNothing(VMstrRegistroTxt) Then
                                ArmaRegistroTarjetas()
                                If VMintCtrlReg = 0 Then
                                    If InsertaRegistroTT(VGstrucDatosTxt) Then
                                        VLintTotalProcesados = VLintTotalProcesados + 1
                                    End If
                                ElseIf VMintCtrlReg = 1 Then
                                    ArchivoPlanoConError()
                                    VLintTotalErroneos = VLintTotalErroneos + 1
                                End If
                            Else
                                VLintTotalEnBlanco = VLintTotalEnBlanco + 1
                            End If
                            llenabarra(VMintTotalRegArchivo, VLintTotalLeidos)
                        End If
                    End While

                End If
                VMioArchivo.Close()
                Me.Cursor = Cursors.Default
                TxtTotalLeidos.Text = VLintTotalLeidos
                TxtTotalProcesados.Text = VLintTotalProcesados
                TxtTotalErroneos.Text = VLintTotalErroneos
                TxtTotalEnBlanco.Text = VLintTotalEnBlanco
                MsgBox("Proceso Terminado OK ", MsgBoxStyle.Information)
                PanelProcesaTT.Enabled = True
                CboLayout.Focus()

            Catch ex As Exception
                VGobjDepuracion.ArchivoPlano("KMRBatch-04001 --> : " & ex.Message)
                MsgBox("KMRBatch-04001 Conflicto al leer archivo", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "KM Replicador Batch")
            End Try
        End If
    End Sub
    Private Sub PicBoxCargaArchivos_Click(sender As System.Object, e As System.EventArgs) Handles PicBoxCargaArchivos.Click

        If dlgArchivo.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                If dlgArchivo.FileName.Length > 0 Then
                    TxtArchivoSeleccionado.Text = dlgArchivo.FileName
                End If
            Catch ex As Exception
                MsgBox("El archivo que selecciono no es valido, verifique por favor ", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "KM Replicador Batch")
                Exit Sub
            End Try
        End If

    End Sub

    Private Function ValidarDatos() As Boolean
        'valida datos para procesar las tablas TT.
        ValidarDatos = False
        If CboLayout.Text.Trim = "" Then
            MsgBox("Debe seleccionar un LayOut, verifique por favor ", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            CboLayout.Focus()
        ElseIf CboFormatos.Text.Trim = "" Then
            MsgBox("Debe seleccionar un Formato, verifique por favor", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            CboFormatos.Focus()
        ElseIf CboTipoRegistro.Text.Trim = "" Then
            MsgBox("Debe seleccionar un Tipo de Registro, verifique por favor", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            CboTipoRegistro.Focus()
        ElseIf TxtArchivoSeleccionado.Text.Trim = "" Then
            MsgBox("Debe seleccionar un Archivo para Procesar, verifique por favor", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
            PicBoxCargaArchivos.Focus()
            Exit Function
        Else
            ValidarDatos = True
        End If
    End Function
    Private Sub ArmaRegistroCheques()
        Try
            VGstrucDatosTxt = Nothing
            VMintCtrlReg = 0

            VGstrucDatosTxt.OrigenKM = 4
            VGstrucDatosTxt.Origen = 4
            VGstrucDatosTxt.FechaHora = Mid(VMstrRegistroTxt, 12, 8)
            VGstrucDatosTxt.CuentaCI = Mid(VMstrRegistroTxt, 70, 11)
            VGstrucDatosTxt.TipoCuenta = 0
            VGstrucDatosTxt.NumCliente = ""
            VGstrucDatosTxt.NombreCliente = ""
            VGstrucDatosTxt.TipoPago = 0
            VGstrucDatosTxt.TipoOperacion = ""
            VGstrucDatosTxt.DescOperacion = ""
            VGstrucDatosTxt.Sucursal = 0
            VGstrucDatosTxt.Cajero = ""
            VGstrucDatosTxt.Autorizador = ""
            VGstrucDatosTxt.Referencia = ""
            VGstrucDatosTxt.NumCheque = Mid(VMstrRegistroTxt, 81, 10)
            VGstrucDatosTxt.Moneda = Mid(VMstrRegistroTxt, 63, 2)
            VGstrucDatosTxt.Monto = Mid(VMstrRegistroTxt, 26, 15)
            VGstrucDatosTxt.SaldoAntOper = 0
            VGstrucDatosTxt.SaldoDespOper = 0
            VGstrucDatosTxt.Banco = Mid(VMstrRegistroTxt, 20, 3)
            VGstrucDatosTxt.CntaBeneficiario = Mid(VMstrRegistroTxt, 159, 11)
            VGstrucDatosTxt.ClienteBeneficiario = Mid(VMstrRegistroTxt, 170, 10)
            VGstrucDatosTxt.Respuesta = Mid(VMstrRegistroTxt, 105, 2)
            VGstrucDatosTxt.IPCliente = ""
            VGstrucDatosTxt.TipoCambio = 0
            VGstrucDatosTxt.NumTrajeta = 0
            VGstrucDatosTxt.EntryMode = 0
            VGstrucDatosTxt.CapCode = 0
            VGstrucDatosTxt.MerchanType = 0
            VGstrucDatosTxt.MemberID = Mid(VMstrRegistroTxt, 23, 3)
            VGstrucDatosTxt.Terminal_ID = ""
            VGstrucDatosTxt.NombreComercio = ""
            VGstrucDatosTxt.Ciudad = ""
            VGstrucDatosTxt.Poblacion = ""
            VGstrucDatosTxt.AutNum = ""
            VGstrucDatosTxt.Afiliacion = ""

        Catch ex As Exception
            VMintCtrlReg = 1
            'VGobjDepuracion.ArchivoPlano("KMRBatch-04005 --> : " & ex.Message)
            'MsgBox("KMRBatch-04005 Conflicto al leer archivo", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "KM Replicador Batch")
        End Try

    End Sub
    Private Sub ArmaRegistroTarjetas()
        Try

            VGstrucDatosTxt = Nothing

            VGstrucDatosTxt.OrigenKM = 5
            VGstrucDatosTxt.Origen = 5
            VGstrucDatosTxt.FechaHora = Mid(VMstrRegistroTxt, 150, 17)
            VGstrucDatosTxt.CuentaCI = ""
            VGstrucDatosTxt.TipoCuenta = 0
            VGstrucDatosTxt.NumCliente = ""
            VGstrucDatosTxt.NombreCliente = ""
            VGstrucDatosTxt.TipoPago = 0
            VGstrucDatosTxt.TipoOperacion = ""
            VGstrucDatosTxt.DescOperacion = ""
            VGstrucDatosTxt.Sucursal = 0
            VGstrucDatosTxt.Cajero = ""
            VGstrucDatosTxt.Autorizador = ""
            VGstrucDatosTxt.Referencia = ""
            VGstrucDatosTxt.NumCheque = ""
            VGstrucDatosTxt.Moneda = ""
            VGstrucDatosTxt.Monto = Mid(VMstrRegistroTxt, 181, 10)
            VGstrucDatosTxt.SaldoAntOper = 0
            VGstrucDatosTxt.SaldoDespOper = 0
            VGstrucDatosTxt.Banco = ""
            VGstrucDatosTxt.CntaBeneficiario = ""
            VGstrucDatosTxt.ClienteBeneficiario = ""
            VGstrucDatosTxt.Respuesta = Mid(VMstrRegistroTxt, 127, 3)
            VGstrucDatosTxt.IPCliente = ""
            VGstrucDatosTxt.TipoCambio = 0
            VGstrucDatosTxt.NumTrajeta = Mid(VMstrRegistroTxt, 37, 19)
            VGstrucDatosTxt.EntryMode = Mid(VMstrRegistroTxt, 271, 3)
            VGstrucDatosTxt.CapCode = Mid(VMstrRegistroTxt, 278, 1)
            VGstrucDatosTxt.MerchanType = 0
            VGstrucDatosTxt.MemberID = Mid(VMstrRegistroTxt, 3, 4)
            VGstrucDatosTxt.Terminal_ID = ""
            VGstrucDatosTxt.NombreComercio = Mid(VMstrRegistroTxt, 103, 15)
            VGstrucDatosTxt.Ciudad = ""
            VGstrucDatosTxt.Poblacion = Mid(VMstrRegistroTxt, 115, 2)
            VGstrucDatosTxt.AutNum = Mid(VMstrRegistroTxt, 227, 6)
            VGstrucDatosTxt.Afiliacion = Mid(VMstrRegistroTxt, 25, 10)

            '            VGstrucDatosTxt.CapCode = Mid(VMstrRegistroTxt, 277, 1) 

        Catch ex As Exception
            VMintCtrlReg = 1
        End Try

    End Sub

    Private Function InsertaRegistroTT(ByVal DatosCehques As DatosTxt) As Boolean
        Dim VLstrSQL As String = ""
        Dim VLdbsBase As Object = Nothing
        Dim VLcmdComando As Object = Nothing
        Dim VLrcsDatos As Object

        InsertaRegistroTT = False

        Try
            Select Case VGstrucTransaccional.Tipo
                Case TipoBase.MySQL
                    VLdbsBase = New MySqlConnection
                Case TipoBase.Oracle
                    VLdbsBase = New OracleConnection
                Case TipoBase.SQLServer
                    VLdbsBase = New SqlClient.SqlConnection
            End Select

            VLdbsBase.ConnectionString = BaseDatos.CadenaConexion(VGstrucTransaccional)

            VLdbsBase.open()
            VLstrSQL = ""
            'obtenemos el tipo de bd para ingresar el formato de fecha
            Select Case VGstrucTransaccional.Tipo
                Case TipoBase.MySQL
                    'VGstrucDatosTxt.FechaHoraKm = Format(CDate(Now), "yyyy-MM-dd HH:mm:ss")
                    VLstrSQL = "INSERT INTO " & VGstrNombreTablaTT & " VALUES ( Now()"
                Case TipoBase.Oracle
                    'VGstrucDatosTxt.FechaHoraKm = "'to_date( SYSDATE, " & "yyyy-MM-dd HH24:MI:SS:SSSSS" & "') "
                    VLstrSQL = "INSERT INTO " & VGstrNombreTablaTT & " VALUES ( SYSDATE()"
                Case TipoBase.SQLServer
                    'VGstrucDatosTxt.FechaHoraKm = " Format(CDate(GETDATE), " & " yyyy/mm/dd HH:mm:ss.fff "
                    VLstrSQL = "INSERT INTO " & VGstrNombreTablaTT & " VALUES ( GETDATE()"
            End Select

            VLstrSQL = VLstrSQL & ", " & _
                   VGstrucDatosTxt.OrigenKM & ", " & _
                   VGstrucDatosTxt.Origen & ", '" & _
                   VGstrucDatosTxt.FechaHora & "', '" & _
                   VGstrucDatosTxt.CuentaCI & "', " & _
                   VGstrucDatosTxt.TipoCuenta & ", '" & _
                   VGstrucDatosTxt.NumCliente & "', '" & _
                   VGstrucDatosTxt.NombreCliente & "', " & _
                   VGstrucDatosTxt.TipoPago & ", '" & _
                   VGstrucDatosTxt.TipoOperacion & "', '" & _
                   VGstrucDatosTxt.DescOperacion & "', " & _
                   VGstrucDatosTxt.Sucursal & ", '" & _
                   VGstrucDatosTxt.Cajero & "', '" & _
                   VGstrucDatosTxt.Autorizador & "', '" & _
                   VGstrucDatosTxt.Referencia & "', '" & _
                   VGstrucDatosTxt.NumCheque & "', '" & _
                   VGstrucDatosTxt.Moneda & "', " & _
                   VGstrucDatosTxt.Monto & ", " & _
                   VGstrucDatosTxt.SaldoAntOper & ", " & _
                   VGstrucDatosTxt.SaldoDespOper & ", '" & _
                   VGstrucDatosTxt.Banco & "', '" & _
                   VGstrucDatosTxt.CntaBeneficiario & "', '" & _
                   VGstrucDatosTxt.ClienteBeneficiario & "', '" & _
                   VGstrucDatosTxt.Respuesta & "', '" & _
                   VGstrucDatosTxt.IPCliente & "', " & _
                   VGstrucDatosTxt.TipoCambio & ", " & _
                   VGstrucDatosTxt.NumTrajeta & ", " & _
                   VGstrucDatosTxt.EntryMode & ", " & _
                   VGstrucDatosTxt.CapCode & ", " & _
                   VGstrucDatosTxt.MerchanType & ", '" & _
                   VGstrucDatosTxt.MemberID & "', '" & _
                   VGstrucDatosTxt.Terminal_ID & "', '" & _
                   VGstrucDatosTxt.NombreComercio & "', '" & _
                   VGstrucDatosTxt.Ciudad & "', '" & _
                   VGstrucDatosTxt.Poblacion & "', '" & _
                   VGstrucDatosTxt.AutNum & "', '" & _
                   VGstrucDatosTxt.Afiliacion & "')"

            Select Case VGstrucTransaccional.Tipo
                Case TipoBase.MySQL
                    VLcmdComando = New MySqlCommand(VLstrSQL, VLdbsBase)
                Case TipoBase.Oracle
                    VLcmdComando = New OracleCommand(VLstrSQL, VLdbsBase)
                Case TipoBase.SQLServer
                    VLcmdComando = New SqlCommand(VLstrSQL, VLdbsBase)
            End Select

            Try
                VLrcsDatos = VLcmdComando.ExecuteReader
                VLrcsDatos.Close()
                VLcmdComando.Dispose()
                InsertaRegistroTT = True
            Catch ex As MySqlException
                VMintCtrlReg = 1
                VLdbsBase.close()
            Catch ex As OracleException
                VMintCtrlReg = 1
                VLdbsBase.close()
            Catch ex As SqlException
                VMintCtrlReg = 1
                VLdbsBase.close()
            Catch ex As Exception
                VLdbsBase.close()
                VGobjDepuracion.ArchivoPlano("KMRBatch-04009 --> : " & ex.Message)
            End Try

        Catch ex As MySqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-04011 --> : " & ex.Message)
            MsgBox("KMRBatch-16007:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As OracleException
            VGobjDepuracion.ArchivoPlano("KMRBatch-04011 --> : " & ex.Message)
            MsgBox("KMRBatch-16007:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As SqlException
            VGobjDepuracion.ArchivoPlano("KMRBatch-04011 --> : " & ex.Message)
            MsgBox("KMRBatch-16007:" & obtenerErrorBD(ex), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        Catch ex As Exception
            VGobjDepuracion.ArchivoPlano("KMRBatch-04011 --> : " & ex.Message)
        End Try

    End Function


    ''' Permite guardar los registros del archivo TXT que se lee y tienen algun error
    Public Sub ArchivoPlanoConError()
        Dim VLioArchivo As System.IO.StreamWriter
        Dim VLstrArchivo As String
        Try

            _RutaDirectorio = My.Application.Info.DirectoryPath & "\log"
            If Not My.Computer.FileSystem.DirectoryExists(_RutaDirectorio) Then
                My.Computer.FileSystem.CreateDirectory(_RutaDirectorio)
            End If
            If Val(CboTipoRegistro.Text) = 1 Then
                VLstrArchivo = "KSLOGErrorCheques" & Format(Date.Now, "yyyyMMdd")
                VLioArchivo = New System.IO.StreamWriter(_RutaDirectorio & "\" & VLstrArchivo & ".err", True, System.Text.Encoding.Default)
                VLioArchivo.WriteLine(VMstrRegistroTxt)
                VLioArchivo.Close()
                VLioArchivo.Dispose()
            ElseIf Val(CboTipoRegistro.Text) = 2 Then
                VLstrArchivo = "KSLOGErrorTarjetas" & Format(Date.Now, "yyyyMMdd")
                VLioArchivo = New System.IO.StreamWriter(_RutaDirectorio & "\" & VLstrArchivo & ".err", True, System.Text.Encoding.Default)
                VLioArchivo.WriteLine(VMstrRegistroTxt)
                VLioArchivo.Close()
                VLioArchivo.Dispose()
            End If
        Catch ex As Exception
            MsgBox("KMRBatch-04013:" & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Sub LimpiaCampos()
        TxtTotalLeidos.Text = ""
        TxtTotalProcesados.Text = ""
        TxtTotalErroneos.Text = ""
        TxtTotalEnBlanco.Text = ""
        CboLayout.SelectedIndex = -1
        CboFormatos.Items.Clear()
        CboTipoRegistro.SelectedIndex = -1
        TxtArchivoSeleccionado.Text = ""
        ProgressBar1.Value = 0
    End Sub

    Private Sub llenabarra(ByVal VPintTotal, ByVal VPintContador)
        Dim ValorBarra
        ValorBarra = VPintContador * 100 / VPintTotal
        If ValorBarra <= 100 Then
            ProgressBar1.Value = CInt(ValorBarra)
        End If

    End Sub

    Private Function ArmaNombreTablaTT() As Boolean
        ' arma nombre de tabla TT

        Dim VLstrLayout As String = ""
        Dim VLstrFormato As String = ""

        ArmaNombreTablaTT = False

        Try
            VLstrLayout = Val(CboLayout.Text)
            VLstrFormato = Val(CboFormatos.Text)

            If Len(VLstrLayout) = 1 Then
                VLstrLayout = "0" & Val(CboLayout.Text)
            End If

            If Len(VLstrFormato) = 1 Then
                VLstrFormato = "0" & Val(CboFormatos.Text)
            End If

            VGstrNombreTablaTT = "TT" & VGstrFechaArchivo & VLstrLayout & VLstrFormato
            ArmaNombreTablaTT = True
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Private Sub BtnSalir_Click(sender As System.Object, e As System.EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

End Class
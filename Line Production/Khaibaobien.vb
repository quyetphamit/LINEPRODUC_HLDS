Imports System
Imports System.String
Imports System.Threading
Imports System.IO
Imports System.IO.Ports
Imports System.ComponentModel
Module Khaibaobien
    ' bien quy dinh duong dan file
    Public PathApplication As String = Application.StartupPath
    Public PathSetup As String = PathApplication & "\Setup"
    Public PathPassrate As String = PathApplication & "\Passrate"
    Public PathSetupComport As String = PathSetup & "\Setting Comport.ini"
    Public PathModelList As String = PathSetup & "\List model.ini"
    Public PathPassList As String = PathSetup & "\SetupListPass.ini"
    Public PathSetupPath As String = PathSetup & "\Setup Path.ini"
    Public PathStatus As String = ""
    Public PathReport As String = PathApplication & "\Report"
    ' cac bien cai dat den line san xuat---------------------------------------------------------------
    Public IdLine As Integer = 1
    Public NameLine As String = ""
    Public NoPeople As Integer = 0 ' bien lua gia tri so nguoi can cua 1 model
    Public BarcodeEnable As Boolean = False ' bien cho phep model co su dung chuc nang barcode hay khong
    Public CycleTimeModel As Double = 30.6
    Public CycleTimeActual As Double = 0.0
    Public Shiftcheck As Boolean = True ' true la ca dem, False la ca dem
    Public Datecheck As String = ""
    Public StartProduct As Boolean = False ' bien quy dinh ve line chay hay dung
    Public TimeLine(30) As DateTime ' bien quy dinh khung gio 
    Public TimePauseLine As Integer ' bien ghi gio tam dung line
    Public CountProduct As Integer = 0 ' dem san luong cua line model
    Public CountProductPerHour(30) As Integer ' dem san luong cua tung gio
    'Public notePerHour(10) As String
    Public ProductPlan As Integer = 0 ' dem san luong tu dong theo cycle time cua model
    Public ProductPlanBegin As Integer = 0 ' dem san luong tu dong theo cycle time cua model
    Public BalanceProduction As Integer = 0 'chenh lech san luong thuc te
    Public BalanceAlarmSetup As Integer ' gia tri dat bat dau canh bao san luong
    Public BalanceErrorSetup As Integer ' gia tri dat bat dau canh bao san luong
    Public BitPress As Boolean = False ' bien xac nhan chong nhieu cho nut an tang qu cong com
    Public PauseProduct As Boolean = False ' bien xac nhan trang thai line dang tam dung
    Public TimeCountPlan As Integer = 0 ' bien dem thoi gian cua cycle time theo line
    Public TimeCycleActual As Double = 0 ' bien dem thoi gian cycle time thuc te ma line dang chay
    Public TimeUse(9) As Boolean ' moc thoi gian ma line da chay va su dung
    Public StatusLine As Integer = 0 ' bien luu trang thai line, 0: chua hoat dong, 1: bat dau hoat dong, 2: Bao dong san luong  3:Bat thuong
    Public TimeCycleActual2 As Integer = 0
    Public FilePassrate As String ' ten file passrate
    Public CheckServer As Boolean = False
    Public PCBBOX As Integer = 10
    Public MacCurrent As String = ""
    Public IDCount As Integer = 0
    Public IDCount_box As Integer = 0
    Public MacLe As Boolean = False
    '///////////////////////////////////////////////////////////////////////
    Public Box_curent As String = ""
    '//////////////////////////////////////////////////////////////////////////////////
    ' cac bien lien quan den may chuc nang--------------------------------------------------------------
    Public PathModelCurrent As String = "" ' duong dan file cai dat model hien tai
    'Public PathLogMachine As String = "" ' duong dan ma log may chuc nang sinh ra khi can su dung 
    'Public PathLogWip As String = "" ' duong dan sinh file log cho WIP
    'Public NoLinePass As Integer = 3 ' dong co ki tu pass khi may chuc nang sinh log
    'Public KiHieuPass As String = "P" ' ki tu thong bao la pass trong log
    'Public SetProcess As String = "ICT_MUR" ' cong doan tren WIP
    'Public setfilenamereport As String ' cai dat ten lien quan den file name của file report may chuc nang
    Public CountLabel As Integer
    Public ModelRev As String = ""
    'Public ModelRev2 As String = ""
    Public ModelRevPosition As Integer = 0
    Public ModelCurrent As String = ""
    Public Idcode As String = ""
    'Public Idcode2 As String = ""
    Public IdCodeLenght As Int16 = 0
    'Public IdCodeLenght2 As Int16 = 0
    '//////////////////////////////////////////////////////////////////////
    Public confirmCode_emp As Boolean = False

    ' cac bien lien quan den com port
    Public ComControl As String = "COM7"
    Public SetBaudRateComControl As Integer = 9600
    Public SetDataBitsComControl As Integer = 8
    Public SetStopBitsComControl As Integer = 1
    Public SetParityComControl As String = "None"
    Public ComPress As String = "COM6"
    Public SetBaudRateComPress As Integer = 9600
    Public SetDataBitsComPress As Integer = 8
    Public SetStopBitsComPress As Integer = 1
    Public SetParityComPress As String = "None"
    Public ArraySend As String = "S00000000000000B"
    '///////////////////////////////////////////////////////////////////////////////////////////////

    Public Function CheckComControlPort() As Boolean
        Dim Line As String
        If File.Exists(PathSetupComport) = True Then
            Line = ReadTextFile(PathSetupComport, 2) ' cai dat cong com
            ComControl = Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1)
            Line = Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length)
            SetBaudRateComControl = Val(Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1))

            Line = Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length)
            SetDataBitsComControl = Val(Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1))

            Line = Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length)
            SetParityComControl = Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1)

            SetStopBitsComControl = Val(Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length))

            With Control.ComControlPort
                .PortName = ComControl
                .BaudRate = SetBaudRateComControl
                .DataBits = SetDataBitsComControl
                If SetParityComControl = "None" Then
                    .Parity = Parity.None
                ElseIf SetParityComControl = "Even" Then
                    .Parity = Parity.Even
                ElseIf SetParityComControl = "Odd" Then
                    .Parity = Parity.Odd
                ElseIf SetParityComControl = "Mark" Then
                    .Parity = Parity.Mark
                ElseIf SetParityComControl = "Space" Then
                    .Parity = Parity.Space
                End If
                If SetStopBitsComControl = 0 Then
                    .StopBits = StopBits.None
                ElseIf SetStopBitsComControl = 1 Then
                    .StopBits = StopBits.One
                ElseIf SetStopBitsComControl = 2 Then
                    .StopBits = StopBits.Two
                End If
                .Handshake = Handshake.None
                .ReceivedBytesThreshold = 1
            End With
            Try
                Control.ComControlPort.Open()
            Catch ex As Exception
                MessageBox.Show("COM Control: " & ComControl & " not connect. Please check connect the device !", "Error device", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
                Exit Function
            End Try
            Return True
        Else
            Return False
        End If
    End Function
    Public Function CheckComPressPort() As Boolean
        Dim Line As String
        If File.Exists(PathSetupComport) = True Then
            Line = ReadTextFile(PathSetupComport, 4) ' cai dat cong com
            ComPress = Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1)
            Line = Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length)
            SetBaudRateComPress = Val(Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1))

            Line = Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length)
            SetDataBitsComPress = Val(Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1))

            Line = Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length)
            SetParityComPress = Mid(Line, 1, InStr(Line, ",", CompareMethod.Text) - 1)

            SetStopBitsComPress = Val(Mid(Line, InStr(Line, ",", CompareMethod.Text) + 1, Line.Length))

            With Control.ComPressPort
                .PortName = ComPress
                .BaudRate = SetBaudRateComPress
                .DataBits = SetDataBitsComPress
                If SetParityComPress = "None" Then
                    .Parity = Parity.None
                ElseIf SetParityComPress = "Even" Then
                    .Parity = Parity.Even
                ElseIf SetParityComPress = "Odd" Then
                    .Parity = Parity.Odd
                ElseIf SetParityComPress = "Mark" Then
                    .Parity = Parity.Mark
                ElseIf SetParityComPress = "Space" Then
                    .Parity = Parity.Space
                End If
                If SetStopBitsComPress = 0 Then
                    .StopBits = StopBits.None
                ElseIf SetStopBitsComPress = 1 Then
                    .StopBits = StopBits.One
                ElseIf SetStopBitsComPress = 2 Then
                    .StopBits = StopBits.Two
                End If
                .Handshake = Handshake.None
                .ReceivedBytesThreshold = 1
            End With
            Try
                Control.ComPressPort.Open()
            Catch ex As Exception
                MessageBox.Show("COM Press: " & ComPress & " not connect. Please check connect the device !", "Error device", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
                Exit Function
            End Try
            Return True
        Else
            Return False
        End If
    End Function
    Public Function CheckPathSetup() As Boolean
        If File.Exists(PathSetupPath) = True Then
            IdLine = ReadTextFile(PathSetupPath, 2)
            NameLine = ReadTextFile(PathSetupPath, 4)
            Control.TextNameLine.Text = NameLine
            PathStatus = ReadTextFile(PathSetupPath, 6)
            If Directory.Exists(PathReport) = False Then Directory.CreateDirectory(PathReport)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function CheckModelList() As Boolean
        If File.Exists(PathModelList) = True Then
            For index = 1 To CounterlineTextFile(PathModelList)
                Control.ComboModel.Items.Add(ReadTextFile(PathModelList, index))
            Next
            Return True
        Else
            Return False
        End If
    End Function
    Public Function LoadModelCurrent(ByVal ModelLoad As String) As Boolean
        Dim Strcheck As String = ModelLoad
        PathModelCurrent = ""
        If Strcheck.Length <> 0 Then
            PathModelCurrent = PathSetup & "\" & ModelLoad & ".ini"
            If File.Exists(PathModelCurrent) = True Then
                NoPeople = Val(ReadTextFile(PathModelCurrent, 2))
                CycleTimeModel = Val(ReadTextFile(PathModelCurrent, 4))
                BarcodeEnable = Val(ReadTextFile(PathModelCurrent, 6))
                BalanceAlarmSetup = Val(ReadTextFile(PathModelCurrent, 8))
                BalanceErrorSetup = Val(ReadTextFile(PathModelCurrent, 10))
                IdCodeLenght = Val(ReadTextFile(PathModelCurrent, 12))
                ModelRevPosition = Val(ReadTextFile(PathModelCurrent, 14))
                ModelRev = ReadTextFile(PathModelCurrent, 16)
                PCBBOX = Val(ReadTextFile(PathModelCurrent, 18))

                Return True
                Exit Function
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Public Function CheckTotalLabel(ByVal Id As String) As Integer
        Dim Strcheck As String = Id
        Dim Linecheck As String = ""
        PathModelCurrent = ""
        If Strcheck.Length <> 0 Then
            Dim CountLine As Integer = CounterlineTextFile(PathModelList)
            While CountLine > 0
                Linecheck = ReadTextFile(PathModelList, CountLine)
                If Strcheck.Contains(Linecheck) = True Then
                    ModelCurrent = Linecheck
                    PathModelCurrent = PathSetup & "\" & Linecheck & ".ini"
                    Return CountLabel
                    Exit Function
                End If
                CountLine = CountLine - 1
            End While
            Return 0
        Else
            Return 0
        End If
    End Function
    Public Function CheckCaSX() As Boolean
        If Now.Hour >= 8 And Now.Hour <= 19 Then
            Shiftcheck = True
            Dim PathTimeLine As String = PathSetup & "\Setup Time Line Day.ini"
            Dim ReadTime As String = ReadTextFile(PathTimeLine, 2)
            TimeLine(1) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(2) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 4)
            TimeLine(3) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(4) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 6)
            TimeLine(5) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(6) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 8)
            TimeLine(7) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(8) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 10)
            TimeLine(9) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(10) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 12)
            TimeLine(11) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(12) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 14)
            TimeLine(13) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(14) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 16)
            TimeLine(15) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(16) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 18)
            TimeLine(17) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(18) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 20)
            TimeLine(19) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(20) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 22)
            TimeLine(21) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(22) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 24)
            TimeLine(23) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(24) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
        Else
            Shiftcheck = False
            Dim PathTimeLine As String = PathSetup & "\Setup Time Line Night.ini"
            Dim ReadTime As String = ReadTextFile(PathTimeLine, 2)
            TimeLine(1) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(2) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 4)
            TimeLine(3) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(4) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 6)
            TimeLine(5) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(6) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 8)
            TimeLine(7) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(8) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 10)
            TimeLine(9) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(10) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 12)
            TimeLine(11) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(12) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 14)
            TimeLine(13) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(14) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 16)
            TimeLine(15) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(16) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 18)
            TimeLine(17) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(18) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 20)
            TimeLine(19) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(20) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 22)
            TimeLine(21) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(22) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
            ReadTime = ReadTextFile(PathTimeLine, 24)
            TimeLine(23) = Convert.ToDateTime(Mid(ReadTime, 1, InStr(1, ReadTime, ",", CompareMethod.Text) - 1))
            TimeLine(24) = Convert.ToDateTime(Mid(ReadTime, InStr(1, ReadTime, ",", CompareMethod.Text) + 1, ReadTime.Length))
        End If
        Return Shiftcheck
    End Function

    Public Function CalTimeWork(t1 As DateTime, t2 As DateTime) As Double
        Dim kq As Double
        Dim span = t2.Subtract(t1)
        kq = span.Hours * 3600 + span.Minutes * 60 + span.Seconds
        Return kq
    End Function

End Module

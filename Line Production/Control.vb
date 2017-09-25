Imports System
Imports System.Drawing.Text
Imports System.IO
Imports System.Math
Imports System.Threading
Public Class Control
    Dim time_scanBarcode As DateTime
    Dim bien_dem As Integer = 0
    '----------------------
    Dim comp_percent1 As Double
    Dim comp_percent2 As Double
    Dim comp_percent3 As Double
    Dim comp_percent4 As Double
    Dim comp_percent5 As Double
    Dim comp_percent6 As Double
    Dim comp_percent7 As Double
    Dim comp_percent8 As Double
    Dim comp_percent9 As Double
    Dim comp_percent10 As Double
    Dim comp_percent11 As Double
    Dim comp_percent12 As Double
    '--------------------------
    Dim signal As String
    Dim listId As List(Of String)
    Public Sub FormatNgayCasx()
        CheckCaSX()
        If Shiftcheck = True Then
            Datecheck = Now.Date.ToString("dd-MM-yyyy")
        Else
            If Now.Hour >= 0 And Now.Hour <= 7 Then
                If Now.Day > 1 Then
                    Datecheck = (Now.Day - 1).ToString.PadLeft(2, "0"c) & "-" & Now.Month.ToString().PadLeft(2, "0"c) & "-" & Now.Year

                Else
                    Select Case Now.Month - 1
                        Case 1
                            Datecheck = Now.Year - 1 & "1231"

                        Case 3, 5, 7, 8, 10, 12
                            Datecheck = "31-" & (Now.Month - 1).ToString().PadLeft(2, "0"c) & "-" & Now.Year

                        Case 4, 6, 9, 11

                            Datecheck = "30-" & (Now.Month - 1).ToString().PadLeft(2, "0"c) & "-" & Now.Year
                        Case 2

                            If (((Now.Year Mod 4 = 0) And (Now.Year Mod 100 <> 0) Or (Now.Year Mod 400 = 0))) Then
                                Datecheck = "29-02" & Now.Year
                            Else
                                Datecheck = "28-02" & Now.Year
                            End If
                    End Select
                End If
            Else
                Datecheck = Now.Date.ToString("dd-MM-yyyy")
            End If
        End If
        TextDateProduct.Text = Datecheck
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadsetting()
        'Me.Height = 885
        SelectDefaultModel()
        BtStart_Click(sender, e)
        lblLastopen.Text = Date.Now.ToString("dd-MM-yyyy HH:mm:ss")
    End Sub
    Public Sub SetupDisplay()
        If ComControlPort.IsOpen = True Then
            If ComControlPort.IsOpen = True Then ComControlPort.WriteLine("S0000000000000R")
            If ComControlPort.IsOpen = True Then ComControlPort.Write("C")
        End If
        ComboModel.Enabled = True
        ComboModel.Text = ""
        For index = 1 To 12
            Table1.Controls("TextTime" & index).Text = ""
            Table1.Controls("TextPlan" & index).Text = ""
            Table1.Controls("TextActual" & index).Text = ""
            Table1.Controls("TextBalance" & index).Text = ""
            '    Table1.Controls("TextComment" & index).Text = ""
            '    notePerHour(index) = ""
        Next
        Copyright.Text = My.Application.Info.Copyright.ToString
        Me.Text = My.Application.Info.Title.ToString & " " & My.Application.Info.Version.ToString
        'quyetpv
        lblAuthor.Text = My.Application.Info.Description
        TextCycleTimeCurrent.Text = 0
        TextCycleTimeModel.Text = 0
        TextShiftProduct.Clear()
        TextDateProduct.Clear()
        PauseProduct = False
        StartProduct = False
        StatusLine = 0
        CountProduct = 0
        ProductPlan = 0
        TimeCycleActual = 0
        IDCount = 0
        IDCount_box = 0
        MacCurrent = ""
        For index = 1 To 9
            CountProductPerHour(index) = 0
        Next
        NoPeople = 0
        ModelCurrent = ""
        Shape2.Visible = False
        Shape1.Visible = False
        Shape3.Visible = False
        LabelShapeOnline.Visible = False
        LabelShapeOffLine.Visible = False
        LabelShapeError.Visible = False
        Timer1.Enabled = True
        'quyetpv
        Timer3.Enabled = True
        'TextCycleTimeCurrent.Text = 0
        TextPerson.Text = 0
        TextPlan.Text = 0
        TextActual.Text = 0
        TextBalance.Text = 0
        CountProduct = 0
        LabelCountProduct.Text = CountProduct
        TextShiftProduct.Text = ""
        BtStart.Enabled = False
        BtStop.Enabled = False
        BtStart.Text = "Bắt đầu"
        BtStart.Image = Image.FromFile(PathApplication & "\Icon\play.png")
        ' If CheckServer = True Then RecordDatabase()
        'Dim pcf As New PrivateFontCollection()
        'pcf.AddFontFile(Directory.GetCurrentDirectory & "\Resource\DS-DIGI.TTF")
        'LabelTimeDate.Font = New Font(pcf.Families(0), 50, FontStyle.Bold)
        ComboModel.SelectedIndex = -1
        ShowPercentage()
    End Sub
    Public Sub SelectDefaultModel()
        ComboModel.SelectedIndex = 0
    End Sub
    Private Sub loadsetting()
        ' CheckServer = CheckConnectServer()
        ' If CheckServer = False Then
        'MessageBox.Show("Ket noi server that bai", "Loi he thong", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End If
        If CheckComPressPort() = True Then
            If CheckComControlPort() = True Then
                If CheckModelList() = True Then
                    If CheckPathSetup() = True Then
                        SetupDisplay()
                    Else
                        MessageBox.Show("File Setup Path WIP Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        ' End
                    End If
                Else
                    MessageBox.Show("File Setup List Model Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End
                End If
            End If
        End If
    End Sub

    Public Sub LoadProduction()

        Dim fileload As String = PathPassrate & "\" & ModelCurrent & "\" & Datecheck & "_" & Shiftcheck & ".txt"
        Dim FolderLoad As String = PathPassrate & "\" & ModelCurrent

        If Directory.Exists(FolderLoad) = False Then Directory.CreateDirectory(FolderLoad)
        If System.IO.File.Exists(fileload) = True Then
            ProductPlanBegin = Val(ReadTextFile(fileload, 2))
            ProductPlan = Val(ReadTextFile(fileload, 2))
            CountProduct = Val(ReadTextFile(fileload, 4))
            'IDCount = Val(ReadTextFile(fileload, 26))
            'IDCount_box = Val(ReadTextFile(fileload, 28))
            'Box_curent = (ReadTextFile(fileload, 30))
            TimeCycleActual = Val(ReadTextFile(fileload, 30))
            For index = 1 To 12
                CountProductPerHour(index) = Val(ReadTextFile(fileload, 2 * (index + 2)))
                'notePerHour(index) = ReadTextFile(fileload, 32 + index)
            Next
        Else
            FilePassrate = fileload
            For index = 1 To 12
                CountProductPerHour(index) = 0
                'notePerHour(index) = ""
            Next
            Dim text_passrate As New System.IO.StreamWriter(FilePassrate)
            Try
                text_passrate.WriteLine("# 1 Plan")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("#2 Actual")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 3 Production In Time 1 8>9")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 4 Production In Time 2 9>10")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 5 Production In Time 3	10>11")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 6 Production In Time 4	11>12")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 7 Production In Time 5	12>13")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 8 Production In Time 6	13>14")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 9 Production In Time 7	14>15")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 10 Production In Time 8	15>16")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 11 Production In Time 9	16>17")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 12 Production In Time 10	17>18")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 13 Production In Time 10	18>19")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 14 Production In Time 10	19>20")
                text_passrate.WriteLine(0)
                text_passrate.WriteLine("# 15 CycleTime hien tai")
                text_passrate.WriteLine(TimeCycleActual)
                'For i = 1 To 10
                '    text_passrate.WriteLine("")
                'Next

            Catch ex As Exception
            End Try
            text_passrate.Close()
        End If

        Dim FileReport As String = PathReport & "\" & ModelCurrent & "\" & Datecheck & ".csv"
        If Directory.Exists(PathReport & "\" & ModelCurrent) = False Then Directory.CreateDirectory(PathReport & "\" & ModelCurrent)

        If File.Exists(FileReport) = False Then
            Dim file As StreamWriter = New System.IO.StreamWriter(FileReport, True) ' append enable
            file.WriteLine("No,NoPCBA,MAC Box,Id PCBA")
            file.Close()
        End If
        ' RecordDatabase()
    End Sub


    Public Sub RecordProduction()
        If ModelCurrent <> "" Then
            Dim fileload As String = PathPassrate & "\" & ModelCurrent & "\" & Datecheck & "_" & Shiftcheck & ".txt"
            If System.IO.File.Exists(fileload) = True Then
                ReplaceLine(fileload, 2, ProductPlan)
                ReplaceLine(fileload, 4, CountProduct)
                'ReplaceLine(fileload, 26, IDCount)
                'ReplaceLine(fileload, 28, IDCount_box)
                'ReplaceLine(fileload, 30, Box_curent)
                ReplaceLine(fileload, 30, TimeCycleActual)
                For index = 1 To 12
                    ReplaceLine(fileload, 2 * (index + 2), CountProductPerHour(index))
                    'ReplaceLine(fileload, 32 + index, Table1.Controls("TextComment" & index).Text)
                Next
            Else
                FilePassrate = fileload
                Dim text_passrate As New System.IO.StreamWriter(FilePassrate)
                Try
                    text_passrate.WriteLine("# 1 Plan")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("#2 Actual")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 3 Production In Time 1 8>9")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 4 Production In Time 2 9>10")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 5 Production In Time 3	10>11")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 6 Production In Time 4	11>12")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 7 Production In Time 5	12>13")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 8 Production In Time 6	13>14")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 9 Production In Time 7	14>15")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 10 Production In Time 8	15>16")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 11 Production In Time 9	16>17")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 12 Production In Time 10	17>18")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 13 Production In Time 10	18>19")
                    text_passrate.WriteLine(0)
                    text_passrate.WriteLine("# 14 Production In Time 10	19>20")
                    text_passrate.WriteLine(0)
                    'text_passrate.WriteLine("# 15 ma thung hien tai")
                    'text_passrate.WriteLine(Box_curent)
                    text_passrate.WriteLine("# 15 CycleTime hien tai")
                    text_passrate.WriteLine(TimeCycleActual)
                    'For i = 1 To 10
                    '    text_passrate.WriteLine("")
                    'Next
                    text_passrate.Close()
                Catch ex As Exception
                    'MessageBox.Show("Error", "Sự cố lưu file", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

            End If
        End If
    End Sub

    Private Sub ComboModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboModel.SelectedIndexChanged
        If ComboModel.Text <> "" Then
            ModelCurrent = ComboModel.Text
            If LoadModelCurrent(ModelCurrent) = True Then
                ComboModel.Enabled = False
                TextCycleTimeModel.Text = CycleTimeModel
                TextCycleTimeCurrent.Text = ""
                TextPerson.Text = NoPeople
                FormatNgayCasx()
                BtStart.Enabled = True
                BtStop.Enabled = True
                LoadProduction()
                TextPlan.Text = Math.Round(TimeCycleActual / CycleTimeModel, 0, MidpointRounding.AwayFromZero) 'ProductPlanBegin.ToString()
                TextActual.Text = CountProduct.ToString()
                TextBalance.Text = Math.Abs(CountProduct - ProductPlanBegin).ToString()
                LabelCountProduct.Text = CountProduct
                TextCycleTimeCurrent.Text = Math.Round(TimeCycleActual / CountProduct, 1, MidpointRounding.AwayFromZero)
                If CheckCaSX() = True Then
                    TextShiftProduct.Text = "Ca Ngày"
                    For index = 1 To 12
                        'Table1.Controls("TextComment" & index).Text = notePerHour(index)
                        Table1.Controls("TextTime" & index).Text = TimeLine(1 + 2 * (index - 1)).Hour & ":" & TimeLine(1 + 2 * (index - 1)).Minute & ":" & TimeLine(1 + 2 * (index - 1)).Second & " - " & TimeLine(2 + 2 * (index - 1)).Hour & ":" & TimeLine(2 + 2 * (index - 1)).Minute & ":" & TimeLine(2 + 2 * (index - 1)).Second
                        Table1.Controls("TextPlan" & index).Text = Format(((Val(TimeLine(2 + 2 * (index - 1)).Hour - TimeLine(1 + 2 * (index - 1)).Hour)) * 3600 + (Val(TimeLine(2 + 2 * (index - 1)).Minute - TimeLine(1 + 2 * (index - 1)).Minute) * 60) + (Val(TimeLine(2 + 2 * (index - 1)).Second - TimeLine(1 + 2 * (index - 1)).Second))) / CycleTimeModel, "0")
                        If CountProductPerHour(index) > 0 Then
                            Table1.Controls("TextActual" & index).Text = CountProductPerHour(index)
                            Table1.Controls("TextBalance" & index).Text = Val(Table1.Controls("TextActual" & index).Text) - Val(Table1.Controls("TextPlan" & index).Text)
                        End If
                    Next
                Else
                    TextShiftProduct.Text = "Ca Đêm"
                    For index = 1 To 12
                        'Table1.Controls("TextComment" & index).Text = notePerHour(index)
                        Table1.Controls("TextTime" & index).Text = TimeLine(1 + 2 * (index - 1)).Hour & ":" & TimeLine(1 + 2 * (index - 1)).Minute & ":" & TimeLine(1 + 2 * (index - 1)).Second & " - " & TimeLine(2 + 2 * (index - 1)).Hour & ":" & TimeLine(2 + 2 * (index - 1)).Minute & ":" & TimeLine(2 + 2 * (index - 1)).Second
                        Table1.Controls("TextPlan" & index).Text = Format(((Val(TimeLine(2 + 2 * (index - 1)).Hour - TimeLine(1 + 2 * (index - 1)).Hour)) * 3600 + (Val(TimeLine(2 + 2 * (index - 1)).Minute - TimeLine(1 + 2 * (index - 1)).Minute) * 60) + (Val(TimeLine(2 + 2 * (index - 1)).Second - TimeLine(1 + 2 * (index - 1)).Second))) / CycleTimeModel, "0")
                        If CountProductPerHour(index) > 0 Then
                            Table1.Controls("TextActual" & index).Text = CountProductPerHour(index)
                            Table1.Controls("TextBalance" & index).Text = Format(((Val(TimeLine(2 + 2 * (index - 1)).Hour - TimeLine(1 + 2 * (index - 1)).Hour)) * 3600 + (Val(TimeLine(2 + 2 * (index - 1)).Minute - TimeLine(1 + 2 * (index - 1)).Minute) * 60) + (Val(TimeLine(2 + 2 * (index - 1)).Second - TimeLine(1 + 2 * (index - 1)).Second))) / CycleTimeModel, "0") - CountProductPerHour(index)
                        End If
                    Next
                End If
                ShowPercentage()
            End If
        End If

    End Sub
    Public Sub ShowStatus(ByVal value As Integer, ByVal VisibleShow As Boolean)

        Select Case value
            Case 0

                'LabelStatus.Text = "Tình trạng Line:       "
                LabelShapeOnline.Visible = False
                LabelShapeOffLine.Visible = False
                LabelShapeError.Visible = False
                Shape1.Visible = False
                Shape2.Visible = False
                Shape3.Visible = False

            Case 1
                'LabelStatus.Text = "Tình trạng Line:Online"
                LabelShapeOnline.Visible = VisibleShow
                LabelShapeOffLine.Visible = False
                LabelShapeError.Visible = False
                Shape1.Visible = VisibleShow
                If VisibleShow = False Then
                    If ComControlPort.IsOpen = True Then ComControlPort.Write("R")
                Else
                    If ComControlPort.IsOpen = True Then ComControlPort.Write("X")
                End If

                Shape2.Visible = False
                Shape3.Visible = False
            Case 2
                'LabelStatus.Text = "Tình trạng Line: Bao Dong"
                LabelShapeOnline.Visible = False
                LabelShapeOffLine.Visible = VisibleShow
                LabelShapeError.Visible = False
                Shape1.Visible = False
                Shape2.Visible = VisibleShow
                If VisibleShow = False Then
                    If ComControlPort.IsOpen = True Then ComControlPort.Write("R")
                Else
                    If ComControlPort.IsOpen = True Then ComControlPort.Write("V")
                End If
                Shape3.Visible = False
            Case 3
                'LabelStatus.Text = "Tình trạng Line: Bat Thuong"
                LabelShapeOnline.Visible = False
                LabelShapeOffLine.Visible = False
                LabelShapeError.Visible = VisibleShow
                Shape1.Visible = False
                Shape2.Visible = False
                Shape3.Visible = VisibleShow
                If VisibleShow = False Then
                    If ComControlPort.IsOpen = True Then ComControlPort.Write("R")
                Else
                    If ComControlPort.IsOpen = True Then ComControlPort.Write("D")
                End If
            Case Else

        End Select
    End Sub
    Private Sub BtStart_Click(sender As Object, e As EventArgs) Handles BtStart.Click
        ComboModel.Enabled = False
        LabelShapeOnline.Visible = True
        LabelShapeOffLine.Visible = False
        LabelShapeError.Visible = False
        Shape1.Visible = False
        Shape2.Visible = False
        Shape3.Visible = False
        TimerPress.Enabled = True
        If BtStart.Text = "Bắt đầu" Then
            PauseProduct = False
            StartProduct = True

            ' GroupBox3.Controls("Shape" & StatusLine).Visible = True
            BtStart.Text = "Online"
            BtStart.Image = Image.FromFile(PathApplication & "\Icon\pause.png")
            Dim sumtime As Integer = ((Now.Hour * 100) + Now.Minute)
            If StatusLine = 0 Then
                StatusLine = 1
                'LabelStatus.Text = "Tình trạng Line: Online"
                ShowStatus(StatusLine, True)
                For index = 1 To 24
                    If index Mod 2 <> 0 And sumtime >= ((TimeLine(index).Hour) * 100 + TimeLine(index).Minute) And sumtime <= ((TimeLine(index + 1).Hour) * 100 + TimeLine(index + 1).Minute) Then
                        time_scanBarcode = Now
                        TimeLine(index) = New DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second)
                        Table1.Controls("TextTime" & (index \ 2) + 1).Text = TimeLine(index).Hour & ":" & TimeLine(index).Minute & ":" & TimeLine(index).Second & " - " & TimeLine(index + 1).Hour & ":" & TimeLine(index + 1).Minute & ":" & TimeLine(index + 1).Second
                        Table1.Controls("TextPlan" & (index \ 2) + 1).Text = Format((((Val(TimeLine(index + 1).Hour - TimeLine(index).Hour)) * 3600 + (Val(TimeLine(index + 1).Minute - TimeLine(index).Minute) * 60) + (Val(TimeLine(index + 1).Second - TimeLine(index).Second))) / CycleTimeModel) + CountProductPerHour((index \ 2) + 1), "0")
                        'TextPlan.Text = Table1.Controls("TextPlan" & (index \ 2) + 1).Text
                        'TextPlan.Text = ProductPlanBegin.ToString()
                        TextActual.Text = CountProduct
                        Exit For
                    End If
                Next


            Else
                'LabelStatus.Text = "Tình trạng Line: Online"
                PauseProduct = False
                StartProduct = True
                ShowStatus(StatusLine, True)
                For index = 1 To 24
                    If index Mod 2 <> 0 And sumtime >= ((TimeLine(index).Hour) * 100 + TimeLine(index).Minute) And sumtime <= ((TimeLine(index + 1).Hour) * 100 + TimeLine(index + 1).Minute) Then
                        Table1.Controls("TextTime" & (index \ 2) + 1).Text = TimeLine(index).Hour & ":" & TimeLine(index).Minute & ":" & TimeLine(index).Second & ">" & TimeLine(index + 1).Hour & ":" & TimeLine(index + 1).Minute & ":" & TimeLine(index + 1).Second
                        Table1.Controls("TextPlan" & (index \ 2) + 1).Text = Format((((Val(TimeLine(index + 1).Hour - TimeLine(index).Hour)) * 3600 + (Val(TimeLine(index + 1).Minute - TimeLine(index).Minute) * 60) + (Val(TimeLine(index + 1).Second - TimeLine(index).Second))) / CycleTimeModel) + CountProductPerHour((index \ 2) + 1), "0")
                        'TextPlan.Text = ProductPlanBegin.ToString()
                        Exit For
                    End If

                Next

                TimePauseLine = 0
            End If
            time_scanBarcode = Now
            ProductPlan = Math.Round(TimeCycleActual / CycleTimeModel, 0, MidpointRounding.AwayFromZero)
            TextPlan.Text = ProductPlan
        End If
    End Sub

    Private Sub BtStop_Click(sender As Object, e As EventArgs) Handles BtStop.Click
        SetupDisplay()
        TimerPress.Enabled = False
    End Sub

    Private Sub TimerPress_Tick(sender As Object, e As EventArgs) Handles TimerPress.Tick
        If StartProduct = True Then
            If ComPressPort.IsOpen = True Then
                'ComPressPort.Write("B")
                'If ComPressPort.ReadExisting() = "B" Then
                '    BitPress = True
                'ElseIf ComPressPort.ReadExisting() <> "B" And BitPress = True Then
                '    BitPress = False
                '    IncreaseProduct()
                'End If
                'Nếu nhận được tín hiệu thì tăng sản lượng
                'Quyetpham
                'TextBox1.Text = ComPressPort.ReadExisting()

                signal = ComPressPort.ReadExisting()
                'signal = TextBox13.Text.Trim
                listId = GetListnumber(signal)
                For Each item In listId
                    If item = IdLine Then
                        IncreaseProduct()
                        ShowPercentage()
                    End If
                Next
                'signal = "L" & IdLine
                'If ComPressPort.ReadExisting() = signal Then
                '    IncreaseProduct()
                '    ShowPercentage()
                'End If
            End If


        End If
    End Sub
    Public Sub IncreaseProduct()
        If StartProduct = True Then
            'For i = 1 To 10
            '    notePerHour(i) = Table1.Controls("TextComment" & i).Text
            'Next
            Dim sumtime As Integer = ((Now.Hour * 100) + Now.Minute)
            For index = 1 To 24
                If index Mod 2 <> 0 And sumtime >= ((TimeLine(index).Hour) * 100 + TimeLine(index).Minute) And sumtime <= ((TimeLine(index + 1).Hour) * 100 + TimeLine(index + 1).Minute) Then
                    'CountProduct = CountProduct + 1
                    bien_dem = bien_dem + 1
                    'If CountProduct = 1 Then
                    '    CycleTimeActual = CalTimeWork(time_scanBarcode, Now)
                    'Else
                    '    TimeCycleActual = TimeCycleActual + CalTimeWork(time_scanBarcode, Now)
                    '    CycleTimeActual = Math.Round(TimeCycleActual / CountProduct, 1, MidpointRounding.AwayFromZero)
                    'End If
                    'time_scanBarcode = Now
                    'TextCycleTimeCurrent.Text = CycleTimeActual
                    'LabelCountProduct.Text = CountProduct
                    'ProductPlan = Math.Round(TimeCycleActual / CycleTimeModel, 0, MidpointRounding.AwayFromZero)
                    'TextPlan.Text = ProductPlan
                    CountProductPerHour((index \ 2) + 1) = CountProductPerHour((index \ 2) + 1) + 1
                    Table1.Controls("TextActual" & (index \ 2) + 1).Text = CountProductPerHour((index \ 2) + 1)
                    Table1.Controls("TextBalance" & (index \ 2) + 1).Text = CountProductPerHour((index \ 2) + 1) - Val(Table1.Controls("TextPlan" & (index \ 2) + 1).Text)
                    'Table1.Controls("TextTime" & (index \ 2) + 1).BackColor = Color.Red
                    Exit For
                Else
                    bien_dem = 0
                    'Table1.Controls("TextTime" & (index \ 2) + 1).BackColor = Color.White
                End If
            Next
            If bien_dem <> 0 Then
                CountProduct = CountProduct + 1
                If CountProduct = 1 Then
                    CycleTimeActual = (Now - time_scanBarcode).TotalSeconds 'CalTimeWork(time_scanBarcode, Now) 
                Else
                    TimeCycleActual = TimeCycleActual + (Now - time_scanBarcode).TotalSeconds 'CalTimeWork(time_scanBarcode, Now)
                    CycleTimeActual = Math.Round(TimeCycleActual / CountProduct, 1, MidpointRounding.AwayFromZero)
                End If
                time_scanBarcode = Now
                TextCycleTimeCurrent.Text = CycleTimeActual
                LabelCountProduct.Text = CountProduct
                ProductPlan = Math.Round(TimeCycleActual / CycleTimeModel, 0, MidpointRounding.AwayFromZero)
                TextPlan.Text = ProductPlan
                time_scanBarcode = Now
            End If
            RecordProduction()
        End If
    End Sub
    Public Sub ReduceProduct()
        If StartProduct = True And CountProduct > 0 Then
            Dim sumtime As Integer = ((Now.Hour * 100) + Now.Minute)
            For index = 1 To 24
                If index Mod 2 <> 0 And sumtime >= ((TimeLine(index).Hour) * 100 + TimeLine(index).Minute) And sumtime <= ((TimeLine(index + 1).Hour) * 100 + TimeLine(index + 1).Minute) Then
                    CountProduct = CountProduct - 1
                    LabelCountProduct.Text = CountProduct
                    CountProductPerHour((index \ 2) + 1) = CountProductPerHour((index \ 2) + 1) - 1
                    Table1.Controls("TextActual" & (index \ 2) + 1).Text = CountProductPerHour((index \ 2) + 1)
                    Table1.Controls("TextBalance" & (index \ 2) + 1).Text = CountProductPerHour((index \ 2) + 1) - Val(Table1.Controls("TextPlan" & (index \ 2) + 1).Text)
                    Exit For
                End If
            Next
            RecordProduction()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick


        If BtStart.Text <> "Bắt đầu" Then
            Dim sumtime As Integer = ((Now.Hour * 100) + Now.Minute)
            For index = 1 To 24
                If index Mod 2 <> 0 Then
                    If sumtime >= ((TimeLine(index).Hour) * 100 + TimeLine(index).Minute) And sumtime <= ((TimeLine(index + 1).Hour) * 100 + TimeLine(index + 1).Minute) Then
                        'ProductPlan = Int(((Val(Now.Hour - TimeLine(index).Hour)) * 3600 + (Val(Now.Minute - TimeLine(index).Minute)) * 60 + (Val(Now.Second - TimeLine(index).Second))) / CycleTimeModel) + ProductPlanBegin
                        bien_dem = bien_dem + 1
                        'Try
                        '    RecordProduction()
                        'Catch ex As Exception

                        'End Try


                        Exit For
                    Else
                        bien_dem = 0
                    End If
                Else

                End If
            Next
            If bien_dem <> 0 Then
                '    Try
                '        RecordProduction()
                '    Catch ex As Exception

                '    End Try

                PauseProduct = False
            Else
                time_scanBarcode = Now
                PauseProduct = True
            End If
            ProductPlan = Math.Round(TimeCycleActual / CycleTimeModel, 0, MidpointRounding.AwayFromZero)
            'If BarcodeEnable = True Then TextSerial.Focus()
            BalanceProduction = CountProduct - ProductPlan
            Dim perBalanceProduction As Integer = IIf(ProductPlan = 0, 0, (CountProduct - ProductPlan) / ProductPlan * 100)
            If (perBalanceProduction) < BalanceErrorSetup Then
                StatusLine = 3
                ShowStatus(StatusLine, True)
            ElseIf (perBalanceProduction) < BalanceAlarmSetup Then
                StatusLine = 2
                ShowStatus(StatusLine, True)
            Else
                StatusLine = 1
                ShowStatus(StatusLine, True)
            End If
            If PauseProduct = True Then
                TimePauseLine = TimePauseLine + 1
                If TimePauseLine Mod 2 = 0 Then
                    ShowStatus(StatusLine, True)
                    TimePauseLine = 0
                Else
                    ShowStatus(StatusLine, False)
                End If
            End If
            TextPlan.Text = ProductPlan
            TextActual.Text = CountProduct
            TextBalance.Text = BalanceProduction
            'MsgBox(Format(BalanceProduction, "0000"))
            If BalanceProduction < 0 Then
                If Math.Abs(BalanceProduction) >= 1000 Then
                    ArraySend = "S-" & Format(999, "000") & Format(CountProduct, "0000") & Format(ProductPlan, "0000") & Format(NoPeople, "00") & "*"
                Else
                    ArraySend = "S" & Format(BalanceProduction, "000") & Format(CountProduct, "0000") & Format(ProductPlan, "0000") & Format(NoPeople, "00") & "*"
                End If

            Else
                ArraySend = "S+" & Format(BalanceProduction, "000") & Format(CountProduct, "0000") & Format(ProductPlan, "0000") & Format(NoPeople, "00") & "*"
            End If

            If ComControlPort.IsOpen = True Then ComControlPort.WriteLine(ArraySend)
            ' RecordDatabase()
        End If
    End Sub

    Private Sub BtIncrease_Click(sender As Object, e As EventArgs) Handles BtIncrease.Click
        IncreaseProduct()
        ShowPercentage()
    End Sub

    Private Sub BtReduce_Click(sender As Object, e As EventArgs) Handles BtReduce.Click
        ReduceProduct()
        ShowPercentage()
    End Sub

    Private Sub Control_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        RecordProduction()
        'SetupDisplay()
    End Sub
    'Public Function CheckIDExist(ByVal Idcheck As String) As Boolean
    '    Dim FileReport As String = PathReport & "\" & ModelCurrent & "\" & Datecheck & ".csv"
    '    If Directory.Exists(PathReport & "\" & ModelCurrent) = False Then Directory.CreateDirectory(PathReport & "\" & ModelCurrent)
    '    Dim astring As String = ReadAllLine(FileReport)
    '    If astring <> "" Then
    '        If astring.Contains(Idcheck) = False Then
    '            Return False
    '        Else
    '            Return True
    '        End If
    '    Else
    '        Return True
    '    End If
    'End Function

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If BtStart.Text <> "Bắt đầu" Then
            Dim sumtime As Integer = ((Now.Hour * 100) + Now.Minute)
            For index = 1 To 24
                If index Mod 2 <> 0 Then
                    If sumtime >= ((TimeLine(index).Hour) * 100 + TimeLine(index).Minute) And sumtime <= ((TimeLine(index + 1).Hour) * 100 + TimeLine(index + 1).Minute) Then
                        bien_dem = bien_dem + 1
                        Exit For
                    Else
                        bien_dem = 0
                    End If
                Else

                End If
            Next
            If bien_dem = 0 Then
                time_scanBarcode = Now
            Else
                TimeCycleActual = TimeCycleActual + (Now - time_scanBarcode).TotalSeconds
                time_scanBarcode = Now
                ProductPlan = Math.Round(TimeCycleActual / CycleTimeModel, 0, MidpointRounding.AwayFromZero)
                TextPlan.Text = ProductPlan
            End If
        End If
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick

        LabelTimeDate.Text = Now.ToString("HH:mm:ss  dd/MM/yyyy")
        If (DateTime.Now.Hour = 20 And DateTime.Now.Minute = 0 And DateTime.Now.Second = 0) Or (DateTime.Now.Hour = 8 And DateTime.Now.Minute = 0 And DateTime.Now.Second = 0) Then
            BtStop_Click(sender, e)
            Refresh()
            SelectDefaultModel()
            BtStart_Click(sender, e)
        End If
        'If Date.Now.Second = 0 Then
        '    BtStop_Click(sender, e)
        '    Refresh()
        '    SelectDefaultModel()
        '    BtStart_Click(sender, e)
        'End If
    End Sub

    Private Sub Control_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim result = MessageBox.Show("Không tự ý tắt chương trình. Liên hệ Quyết - LCA", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
    Private Sub ShowPercentage()
        'Row 1
        If TextActual1.Text <> "" Then
            If Val(TextActual1.Text) < Val(TextPlan1.Text) Then
                comp_percent1 = Val(TextActual1.Text) / Val(TextPlan1.Text) * 100
                ProgressBar1.Value = Val(comp_percent1)
            Else
                comp_percent1 = 100 + ((Val(TextActual1.Text) - Val(TextPlan1.Text)) / Val(TextPlan1.Text)) * 100
                ProgressBar1.Value = 100
            End If
            TextBox1.Text = Round(comp_percent1, 1) & "%"
        Else
            ProgressBar1.Value = 0
            TextBox1.Text = ""
        End If

        'Row 2
        If TextActual2.Text <> "" Then
            If Val(TextActual2.Text) < Val(TextPlan2.Text) Then
                comp_percent2 = Val(TextActual2.Text) / Val(TextPlan2.Text) * 100
                ProgressBar2.Value = Val(comp_percent2)
            Else
                comp_percent2 = 100 + ((Val(TextActual2.Text) - Val(TextPlan2.Text)) / Val(TextPlan2.Text)) * 100
                ProgressBar2.Value = 100
            End If
            TextBox2.Text = Round(comp_percent2, 1) & "%"
        Else
            ProgressBar2.Value = 0
            TextBox2.Text = ""
        End If
        'Row 3
        If TextActual3.Text <> "" Then
            If Val(TextActual3.Text) < Val(TextPlan3.Text) Then
                comp_percent3 = Val(TextActual3.Text) / Val(TextPlan3.Text) * 100
                ProgressBar3.Value = Val(comp_percent3)
            Else
                comp_percent3 = 100 + ((Val(TextActual3.Text) - Val(TextPlan3.Text)) / Val(TextPlan3.Text)) * 100
                ProgressBar3.Value = 100
            End If
            TextBox3.Text = Round(comp_percent3, 1) & "%"
        Else
            ProgressBar3.Value = 0
            TextBox3.Text = ""
        End If
        'Row 4
        If TextActual4.Text <> "" Then
            If Val(TextActual4.Text) < Val(TextPlan4.Text) Then
                comp_percent4 = Val(TextActual4.Text) / Val(TextPlan4.Text) * 100
                ProgressBar4.Value = Val(comp_percent4)
            Else
                comp_percent4 = 100 + ((Val(TextActual4.Text) - Val(TextPlan4.Text)) / Val(TextPlan4.Text)) * 100
                ProgressBar4.Value = 100
            End If
            TextBox4.Text = Round(comp_percent4, 1) & "%"
        Else
            ProgressBar4.Value = 0
            TextBox4.Text = ""
        End If
        'Row 5
        If TextActual5.Text <> "" Then
            If Val(TextActual5.Text) < Val(TextPlan5.Text) Then
                comp_percent5 = Val(TextActual5.Text) / Val(TextPlan5.Text) * 100
                ProgressBar5.Value = Val(comp_percent5)
            Else
                comp_percent5 = 100 + ((Val(TextActual5.Text) - Val(TextPlan5.Text)) / Val(TextPlan5.Text)) * 100
                ProgressBar5.Value = 100
            End If
            TextBox5.Text = Round(comp_percent5, 1) & "%"
        Else
            ProgressBar5.Value = 0
            TextBox5.Text = ""
        End If
        'Row 6
        If TextActual6.Text <> "" Then
            If Val(TextActual6.Text) < Val(TextPlan6.Text) Then
                comp_percent6 = Val(TextActual6.Text) / Val(TextPlan6.Text) * 100
                ProgressBar6.Value = Val(comp_percent6)
            Else
                comp_percent6 = 100 + ((Val(TextActual6.Text) - Val(TextPlan6.Text)) / Val(TextPlan6.Text)) * 100
                ProgressBar6.Value = 100
            End If
            TextBox6.Text = Round(comp_percent6, 1) & "%"
        Else
            ProgressBar6.Value = 0
            TextBox6.Text = ""
        End If
        'Row 7
        If TextActual7.Text <> "" Then
            If Val(TextActual7.Text) < Val(TextPlan7.Text) Then
                comp_percent7 = Val(TextActual7.Text) / Val(TextPlan7.Text) * 100
                ProgressBar7.Value = Val(comp_percent7)
            Else
                comp_percent7 = 100 + ((Val(TextActual7.Text) - Val(TextPlan7.Text)) / Val(TextPlan7.Text)) * 100
                ProgressBar7.Value = 100
            End If
            TextBox7.Text = Round(comp_percent7, 1) & "%"
        Else
            ProgressBar7.Value = 0
            TextBox7.Text = ""
        End If
        'Row 8
        If TextActual8.Text <> "" Then
            If Val(TextActual8.Text) < Val(TextPlan8.Text) Then
                comp_percent8 = Val(TextActual8.Text) / Val(TextPlan8.Text) * 100
                ProgressBar8.Value = Val(comp_percent8)
            Else
                comp_percent8 = 100 + ((Val(TextActual8.Text) - Val(TextPlan8.Text)) / Val(TextPlan8.Text)) * 100
                ProgressBar8.Value = 100
            End If
            TextBox8.Text = Round(comp_percent8, 1) & "%"
        Else
            ProgressBar8.Value = 0
            TextBox8.Text = ""
        End If
        'Row 9
        If TextActual9.Text <> "" Then
            If Val(TextActual9.Text) < Val(TextPlan9.Text) Then
                comp_percent9 = Val(TextActual9.Text) / Val(TextPlan9.Text) * 100
                ProgressBar9.Value = Val(comp_percent9)
            Else
                comp_percent9 = 100 + ((Val(TextActual9.Text) - Val(TextPlan9.Text)) / Val(TextPlan9.Text)) * 100
                ProgressBar9.Value = 100
            End If
            TextBox9.Text = Round(comp_percent9, 1) & "%"
        Else
            ProgressBar9.Value = 0
            TextBox9.Text = ""
        End If
        'Row 10
        If TextActual10.Text <> "" Then
            If Val(TextActual10.Text) < Val(TextPlan10.Text) Then
                comp_percent10 = 100 + ((Val(TextActual10.Text) - Val(TextPlan10.Text)) / Val(TextPlan10.Text)) * 100
                ProgressBar10.Value = Val(comp_percent10)
            Else
                comp_percent10 = 100 + Val(TextActual10.Text) - Val(TextPlan10.Text)
                ProgressBar10.Value = 100
            End If
            TextBox10.Text = Round(comp_percent10, 1) & "%"
        Else
            ProgressBar10.Value = 0
            TextBox10.Text = ""
        End If
        'Row 11
        If TextActual11.Text <> "" Then
            If Val(TextActual11.Text) < Val(TextPlan11.Text) Then
                comp_percent11 = 100 + ((Val(TextActual11.Text) - Val(TextPlan11.Text)) / Val(TextPlan11.Text)) * 100
                ProgressBar11.Value = Val(comp_percent11)
            Else
                comp_percent11 = 100 + Val(TextActual11.Text) - Val(TextPlan11.Text)
                ProgressBar11.Value = 100
            End If
            TextBox11.Text = Round(comp_percent11, 1) & "%"
        Else
            ProgressBar11.Value = 0
            TextBox11.Text = ""
        End If
        'Row 12
        If TextActual12.Text <> "" Then
            If Val(TextActual12.Text) < Val(TextPlan12.Text) Then
                comp_percent12 = 100 + ((Val(TextActual12.Text) - Val(TextPlan12.Text)) / Val(TextPlan12.Text)) * 100
                ProgressBar12.Value = Val(comp_percent12)
            Else
                comp_percent12 = 100 + Val(TextActual12.Text) - Val(TextPlan12.Text)
                ProgressBar12.Value = 100
            End If
            TextBox12.Text = Round(comp_percent12, 1) & "%"
        Else
            ProgressBar12.Value = 0
            TextBox12.Text = ""
        End If
    End Sub
End Class

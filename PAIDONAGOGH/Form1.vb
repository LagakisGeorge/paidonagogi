Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Xml
Imports System.Data.OleDb
Imports System.Xml.Schema
Imports System.Data.SqlClient
Imports System.Web


Public Class Form1
    Dim dataBytes As Byte()
    Dim f_sqlDT As Integer 'ποιο στοιχειο εχω τσιμπήσει απο το sqldt.rows(f_sqldt)("..") (πελατες που ταιριάζουν στην αναζητηση)
    Dim SQLpELATES As New DataTable
    Dim F_CIdDiagn As String ' id gnomateysi 
    Dim F_cIdPel As String ' id pel που εχω διαλεξει

    Dim F_ImageFile As String = ""
    'create data adapter
    Dim da As OleDbDataAdapter ' SqlDataAdapter

    'create dataset
    Dim ds As DataSet = New DataSet





    Public gSQLCon As String
    ' Public GCONNECT As String
    Private sqlDT As DataTable



    Public Function checkServer(ByVal check_path As Integer) As Boolean
        Dim c As String
        Dim tmpStr As String
        c = "Config.ini"


        Dim par As String = ""
        Dim mf As String
        mf = c   ' "c:\mercvb\err3.txt"
        If Len(Dir(UCase(mf))) = 0 Then
            par = ":(local)\sql2012:sa:12345678:1:EMP"    '" 'G','g','Ξ','D'  "
            par = InputBox("ΒΑΣΗ ΔΕΔΟΜΕΝΩΝ", , par)
        Else
            FileOpen(1, mf, OpenMode.Input)
            '   Input(1, par)
            par = LineInput(1)
            FileClose(1)
        End If
        If check_path = 1 Then
            par = InputBox("ΒΑΣΗ ΔΕΔΟΜΕΝΩΝ  (CONFIG.INI ΣΤΟΝ ΤΡΕΧΟΝΤΑ ΦΑΚΕΛΟ) ", ":Π.Χ. (local)\sql2012:sa:12345678:1:EMP", par)
        End If

        'Input = InputBox("Text:")

        If String.IsNullOrEmpty(par) Then
            ' Cancelled, or empty
            checkServer = False
            ' MsgBox("εξοδος λογω μη σύνδεσης με βάση δεδομένων")
            Exit Function
        Else
            ' Normal
        End If


        FileOpen(1, mf, OpenMode.Output)
        PrintLine(1, par)
        FileClose(1)





        ':(local)\sql2012:::2:EMP
        ':(local)\sql2012:sa:12345678:1:EMP





        Try

            ' With FrmSERVERSETTINGS
            OpenFileDialog1.FileName = c
            Dim openedFileStream As IO.Stream
            openedFileStream = OpenFileDialog1.OpenFile()
            'End With

            ReDim dataBytes(openedFileStream.Length - 1) 'Init 
            openedFileStream.Read(dataBytes, 0, openedFileStream.Length)
            openedFileStream.Close()
            tmpStr = par ' System.Text.Encoding.Unicode.GetString(dataBytes)

            '     With FrmSERVERSETTINGS
            If Val(Split(tmpStr, ":")(4)) = 1 Then
                'network
                'gConnect = "Provider=SQLOLEDB.1;" & _
                '           "Data Source=" & Split(tmpStr, ":")(0) & _
                '           ";Network=" & Split(tmpStr, ":")(1) & _
                '           ";Server=" & Split(tmpStr, ":")(1) & _
                '           ";Initial Catalog=" & Trim(Split(tmpStr, ":")(5)) & _
                '           ";User Id=" & Split(tmpStr, ":")(2) & _
                '           ";Password=" & Split(tmpStr, ":")(3)

                gConnect = "Provider=SQLOLEDB.1;;Password=" & Split(tmpStr, ":")(3) &
                ";Persist Security Info=True ;" &
                ";User Id=" & Split(tmpStr, ":")(2) &
                ";Initial Catalog=" & Trim(Split(tmpStr, ":")(5)) &
                ";Data Source=" & Split(tmpStr, ":")(1)

                ''   gConnect = "Provider=SQLOLEDB.1;;Password=" & Split(tmpStr, ":")(3) &
                gSQLCon = "Server=" + Split(tmpStr, ":")(1)
                gSQLCon = gSQLCon + ";Database=" + Trim(Split(tmpStr, ":")(5))
                gSQLCon = gSQLCon + ";Uid=" + Split(tmpStr, ":")(2) + ";Pwd=" + Split(tmpStr, ":")(3)



            Else
                'local
                'MsgBox(Split(tmpStr, ":")(1))
                '  gConnect = "Provider=SQLOLEDB;Server=" & Split(tmpStr, ":")(1) &
                '         ";Database=" & Split(tmpStr, ":")(5) & "; Trusted_Connection=yes;"

                '    gConSQL = "Data Source=" & Split(tmpStr, ":")(1) & ";Integrated Security=True;database=" & Split(tmpStr, ":")(5)
                'cnString = "Data Source=localhost\SQLEXPRESS;Integrated Security=True;database=YGEIA"

            End If
            'End With
            Dim sqlCon As New OleDbConnection
            '
            ' gConnect = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;PWD=12345678;Initial Catalog=D2014;Data Source=logisthrio\sqlexpress"
            'GDB.Open(gConnect)



            'OK
            'gConnect = "Provider=SQLOLEDB.1;;Password=12345678;Persist Security Info=True ;User Id=sa;Initial Catalog=EMP;Data Source=LOGISTHRIO\SQLEXPRESS"
            sqlCon.ConnectionString = gConnect
            sqlCon.Open()
            checkServer = True
            sqlCon.Close()

            '            Dim GDB As New ADODB.Connection

        Catch ex As Exception
            checkServer = False
            MsgBox("εξοδος λογω μη σύνδεσης με βάση δεδομένων")
            'End
        End Try
    End Function


    Public Function ExecuteSQLQuery(ByVal SQLQuery As String) As DataTable
        Try
            Dim sqlCon As New OleDbConnection(gConnect)

            Dim sqlDA As New OleDbDataAdapter(SQLQuery, sqlCon)

            Dim sqlCB As New OleDbCommandBuilder(sqlDA)
            sqlDT.Reset() ' refresh 
            sqlDA.Fill(sqlDT)
            'rowsAffected = command.ExecuteNonQuery();
            ' sqlDA.Fill(sqlDaTaSet, "PEL")

        Catch ex As Exception
            MsgBox("Error: " & ex.ToString)
            If Err.Number = 5 Then
                MsgBox("Invalid Database, Configure TCP/IP", MsgBoxStyle.Exclamation, "Sales and Inventory")
            Else
                MsgBox("Error : " & ex.Message)
            End If
            MsgBox("Error No. " & Err.Number & " Invalid database or no database found !! Adjust settings first", MsgBoxStyle.Critical, "Sales And Inventory")
            MsgBox(SQLQuery)
        End Try
        Return sqlDT
    End Function

    Public Sub ExecuteSQLQuery(ByVal SQLQuery As String, ByRef SQLDT As DataTable)
        'αν χρησιμοποιώ  byref  tote prepei να δηλωθεί   
        'Dim DTI As New DataTable


        Try
            Dim sqlCon As New OleDbConnection(gConnect)

            Dim sqlDA As New OleDbDataAdapter(SQLQuery, sqlCon)

            Dim sqlCB As New OleDbCommandBuilder(sqlDA)
            SQLDT.Reset() ' refresh 
            sqlDA.Fill(SQLDT)
            ' sqlDA.Fill(sqlDaTaSet, "PEL")

        Catch ex As Exception
            MsgBox("Error: " & ex.ToString)
            If Err.Number = 5 Then
                MsgBox("Invalid Database, Configure TCP/IP", MsgBoxStyle.Exclamation, "Sales and Inventory")
            Else
                MsgBox("Error : " & ex.Message)
            End If
            MsgBox("Error No. " & Err.Number & " Invalid database or no database found !! Adjust settings first", MsgBoxStyle.Critical, "Sales And Inventory")
            MsgBox(SQLQuery)
        End Try
        'Return sqlDT
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        'DIORTOSI.Visible = False
        NEADIAGNOSI.Visible = False


        ExecuteSQLQuery("SELECT * FROM PEL WHERE EIDOS='e' and EPO LIKE '" + TextBox1.Text + "%'", SQLpELATES)
        If SQLpELATES.Rows.Count > 0 Then
            EGGRAFESN.Text = Str(SQLpELATES.Rows.Count)
            EPO.Text = SQLpELATES.Rows(0)("EPO")
            kod.Text = SQLpELATES.Rows(0)("kod")
            f_sqlDT = 0
            F_cIdPel = SQLpELATES.Rows(f_sqlDT)("ID")  'f_sqlDT = 0
        Else
            BNext.Enabled = False
            EPO.Text = ""
            Exit Sub


        End If
        'BindingSource1.DataSource = SQLpELATES
        SAVEDIAGN.Enabled = False
        ' BindingNavigator.BindingNavigatorPositionItem
        ' BindingNavigatorPositionItem.v
        SHOW_GNOMATEYSI()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        sqlDT = New DataTable

        checkServer(0)

        Dim sqldt3 As New DataTable
        ExecuteSQLQuery("select * from THERAP", sqldt3)
        For K As Integer = 0 To sqldt3.Rows.Count - 1
            ComboTher.Items.Add(sqldt3.Rows(K)("EPO") + Space(50) + ";" + Str(sqldt3.Rows(K)("ID").ToString))
        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles BNext.Click
        f_sqlDT = f_sqlDT + 1
        If f_sqlDT < SQLpELATES.Rows.Count Then

            EPO.Text = SQLpELATES.Rows(f_sqlDT)("EPO")
            kod.Text = SQLpELATES.Rows(f_sqlDT)("kod")
            F_cIdPel = SQLpELATES.Rows(f_sqlDT)("ID")  'f_sqlDT = 0
            EGGRAFESN.Text = F_cIdPel




        End If
        SHOW_GNOMATEYSI()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles NEADIAGNOSI.Click
        Dim SQLDT2 As New DataTable
        Dim MKOD, C As String
        MKOD = SQLpELATES.Rows(f_sqlDT)("KOD")
        C = SQLpELATES.Rows(f_sqlDT)("ID").ToString
        F_cIdPel = C
        ExecuteSQLQuery("INSERT INTO GNOMATEYSI (KOD,ENERGH,IDPEL) VALUES ('" + MKOD + "',1," + C + ")", SQLDT2)
        ExecuteSQLQuery("select max(ID) FROM  GNOMATEYSI WHERE KOD='" + MKOD + "'", SQLDT2)
        Dim NEWID As String = SQLDT2.Rows(0)(0).ToString
        F_CIdDiagn = NEWID
        SAVEDIAGN.Enabled = True

        p1.Image = Nothing


        'INSERT INTO [dbo].[GNOMATEYSI] 
        ' (<KOD, nchar(10),>
        ' ,<IDPEL, int,>
        ' ,<KATHGORIA, nvarchar(50),>
        ' ,<KODNOSIMATOS, nvarchar(10),>
        ' ,<TITLOSNOSIMATOS, nvarchar(50),>
        ' ,<EIDIK1, nvarchar(50),>
        ' ,<EIDIK2, nvarchar(50),>
        ' ,<EIDIK3, nvarchar(50),>
        ' ,<EIDIK4, nvarchar(50),>
        ' ,<LOGH, int,>
        ' ,<ERGH, int,>
        ' ,<PSIH, int,>
        ' ,<FYSH, int,>
        ' ,<EIDH, int,>
        ' ,<OIKH, int,>
        ' ,<ANANEOSI, nchar(20),>
        ' ,<ENARXI, date,>
        ' ,<LHXH, date,>
        ' ,<ANANEOSIAAMHNOS, int,>
        ' ,<IMAGE, image,>
        ' ,<ENERGH, int,>)
    End Sub


    Private Sub SAVEDIAGN_Click(sender As Object, e As EventArgs) Handles SAVEDIAGN.Click
        BNext.Enabled = True
        bPrev.Enabled = True

        NEADIAGNOSI.Enabled = True
        ' DIORTOSI.Enabled = True

        upd_Cfield("KATHGORIA", KATHGORIA)
        upd_Cfield("KODNOSIMATOS", KODNOSIMATOS)
        upd_Cfield("TITLOSNOSIMATOS", TITLOSNOSIMATOS)
        upd_Cfield("EIDIK1", EIDIK1)
        upd_Cfield("EIDIK2", EIDIK2)
        upd_Cfield("EIDIK3", EIDIK3)
        upd_Cfield("EIDIK4", EIDIK4)

        upd_Nfield("LOGH", LOGH)
        upd_Nfield("ERGH", ERGH)

        upd_Nfield("PSIH", PSIH)
        upd_Nfield("FYSH", FYSH)
        SAVEDIAGN.Enabled = False


    End Sub

    Sub upd_Cfield(f As String, v As TextBox)
        ExecuteSQLQuery("UPDATE GNOMATEYSI SET " + f + "='" + v.Text + "' where ID=" + F_CIdDiagn)
    End Sub
    Sub upd_Nfield(f As String, v As TextBox)
        Dim VC As String
        If Val(v.Text) = 0 Then
            VC = "0"
        Else
            VC = v.Text
        End If
        ExecuteSQLQuery("UPDATE GNOMATEYSI SET " + f + "=" + VC + " where ID=" + F_CIdDiagn)
    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles EPILOGH.Click
        BNext.Enabled = False
        bPrev.Enabled = False


        '  DIORTOSI.Visible = True

        If SHOW_GNOMATEYSI() = 1 Then ' ADEIO
            NEADIAGNOSI.Visible = True
            SAVEDIAGN.Enabled = False
        Else
            SAVEDIAGN.Enabled = True
        End If

        If F_CIdDiagn = Nothing Or Val(F_CIdDiagn) = 0 Then
            F_CIdDiagn = "0"
        End If
        PAINT_GRID()

        PAINT_GRID_PERIOD()
        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = "dd/MM/yyyy  hh:mm "


        ' Dim bm_source As New Bitmap(p1.Image)
        '  p1.Image = ResizeImage(bm_source)
        'Try
        '    Dim CC As String
        '    CC = "c:\mercvb\images\" + F_CIdDiagn + ".JPG"  'p1.ImageLocation
        '    Dim source As New Bitmap(CC) 'OpenFileDialog2.FileName) '"C:\image.png")
        '    p1.Image = ResizeImage(source)
        'Catch ex As Exception
        '    p1.Image = Nothing

        'End Try


    End Sub
    Function SHOW_GNOMATEYSI() As Integer
        Dim ADEIO As Integer
        ExecuteSQLQuery(" Select * from GNOMATEYSI WHERE ENERGH=1 AND IDPEL=" + F_cIdPel)
        If sqlDT.Rows.Count > 0 Then
            ADEIO = 0
            NEADIAGNOSI.Enabled = False
            NEADIAGNOSI.BackColor = Color.White
            '  DIORTOSI.BackColor = Color.Green
            KATHGORIA.Text = sqlDT.Rows(0)("KATHGORIA").ToString
            TITLOSNOSIMATOS.Text = sqlDT.Rows(0)("TITLOSNOSIMATOS").ToString
            F_CIdDiagn = sqlDT.Rows(0)("ID").ToString
            KODNOSIMATOS.Text = sqlDT.Rows(0)("KODNOSIMATOS").ToString
            EIDIK1.Text = sqlDT.Rows(0)("EIDIK1").ToString
            EIDIK2.Text = sqlDT.Rows(0)("EIDIK2").ToString
            EIDIK3.Text = sqlDT.Rows(0)("EIDIK3").ToString
            EIDIK4.Text = sqlDT.Rows(0)("EIDIK4").ToString

            LOGH.Text = sqlDT.Rows(0)("LOGH").ToString
            ERGH.Text = sqlDT.Rows(0)("ERGH").ToString
            PSIH.Text = sqlDT.Rows(0)("PSIH").ToString
            EIDH.Text = sqlDT.Rows(0)("EIDH").ToString
            OIKH.Text = sqlDT.Rows(0)("OIKH").ToString
            FYSH.Text = sqlDT.Rows(0)("FYSH").ToString
            If IsDBNull(sqlDT.Rows(0)("EIK")) Then
                F_ImageFile = ""

            Else
                F_ImageFile = sqlDT.Rows(0)("EIK").ToString

            End If

            If Len(sqlDT.Rows(0)("EIK").ToString) > 0 Then
                    Dim source As New Bitmap(F_ImageFile)
                    P1.Image = ResizeImage(source)

                End If



            Else
                ADEIO = 1
            NEADIAGNOSI.Enabled = True
            NEADIAGNOSI.BackColor = Color.Green
            ' DIORTOSI.Enabled = False
            ' NEADIAGNOSI.Enabled = False
            ' DIORTOSI.BackColor = Color.White
            '  DIORTOSI.BackColor = Color.Green

            p1.Image = Nothing


            KATHGORIA.Text = ""
            TITLOSNOSIMATOS.Text = ""
            ' F_CIdDiagn = sqlDT.Rows(0)("ID").ToString
            KODNOSIMATOS.Text = ""
            EIDIK1.Text = ""
            EIDIK2.Text = ""
            EIDIK3.Text = ""
            EIDIK4.Text = ""

            LOGH.Text = ""
            ERGH.Text = ""
            PSIH.Text = ""
            EIDH.Text = ""
            OIKH.Text = ""
            FYSH.Text = ""

        End If


        SHOW_GNOMATEYSI = ADEIO




    End Function

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles bPrev.Click
        f_sqlDT = f_sqlDT - 1
        If f_sqlDT >= 0 Then 'SQLpELATES.Rows.Count Then
            Try

                EPO.Text = SQLpELATES.Rows(f_sqlDT)("EPO")
                kod.Text = SQLpELATES.Rows(f_sqlDT)("kod")
                F_cIdPel = SQLpELATES.Rows(f_sqlDT)("ID")  'f_sqlDT = 0
                EGGRAFESN.Text = F_cIdPel


            Catch ex As Exception

            End Try


        Else
            f_sqlDT = 0

        End If
        SHOW_GNOMATEYSI()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form2.Show()

    End Sub
    Private Sub PAINT_GRID()
        Dim STHLHTOY_ID As Int16 = 0
        'cnString = gConnect ' "Data Source=localhost\SQLEXPRESS;Integrated Security=True;database=thermo"
        'Str_Connection = cnString
        Dim SQLqry
        SQLqry = "SELECT TOP 100 ID,CONVERT(CHAR(10),HME,3) AS [ΗΜΕΡ],ORES AS [ΩΡΕΣ] FROM SYNEDRIES WHERE IDGN= " + F_CIdDiagn  ' ORDER BY HME "
        'conn = New SqlConnection(cnString)

        Dim conn As New OleDbConnection
        conn.ConnectionString = gConnect
        conn.Open()



        Try
            ' Open connection
            'conn.Open()

            da = New OleDbDataAdapter(SQLqry, conn)

            'create command builder
            Dim cb As OleDbCommandBuilder = New OleDbCommandBuilder(da)
            ds.Clear()
            'fill dataset
            da.Fill(ds, "PEL")
            GridView1.ClearSelection()
            GridView1.DataSource = ds
            GridView1.DataMember = "PEL"

            GridView1.Columns(STHLHTOY_ID).Width = 0
            GridView1.Columns(STHLHTOY_ID).Visible = False
            '  Dim n As Integer = GridView1.Columns.Count
            GridView1.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        Catch ex As SqlException
            MsgBox(ex.ToString)
        Finally
            ' Close connection
            conn.Close()
        End Try

    End Sub
    Private Sub SaveSynedr_Click(sender As Object, e As EventArgs) Handles saveSynedr.Click
        Dim cIDTH As String = Split(ComboTher.Text, ";")(1)
        Dim cDGN = F_CIdDiagn

        Dim Chme As String = Format(DateTimePicker1.Value, "MM/dd/yyyy")
        Dim CAPO As String = Format(APO.Value, "MM/dd/yyyy")
        Dim CEOS As String = Format(EOS.Value, "MM/dd/yyyy")



        Dim sqlt1 As New DataTable
        ExecuteSQLQuery("select count(*) from PERIODOI WHERE '" + Chme + "'>=APO AND '" + Chme + "'<=EOS AND IDGN=" + cDGN, sqlt1)
        If sqlt1.Rows(0)(0) = 0 Then
            MsgBox("ΔΕΝ ΕΧΕΙ ΟΡΙΣΤΕΙ ΠΕΡΙΟΔΟΣ ΣΕ ΑΥΤΟ ΤΟ ΔΙΑΤΗΜΑ")
            Exit Sub
        End If






        ExecuteSQLQuery("insert into SYNEDRIES (IDTH,IDGN,ORES,HME) VALUES (" + cIDTH + "," + cDGN + ",1,'" + Format(DateTimePicker1.Value, "MM/dd/yyyy HH:mm") + "')")
        '    INSERT INTO [dbo].[SYNEDRIES]
        '   ([ IDGN]
        '   ,[IDTH]
        '   ,[HME]
        '   ,[ORES]
        '   ,[N1]
        '   ,[C1]
        '   ,[N2]
        '   ,[C2])
        ''        (<IDGN, int,>   F_CIdDiagn
        ',<IDTH, int,>   combo-split(1)
        ',<HME, datetime,>
        ',<ORES, real,>
        ',<N1, real,>
        ',<C1, nvarchar(50),>
        ',<N2, real,>
        ',<C2, nvarchar(50),>)

        PAINT_GRID()







    End Sub

    Private Sub PAINT_GRID_PERIOD()
        'create data adapter
        Dim da As OleDbDataAdapter ' SqlDataAdapter

        'create dataset
        Dim ds As DataSet = New DataSet



        Dim STHLHTOY_ID As Int16 = 0
        'cnString = gConnect ' "Data Source=localhost\SQLEXPRESS;Integrated Security=True;database=thermo"
        'Str_Connection = cnString
        Dim SQLqry
        SQLqry = "SELECT TOP 100 ID,CONVERT(CHAR(10),APO,3) AS [ΑΠΟ],CONVERT(CHAR(10),EOS,3) AS [ΕΩΣ] FROM PERIODOI WHERE IDGN= " + F_CIdDiagn  ' ORDER BY HME "
        'conn = New SqlConnection(cnString)

        Dim conn As New OleDbConnection
        conn.ConnectionString = gConnect
        conn.Open()



        Try
            ' Open connection
            'conn.Open()

            da = New OleDbDataAdapter(SQLqry, conn)

            'create command builder
            Dim cb As OleDbCommandBuilder = New OleDbCommandBuilder(da)
            ds.Clear()
            'fill dataset
            da.Fill(ds, "PEL")
            GridView2.ClearSelection()
            GridView2.DataSource = ds
            GridView2.DataMember = "PEL"

            GridView2.Columns(STHLHTOY_ID).Width = 0
            GridView2.Columns(STHLHTOY_ID).Visible = False

            Dim n As Integer = GridView2.Columns.Count
            GridView2.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        Catch ex As SqlException
            MsgBox(ex.ToString)
        Finally
            ' Close connection
            conn.Close()
        End Try

    End Sub
    Private Sub BAkyr_Click(sender As Object, e As EventArgs) Handles bAkyr.Click
        BNext.Enabled = True
        bPrev.Enabled = True
        F_CIdDiagn = Nothing

    End Sub

    Private Sub bDeleSynedr_Click(sender As Object, e As EventArgs) Handles bDeleSynedr.Click
        ' ο κωδικος του προιοντος που διαλεξα
        Dim mk As String = GridView1.CurrentRow.Cells(0).Value.ToString
        Dim FOFO As New DataTable
        ExecuteSQLQuery("DELETE FROM SYNEDRIES WHERE ID=" + mk, FOFO)
        MsgBox("ΔΙΕΓΡΑΦΗ")
        PAINT_GRID()


    End Sub

    Private Sub SavePeriodos_Click(sender As Object, e As EventArgs) Handles SavePeriodos.Click
        '  Dim cIDTH As String = Split(ComboTher.Text, ";")(1)
        Dim cDGN = F_CIdDiagn
        ExecuteSQLQuery("insert into PERIODOI (IDGN,APO,EOS) VALUES (" + cDGN + ",'" + Format(APO.Value, "MM/dd/yyyy") + "','" + Format(EOS.Value, "MM/dd/yyyy") + "')")
        PAINT_GRID_PERIOD()





        '        USE [MERCURY]
        'GO

        '/****** Object  Table [dbo].[PERIODOI]    Script Date:  9/2/2020 10:58:19 πμ ******/
        'SET ANSI_NULLS ON
        'GO

        'SET QUOTED_IDENTIFIER ON
        'GO

        '        CREATE TABLE [dbo].[PERIODOI](
        '    [ID] [Int] Not NULL,
        '    [IDGN] [Int] NULL,
        '    [APO] [DateTime] NULL,
        '    [EOS] [DateTime] NULL,
        '    [N1] [real] NULL,
        '    [N2] [real] NULL,
        '    [C1] [nvarchar](50) NULL,
        '    [C2] [nvarchar](50) NULL,
        ' Constraint [PK_PERIODOI] PRIMARY KEY CLUSTERED 
        '(
        '	[ID] Asc
        ')WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ') ON [PRIMARY]
        'GO


    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        ' ο κωδικος του προιοντος που διαλεξα
        Dim mk As String = GridView2.CurrentRow.Cells(0).Value.ToString
        Dim FOFO As New DataTable
        ExecuteSQLQuery("DELETE FROM PERIODOI WHERE ID=" + mk, FOFO)
        MsgBox("ΔΙΕΓΡΑΦΗ")
        PAINT_GRID_PERIOD()
    End Sub

    ' Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click





    '  Dim ΒΜ_source As New Bitmap(p1.Image)
    '  p1.Image = ResizeImage(bm_source)


    '   p2.Image = ResizeImage(p1.Image)





    'Dim x As Integer = 0
    'Dim y As Integer = 0
    'Dim k = 0
    'Dim l = 0
    'Dim bm As New Bitmap(p1.Image)
    'Dim om As New Bitmap(p1.Image.Width, p1.Image.Height)
    'Dim r, g, b As Byte
    'Do While x < bm.Width - 1
    '    y = 0
    '    l = 0
    '    Do While y < bm.Height - 1
    '        r = 255 - bm.GetPixel(x, y).R
    '        g = 255 - bm.GetPixel(x, y).G
    '        b = 255 - bm.GetPixel(x, y).B
    '        om.SetPixel(k, l, Color.FromArgb(r, g, b))
    '        y += 3
    '        l += 1
    '    Loop
    '    x += 3
    '    k += 1
    'Loop
    'p2.Image = om
    '  End Sub

    Private Sub P1_Click(sender As Object, e As EventArgs) Handles p1.Click
        If F_CIdDiagn = Nothing Then
            Exit Sub
        End If

        ' OpenFileDialog2.InitialDirectory = "C:\"
        OpenFileDialog2.Title = "Open a Text File"
        OpenFileDialog2.Filter = "Text Files|*.JPG"




        '  OpenFileDialog2.Filter = "*.JPG"
        ' OpenFileDialog2.ShowDialog()

        Dim res As DialogResult = OpenFileDialog2.ShowDialog()
        If res = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If



        Dim source As New Bitmap(OpenFileDialog2.FileName) '"C:\image.png")
        '  Dim target As New Bitmap(Size.Width, Size.Height) ', PixelFormat.Format24bppRgb)

        ' Using graphics As Graphics = Graphics.FromImage(target)
        '  Graphics.DrawImage(source, New Size(48, 48))
        '   End Using


        p1.Image = ResizeImage(source)

        Dim C As String = "c:\mercvb" + "\images\" + F_CIdDiagn + ".JPG"
        ExecuteSQLQuery("UPDATE GNOMATEYSI SET EIK='" + C + "' WHERE ID=" + F_CIdDiagn)
        If My.Computer.FileSystem.FileExists(C) Then
        Else

            '     If FileSystem.FileExists("c:\Check.txt") Then
            ' DOYLEYEI OK
            FileSystem.FileCopy(OpenFileDialog2.FileName, C)
        End If


        p1.ImageLocation = C
    End Sub

    Public Overloads Shared Function ResizeImage(ByVal InputImage As Image) As Image
        Return New Bitmap(InputImage, New Size(135, 99))
    End Function

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        ' picture.PictureBox1.Image = p1.Image
        ' PICTURE.Show()


        If Len(F_ImageFile) = 0 Then
            Exit Sub
        End If

        Dim myForm2 As New PICTURE
        myForm2.FIMAGEFILE.Text = F_ImageFile

        myForm2.PictureBox1.Image = Image.FromFile(F_ImageFile)
        myForm2.ResizeImage(myForm2.PictureBox1.Image)
        myForm2.ShowDialog()
        'Any actions after the user returns would be here
        myForm2.Dispose()





    End Sub


End Class


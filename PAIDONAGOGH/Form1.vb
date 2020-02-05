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
            Dim sqlCon As New OleDbConnection(GCONNECT)

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
            Dim sqlCon As New OleDbConnection(GCONNECT)

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

        '  DIORTOSI.Visible = True

        If SHOW_GNOMATEYSI() = 1 Then ' ADEIO
            NEADIAGNOSI.Visible = True
            SAVEDIAGN.Enabled = False
        Else
            SAVEDIAGN.Enabled = True
        End If
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
            'SAVEDIAGN.Enabled = True
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


        Else
            ADEIO = 1
            NEADIAGNOSI.Enabled = True
            NEADIAGNOSI.BackColor = Color.Green
            ' DIORTOSI.Enabled = False
            ' NEADIAGNOSI.Enabled = False
            ' DIORTOSI.BackColor = Color.White
            '  DIORTOSI.BackColor = Color.Green



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

    'Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs)
    '    f_sqlDT = f_sqlDT + 1
    '    If f_sqlDT < SQLpELATES.Rows.Count Then

    '        EPO.Text = SQLpELATES.Rows(f_sqlDT)("EPO")
    '        kod.Text = SQLpELATES.Rows(f_sqlDT)("kod")
    '        F_cIdPel = SQLpELATES.Rows(f_sqlDT)("ID")  'f_sqlDT = 0
    '        EGGRAFESN.Text = F_cIdPel


    '    End If
    '    SHOW_GNOMATEYSI()
    'End Sub


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        f_sqlDT = f_sqlDT - 1
        If f_sqlDT >= 0 Then 'SQLpELATES.Rows.Count Then

            EPO.Text = SQLpELATES.Rows(f_sqlDT)("EPO")
            kod.Text = SQLpELATES.Rows(f_sqlDT)("kod")
            F_cIdPel = SQLpELATES.Rows(f_sqlDT)("ID")  'f_sqlDT = 0
            EGGRAFESN.Text = F_cIdPel

        Else
            f_sqlDT = 0

        End If
        SHOW_GNOMATEYSI()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form2.Show()

    End Sub
End Class

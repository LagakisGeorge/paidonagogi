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
    Dim f_sqlDT As Integer

    Dim F_CIdDiagn As String


    Dim gSQLCon As String
    Dim GCONNECT As String
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
            'SQLDT.Reset() ' refresh 
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
        ExecuteSQLQuery("SELECT * FROM PEL WHERE EIDOS='e' and EPO LIKE '%" + TextBox1.Text + "%'")
        If sqlDT.Rows.Count > 0 Then
            EGGRAFESN.Text = Str(sqlDT.Rows.Count)
            EPO.Text = sqlDT.Rows(0)("EPO")
            kod.Text = sqlDT.Rows(0)("kod")
            f_sqlDT = 0
        Else
            Button2.Enabled = False

        End If



    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        sqlDT = New DataTable

        checkServer(0)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        f_sqlDT = f_sqlDT + 1
        If f_sqlDT < sqlDT.Rows.Count Then

            EPO.Text = sqlDT.Rows(f_sqlDT)("EPO")
            kod.Text = sqlDT.Rows(f_sqlDT)("kod")
            'f_sqlDT = 0


        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SQLDT2 As New DataTable
        Dim MKOD, C As String
        MKOD = sqlDT.Rows(f_sqlDT)("KOD")
        C = sqlDT.Rows(f_sqlDT)("ID").ToString
        F_CIdDiagn = C
        ExecuteSQLQuery("INSERT INTO GNOMATEYSI (KOD,ENERGH,IDPEL) VALUES ('" + MKOD + "',1," + C + ")", SQLDT2)
        ExecuteSQLQuery("select max(ID) FROM  GNOMATEYSI WHERE KOD='" + MKOD + "'", SQLDT2)
        Dim NEWID As String = SQLDT2.Rows(0)(0).ToString


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

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TITLOSNOSIMATOS.TextChanged

    End Sub

    Private Sub SAVEDIAGN_Click(sender As Object, e As EventArgs) Handles SAVEDIAGN.Click
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

    End Sub

    Sub upd_Cfield(f As String, v As TextBox)
        ExecuteSQLQuery("UPDATE GNOMATEYSI SET " + f + "='" + v.Text + "' where ID=" + F_CIdDiagn)
    End Sub
    Sub upd_Nfield(f As String, v As TextBox)
        ExecuteSQLQuery("UPDATE GNOMATEYSI SET " + f + "=" + v.Text + " where ID=" + F_CIdDiagn)
    End Sub

End Class

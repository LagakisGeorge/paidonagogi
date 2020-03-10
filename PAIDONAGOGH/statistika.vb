Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Xml
Imports System.Data.OleDb
Imports System.Xml.Schema
Imports System.Data.SqlClient
Imports System.Web


Public Class statistika
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles BsYNEDRIES.Click
        'create data adapter
        Dim da As OleDbDataAdapter ' SqlDataAdapter

        'create dataset
        Dim ds As DataSet = New DataSet



        Dim STHLHTOY_ID As Int16 = 0
        'cnString = gConnect ' "Data Source=localhost\SQLEXPRESS;Integrated Security=True;database=thermo"
        'Str_Connection = cnString

        Dim SQLqry As String
        Dim TH As String

        Dim en As String = " ENERGH=1 AND "
        If Len(tEN.Text) > 0 Then
            If tEN.Text = "*" Then
                en = ""
            Else
                If tEN.Text = "2" Then
                    en = " ENERGH=2 AND "
                End If
            End If

        End If


        If Len(Combother.Text) > 1 Then
            TH = "  IDTH=" + Split(Combother.Text, ";")(1) + " and "
        Else
            TH = ""
        End If



        SQLqry = "SELECT TOP 100 S.ID,CONVERT(CHAR(10),HME,3) AS [ΗΜΕΡ],ORES AS [ΩΡΕΣ],EPO AS [ΘΕΡΑΠ],C1 AS [ΘΕΡΑΠΕΙΑ],DATEKATAX AS [ΗΜΕΡ.ΚΑΤΑΧ] "
        SQLqry = SQLqry + "  FROM SYNEDRIES S INNER JOIN THERAP T ON S.IDTH=T.ID WHERE " + EN + TH
        SQLqry = SQLqry + "  HME >='" + Format(APO.Value, "MM/dd/yyyy") + "' and HME<='" + Format(EOS.Value, "MM/dd/yyyy") + "' ORDER BY ID DESC "



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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles BGnomateyseis.Click
        'create data adapter
        Dim da As OleDbDataAdapter ' SqlDataAdapter

        'create dataset
        Dim ds As DataSet = New DataSet



        Dim STHLHTOY_ID As Int16 = 0
        'cnString = gConnect ' "Data Source=localhost\SQLEXPRESS;Integrated Security=True;database=thermo"
        'Str_Connection = cnString
        Dim SQLqry
        SQLqry = "Select P.EPO,G.* from GNOMATEYSI G INNER JOIN PEL P  ON P.ID=G.IDPEL WHERE ENERGH=1  and DATEKATAX>='" + Format(APO.Value, "MM/dd/yyyy") + "' and DATEKATAX<='" + Format(EOS.Value, "MM/dd/yyyy") + "'order by ID DESC "
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

    Private Sub Statistika_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim sqldt3 As New DataTable
        Form1.ExecuteSQLQuery("select * from THERAP", sqldt3)
        For K As Integer = 0 To sqldt3.Rows.Count - 1
            ComboTher.Items.Add(sqldt3.Rows(K)("EPO") + Space(50) + ";" + Str(sqldt3.Rows(K)("ID").ToString))
        Next
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim sqldt3 As New DataTable
        Form1.ExecuteSQLQuery("select T.*,P.ID AS IDPEL from TIM T INNER JOIN PEL P ON P.EIDOS=T.EIDOS AND P.KOD=T.KPE WHERE SKOPOS2 IS NULL", sqldt3)  'WHERE SKOPOS2 IS NULL
        For K As Integer = 0 To sqldt3.Rows.Count - 1
            Dim SQL As String = "UPDATE PERIODOI SET AJIAAPOD=" + Replace(sqldt3.Rows(K)("AJI").ToString, ",", ".") + ",ATIM='" + sqldt3.Rows(K)("ATIM").ToString + "' WHERE IDPEL=" + sqldt3.Rows(K)("IDPEL").ToString
            SQL = SQL + " AND '" + Format(sqldt3.Rows(K)("HME"), "MM/dd/yyyy") + "'>APO AND '" + Format(sqldt3.Rows(K)("HME"), "MM/dd/yyyy") + "'<EOS "
            Dim sqldt5 As New DataTable
            Form1.ExecuteSQLQuery(SQL, sqldt5)
            ' If sqldt5.Rows.Count > 0 Then
            Form1.ExecuteSQLQuery("update TIM SET SKOPOS2='1' WHERE ID_NUM=" + sqldt3.Rows(K)("ID_NUM").ToString)
            ' End If
        Next







    End Sub
End Class
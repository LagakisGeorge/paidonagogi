Public Class PICTURE
    Private Sub PICTURE_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PICTURE_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PictureBox1.Width = Me.Width
        PictureBox1.Height = Me.Height
        If Len(FIMAGEFILE.Text) > 0 Then
            PictureBox1.Image = Image.FromFile(FIMAGEFILE.Text)
            '  Dim source As New Bitmap(FIMAGEFILE.Text)
            PictureBox1.Image = ResizeImage(PictureBox1.Image)
        End If

    End Sub


    Public Function ResizeImage(ByVal InputImage As Image) As Image
        Dim W, H As Integer
        W = PictureBox1.Width
        H = PictureBox1.Height
        Return New Bitmap(InputImage, New Size(W, H))
    End Function


End Class
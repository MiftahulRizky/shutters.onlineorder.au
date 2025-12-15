Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

Partial Class Order_API_Evolve
    Inherits Page

    Dim myConn As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.ContentType = "application/json"
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()

        Dim id As String = Request.QueryString("id")
        If String.IsNullOrEmpty(id) Then
            WriteJson(New With {.status = "error", .message = "Parameter 'id' is required"})
            Return
        End If

        Dim dt As New DataTable()

        Try
            Using con As New SqlConnection(myConn)
                Using cmd As New SqlCommand("SELECT TOP 1 * FROM Designs WHERE Id=@Id", con)
                    cmd.Parameters.AddWithValue("@Id", id)
                    con.Open()

                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using

            If dt.Rows.Count = 0 Then
                WriteJson(New With {.status = "not_found", .id = id})
            Else
                Dim row = dt.Rows(0)
                Dim result = New Dictionary(Of String, Object)

                For Each col As DataColumn In dt.Columns
                    result(col.ColumnName) = row(col)
                Next

                WriteJson(result)
            End If

        Catch ex As Exception
            WriteJson(New With {.status = "error", .message = ex.Message})
        End Try

    End Sub

    Private Sub WriteJson(obj As Object)
        Dim serializer As New JavaScriptSerializer()
        Dim json As String = serializer.Serialize(obj)
        Response.Write(json)
        Response.End()
    End Sub
End Class

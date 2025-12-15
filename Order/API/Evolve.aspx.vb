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

        Dim headerId As String = Request.QueryString("id")
        If String.IsNullOrEmpty(headerId) Then
            WriteJson(New With {.error = "id is required"})
            Return
        End If

        Try
            Dim result As New Dictionary(Of String, Object)

            Dim header As Dictionary(Of String, Object) = GetHeader(headerId)
            If header Is Nothing Then
                WriteJson(New With {.error = "Header not found"})
                Return
            End If

            result("header") = header

            ' ================= ITEMS =================
            Dim items As List(Of Dictionary(Of String, Object)) = GetItems(headerId)
            result("items") = items

            WriteJson(result)
    End Sub

    Private Function GetHeader(headerId As String) As Dictionary(Of String, Object)
        Using con As New SqlConnection(myConn)
            Using cmd As New SqlCommand("SELECT OrderId, OrderNumber, OrderName, OrderNote FROM OrderHeaders WHERE HeaderId =@HeaderId", con)
                cmd.Parameters.AddWithValue("@HeaderId", headerId)
                con.Open()

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        Dim header As New Dictionary(Of String, Object)
                        header("OrderId") = dr("OrderId")
                        header("OrderNumber") = dr("OrderNumber")
                        header("OrderName") = dr("OrderName")
                        header("OrderNote") = dr("OrderNote")
                        Return header
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    ' ====== GET ITEMS ======
    Private Function GetItems(headerId As String) As List(Of Dictionary(Of String, Object))
        Dim items As New List(Of Dictionary(Of String, Object))

        Using con As New SqlConnection(myConn)
            Using cmd As New SqlCommand("SELECT Room, Mounting, Width, [Drop] FROM OrderDetails WHERE HeaderId=@HeaderId AND Active=1", con)

                cmd.Parameters.AddWithValue("@HeaderId", headerId)
                con.Open()

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim item As New Dictionary(Of String, Object)
                        item("Room") = dr("Room")
                        item("Mounting") = dr("Mounting")
                        item("Width") = dr("Width")
                        item("Height") = dr("Drop")
                        item("Qty") = dr("Qty")
                        items.Add(item)
                    End While
                End Using
            End Using
        End Using

        Return items
    End Function

    Private Sub WriteJson(obj As Object)
        Dim serializer As New JavaScriptSerializer()
        Dim json As String = serializer.Serialize(obj)
        Response.Write(json)
        Response.End()
    End Sub
End Class

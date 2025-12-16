<%@ WebHandler Language="VB" Class="Evolve" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Script.Serialization

Public Class Evolve
    Implements IHttpHandler

    Private ReadOnly myConn As String =
        ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        context.Response.Cache.SetNoStore()

        Dim headerId As String = context.Request.QueryString("id")

        If String.IsNullOrEmpty(headerId) Then
            WriteJson(context, New With {.error = "id is required"})
            Return
        End If

        Try
            Dim result As New Dictionary(Of String, Object)

            Dim header = GetHeader(headerId)
            If header Is Nothing Then
                WriteJson(context, New With {.error = "Header not found"})
                Return
            End If

            result("header") = header
            result("items") = GetItems(headerId)

            WriteJson(context, result)

        Catch ex As Exception
            WriteJson(context, New With {.error = ex.Message})
        End Try
    End Sub

    Private Function GetHeader(headerId As String) As Dictionary(Of String, Object)
        Using con As New SqlConnection(myConn)
            Using cmd As New SqlCommand(
                "SELECT OrderId, OrderNumber, OrderName, OrderNote 
                 FROM OrderHeaders WHERE Id=@HeaderId", con)

                cmd.Parameters.AddWithValue("@HeaderId", headerId)
                con.Open()

                Using dr = cmd.ExecuteReader()
                    If dr.Read() Then
                        Return New Dictionary(Of String, Object) From {
                            {"OrderId", dr("OrderId")},
                            {"OrderNumber", dr("OrderNumber")},
                            {"OrderName", dr("OrderName")},
                            {"OrderNote", dr("OrderNote")}
                        }
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    Private Function GetItems(headerId As String) As List(Of Dictionary(Of String, Object))
        Dim items As New List(Of Dictionary(Of String, Object))

        Using con As New SqlConnection(myConn)
            Using cmd As New SqlCommand(
                "SELECT Qty, Room, Mounting, Width, [Drop]
                 FROM OrderDetails 
                 WHERE HeaderId=@HeaderId AND Active=1", con)

                cmd.Parameters.AddWithValue("@HeaderId", headerId)
                con.Open()

                Using dr = cmd.ExecuteReader()
                    While dr.Read()
                        items.Add(New Dictionary(Of String, Object) From {
                            {"Room", dr("Room")},
                            {"Mounting", dr("Mounting")},
                            {"Width", dr("Width")},
                            {"Height", dr("Drop")},
                            {"Qty", dr("Qty")}
                        })
                    End While
                End Using
            End Using
        End Using

        Return items
    End Function

    Private Sub WriteJson(context As HttpContext, obj As Object)
        Dim serializer As New JavaScriptSerializer()
        context.Response.Write(serializer.Serialize(obj))
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class

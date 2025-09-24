Partial Class Order_Maintenance
    Inherits Page

    Dim orderCfg As New OrderConfig

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("designId") = "" Then
            Response.Redirect("~/order/detail", False)
            Exit Sub
        End If

        If Not IsPostBack Then
            Dim designName As String = orderCfg.GetItemData("SELECT Name FROM Designs WHERE Id='" + Session("designId") + "'")
            cardTitle.InnerHtml = designName
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        Session("designId") = String.Empty
        Response.Redirect("~/order/detail", False)
    End Sub
End Class

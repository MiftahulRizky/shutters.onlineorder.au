
Partial Class _Default
    Inherits Page

    Dim settingCfg As New SettingConfig

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Or Session("RoleName") = "Account" Or Session("RoleName") = "Sunlight Product" Or Session("RoleName") = "Representative" Then
            Response.Redirect("~/order/", False)
            Exit Sub
        End If

        Dim newsletter As Boolean = settingCfg.CustomerNewsletter(Session("CustomerId"))
        If Session("RoleName") = "Customer" And newsletter = False Then
            Response.Redirect("~/order/", False)
            Exit Sub
        End If

        If Not IsPostBack Then
            MessageError(False, String.Empty)
        End If
    End Sub

    Private Sub MessageError(Show As Boolean, Msg As String)
        divError.Visible = Show : msgError.InnerText = Msg
    End Sub
End Class
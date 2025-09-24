Partial Class Account_Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("RoleName") <> "Administrator" Or Session("LevelName") <> "Leader" Then
            Response.Redirect("~/", False)
            Exit Sub
        End If

        'lblLoginId.Text = UCase(Session("LoginId")).ToString()
        'If Not IsPostBack Then
        '     BindData(lblLoginId.Text)
        'End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/", False)
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        ' MessageError(False, String.Empty)
        'Try
        '    If msgError.InnerText = "" Then
        '        sdsPage.Update()
        '        Session("UserName") = txtUserName.Text
        '        Response.Redirect("~/account/", False)
        '        Exit Sub
        '    End If
        'Catch ex As Exception
        '     MessageError(True, ex.ToString())
        '    If Not Session("RoleName") = "Administrator" Then
        '         MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
        '        publicCfg.MailError(Page.Title, "btnSubmit_Click", Session("LoginId"), ex.ToString())
        '    End If
        'End Try
    End Sub

    Private Sub BindData(UserId As String)
        ' MessageError(False, String.Empty)
        'Try
        '    Dim myData As DataSet = publicCfg.GetListData("SELECT * FROM CustomerLogins WHERE Id = '" + UCase(UserId).ToString() + "'")

        '    If myData.Tables(0).Rows.Count = 0 Then
        '        Response.Redirect("~/account/login", False)
        '        Exit Sub
        '    End If

        '    txtUserName.Text = myData.Tables(0).Rows(0).Item("UserName").ToString()
        '    txtFullName.Text = myData.Tables(0).Rows(0).Item("FullName").ToString()
        '    Dim roleId As String = myData.Tables(0).Rows(0).Item("RoleId").ToString()
        '    Dim levelId As String = myData.Tables(0).Rows(0).Item("LevelId").ToString()

        '     BindRole(roleId)
        '     BindLevel(levelId)
        'Catch ex As Exception
        '     MessageError(True, ex.ToString())
        '    If Not Session("RoleName") = "Administrator" Then
        '         MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
        '        publicCfg.MailError(Page.Title, "BindData", Session("LoginId"), ex.ToString())
        '    End If
        'End Try
    End Sub

    Private Sub BindRole(RoleId As String)
        'ddlRole.Items.Clear()
        'Try
        '    ddlRole.DataSource = publicCfg.GetListData("SELECT * FROM CustomerLoginRoles WHERE Id='" + UCase(RoleId).ToString() + "' ORDER BY Name ASC")
        '    ddlRole.DataTextField = "Name"
        '    ddlRole.DataValueField = "Id"
        '    ddlRole.DataBind()
        'Catch ex As Exception
        '     MessageError(True, ex.ToString())
        '    If Not Session("RoleName") = "Administrator" Then
        '         MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
        '        publicCfg.MailError(Page.Title, "BindRole", Session("LoginId"), ex.ToString())
        '    End If
        'End Try
    End Sub

    Private Sub BindLevel(LevelId As String)
        'ddlLevel.Items.Clear()
        'Try
        '    ddlLevel.DataSource = publicCfg.GetListData("SELECT * FROM CustomerLoginLevels WHERE Id='" + UCase(LevelId).ToString() + "' ORDER BY Name ASC")
        '    ddlLevel.DataTextField = "Name"
        '    ddlLevel.DataValueField = "Id"
        '    ddlLevel.DataBind()
        'Catch ex As Exception
        '     MessageError(True, ex.ToString())
        '    If Not Session("RoleName") = "Administrator" Then
        '         MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
        '        publicCfg.MailError(Page.Title, "BindLevel", Session("LoginId"), ex.ToString())
        '    End If
        'End Try
    End Sub

    Private Sub MessageError(Show As Boolean, Msg As String)
        divError.Visible = Show : msgError.InnerText = Msg
    End Sub
End Class

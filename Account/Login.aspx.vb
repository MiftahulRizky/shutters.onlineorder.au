Imports System.Data

Partial Class Account_Login
    Inherits Page

    Dim settingCfg As New SettingConfig

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Clear()
        If Not IsPostBack Then
            MessageError(False, String.Empty)
            CheckSessionStates()
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        BackColor()
        Try
            If txtUserLogin.Text = "" Then
                MessageError(True, "USERNAME IS REQUIRED !")
                txtUserLogin.BackColor = Drawing.Color.Empty
                txtUserLogin.Focus()
                Exit Sub
            End If

            If txtPassword.Text = "" Then
                MessageError(True, "PASSWORD IS REQUIRED !")
                txtPassword.BackColor = Drawing.Color.Empty
                txtPassword.Focus()
                Exit Sub
            End If

            Dim thisQuery As String = "SELECT CustomerLogins.*, Applications.Name AS AppName, Applications.Active AS AppActive, CustomerLoginRoles.Name AS RoleName, CustomerLoginRoles.Active AS RoleActive, CustomerLoginLevels.Name AS LevelName, CustomerLoginLevels.Active AS LevelActive FROM CustomerLogins INNER JOIN Applications ON CustomerLogins.ApplicationId = Applications.Id INNER JOIN CustomerLoginRoles ON CustomerLogins.RoleId = CustomerLoginRoles.Id INNER JOIN CustomerLoginLevels ON CustomerLogins.LevelId = CustomerLoginLevels.Id INNER JOIN Customers ON CustomerLogins.CustomerId = Customers.Id WHERE CustomerLogins.UserName = '" + txtUserLogin.Text + "'"

            Dim myData As DataSet = settingCfg.GetListData(thisQuery)

            If myData.Tables(0).Rows.Count = 0 Then
                MessageError(True, "USERNAME NOT FOUND !")
                Exit Sub
            End If

            If myData.Tables(0).Rows.Count > 1 Then
                MessageError(True, "USERNAME NOT FOUND !")
                Exit Sub
            End If

            Dim loginId As String = myData.Tables(0).Rows(0).Item("Id").ToString()
            Dim userName As String = myData.Tables(0).Rows(0).Item("UserName").ToString()
            Dim password As String = myData.Tables(0).Rows(0).Item("Password").ToString()
            Dim failedCount As Integer = myData.Tables(0).Rows(0).Item("FailedCount")
            Dim customerActive As Boolean = myData.Tables(0).Rows(0).Item("Active")
            Dim customerId As String = myData.Tables(0).Rows(0).Item("CustomerId").ToString()

            Dim appId As String = myData.Tables(0).Rows(0).Item("ApplicationId").ToString()
            Dim appActive As Boolean = myData.Tables(0).Rows(0).Item("AppActive")

            Dim roleActive As Boolean = myData.Tables(0).Rows(0).Item("RoleActive")
            Dim levelActive As Boolean = myData.Tables(0).Rows(0).Item("LevelActive")

            If settingCfg.Encrypt(txtPassword.Text) <> password Then
                MessageError(True, "YOUR PASSWORD IS WRONG !")
                Exit Sub
            End If

            If appActive = False Then
                Response.Redirect("~/system/maintenance", False)
                Exit Sub
            End If

            If customerActive = False Then
                MessageError(True, "YOUR ACCOUNT (LOGIN) IS BEING BLOCKED !")
                Exit Sub
            End If

            If msgError.InnerText = "" Then
                settingCfg.UpdateSession(lblDeviceId.Text, loginId)
                Session.Add("IsLoggedIn", True)
                Session.Add("LoginId", UCase(loginId).ToString())
                Session.Add("ApplicationId", UCase(appId).ToString())
                Session.Add("UserName", userName)

                Response.Redirect("~/", False)
            End If
        Catch ex As Exception
            MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
        End Try
    End Sub

    Private Sub BackColor()
        MessageError(False, String.Empty)
        txtUserLogin.BackColor = Drawing.Color.Empty
        txtPassword.BackColor = Drawing.Color.Empty
    End Sub

    Private Sub MessageError(Show As Boolean, Msg As String)
        divError.Visible = Show : msgError.InnerText = Msg
    End Sub

    Private Sub CheckSessionStates()
        If Request.Cookies("deviceId") IsNot Nothing Then
            lblDeviceId.Text = Request.Cookies("deviceId").Value
            Dim checkSession As Integer = settingCfg.GetItemData_Integer("SELECT COUNT(*) FROM Sessions WHERE Id = '" + UCase(lblDeviceId.Text).ToString() + "'")
            If checkSession = 1 Then
                Dim loginId As String = settingCfg.GetItemData("SELECT LoginId FROM Sessions WHERE Id = '" + UCase(lblDeviceId.Text).ToString() + "'")
                If Not loginId = "" Then
                    Dim appId As String = settingCfg.GetItemData("SELECT ApplicationId FROM CustomerLogins WHERE Id = '" + UCase(loginId).ToString() + "'")
                    Dim userName As String = settingCfg.GetItemData("SELECT UserName FROM CustomerLogins WHERE Id = '" + UCase(loginId).ToString() + "'")

                    Session.Add("IsLoggedIn", True)
                    Session.Add("LoginId", UCase(loginId).ToString())
                    Session.Add("ApplicationId", UCase(appId).ToString())
                    Session.Add("UserName", userName)

                    Response.Redirect("~/", False)
                    Exit Sub
                Else
                    lblDeviceId.Text = settingCfg.InsertSession()
                    Dim deviceCookie As New HttpCookie("deviceId", UCase(lblDeviceId.Text).ToString())
                    deviceCookie.Expires = DateTime.Now.AddMonths(1)
                    Response.Cookies.Add(deviceCookie)
                    Exit Sub
                End If
            Else
                lblDeviceId.Text = settingCfg.InsertSession()
                Dim deviceCookie As New HttpCookie("deviceId", UCase(lblDeviceId.Text).ToString())
                deviceCookie.Expires = DateTime.Now.AddMonths(1)
                Response.Cookies.Add(deviceCookie)
                Exit Sub
            End If
        Else
            lblDeviceId.Text = settingCfg.InsertSession()
            Dim deviceCookie As New HttpCookie("deviceId", UCase(lblDeviceId.Text).ToString())
            deviceCookie.Expires = DateTime.Now.AddMonths(1)
            Response.Cookies.Add(deviceCookie)
            Exit Sub
        End If
    End Sub
End Class

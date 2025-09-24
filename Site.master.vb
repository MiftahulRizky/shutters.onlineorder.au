Imports System.Data
Imports System.Data.SqlClient

Public Partial Class SiteMaster
    Inherits MasterPage

    Dim settingCfg As New SettingConfig
    Dim mailCfg As New MailConfig

    Dim myConn As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Protected Sub Page_Init(sender As Object, e As EventArgs)
        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    Protected Sub master_Page_PreLoad(sender As Object, e As EventArgs)
        CheckSessions(Session("IsLoggedIn"))
        MyLoad()
        BindListNavigation()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        CheckSessions(Session("IsLoggedIn"))
        BindActiveNavigasi()
    End Sub

    Private Sub MyLoad()
        Try
            If Session("isLoggedIn") = True Then
                Dim loginId As String = Session("LoginId")

                Dim thisQuery As String = "SELECT CustomerLogins.*, Applications.Name AS AppName, Applications.Active AS AppActive, Customers.Account AS CustomerAccount, CustomerLoginRoles.Name AS RoleName, CustomerLoginRoles.Active AS RoleActive, CustomerLoginLevels.Name AS LevelName, CustomerLoginLevels.Active AS LevelActive FROM CustomerLogins INNER JOIN Applications ON CustomerLogins.ApplicationId = Applications.Id INNER JOIN CustomerLoginRoles ON CustomerLogins.RoleId = CustomerLoginRoles.Id INNER JOIN CustomerLoginLevels ON CustomerLogins.LevelId = CustomerLoginLevels.Id INNER JOIN Customers ON CustomerLogins.CustomerId = Customers.Id WHERE CustomerLogins.Id = '" + loginId + "'"

                Dim myData As DataSet = settingCfg.GetListData(thisQuery)

                Session("FullName") = myData.Tables(0).Rows(0).Item("FullName").ToString()
                Session("AppName") = myData.Tables(0).Rows(0).Item("AppName").ToString()
                Session("RoleId") = myData.Tables(0).Rows(0).Item("RoleId").ToString()
                Session("RoleName") = myData.Tables(0).Rows(0).Item("RoleName").ToString()
                Session("LevelId") = myData.Tables(0).Rows(0).Item("LevelId").ToString()
                Session("LevelName") = myData.Tables(0).Rows(0).Item("LevelName").ToString()
                Session("CustomerId") = myData.Tables(0).Rows(0).Item("CustomerId").ToString()
                Session("resetLogin") = myData.Tables(0).Rows(0).Item("Reset")
                Session("CustomerAccount") = myData.Tables(0).Rows(0).Item("CustomerAccount").ToString()

                Dim appActive As Boolean = myData.Tables(0).Rows(0).Item("AppActive")
                Dim customerActive As Boolean = myData.Tables(0).Rows(0).Item("Active")
                Dim roleActive As Boolean = myData.Tables(0).Rows(0).Item("RoleActive")
                Dim levelActive As Boolean = myData.Tables(0).Rows(0).Item("LevelActive")
                Dim resetLogin As Boolean = myData.Tables(0).Rows(0).Item("Reset")

                If resetLogin = True AndAlso Not Request.Url.AbsolutePath.ToLower().EndsWith("/account/password") Then
                    Response.Redirect("~/account/password", False)
                    Exit Sub
                End If

                If appActive = False Then
                    Response.Redirect("~/system/maintenance", False)
                    Exit Sub
                End If

                Using thisConn As New SqlConnection(myConn)
                    Using myCmd As SqlCommand = New SqlCommand("UPDATE CustomerLogins SET LastLogin=GETDATE() WHERE Id=@Id")
                        myCmd.Parameters.AddWithValue("@Id", loginId)

                        myCmd.Connection = thisConn
                        thisConn.Open()
                        myCmd.ExecuteNonQuery()
                        thisConn.Close()
                    End Using
                End Using
            End If
        Catch ex As Exception
            mailCfg.MailError(Page.Title, "MyLoad", Session("LoginId"), ex.ToString())
            Session.Clear()
            Response.Redirect("~/account/login", False)
        End Try
    End Sub

    Private Sub BindListNavigation()
        Try
            liOrder.Visible = False
            liShipment.Visible = False

            liExport.Visible = False
            aExportBoe.Visible = False
            aExportLS.Visible = False
            aExportSP.Visible = False

            liReport.Visible = False
            liSettingCustomer.Visible = False
            liSales.Visible = False

            liSettingAdmin.Visible = False
            divSystem.Visible = False
            aSession.Visible = False
            divAccess.Visible = False
            aCustomer.Visible = False
            aCustomerGroup.Visible = False
            aCustomerLogin.Visible = False
            aCustomerPriceGroup.Visible = False
            divDividerCustomer.Visible = False
            divDividerCustomerDisc.Visible = False
            aCustomerDiscount.Visible = False
            divCustomerAdmin.Visible = False

            divProduct.Visible = False

            divJob.Visible = False

            divPrice.Visible = False

            divLog.Visible = False
            divOther.Visible = False

            spanOrder.InnerText = "View Order"

            If Session("RoleName") = "Administrator" Then
                liOrder.Visible = True
                liShipment.Visible = True

                liExport.Visible = True
                aExportBoe.Visible = True
                aExportLS.Visible = True
                aExportSP.Visible = True

                liSales.Visible = True
                liReport.Visible = True
                liSettingAdmin.Visible = True

                divSystem.Visible = True
                divAccess.Visible = True

                divCustomerAdmin.Visible = True
                divProduct.Visible = True
                divPrice.Visible = True

                If Session("LevelName") = "Leader" Then
                    divJob.Visible = True
                    divLog.Visible = True
                    divOther.Visible = True
                    aSession.Visible = True
                End If
            End If

            If Session("RoleName") = "Customer Service" Then
                liOrder.Visible = True
                liShipment.Visible = True
                liReport.Visible = True
                liSales.Visible = True

                liSettingAdmin.Visible = True

                aCustomer.Visible = True
                aCustomerGroup.Visible = True
                aCustomerPriceGroup.Visible = True
                divDividerCustomer.Visible = True
                divDividerCustomerDisc.Visible = True
                aCustomerDiscount.Visible = True
            End If

            If Session("RoleName") = "Account" Then
                liOrder.Visible = True
                liSales.Visible = True
                liReport.Visible = True
            End If

            If Session("RoleName") = "Representative" Then
                liOrder.Visible = True

                liSettingAdmin.Visible = True

                aCustomer.Visible = True
                divDividerCustomerDisc.Visible = True
                aCustomerDiscount.Visible = True
            End If

            If Session("RoleName") = "Data Entry" Then
                liOrder.Visible = True
                liReport.Visible = True
            End If

            If Session("RoleName") = "Customer" Then
                spanOrder.InnerText = "Your Order"

                liOrder.Visible = True
                liSettingCustomer.Visible = True
            End If
        Catch ex As Exception
            mailCfg.MailError(Page.Title, "BindListNavigation", Session("LoginId"), ex.ToString())
            Session.Clear()
            Response.Redirect("~/account/login", False)
        End Try
    End Sub

    Private Sub BindActiveNavigasi()
        Try
            If Page.Title = "Home Page" Then
                liHome.Attributes.Add("class", "nav-item active")
            End If

            If Page.Title = "List Order" Or Page.Title = "Create Order" Or Page.Title = "Edit Order" Or Page.Title = "Detail Order" Or Page.Title = "Change Status" Or Page.Title = "Maintenance" Then
                liOrder.Attributes.Add("class", "nav-item active")
            End If

            If Page.Title = "Additional" Or Page.Title = "Aluminium Blind" Or Page.Title = "Cellular Shades" Or Page.Title = "Curtain" Or Page.Title = "Privacy Venetian" Or Page.Title = "Evolve Shutters" Or Page.Title = "Evolve Parts" Or Page.Title = "Panel Glide" Or Page.Title = "Panorama PVC Parts" Or Page.Title = "Panorama PVC Shutters" Or Page.Title = "Pelmet" Or Page.Title = "Roller Blind" Or Page.Title = "Roman Blind" Or Page.Title = "Skin Only" Or Page.Title = "Venetian Blind" Or Page.Title = "Veri Shades" Or Page.Title = "Vertical" Or Page.Title = "Zebra Blind" Then
                liOrder.Attributes.Add("class", "nav-item active")
            End If

            If Page.Title = "Shipment" Or Page.Title = "Add Shipment" Or Page.Title = "Detail Shipment" Then
                liShipment.Attributes.Add("class", "nav-item active")
            End If

            If Page.Title = "Export" Or Page.Title = "Export BOE" Or Page.Title = "Export SP" Or Page.Title = "Export LS" Then
                liExport.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "Sales LS" Or Page.Title = "Sales BIG" Or Page.Title = "Sales Budget" Then
                liSales.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "Report" Then
                liReport.Attributes.Add("class", "nav-item dropdown active")
            End If

            ' SETTING
            If Page.Title = "Logo" Or Page.Title = "Terms & Conditions" Then
                liSettingCustomer.Attributes.Add("class", "nav-item active")
            End If

            If Page.Title = "Setting" Or Page.Title = "Application" Or Page.Title = "Mailing" Or Page.Title = "Session" Or Page.Title = "Micronet" Or Page.Title = "Exact SP" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "Login Role" Or Page.Title = "Login Level" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "Customer" Or Page.Title = "Add Customer" Or Page.Title = "Edit Customer" Or Page.Title = "Customer Detail" Or Page.Title = "Customer Login" Or Page.Title = "Customer Group" Or Page.Title = "Customer Group Discount" Or Page.Title = "Customer Group Discount Add" Or Page.Title = "Customer Price Group" Or Page.Title = "Customer Discount" Or Page.Title = "Custom Customer Discount" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            ' SETTING PRODUCT
            If Page.Title = "Design Type" Or Page.Title = "Blind Type" Or Page.Title = "Product" Or Page.Title = "Add Product" Or Page.Title = "Edit Product" Or Page.Title = "Product Detail" Or Page.Title = "Control Type" Or Page.Title = "Tube Type" Or Page.Title = "Colour Type" Or Page.Title = "Fabric" Or Page.Title = "Fabric Detail" Or Page.Title = "Chain / Remote" Or Page.Title = "Bottom Rail" Or Page.Title = "Mounting" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "Job Sheet" Or Page.Title = "Add Job Sheet" Or Page.Title = "Job Sheet Detail" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "List Newsletter" Or Page.Title = "Add Newsletter" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            ' SETTING PRICING
            If Page.Title = "Product Price Group" Or Page.Title = "Price Matrix" Or Page.Title = "Price Surcharge" Or Page.Title = "Add Price Surcharge" Or Page.Title = "Price Detail Surcharge" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "Surcharge" Or Page.Title = "Add Surcharge" Or Page.Title = "Surcharge Detail" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            ' SETTING OTHER
            If Page.Title = "Log Order" Or Page.Title = "Log System" Or Page.Title = "Log Access" Or Page.Title = "Log Customer" Or Page.Title = "Log Product" Or Page.Title = "Log Price" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If

            If Page.Title = "Query" Or Page.Title = "Test Email" Then
                liSettingAdmin.Attributes.Add("class", "nav-item dropdown active")
            End If
        Catch ex As Exception
            mailCfg.MailError(Page.Title, "BindActiveNavigasi", Session("LoginId"), ex.ToString())
            Session.Clear()
            Response.Redirect("~/account/login", False)
        End Try
    End Sub

    Protected Sub btnSearchAll_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/account/login", False)
    End Sub

    Protected Sub linkLogout_Click(sender As Object, e As EventArgs)
        Dim sessionId As String = String.Empty

        If Request.Cookies("deviceId") IsNot Nothing Then
            sessionId = Request.Cookies("deviceId").Value
            settingCfg.DeleteSession(sessionId)
        End If

        Session.Clear()
        Response.Redirect("~/", False)
    End Sub

    Protected Sub linkRequestAccess_Click(sender As Object, e As EventArgs)
        Try

        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckSessions(IsLogged As Boolean)
        Try
            Dim sessionId As String = String.Empty
            If IsLogged = True Then
                If Request.Cookies("deviceId") IsNot Nothing Then
                    sessionId = Request.Cookies("deviceId").Value
                    Dim checkData As DataSet = settingCfg.GetListData("SELECT * FROM Sessions WHERE Id = '" + UCase(sessionId) + "' AND LoginId = '" + UCase(Session("LoginId")) + "'")
                    If checkData.Tables(0).Rows.Count = 0 Then
                        Response.Redirect("~/account/login", False)
                        Exit Sub
                    End If
                Else
                    Response.Redirect("~/account/login", False)
                    Exit Sub
                End If
            Else
                If Request.Cookies("deviceId") IsNot Nothing Then
                    sessionId = Request.Cookies("deviceId").Value
                    Dim loginId As String = settingCfg.GetItemData("SELECT LoginId FROM Sessions WHERE Id = '" + UCase(sessionId).ToString() + "'")
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
                        Response.Redirect("~/account/login", False)
                        Exit Sub
                    End If
                Else
                    Response.Redirect("~/account/login", False)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Response.Redirect("~/account/login", False)
            Exit Sub
        End Try
    End Sub
End Class

Imports System.Data
Imports System.Data.SqlClient

Partial Class Order_Default
    Inherits Page

    Dim orderCfg As New OrderConfig
    Dim mailCfg As New MailConfig

    Dim myConn As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MessageError(False, String.Empty)
            BindListStatus()

            ddlStatus.SelectedValue = Session("orderStatus")
            txtSearch.Text = Session("orderSearch")

            BindDataOrder(txtSearch.Text, ddlStatus.SelectedValue)
        End If
    End Sub

    Protected Sub btnSubmitDailyMail_Click(sender As Object, e As EventArgs)
        MessageError(False, String.Empty)
        Try
            mailCfg.MailProduction()
            Response.Redirect("~/order/", False)
        Catch ex As Exception
            MessageError(True, ex.ToString())
        End Try
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/order/add", False)
    End Sub

    Protected Sub ddlStatus_SelectedIndexChanged(sender As Object, e As EventArgs)
        BindDataOrder(txtSearch.Text, ddlStatus.SelectedValue)
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        BindDataOrder(txtSearch.Text, ddlStatus.SelectedValue)
    End Sub

    Protected Sub gvList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvList.PageIndex = e.NewPageIndex
        BindDataOrder(txtSearch.Text, ddlStatus.SelectedValue)
    End Sub

    Protected Sub gvList_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If Not String.IsNullOrEmpty(e.CommandArgument) Then
            If e.CommandName = "Detail" Then
                MessageError(False, String.Empty)
                Try
                    Dim headerId As String = e.CommandArgument.ToString()

                    Session("orderSearch") = txtSearch.Text
                    Session("orderStatus") = ddlStatus.SelectedValue
                    Session("headerId") = headerId

                    Response.Redirect("~/order/detail", False)
                Catch ex As Exception
                    MessageError(True, ex.ToString())
                    If Not Session("RoleName") = "Administrator" Then
                        MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                        If Session("RoleName") = "Customer" Then
                            MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                            'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                        End If
                        mailCfg.MailError(Page.Title, "linkView_Click", Session("LoginId"), ex.ToString())
                    End If
                End Try
            ElseIf e.CommandName = "Log" Then
                MessageError_Log(False, String.Empty)
                Dim thisScript As String = "window.onload = function() { showLog(); };"
                Try
                    Dim headerId As String = e.CommandArgument.ToString()

                    gvListLogs.DataSource = orderCfg.GetListData("SELECT * FROM Log_Orders WHERE HeaderId = '" + headerId + "' ORDER BY ActionDate ASC")
                    gvListLogs.DataBind()

                    ClientScript.RegisterStartupScript(Me.GetType(), "showLog", thisScript, True)
                Catch ex As Exception
                    MessageError_Log(True, ex.ToString())
                    If Not Session("RoleName") = "Administrator" Then
                        MessageError_Log(True, "Please contact IT Support at reza@bigblinds.co.id")
                        If Session("RoleName") = "Customer" Then
                            MessageError_Log(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                            'MessageError_Log(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                        End If
                        mailCfg.MailError(Page.Title, "linkLog_Click", Session("LoginId"), ex.ToString())
                    End If
                    ClientScript.RegisterStartupScript(Me.GetType(), "showLog", thisScript, True)
                End Try
            End If
        End If
    End Sub

    Protected Sub btnSubmitDelete_Click(sender As Object, e As EventArgs)
        MessageError(False, String.Empty)
        Try
            Dim thisId As String = txtIdDelete.Text

            Using thisConn As New SqlConnection(myConn)
                Using myCmd As SqlCommand = New SqlCommand("UPDATE OrderHeaders SET Active=0 WHERE Id=@Id")
                    myCmd.Parameters.AddWithValue("@Id", thisId)

                    myCmd.Connection = thisConn
                    thisConn.Open()
                    myCmd.ExecuteNonQuery()
                    thisConn.Close()
                End Using
            End Using

            Using thisConn As New SqlConnection(myConn)
                Using myCmd As SqlCommand = New SqlCommand("UPDATE OrderDetails SET Active=0 WHERE HeaderId=@Id")
                    myCmd.Parameters.AddWithValue("@Id", thisId)

                    myCmd.Connection = thisConn
                    thisConn.Open()
                    myCmd.ExecuteNonQuery()
                    thisConn.Close()
                End Using
            End Using

            Using thisConn As New SqlConnection(myConn)
                Using myCmd As SqlCommand = New SqlCommand("UPDATE OrderAuthorizations SET STATUS=NULL, LoginId=NULL, Active=0 WHERE HeaderId=@Id")
                    myCmd.Parameters.AddWithValue("@Id", thisId)

                    myCmd.Connection = thisConn
                    thisConn.Open()
                    myCmd.ExecuteNonQuery()
                    thisConn.Close()
                End Using
            End Using

            Dim dataLog As Object() = {thisId, "", Session("LoginId").ToString(), "Delete Order"}
            orderCfg.Log_Orders(dataLog)

            Session("orderStatus") = ddlStatus.SelectedValue
            Session("orderSearch") = txtSearch.Text

            Response.Redirect("~/order", False)
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "btnSubmitDelete_Click", Session("LoginId"), ex.ToString())
            End If
        End Try
    End Sub

    Protected Sub lvOtorisasi_ItemCommand(sender As Object, e As ListViewCommandEventArgs)
        MessageError(False, String.Empty)
        Try
            If e.CommandName = "View" Then
                Session("headerId") = e.CommandArgument
                Session("orderSearch") = txtSearch.Text
                Session("orderStatus") = ddlStatus.SelectedValue
                Response.Redirect("~/order/detail", False)
            End If
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Supoort at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "lvOtorisasi_ItemCommand", Session("LoginId"), ex.ToString())
            End If
        End Try
    End Sub

    Private Sub BindDataOrder(Search As String, Status As String)
        Try
            Dim byStatus As String = String.Empty
            Dim byText As String = String.Empty
            Dim byOrder As String = " ORDER BY OrderHeaders.Id DESC"
            Dim byRole As String = String.Empty

            If Not Search = "" Then
                byText = " AND (OrderHeaders.Id LIKE '%" + Search.Trim() + "%' OR OrderHeaders.OrderId LIKE '%" + Search.Trim() + "%' OR OrderHeaders.JobId LIKE '%" + Search.Trim() + "%' OR OrderHeaders.OrderNumber LIKE '%" + Search.Trim() + "%' OR OrderHeaders.OrderName LIKE '%" + Search.Trim() + "%' OR OrderHeaders.CustomerId LIKE '%" + Search.Trim() + "%' OR Customers.Name LIKE '%" + Search.Trim() + "%' OR Customers.MicronetId LIKE '%" + Search.Trim() + "%' OR OrderHeaders.OrderType LIKE '%" + Search.Trim() + "%')"
            End If

            If Session("RoleName") = "Administrator" Then
                byStatus = ""
                byOrder = " ORDER BY OrderHeaders.Id, CASE WHEN Status = 'New Order' THEN 1 WHEN Status = 'In Production' THEN 2 WHEN Status = 'Completed' THEN 3 WHEN Status = 'Unsubmitted' THEN 4 WHEN Status = 'Canceled' THEN 5 END DESC"
                If Not Status = "" Then
                    byStatus = " AND OrderHeaders.Status = '" + Status + "'"
                    byRole = ""
                End If
            End If

            If Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                byStatus = ""
                byOrder = " ORDER BY OrderHeaders.Id, CASE WHEN Status = 'New Order' THEN 1 WHEN Status = 'In Production' THEN 2 WHEN Status = 'Completed' THEN 3 WHEN Status = 'Unsubmitted' THEN 4 WHEN Status = 'Canceled' THEN 5 END DESC"
                If Not Status = "" Then
                    byStatus = " AND OrderHeaders.Status = '" + Status + "'"
                    byRole = ""
                End If
            End If

            If Session("RoleName") = "Account" Then
                byStatus = ""
                byOrder = " ORDER BY OrderHeaders.Id, CASE WHEN Status = 'New Order' THEN 1 WHEN Status = 'In Production' THEN 2 WHEN Status = 'Completed' THEN 3 END DESC"
                If Not Status = "" Then
                    byStatus = " AND OrderHeaders.Status = '" + Status + "'"
                    byRole = ""
                End If
            End If

            If Session("RoleName") = "Representative" Then
                byStatus = ""
                byOrder = " ORDER BY OrderHeaders.Id DESC"
                byRole = " AND OrderHeaders.CustomerId = '" + UCase(Session("CustomerId")).ToString() + "' AND OrderHeaders.CreatedBy = '" + UCase(Session("LoginId")).ToString() + "'"

                If Not Status = "" Then
                    byStatus = " AND OrderHeaders.Status = '" + Status + "'"
                End If
            End If

            If Session("RoleName") = "Customer" Then
                byStatus = ""
                byOrder = " ORDER BY OrderHeaders.Id DESC"
                byRole = " AND OrderHeaders.CustomerId = '" + UCase(Session("CustomerId")).ToString() + "'"
                If Session("CustomerAccount") = "Master" Then
                    byRole = " AND OrderHeaders.CustomerId = '" + UCase(Session("CustomerId")).ToString() + "' AND Customers.MasterId = '" + Session("CustomerId") + "'"
                End If
                If Not Status = "" Then
                    byStatus = " AND OrderHeaders.Status = '" + Status + "'"
                End If
            End If

            Dim thisQuery As String = String.Format("SELECT OrderHeaders.*, Customers.Name AS CustomerName FROM OrderHeaders LEFT JOIN Customers ON OrderHeaders.CustomerId = Customers.Id WHERE OrderHeaders.Active = 1 {0} {1} {2} {3}", byText, byRole, byStatus, byOrder)

            gvList.DataSource = orderCfg.GetListData(thisQuery)
            gvList.DataBind()

            gvList.Columns(1).Visible = False ' ID
            gvList.Columns(3).Visible = False ' CUSTOMER NAME
            If Session("RoleName") = "Administrator" And Session("LevelName") = "Leader" Then
                gvList.Columns(1).Visible = True ' ID
            End If
            If Session("RoleName") = "Administrator" Or Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                gvList.Columns(3).Visible = True ' CUSTOMER NAME
            End If

            If Session("RoleName") = "Customer" And (Session("CustomerId") = "LS-A012" Or Session("CustomerId") = "LS-A333") Then
                gvList.Columns(3).Visible = True ' CUSTOMER NAME
            End If

            gvList.Columns(6).Visible = True
            If Session("RoleName") = "Customer" Or Session("RoleName") = "Representative" Or Session("Sunlight Product") Then
                gvList.Columns(6).Visible = False
            End If

            BindOtorisasi()

            aDailyMail.Visible = False
            aOtorisasi.Visible = False

            If Session("RoleName") = "Administrator" And Session("LevelName") = "Leader" Then
                aDailyMail.Visible = True
            End If
            If Session("RoleName") = "Administrator" Or Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                aOtorisasi.Visible = True
            End If

            btnAdd.Visible = False
            If Session("RoleName") = "Administrator" Or Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Or Session("RoleName") = "Representative" Then
                btnAdd.Visible = True
            End If
            If Session("RoleName") = "Customer" Then
                Dim thisStatus As Boolean = orderCfg.GetItemData_Boolean("SELECT OnStop FROM Customers WHERE Id='" + Session("CustomerId") + "'")
                If thisStatus = False Then : btnAdd.Visible = True : End If
            End If
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "BindDataOrder", Session("LoginId"), ex.ToString())
            End If
        End Try
    End Sub

    Private Sub BindListStatus()
        ddlStatus.Items.Clear()
        Try
            ddlStatus.Items.Add(New ListItem("ALL ORDER", ""))
            ddlStatus.Items.Add(New ListItem("UNSUBMITTED", "Unsubmitted"))
            ddlStatus.Items.Add(New ListItem("NEW ORDER", "New Order"))
            ddlStatus.Items.Add(New ListItem("IN PRODUCTION", "In Production"))
            ddlStatus.Items.Add(New ListItem("COMPLETED", "Completed"))
            ddlStatus.Items.Add(New ListItem("CANCELED", "Canceled"))

            If Session("RoleName") = "Sunlight Product" Then
                ddlStatus.Items.Clear()
                ddlStatus.Items.Add(New ListItem("ALL ORDER", ""))
                ddlStatus.Items.Add(New ListItem("NEW ORDER", "New Order"))
                ddlStatus.Items.Add(New ListItem("IN PRODUCTION", "In Production"))
                ddlStatus.Items.Add(New ListItem("COMPLETED", "Completed"))
            End If

            If Session("RoleName") = "Account" Then
                ddlStatus.Items.Clear()
                ddlStatus.Items.Add(New ListItem("ALL ORDER", ""))
                ddlStatus.Items.Add(New ListItem("NEW ORDER", "New Order"))
                ddlStatus.Items.Add(New ListItem("IN PRODUCTION", "In Production"))
                ddlStatus.Items.Add(New ListItem("COMPLETED", "Completed"))
            End If

            If Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                ddlStatus.Items.Clear()
                ddlStatus.Items.Add(New ListItem("ALL ORDER", ""))
                ddlStatus.Items.Add(New ListItem("UNSUBMITTED", "Unsubmitted"))
                ddlStatus.Items.Add(New ListItem("NEW ORDER", "New Order"))
                ddlStatus.Items.Add(New ListItem("IN PRODUCTION", "In Production"))
                ddlStatus.Items.Add(New ListItem("COMPLETED", "Completed"))
                ddlStatus.Items.Add(New ListItem("CANCELED", "Canceled"))
            End If
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "BindListStatus", Session("LoginId"), ex.ToString())
            End If
        End Try
    End Sub

    Private Sub BindOtorisasi()
        Try
            Dim otorisasiQuery As String = "SELECT OrderAuthorizations.*, CONVERT(VARCHAR, OrderHeaders.OrderId) + ' (Item# ' + CONVERT(VARCHAR, OrderDetails.Number) + ')' AS JobOtorisasi FROM OrderAuthorizations INNER JOIN OrderHeaders ON OrderAuthorizations.HeaderId = OrderHeaders.Id LEFT JOIN OrderDetails ON OrderAuthorizations.ItemId = OrderDetails.Id WHERE OrderHeaders.Status = 'New Order' AND OrderAuthorizations.Active = 1 AND (OrderAuthorizations.Status = '' OR OrderAuthorizations.Status IS NULL) ORDER BY OrderAuthorizations.HeaderId, OrderAuthorizations.ItemId ASC"

            lvOtorisasi.DataSource = orderCfg.GetListData(otorisasiQuery)
            lvOtorisasi.DataBind()

            spanOtorisasi.InnerText = orderCfg.GetItemData_Integer("SELECT COUNT(*) FROM OrderAuthorizations LEFT JOIN OrderHeaders ON OrderAuthorizations.HeaderId = OrderHeaders.Id WHERE OrderHeaders.Status = 'New Order' AND OrderAuthorizations.Active = 1 AND (OrderAuthorizations.Status IS NULL OR OrderAuthorizations.Status = '')")
        Catch ex As Exception
            mailCfg.MailError(Page.Title, "BindOtorisasi", Session("LoginId"), ex.ToString())
        End Try
    End Sub

    Private Sub MessageError(Show As Boolean, Msg As String)
        divError.Visible = Show : msgError.InnerText = Msg
    End Sub

    Private Sub MessageError_Log(Show As Boolean, Msg As String)
        divErrorLog.Visible = Show : msgErrorLog.InnerText = Msg
    End Sub

    Protected Function VisibleDelete(CreatedBy As String, Status As String) As Boolean
        Dim result As Boolean = False
        If Status = "Unsubmitted" Then
            If Session("RoleName") = "Administrator" Then
                Return True
            End If
            If Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                If Session("LoginId") = UCase(CreatedBy).ToString() Then
                    Return True
                End If
                Return False
            End If
            If Session("RoleName") = "Customer" Or Session("RoleName") = "Representative" Then
                Return True
            End If
        End If
        Return False
    End Function

    Protected Function BindTextLog(Id As String) As String
        Dim result As String = String.Empty
        Try
            Dim thisData As DataSet = orderCfg.GetListData("SELECT * FROM Log_Orders WHERE Id = '" + Id + "'")
            Dim actionBy As String = thisData.Tables(0).Rows(0).Item("ActionBy").ToString()
            Dim actionDate As String = Convert.ToDateTime(thisData.Tables(0).Rows(0).Item("ActionDate")).ToString("dd MMM yyyy hh:mm")
            Dim description As String = thisData.Tables(0).Rows(0).Item("Description").ToString()

            Dim fullName As String = orderCfg.GetItemData("SELECT FullName FROM CustomerLogins WHERE Id = '" + UCase(actionBy).ToString() + "'")

            result = "<b>" & fullName & "</b> on " & actionDate & ". Action: " & description
        Catch ex As Exception
            result = ""
        End Try
        Return result
    End Function
End Class

Imports System.Data.SqlClient

Partial Class Order_Add
    Inherits Page

    Dim orderCfg As New OrderConfig
    Dim mailCfg As New MailConfig

    Dim myConn As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim status As Boolean = orderCfg.GetItemData_Boolean("SELECT OnStop FROM Customers WHERE Id='" + Session("CustomerId") + "'")
        If status = True Then
            Response.Redirect("~/order/", False)
            Exit Sub
        End If

        If Session("RoleName") = "Sunlight Product" Then
            Response.Redirect("~/order", False)
            Exit Sub
        End If

        If Not IsPostBack Then
            BackColor()
            BindDataCustomer()
            BindDataUser()

            txtCreatedDate.Text = Now.ToString("yyyy-MM-dd")

            BindComponentForm()
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        BackColor()
        Try
            If ddlCustomer.SelectedValue = "" Then
                MessageError(True, "CUSTOMER NAME IS REQUIRED !")
                ddlCustomer.BackColor = Drawing.Color.Red
                ddlCustomer.Focus()
                Exit Sub
            End If

            If txtOrderNumber.Text = "" Then
                MessageError(True, "ORDER NUMBER IS REQUIRED !")
                txtOrderNumber.BackColor = Drawing.Color.Red
                txtOrderNumber.Focus()
                Exit Sub
            End If

            If Regex.IsMatch(txtOrderNumber.Text, "[\\/,&#'\.]") Then
                MessageError(True, "PLEASE DON'T USE [ \ ], [ / ], [ , ], [ & ], [ # ], [ ' ], AND [ . ]")
                txtOrderNumber.BackColor = Drawing.Color.Red
                txtOrderNumber.Focus()
                Exit Sub
            End If

            If Trim(txtOrderNumber.Text).Length > 20 Then
                MessageError(True, "MAXIMUM 20 CHARACTERS FOR RETAILER ORDER NUMBER !")
                txtOrderNumber.BackColor = Drawing.Color.Red
                txtOrderNumber.Focus()
                Exit Sub
            End If

            If txtOrderName.Text = "" Then
                MessageError(True, "CUSTOMER NAME IS REQUIRED !")
                txtOrderName.BackColor = Drawing.Color.Red
                txtOrderName.Focus()
                Exit Sub
            End If

            If Regex.IsMatch(txtOrderName.Text, "[\\/,&#'\.]") Then
                MessageError(True, "PLEASE DON'T USE [ \ ], [ / ], [ , ], [ & ], [ # ], [ ' ], AND [ . ]")
                txtOrderName.BackColor = Drawing.Color.Red
                txtOrderName.Focus()
                Exit Sub
            End If

            If txtOrderNumber.Text = orderCfg.GetItemData("SELECT OrderNumber FROM OrderHeaders WHERE OrderNumber = '" + txtOrderNumber.Text + "' AND CustomerId = '" + ddlCustomer.SelectedValue + "' AND Active=1") Then
                MessageError(True, "RETAILER ORDER NUMBER ALREADY EXISTS !")
                txtOrderNumber.BackColor = Drawing.Color.Red
                txtOrderNumber.Focus()
                Exit Sub
            End If

            If msgError.InnerText = "" Then
                Dim headerId As String = orderCfg.CreateOrderHeaderId()
                Dim orderId As String = "SPP-" & headerId
                Dim createdBy As String = UCase(ddlCreatedBy.SelectedValue).ToString()
                If createdBy = "" Then
                    createdBy = UCase(Session("LoginId")).ToString()
                End If

                Using thisConn As New SqlConnection(myConn)
                    Using myCmd As SqlCommand = New SqlCommand("INSERT INTO OrderHeaders(Id, OrderId, CustomerId, OrderNumber, OrderName, OrderNote, Status, CreatedBy, CreatedDate, Deposit, Approved, Active) VALUES (@Id, @OrderId, @CustomerId, @OrderNumber, @OrderName, @OrderNote, 'Unsubmitted', @CreatedBy, @CreatedDate, 0, 0, 1) INSERT INTO OrderQuotes VALUES (@Id, '', '', '', '', '', '', 0.00, 0.00, 0.00, 0.00)")
                        myCmd.Parameters.AddWithValue("@Id", headerId)
                        myCmd.Parameters.AddWithValue("@OrderId", orderId)
                        myCmd.Parameters.AddWithValue("@CustomerId", ddlCustomer.SelectedValue)
                        myCmd.Parameters.AddWithValue("@OrderNumber", txtOrderNumber.Text.Trim())
                        myCmd.Parameters.AddWithValue("@OrderName", txtOrderName.Text.Trim())
                        myCmd.Parameters.AddWithValue("@OrderNote", txtNote.Text.Trim())
                        myCmd.Parameters.AddWithValue("@CreatedBy", createdBy)
                        myCmd.Parameters.AddWithValue("@CreatedDate", txtCreatedDate.Text)

                        myCmd.Connection = thisConn
                        thisConn.Open()
                        myCmd.ExecuteNonQuery()
                        thisConn.Close()
                    End Using
                End Using

                Dim dataLog As Object() = {headerId, "", Session("LoginId").ToString(), "Create Order"}
                orderCfg.Log_Orders(dataLog)

                Session("headerId") = headerId

                Response.Redirect("~/order/detail", False)
            End If
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "btnSubmit_Click", Session("LoginId"), ex.ToString())
            End If
        End Try
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/order/", False)
    End Sub

    Private Sub BindComponentForm()
        Try
            divCustomer.Visible = False
            divCreatedBy.Visible = False
            divCreatedDate.Visible = False
            If Session("RoleName") = "Administrator" Or Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                divCustomer.Visible = True
            End If

            If Session("RoleName") = "Administrator" Then
                divCreatedBy.Visible = True
                divCreatedDate.Visible = True
            End If
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "BindComponentForm", Session("LoginId"), ex.ToString())
            End If
        End Try
    End Sub

    Private Sub BindDataCustomer()
        ddlCustomer.Items.Clear()
        Try
            Dim thisQuery As String = "SELECT *, UPPER(Name) AS NameText FROM Customers WHERE Active=1 ORDER BY Name ASC"
            If Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                thisQuery = "SELECT *, UPPER(Name) AS NameText FROM Customers WHERE Id <> 'DEFAULT' AND Active=1 ORDER BY Name ASC"
            End If
            ddlCustomer.DataSource = orderCfg.GetListData(thisQuery)
            ddlCustomer.DataTextField = "NameText"
            ddlCustomer.DataValueField = "Id"
            ddlCustomer.DataBind()

            If ddlCustomer.Items.Count > 1 Then
                ddlCustomer.Items.Insert(0, New ListItem("", ""))
            End If

            If Session("RoleName") = "Customer" Or Session("RoleName") = "Representative" Then
                ddlCustomer.SelectedValue = Session("CustomerId").ToString()
                If ddlCustomer.SelectedValue = "" Then
                    Response.Redirect("~/order/", False)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ddlCustomer.Items.Clear()
            mailCfg.MailError(Page.Title, "BindDataCustomer", Session("LoginId"), ex.ToString())
        End Try
    End Sub

    Private Sub BindDataUser()
        ddlCreatedBy.Items.Clear()
        Try
            ddlCreatedBy.DataSource = orderCfg.GetListData("SELECT UPPER(Id) AS IdText, UPPER(UserName) + ' | ' + UPPER(FullName) AS NameText FROM CustomerLogins ORDER BY UserName ASC")
            ddlCreatedBy.DataTextField = "NameText"
            ddlCreatedBy.DataValueField = "IdText"
            ddlCreatedBy.DataBind()

            If ddlCreatedBy.Items.Count > 0 Then
                ddlCreatedBy.Items.Insert(0, New ListItem("", ""))
            End If

            ddlCreatedBy.SelectedValue = Session("LoginId")
        Catch ex As Exception
            ddlCreatedBy.Items.Clear()
            mailCfg.MailError(Page.Title, "BindDataLogin", Session("LoginId"), ex.ToString())
        End Try
    End Sub

    Private Sub BackColor()
        MessageError(False, String.Empty)

        ddlCustomer.BackColor = Drawing.Color.Empty
        txtOrderNumber.BackColor = Drawing.Color.Empty
        txtOrderName.BackColor = Drawing.Color.Empty
        txtNote.BackColor = Drawing.Color.Empty
    End Sub

    Private Sub MessageError(Show As Boolean, Msg As String)
        divError.Visible = Show : msgError.InnerText = Msg
    End Sub
End Class

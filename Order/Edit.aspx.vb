Imports System.Data
Imports System.Data.SqlClient

Partial Class Order_Edit
    Inherits Page

    Dim orderCfg As New OrderConfig
    Dim mailCfg As New MailConfig
    Dim myConn As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("RoleName") = "Sunlight Product" Then
            Response.Redirect("~/order", False)
            Exit Sub
        End If

        If Not Session("headerAction") = "EditHeader" Then
            Response.Redirect("~/order/detail", False)
            Exit Sub
        End If

        lblHeaderId.Text = Session("headerId")
        If Not IsPostBack Then
            BackColor()
            BindDataHeader(lblHeaderId.Text)
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        BackColor()
        Try
            If ddlCustomer.SelectedValue = "" Then
                MessageError(True, "CUSTOMER ACCOUNT IS REQUIRED !")
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
                MessageError(True, "MAXIMUM 20 CHARACTERS FOR RETAILER ORDER NO !")
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

            If Not txtOrderNumber.Text = lblOrderNo.Text Then
                If txtOrderNumber.Text = orderCfg.GetItemData("SELECT OrderNumber FROM OrderHeaders WHERE OrderNumber = '" + txtOrderNumber.Text + "' AND CustomerId = '" + ddlCustomer.SelectedValue + "' AND Active=1") Then
                    MessageError(True, "SORRY. YOU CAN NOT USE THIS RETAILER ORDER NO !")
                    txtOrderNumber.BackColor = Drawing.Color.Red
                    txtOrderNumber.Focus()
                    Exit Sub
                End If
            End If

            If msgError.InnerText = "" Then
                Dim createdBy As String = UCase(ddlCreatedBy.SelectedValue).ToString()

                Using thisConn As New SqlConnection(myConn)
                    Using myCmd As SqlCommand = New SqlCommand("UPDATE OrderHeaders SET OrderId=@OrderId, JobId=@JobId, JobDate=@JobDate, ShipmentId=@ShipmentId, CustomerId=@CustomerId, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate, OrderNumber=@OrderNumber, OrderName=@OrderName, OrderNote=@OrderNote WHERE Id=@Id")
                        myCmd.Parameters.AddWithValue("@Id", lblHeaderId.Text)
                        myCmd.Parameters.AddWithValue("@OrderId", txtOrderId.Text.Trim())
                        myCmd.Parameters.AddWithValue("@JobId", txtJobId.Text.Trim())
                        myCmd.Parameters.AddWithValue("@JobDate", txtJobDate.Text)
                        myCmd.Parameters.AddWithValue("@ShipmentId", If(String.IsNullOrEmpty(ddlShipmentId.SelectedValue), DBNull.Value, ddlShipmentId.SelectedValue))
                        myCmd.Parameters.AddWithValue("@CustomerId", ddlCustomer.SelectedValue)
                        myCmd.Parameters.AddWithValue("@CreatedBy", createdBy)
                        myCmd.Parameters.AddWithValue("@CreatedDate", txtCreatedDate.Text)
                        myCmd.Parameters.AddWithValue("@OrderNumber", txtOrderNumber.Text.Trim())
                        myCmd.Parameters.AddWithValue("@OrderName", txtOrderName.Text.Trim())
                        myCmd.Parameters.AddWithValue("@OrderNote", txtNote.Text.Trim())

                        myCmd.Connection = thisConn
                        thisConn.Open()
                        myCmd.ExecuteNonQuery()
                        thisConn.Close()
                    End Using
                End Using

                Dim dataLog As Object() = {lblHeaderId.Text, "", Session("LoginId").ToString(), "Edit Order"}
                orderCfg.Log_Orders(dataLog)

                Response.Redirect("~/order/detail", False)
                Exit Sub
            End If
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "Submit", Session("LoginId"), ex.ToString())
            End If
            Exit Sub
        End Try
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/order/detail", False)
    End Sub

    Protected Sub btnSubmitAddress_Click(sender As Object, e As EventArgs)
        BackColor()
        Dim thisScript As String = "window.onload = function() { showAddress(); };"
        Try
            If txtAddressStreet.Text = "" Then
                MessageError_Address(True, "STREET ADDRESS IS REQUIRED !")
                ClientScript.RegisterStartupScript(Me.GetType(), "showAddress", thisScript, True)
                Exit Sub
            End If

            If txtAddressSuburb.Text = "" Then
                MessageError_Address(True, "SUBURB IS REQUIRED !")
                ClientScript.RegisterStartupScript(Me.GetType(), "showAddress", thisScript, True)
                Exit Sub
            End If

            If ddlAddressStates.SelectedValue = "" Then
                MessageError_Address(True, "STATES IS REQUIRED !")
                ClientScript.RegisterStartupScript(Me.GetType(), "showAddress", thisScript, True)
                Exit Sub
            End If

            If txtAddressPostCode.Text = "" Then
                MessageError_Address(True, "POST CODE IS REQUIRED !")
                ClientScript.RegisterStartupScript(Me.GetType(), "showAddress", thisScript, True)
                Exit Sub
            End If

            If ddlAddressPort.SelectedValue = "" Then
                MessageError_Address(True, "NEAREST PORT IS REQUIRED !")
                ClientScript.RegisterStartupScript(Me.GetType(), "showAddress", thisScript, True)
                Exit Sub
            End If

            If msgErrorAddress.InnerText = "" Then
                If lblActionAddress.Text = "Add" Then
                    lblCustomerAddressId.Text = orderCfg.GetCustomerAddressId()

                    Using thisConn As New SqlConnection(myConn)
                        Using myCmd As SqlCommand = New SqlCommand("UPDATE CustomerAddress SET [Primary]=0 WHERE CustomerId = @CustomerId; INSERT INTO CustomerAddress VALUES (NEWID(), @CustomerId, 'Delivery', @UnitNumber, @Street, @Suburb, @States, @PostCode, @Port, 'Delivery', NULL, 1)")
                            myCmd.Parameters.AddWithValue("@Id", lblCustomerAddressId.Text)
                            myCmd.Parameters.AddWithValue("@CustomerId", ddlCustomer.SelectedValue)
                            myCmd.Parameters.AddWithValue("@UnitNumber", txtAddressUnitNumber.Text.Trim())
                            myCmd.Parameters.AddWithValue("@Street", txtAddressStreet.Text.Trim())
                            myCmd.Parameters.AddWithValue("@Suburb", txtAddressSuburb.Text.Trim())
                            myCmd.Parameters.AddWithValue("@States", ddlAddressStates.SelectedValue)
                            myCmd.Parameters.AddWithValue("@PostCode", txtAddressPostCode.Text.Trim())
                            myCmd.Parameters.AddWithValue("@Port", ddlAddressPort.SelectedValue)

                            myCmd.Connection = thisConn
                            thisConn.Open()
                            myCmd.ExecuteNonQuery()
                            thisConn.Close()
                        End Using
                    End Using

                    Response.Redirect("~/order/edit", False)
                End If
                If lblActionAddress.Text = "Edit" Then
                    Using thisConn As New SqlConnection(myConn)
                        Using myCmd As SqlCommand = New SqlCommand("UPDATE CustomerAddress SET UnitNumber=@UnitNumber, Street=@Street, Suburb=@Suburb, States=@States, PostCode=@PostCode, Port=@Port WHERE Id=@Id")
                            myCmd.Parameters.AddWithValue("@Id", lblCustomerAddressId.Text)
                            myCmd.Parameters.AddWithValue("@CustomerId", ddlCustomer.SelectedValue)
                            myCmd.Parameters.AddWithValue("@UnitNumber", txtAddressUnitNumber.Text.Trim())
                            myCmd.Parameters.AddWithValue("@Street", txtAddressStreet.Text.Trim())
                            myCmd.Parameters.AddWithValue("@Suburb", txtAddressSuburb.Text.Trim())
                            myCmd.Parameters.AddWithValue("@States", ddlAddressStates.SelectedValue)
                            myCmd.Parameters.AddWithValue("@PostCode", txtAddressPostCode.Text.Trim())
                            myCmd.Parameters.AddWithValue("@Port", ddlAddressPort.SelectedValue)

                            myCmd.Connection = thisConn
                            thisConn.Open()
                            myCmd.ExecuteNonQuery()
                            thisConn.Close()
                        End Using
                    End Using

                    Response.Redirect("~/order/edit", False)
                End If
            End If
        Catch ex As Exception
            MessageError_Address(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError_Address(True, "Please contact IT Support at reza@bigblinds.co.id !")
                mailCfg.MailError(Page.Title, "btnSubmitAddress_Click", Session("LoginId"), ex.ToString())
            End If
            ClientScript.RegisterStartupScript(Me.GetType(), "showAddress", thisScript, True)
        End Try
    End Sub

    Private Sub BindDataHeader(HeaderId As String)
        BackColor()
        Try
            BindDataCustomer()
            BindDataUser()
            BindDataShipment()

            Dim myData As DataSet = orderCfg.GetListData("SELECT * FROM OrderHeaders WHERE Id = '" + HeaderId + "'")

            Dim statusOrder As String = myData.Tables(0).Rows(0).Item("Status").ToString()
            If (Session("RoleName") = "Customer" Or Session("RoleName") = "Representative") And (statusOrder = "In Production" Or statusOrder = "Canceled" Or statusOrder = "Completed") Then
                Response.Redirect("~/order/detail", False)
                Exit Sub
            End If

            ddlCustomer.SelectedValue = myData.Tables(0).Rows(0).Item("CustomerId").ToString()
            ddlCreatedBy.SelectedValue = myData.Tables(0).Rows(0).Item("CreatedBy").ToString()
            txtCreatedDate.Text = Convert.ToDateTime(myData.Tables(0).Rows(0).Item("CreatedDate")).ToString("yyyy-MM-dd")
            txtOrderNumber.Text = myData.Tables(0).Rows(0).Item("OrderNumber").ToString()
            lblOrderNo.Text = myData.Tables(0).Rows(0).Item("OrderNumber").ToString()

            txtOrderName.Text = myData.Tables(0).Rows(0).Item("OrderName").ToString()
            txtNote.Text = myData.Tables(0).Rows(0).Item("OrderNote").ToString()
            txtOrderId.Text = myData.Tables(0).Rows(0).Item("OrderId").ToString()
            txtJobId.Text = myData.Tables(0).Rows(0).Item("JobId").ToString()
            txtJobDate.Text = If(myData.Tables(0).Rows(0).Item("JobDate").ToString() <> "", Convert.ToDateTime(myData.Tables(0).Rows(0).Item("JobDate")).ToString("yyyy-MM-dd"), "")
            ddlShipmentId.SelectedValue = myData.Tables(0).Rows(0).Item("ShipmentId").ToString()
            Dim addressData As DataSet = orderCfg.GetListData("SELECT * FROM CustomerAddress WHERE CustomerId = '" + ddlCustomer.SelectedValue + "' AND [Primary] = 1")

            lblActionAddress.Text = "Add"
            mAddressTitle.InnerText = "Add Primary Address"
            If addressData.Tables(0).Rows.Count = 1 Then
                lblActionAddress.Text = "Edit"
                mAddressTitle.InnerText = "Edit Primary Address"
                lblCustomerAddressId.Text = addressData.Tables(0).Rows(0).Item("Id").ToString()
                txtAddressUnitNumber.Text = addressData.Tables(0).Rows(0).Item("UnitNumber").ToString()
                txtAddressStreet.Text = addressData.Tables(0).Rows(0).Item("Street").ToString()
                txtAddressSuburb.Text = addressData.Tables(0).Rows(0).Item("Suburb").ToString()
                ddlAddressStates.SelectedValue = addressData.Tables(0).Rows(0).Item("States").ToString()
                txtAddressPostCode.Text = addressData.Tables(0).Rows(0).Item("PostCode").ToString()
                ddlAddressPort.SelectedValue = addressData.Tables(0).Rows(0).Item("Port").ToString()
                txtShippingAddress.Text = txtAddressUnitNumber.Text & " " & txtAddressStreet.Text & ", " & txtAddressSuburb.Text & " " & ddlAddressStates.SelectedValue & " " & txtAddressPostCode.Text
            End If

            divCustomer.Visible = False
            divCreatedBy.Visible = False
            divCreatedDate.Visible = False
            divJobId.Visible = False
            divJobDate.Visible = False
            divShipment.Visible = False

            ddlCustomer.Enabled = False
            ddlCreatedBy.Enabled = False
            txtCreatedDate.Enabled = False

            txtOrderId.Enabled = False
            ddlCustomer.Enabled = False
            ddlCreatedBy.Enabled = False
            txtCreatedDate.Enabled = False
            txtJobId.Enabled = False
            txtJobDate.Enabled = False

            If Session("RoleName") = "Administrator" Then
                divCustomer.Visible = True
                divCreatedBy.Visible = True
                divCreatedDate.Visible = True
                divShipment.Visible = True

                If statusOrder = "In Production" Then
                    divJobId.Visible = True
                    divJobDate.Visible = True
                End If

                ddlCustomer.Enabled = True
                ddlCreatedBy.Enabled = True
                txtCreatedDate.Enabled = True
                If Session("LevelName") = "Leader" Then
                    txtOrderId.Enabled = True
                    ddlCustomer.Enabled = True
                    ddlCreatedBy.Enabled = True
                    txtCreatedDate.Enabled = True
                    txtJobId.Enabled = True
                    txtJobDate.Enabled = True
                End If
            End If

            If Session("RoleName") = "Customer Service" Or Session("RoleName") = "Data Entry" Then
                divCustomer.Visible = True
                divCreatedBy.Visible = True
                divCreatedDate.Visible = True
                divShipment.Visible = True
            End If
        Catch ex As Exception
            MessageError(True, ex.ToString())
            If Not Session("RoleName") = "Administrator" Then
                MessageError(True, "Please contact IT Support at reza@bigblinds.co.id")
                If Session("RoleName") = "Customer" Then
                    MessageError(True, "Please contact Customer Service at customerservice@lifestyleshutters.com.au")
                    'MessageError(True, "Please contact Customer Service at customerservice@sunlight.com.au")
                End If
                mailCfg.MailError(Page.Title, "BindDataHeader", Session("LoginId"), ex.ToString())
            End If
        End Try
    End Sub

    Private Sub BindDataCustomer()
        ddlCustomer.Items.Clear()
        Try
            ddlCustomer.DataSource = orderCfg.GetListData("SELECT *, UPPER(Name) AS NameText FROM Customers ORDER BY Name ASC")
            ddlCustomer.DataTextField = "NameText"
            ddlCustomer.DataValueField = "Id"
            ddlCustomer.DataBind()

            If ddlCustomer.Items.Count > 1 Then
                ddlCustomer.Items.Insert(0, New ListItem("", ""))
            End If
        Catch ex As Exception
            ddlCustomer.Items.Clear()
            mailCfg.MailError(Page.Title, "BindDataCustomer", Session("LoginId"), ex.ToString())
        End Try
    End Sub

    Private Sub BindDataUser()
        ddlCreatedBy.Items.Clear()
        Try
            ddlCreatedBy.DataSource = orderCfg.GetListData("SELECT *, UPPER(UserName) + ' | ' + UPPER(FullName) AS NameText FROM CustomerLogins ORDER BY UserName ASC")
            ddlCreatedBy.DataTextField = "NameText"
            ddlCreatedBy.DataValueField = "Id"
            ddlCreatedBy.DataBind()

            If ddlCreatedBy.Items.Count > 1 Then
                ddlCreatedBy.Items.Insert(0, New ListItem("", ""))
            End If
        Catch ex As Exception
            ddlCreatedBy.Items.Clear()
            mailCfg.MailError(Page.Title, "BindDataCustomer", Session("LoginId"), ex.ToString())
        End Try
    End Sub

    Private Sub BindDataShipment()
        ddlShipmentId.Items.Clear()
        Try
            ddlShipmentId.DataSource = orderCfg.GetListData("SELECT * FROM OrderShipments ORDER BY Id ASC")
            ddlShipmentId.DataTextField = "ShipmentNumber"
            ddlShipmentId.DataValueField = "Id"
            ddlShipmentId.DataBind()

            If ddlShipmentId.Items.Count > 1 Then
                ddlShipmentId.Items.Insert(0, New ListItem("", ""))
            End If
        Catch ex As Exception
            ddlShipmentId.Items.Clear()
            mailCfg.MailError(Page.Title, "BindDataShipment", Session("LoginId"), ex.ToString())
        End Try
    End Sub

    Private Sub BackColor()
        MessageError(False, String.Empty)
        MessageError_Address(False, String.Empty)

        ddlCustomer.BackColor = Drawing.Color.Empty
        txtOrderNumber.BackColor = Drawing.Color.Empty
        txtOrderName.BackColor = Drawing.Color.Empty
        txtNote.BackColor = Drawing.Color.Empty
    End Sub

    Private Sub MessageError(Show As Boolean, Msg As String)
        divError.Visible = Show : msgError.InnerText = Msg
    End Sub

    Private Sub MessageError_Address(Show As Boolean, Msg As String)
        divErrorAddress.Visible = Show : msgErrorAddress.InnerText = Msg
    End Sub
End Class

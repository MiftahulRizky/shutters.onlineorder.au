<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Inherits="Order_Edit" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" Debug="true" Title="Edit Order" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div class="container-xl">
            <div class="row g-2 align-items-center">
                <div class="col">
                    <div class="page-pretitle">Order</div>
                    <h2 runat="server">Order Header</h2>
                </div>
            </div>
        </div>
    </div>

    <div class="page-body">
        <div class="container-xl">
            <div class="row">
                <div class="col-lg-7 col-md-12 col-sm-12">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">Edit Data Order</h3>
                        </div>

                        <div class="card-body">
                            <div class="mb-2 row" runat="server" id="divCustomer">
                                <label class="col-lg-3 col-form-label required">CUSTOMER NAME</label>
                                <div class="col-lg-9 col-md-12 col-sm-12">
                                    <asp:DropDownList runat="server" ID="ddlCustomer" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="mb-2 row" runat="server" id="divCreatedBy">
                                <label class="col-lg-3 col-form-label required">CREATED BY</label>
                                <div class="col-lg-8 col-md-12 col-sm-12">
                                    <asp:DropDownList runat="server" ID="ddlCreatedBy" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="mb-5 row" runat="server" id="divCreatedDate">
                                <label class="col-lg-3 col-form-label required">CREATED DATE</label>
                                <div class="col-lg-3 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtCreatedDate" CssClass="form-control" TextMode="Date" placeholder="" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="mb-3 row" runat="server">
                                <label class="col-lg-3 col-form-label required">ORDER ID</label>
                                <div class="col-lg-3 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtOrderId" CssClass="form-control" placeholder="Order Id ..." autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            
                            <div class="mb-2 row">
                                <label class="col-lg-3 col-form-label required">ORDER NUMBER</label>
                                <div class="col-lg-6 col-md-12 col-sm-12">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtOrderNumber" CssClass="form-control" placeholder="Order Number ..." autocomplete="off"></asp:TextBox>
                                        <a class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalInfo" onclick="return showInfo('Order Number');"> ? </a>
                                    </div>
                                </div>
                            </div>

                            <div class="mb-2 row">
                                <label class="col-lg-3 col-form-label required">CUSTOMER NAME</label>
                                <div class="col-lg-8 col-md-12 col-sm-12">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtOrderName" CssClass="form-control" placeholder="Customer Name ..." autocomplete="off"></asp:TextBox>
                                        <a class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalInfo" onclick="return showInfo('Customer Name');"> ? </a>
                                    </div>
                                </div>
                            </div>

                            <div class="mb-2 row">
                                <label class="col-lg-3 col-form-label">NOTE</label>
                                <div class="col-lg-9 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtNote" Height="100px" TextMode="MultiLine" CssClass="form-control" placeholder="Your note for this order ..." autocomplete="off" style="resize:none;"></asp:TextBox>
                                </div>
                            </div>
                            
                            <div class="mb-2 mt-5 row" runat="server" id="divJobId">
                                <label class="col-lg-3 col-form-label required">JOB ID</label>
                                <div class="col-lg-3 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtJobId" CssClass="form-control" placeholder="Job Id ..." autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="mb-2 row" runat="server" id="divJobDate">
                                <label class="col-lg-3 col-form-label required">JOB DATE</label>
                                <div class="col-lg-3 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtJobDate" CssClass="form-control" TextMode="Date" placeholder="" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="mb-2 mt-5 row" runat="server" id="divShipmentId">
                                <label class="col-lg-3 col-form-label required">SHIPMENT NUMBER</label>
                                <div class="col-lg-3 col-md-12 col-sm-12">
                                    <asp:DropDownList runat="server" ID="ddlShipmentId" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="mb-2 mt-5 row" runat="server" id="divShipment">
                                <label class="col-lg-3 col-form-label">SHIPPING ADDRESS</label>
                                <div class="col-lg-9 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtShippingAddress" ClientIDMode="Static" Height="80px" TextMode="MultiLine" CssClass="form-control" placeholder="Shipping Address ..." autocomplete="off" style="resize:none;"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row" runat="server" id="divError">
                                <div class="col-12">
                                    <div class="alert alert-important alert-danger alert-dismissible" role="alert">
                                        <div class="d-flex">
                                            <div>
                                                <svg xmlns="http://www.w3.org/2000/svg" class="icon alert-icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M3 12a9 9 0 1 0 18 0a9 9 0 0 0 -18 0" /><path d="M12 8v4" /><path d="M12 16h.01" /></svg>
                                            </div>
                                            <div>
                                                <span runat="server" id="msgError"></span>
                                            </div>
                                        </div>
                                        <a class="btn-close" data-bs-dismiss="alert" aria-label="close"></a>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card-footer text-center">
                            <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal modal-blur fade" id="modalInfo" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm modal-dialog-centered modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalTitle"></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="text-secondary">
                        <span id="spanInfo"></span>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="w-100">
                        <div class="row">
                            <div class="col"><a href="#" class="btn btn-secondary w-100" data-bs-dismiss="modal">OK</a></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal modal-blur fade" id="modalAddress" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" runat="server" id="mAddressTitle"></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
            
                <div class="modal-body">
                    <div class="mb-4 row">
                        <div class="col-4">
                            <label class="form-label">Unit Number</label>
                            <asp:TextBox runat="server" ID="txtAddressUnitNumber" CssClass="form-control" placeholder="Unit Number ..." autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-8">
                            <label class="form-label required">Street Address</label>
                            <asp:TextBox runat="server" ID="txtAddressStreet" CssClass="form-control" placeholder="Street Address ..." autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                
                    <div class="mb-4 row">
                        <div class="col-6">
                            <label class="form-label required">Suburb</label>
                            <asp:TextBox runat="server" ID="txtAddressSuburb" CssClass="form-control" placeholder="Suburb ..." autocomplete="off"></asp:TextBox>
                        </div>

                        <div class="col-6">
                            <label class="form-label required">States</label>
                            <asp:DropDownList runat="server" ID="ddlAddressStates" CssClass="form-select">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="ACT" Text="ACT"></asp:ListItem>
                                <asp:ListItem Value="NSW" Text="NSW"></asp:ListItem>
                                <asp:ListItem Value="NT" Text="NT"></asp:ListItem>
                                <asp:ListItem Value="QLD" Text="QLD"></asp:ListItem>
                                <asp:ListItem Value="SA" Text="SA"></asp:ListItem>
                                <asp:ListItem Value="TAS" Text="TAS"></asp:ListItem>
                                <asp:ListItem Value="VIC" Text="VIC"></asp:ListItem>
                                <asp:ListItem Value="WA" Text="WA"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                
                    <div class="mb-4 row">
                        <div class="col-6">
                            <label class="form-label required">Post Code</label>
                            <asp:TextBox runat="server" ID="txtAddressPostCode" CssClass="form-control" placeholder="Post Code ..." autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <label class="form-label required">Nearest Port</label>
                            <asp:DropDownList runat="server" ID="ddlAddressPort" CssClass="form-select">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="Adelaide" Text="Adelaide"></asp:ListItem>
                                <asp:ListItem Value="Brisbane" Text="Brisbane"></asp:ListItem>
                                <asp:ListItem Value="Melbourne" Text="Melbourne"></asp:ListItem>
                                <asp:ListItem Value="Perth" Text="Perth"></asp:ListItem>
                                <asp:ListItem Value="Sydney" Text="Sydney"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                
                    <div class="row" runat="server" id="divErrorAddress">
                        <div class="col-12">
                            <div class="alert alert-important alert-danger alert-dismissible" role="alert">
                                <div class="d-flex">
                                    <div>
                                        <svg xmlns="http://www.w3.org/2000/svg" class="icon alert-icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M3 12a9 9 0 1 0 18 0a9 9 0 0 0 -18 0" /><path d="M12 8v4" /><path d="M12 16h.01" /></svg>
                                    </div>
                                    <div>
                                        <span runat="server" id="msgErrorAddress"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnSubmitAddress" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmitAddress_Click" />
                    <a href="#" class="btn" data-bs-dismiss="modal">Cancel</a>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showAddress() {
            $("#modalAddress").modal("show");
        }
        $("#txtShippingAddress").on("click", function (e) {
            e.preventDefault();
            showAddress();
        })
        function showInfo(type) {
            let header;
            let body;
            if (type === "Order Number") {
                header = "Order Number Information";
                body = "Please do not use the following characters :";
                body += "[ / ], [ \ ], [ & ], [ # ], [ ` ] AND [ , ]";
                body += "Maximum 20 characters for retailer order number.";
            }
            else if (type === "Customer Name") {
                header = "Customer Name Information";
                body = "Please do not use the following characters :";
                body += "[ / ], [ \ ], [ & ], [ # ], [ ` ] AND [ , ]";
            }
            else {
                body = "";
            }
            document.getElementById("modalTitle").innerHTML = header;
            document.getElementById("spanInfo").innerHTML = body;
        }
    </script>

    <div runat="server" visible="false">        
        <asp:Label runat="server" ID="lblHeaderId"></asp:Label>
        <asp:Label runat="server" ID="lblOrderNo"></asp:Label>

        <asp:Label runat="server" ID="lblActionAddress"></asp:Label>
        <asp:Label runat="server" ID="lblCustomerAddressId"></asp:Label>
    </div>
</asp:Content>

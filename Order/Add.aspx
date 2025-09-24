<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Add.aspx.vb" Inherits="Order_Add" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" Debug="true" Title="Create Order" %>

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
                <div class="col-lg-7 col-sm-12 col-md-12">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">Create New Order</h3>
                        </div>

                        <div class="card-body">
                            <div class="mb-5 row" runat="server" id="divCustomer">
                                <label class="col-lg-4 col-form-label required">CUSTOMER NAME</label>
                                <div class="col-lg-8 col-md-12 col-sm-12">
                                    <asp:DropDownList runat="server" ID="ddlCustomer" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="mb-2 row" runat="server" id="divCreatedBy">
                                <label class="col-lg-4 col-form-label required">CREATED BY</label>
                                <div class="col-lg-8 col-md-12 col-sm-12">
                                    <asp:DropDownList runat="server" ID="ddlCreatedBy" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="mb-5 row" runat="server" id="divCreatedDate">
                                <label class="col-lg-4 col-form-label required">CREATED DATE</label>
                                <div class="col-lg-3 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtCreatedDate" CssClass="form-control" TextMode="Date" placeholder="" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="mb-3 row">
                                <label class="col-lg-4 col-form-label required">ORDER NUMBER</label>
                                <div class="col-lg-5 col-md-12 col-sm-12">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtOrderNumber" CssClass="form-control" placeholder="Order Number ..." autocomplete="off"></asp:TextBox>
                                        <a class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalInfo" onclick="return showInfo('Order Number');"> ? </a>
                                    </div>
                                </div>
                            </div>

                            <div class="mb-3 row">
                                <label class="col-lg-4 col-form-label required">CUSTOMER NAME</label>
                                <div class="col-lg-7 col-md-12 col-sm-12">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtOrderName" CssClass="form-control" placeholder="Customer Name ..." autocomplete="off"></asp:TextBox>
                                        <a class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalInfo" onclick="return showInfo('Customer Name');"> ? </a>
                                    </div>
                                </div>
                            </div>

                            <div class="mb-3 row">
                                <label class="col-lg-4 col-form-label">NOTE</label>
                                <div class="col-lg-7 col-md-12 col-sm-12">
                                    <asp:TextBox runat="server" ID="txtNote" Height="100px" TextMode="MultiLine" CssClass="form-control" placeholder="Your note for this order ..." autocomplete="off" style="resize:none;"></asp:TextBox>
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

    <script type="text/javascript">
        function showInfo(type) {
            let header;
            let body;
            if (type === "Order Number") {
                header = "Order Number Information";
                body = "Please do not use the following characters:";
                body += "<br />";
                body += "[ / ], [ \\ ], [ & ], [ # ], [ ` ], [ , ], AND [ \" ]";
                body += "<br />";
                body += "Maximum 20 characters for retailer order number.";
            }
            else if (type === "Customer Name") {
                header = "Customer Name Information";
                body = "Please do not use the following characters:";
                body += "<br />";
                body += "[ / ], [ \\ ], [ & ], [ # ], [ ` ], [ , ], AND [ \" ]";
                body += "<br />";
            }
            else {
                body = "";
            }
            document.getElementById("modalTitle").innerHTML = header;
            document.getElementById("spanInfo").innerHTML = body;
        }
    </script>
</asp:Content>
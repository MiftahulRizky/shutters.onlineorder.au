<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Order_Default" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" Debug="true" Title="List Order" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div class="container-xl">
            <div class="row g-2 align-items-center">
                <div class="col-5">
                    <div class="page-pretitle">Order</div>
                    <h2 class="page-title">List Order</h2>
                </div>

                <div class="col-7 text-end">
                    <a href="#" runat="server" id="aDailyMail" class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#modalDailyMail">Daily Mail</a>
                    <a runat="server" id="aOtorisasi" class="btn btn-info position-relative" data-bs-toggle="offcanvas" href="#canvasOtorisasi" role="button" aria-controls="canvasAdd">
                        Waiting for Authorization List
                        <span runat="server" id="spanOtorisasi" class="badge bg-red text-blue-fg badge-notification badge-pill">10</span>
                    </a>
                </div>
            </div>
        </div>
    </div>

    <div class="page-body">
        <div class="container-xl">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">Data Order</h3>
                            <div class="card-actions">
                                <asp:Button runat="server" ID="btnAdd" CssClass="btn btn-primary" Text="New Order" OnClick="btnAdd_Click" />
                            </div>
                        </div>

                        <div class="card-body border-bottom py-3">
                            <div class="d-flex">
                                <div class="text-secondary">
                                    <div class="ms-2 d-inline-block">
                                        <asp:DropDownList runat="server" ID="ddlStatus" Width="200px" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="ms-auto text-secondary">
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="ms-2 d-inline-block">
                                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control" placeholder="Search Data" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="ms-2 d-inline-block">
                                            <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>

                        <div class="card-body border-bottom py-3" runat="server" id="divError">
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
                        
                        <div class="table-responsive">
                            <asp:GridView runat="server" ID="gvList" Font-Size="15px" Width="100%" CssClass="table table-striped table-hover table-vcenter card-table" AutoGenerateColumns="false" AllowPaging="true" ShowHeaderWhenEmpty="true" PagerSettings-Position="TopAndBottom" EmptyDataText="ORDER DATA NOT FOUND" EmptyDataRowStyle-HorizontalAlign="Center" PageSize="50" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                <RowStyle />
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="OrderId" HeaderText="Order #" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Retailer Name" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="OrderNumber" HeaderText="Order Number" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="OrderName" HeaderText="Order Name" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="OrderType" HeaderText="Type" ItemStyle-Wrap="true" />
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <%# Eval("Status").ToString() %>
                                            <span class="form-hint">
                                                <%# Eval("StatusAdditional").ToString() %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Created" DataFormatString="{0:dd MMM yyyy}" />
                                    <asp:BoundField DataField="SubmittedDate" HeaderText="Submitted" DataFormatString="{0:dd MMM yyyy}" />
                                    <asp:TemplateField ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <button class="btn btn-sm btn-pill btn-orange" data-bs-toggle="dropdown">Actions</button>
                                            <div class="dropdown-menu dropdown-menu-end">
                                                <asp:LinkButton runat="server" ID="linkView" CssClass="dropdown-item" Text="Detail" CommandName="Detail" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                                <a href="#" runat="server" class="dropdown-item" data-bs-toggle="modal" data-bs-target="#modalDelete" onclick='<%# String.Format("return showDelete(`{0}`, `{1}` , `{2}`);", Eval("Id").ToString(), Eval("OrderNumber").ToString(), Eval("OrderName")) %>' visible='<%# VisibleDelete(Eval("CreatedBy").ToString(), Eval("Status").ToString()) %>'>Delete</a>
                                                <div class="dropdown-divider"></div>
                                                <asp:LinkButton runat="server" CssClass="dropdown-item" ID="linkLog" Text="Logs" CommandName="Log" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="DodgerBlue" ForeColor="White" HorizontalAlign="Center" />
                                <PagerSettings PreviousPageText="Prev" NextPageText="Next" Mode="NumericFirstLast" />
                                <AlternatingRowStyle BackColor="" />
                            </asp:GridView>
                        </div>

                        <div class="card-footer text-end"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="offcanvas offcanvas-end" tabindex="-1" id="canvasOtorisasi" aria-labelledby="canvasAddLabel">
        <div class="offcanvas-header">
            <h2 class="offcanvas-title" id="canvasAddLabel">Waiting for Authorisation List</h2>
            <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>
        <div class="offcanvas-body">
            <asp:ListView runat="server" ID="lvOtorisasi" OnItemCommand="lvOtorisasi_ItemCommand">
                <ItemTemplate>
                    <div class="row row-cards mb-3">
                        <div class="col-lg-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="card-title">Out Of Spec</h5>
                                </div>
                                <div class="card-body">
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" CommandName="View" CommandArgument='<%# Eval("HeaderId").ToString() %>'>
                                        <h5 class="card-title">Order# <%# Eval("JobOtorisasi").ToString() %></h5>
                                    </asp:LinkButton>
                                    <span>- <%# Eval("Description").ToString() %></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>

    <div class="modal modal-blur fade" id="modalDelete" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm modal-dialog-centered" role="document">
            <div class="modal-content">
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                <div class="modal-status bg-danger"></div>
                <div class="modal-body text-center py-4">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-danger icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M10.24 3.957l-8.422 14.06a1.989 1.989 0 0 0 1.7 2.983h16.845a1.989 1.989 0 0 0 1.7 -2.983l-8.423 -14.06a1.989 1.989 0 0 0 -3.4 0z" /><path d="M12 9v4" /><path d="M12 17h.01" /></svg>
                    <asp:TextBox runat="server" ID="txtIdDelete" style="display:none;"></asp:TextBox>
                    <h3>Delete Order</h3>
                    <div class="text-secondary">
                        Are you sure you would like to do this?
                        <span id="spanDescription"></span>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="w-100">
                        <div class="row">
                            <div class="col"><a href="#" class="btn w-100" data-bs-dismiss="modal">Cancel</a></div>
                            <div class="col">
                                <asp:Button runat="server" ID="btnSubmitDelete" CssClass="btn btn-danger w-100" Text="Confirm" OnClick="btnSubmitDelete_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal modal-blur fade" id="modalLog" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Changelog</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
            
                <div class="modal-body">
                    <div class="row" runat="server" id="divErrorLog">
                        <div class="col-12">
                            <div class="alert alert-important alert-danger alert-dismissible" role="alert">
                                <div class="d-flex">
                                    <div>
                                        <svg xmlns="http://www.w3.org/2000/svg" class="icon alert-icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M3 12a9 9 0 1 0 18 0a9 9 0 0 0 -18 0" /><path d="M12 8v4" /><path d="M12 16h.01" /></svg>
                                    </div>
                                    <div>
                                        <span runat="server" id="msgErrorLog"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView runat="server" ID="gvListLogs" CssClass="table table-vcenter card-table" AutoGenerateColumns="false" EmptyDataText="DATA LOG NOT FOUND" EmptyDataRowStyle-HorizontalAlign="Center" ShowHeader="false" GridLines="None" BorderStyle="None">
                            <RowStyle />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%# BindTextLog(Eval("Id").ToString()) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal modal-blur fade" id="modalDailyMail" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm modal-dialog-centered" role="document">
            <div class="modal-content">
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                <div class="modal-status bg-secondary"></div>
                <div class="modal-body text-center py-4">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon mb-2 text-secondary icon-lg">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none" /><path d="M17 10l-2 -6" /><path d="M7 10l2 -6" /><path d="M11 20h-3.756a3 3 0 0 1 -2.965 -2.544l-1.255 -7.152a2 2 0 0 1 1.977 -2.304h13.999a2 2 0 0 1 1.977 2.304l-.479 2.729" /><path d="M10 14a2 2 0 1 0 4 0a2 2 0 0 0 -4 0" /><path d="M15 19l2 2l4 -4" />
                    </svg>
                    <h3 runat="server" id="titleSubmit">Send Production Order</h3>
                    <div class="text-secondary">
                        Are you sure you would like to do this?
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="w-100">
                        <div class="row">
                            <div class="col"><a href="#" class="btn w-100" data-bs-dismiss="modal">Cancel</a></div>
                            <div class="col">
                                <asp:Button runat="server" ID="btnSubmitDailyMail" CssClass="btn w-100 btn-secondary" Text="Confirm" OnClick="btnSubmitDailyMail_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">        
        function showDelete(id, orderNumber, orderName) {
            let description = "Are you sure you want to delete order? <br /><br />";
            description += "- ORDER NUMBER : " + orderNumber;
            description += "<br />";
            description += "- CUSTOMER NAME : " + orderName;
            description += "<br />";
            document.getElementById("<%=txtIdDelete.ClientID %>").value = id;
            document.getElementById("spanDescription").innerHTML = description;
        }

        function showLog() {
            $("#modalLog").modal("show");
        }
    </script>
</asp:Content>
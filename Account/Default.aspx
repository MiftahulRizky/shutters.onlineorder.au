<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Account_Default" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" Debug="true" Title="My Account" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div class="container-xl">
            <div class="row g-2 align-items-center">
                <div class="col">
                    <h2 class="page-title">Account Settings</h2>
                </div>
            </div>
        </div>
    </div>

    <div class="page-body">
        <div class="container-xl">
            <div class="card">
                <div class="row g-0">
                    <div class="col-12 col-md-3 border-end">
                        <div class="card-body">
                            <h4 class="subheader">Business settings</h4>
                            <div class="list-group list-group-transparent">
                                <a runat="server" href="~/account/" class="list-group-item list-group-item-action d-flex align-items-center active">My Account</a>
                                <a runat="server" href="~/account/contact" class="list-group-item list-group-item-action d-flex align-items-center">My Contact</a>
                                <a runat="server" href="~/account/store" class="list-group-item list-group-item-action d-flex align-items-center">My Store</a>
                                <a runat="server" href="~/account/logo" class="list-group-item list-group-item-action d-flex align-items-center">My Logo</a>
                            </div>
                            <h4 class="subheader mt-4">Experience</h4>
                            <div class="list-group list-group-transparent">
                                <a runat="server" href="~/account/feedback" class="list-group-item list-group-item-action">Give Feedback</a>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-9 d-flex flex-column">
                        <div class="card-body">
                            <h2 class="mb-4">My Account</h2>
                            <h3 class="card-title">Profile Details</h3>
                            <div class="row align-items-center">
                                <div class="col-auto"><span class="avatar avatar-xl" style="background-image: url(https://ordering.lifestyleshutters.com.au/Content/static/avatars.jpg)"></span>
                                </div>
                            </div>

                            <h3 class="card-title mt-4">System Profile</h3>
                            <div class="row g-3">
                                <div class="col-md">
                                    <div class="form-label required">Full Name</div>
                                    <asp:TextBox runat="server" ID="txtFullName" CssClass="form-control" autocomplete="off" placeholder="Full Name ....."></asp:TextBox>
                                </div>

                                <div class="col-md">
                                    <div class="form-label required">User Name</div>
                                    <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" autocomplete="off" placeholder="Username ....."></asp:TextBox>
                                </div>
                            </div>

                            <h3 class="card-title mt-4">Password</h3>
                            <p class="card-subtitle">You can set a permanent password if you don't want to use temporary login codes.</p>
                            <div>
                                <a runat="server" href="~/account/password" class="btn">
                                    Set new password
                                </a>
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

                        <div class="card-footer bg-transparent mt-auto">
                            <div class="btn-list justify-content-end">
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn" OnClick="btnCancel_Click" />
                                <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="false">
        <asp:Label runat="server" ID="lblLoginId"></asp:Label>
    </div>
</asp:Content>

<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Account_Login" %>

<!DOCTYPE html>

<html lang="en">

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge"/>
    <title>Login - Lifestyle Blinds & Shutters</title>
    <link rel="stylesheet" href="https://rsms.me/inter/inter.css">
    <link href="../Content/dist/css/tabler.min.css?1692870487" rel="stylesheet"/>
    <link href="../Content/dist/css/tabler-flags.min.css?1692870487" rel="stylesheet"/>
    <link href="../Content/dist/css/tabler-payments.min.css?1692870487" rel="stylesheet"/>
    <link href="../Content/dist/css/tabler-vendors.min.css?1692870487" rel="stylesheet"/>
    <link href="../Content/dist/css/demo.min.css?1692870487" rel="stylesheet"/>
    <link rel="icon" type="image/x-icon" href="../Content/static/favicon.ico" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@tabler/icons-webfont@latest/dist/tabler-icons.min.css" />
    <style>
      :root {
      	--tblr-font-sans-serif: 'Inter Var', -apple-system, BlinkMacSystemFont, San Francisco, Segoe UI, Roboto, Helvetica Neue, sans-serif;
      }
      body {
      	font-feature-settings: "cv03", "cv04", "cv11";
      }
    </style>
</head>
<body class=" d-flex flex-column">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Content/dist/js/demo-theme.min.js?1692870487" />
                <asp:ScriptReference Path="~/Content/dist/js/tabler.min.js?1692870487" />
                <asp:ScriptReference Path="~/Content/dist/js/demo.min.js?1692870487" />
            </Scripts>
        </asp:ScriptManager>

        <div class="page page-center mt-6">
            <div class="container mt-3">
                <div class="d-none d-md-flex float-end">
                    <a href="?theme=dark" class="nav-link px-0 hide-theme-dark" title="Enable dark mode"
                        data-bs-toggle="tooltip" data-bs-placement="bottom">
                        <i class="ti ti-moon fs-2" width="24" height="24"></i>
                    </a>
                    <a href="?theme=light" class="nav-link px-0 hide-theme-light" title="Enable light mode"
                        data-bs-toggle="tooltip" data-bs-placement="bottom">
                        <i class="ti ti-sun fs-2" width="24" height="24"></i>
                    </a>
                </div>
            </div>

            <div class="container container-tight py-4">
                <div class="text-center mb-5">
                    <a runat="server" href="~/" class="navbar-brand navbar-brand-autodark">
                        <%--<img runat="server" src="~/Content/static/LS.jpeg" width="210" height="90" alt="Lifestyle" class="navbar-brand-image">--%>
                        <%--<img runat="server" src="~/Content/static/ShutterLogo.png" alt="Lifestyle" class="navbar-brand-image">--%>
                    </a>
                    <h1>ORDER SHUTTERS</h1>
                </div>

                <div class="card card-md">
                    <div class="card-body">
                        <h2 class="h2 text-center mb-4">Login to your account</h2>
                        <div class="mb-3">
                            <label class="form-label">UserName</label>
                            <asp:TextBox runat="server" ID="txtUserLogin" CssClass="form-control" placeholder="User Name" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label">
                                Password
                                <%--<span class="form-label-description">
                                    <a runat="server" href="~/account/forgot">I forgot password</a>
                                </span>--%>
                            </label>
                            
                            <div class="input-group input-group-flat">
                                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" placeholder="Your password" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-4 row">
                            <div class="col-lg-8">
                                <label for="chkShowPass" style="cursor: pointer;">
                                    <input type="checkbox" id="chkShowPass" onclick="togglePassword()"> Show Password
                                </label>
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
                        
                        <div class="form-footer">
                            <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-primary w-100" Text="Log In" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </div>

                <div class="text-center text-secondary mt-3">
                  Want to order blinds?, <a href="https://onlineorder.au/" tabindex="-1">click here.</a>
                </div>
                
            </div>
        </div>
        <div runat="server" visible="false">
            <asp:Label runat="server" ID="lblDeviceId"></asp:Label>
        </div>

        <script type="text/javascript">
            function togglePassword() {
                var password = document.getElementById('<%= txtPassword.ClientID %>');
                var checkBox = document.getElementById('chkShowPass');

                if (checkBox.checked) {
                    password.type = "text";
                } else {
                    password.type = "password";
                }
            }
        </script>
    </form>
</body>
</html>

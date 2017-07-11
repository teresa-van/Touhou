<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Website.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <center><img src="https://static.tumblr.com/4d95c6dd7b14fd2242e95f6c4f319d21/oorg7z2/AFjn0s7gl/tumblr_static_tumblr_header.jpg" width=100%/></center>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <%-- Registers --%>
    <div class="row">
        <div class="col-lg-6 col-md-offset-3" style="padding-left: 150px; padding-right: 150px; padding-top: 50px;">
            <div class="panel panel-default">
                <div class="panel-header">
                    <h3 style="font-size: 28px;"><strong>Register</strong></h3>
                </div>
                <div class="panel-body">
                    <div class="row" style="margin-right: 30px; margin-left: 30px;">

                        <div class="row" style="margin-top: 15px; margin-bottom: 15px;">
                            <div class="input-group">
                                <span class="input-group-addon">Username: </span>
                                <input type="text" id="username" class="form-control" placeholder="Enter a username" />
                            </div>
                        </div>

                        <div class="row" style="margin-top: 15px; margin-bottom: 15px;">
                            <div class="input-group">
                                <span class="input-group-addon">Password: </span>
                                <input type="password" id="password" class="form-control" data-toggle="password" placeholder="Enter a password" />
                            </div>
                        </div>

                        <div class="row" style="margin-top: 15px; margin-bottom: 15px;">
                            <div class="input-group">
                                <span class="input-group-addon">Nickname: </span>
                                <input type="text" id="nickname" class="form-control" placeholder="Enter a nickname" />
                            </div>
                        </div>

                        <div class="row" style="margin-top: 15px; margin-bottom: 15px;">
                            <div class="row" style="margin-top: 15px; margin-bottom: 15px;">
                                <center><button type="button" id="registerButton" class="btn btn-primary btn-lg" style="width: 50%;" onclick="Register()">Register</button></center>
                            </div>

                            <div class="alert alert-success" id="successful" role="alert">Successfully registered! ^_^</div>
                            <div class="alert alert-danger" id="failed" role="alert">Something went wrong with the registration... T_T</div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JavascriptContentPlaceHolder" runat="server">

    <script>

        $("#successful").hide();
        $("#failed").hide();

        //Function for adding a user to the system
        function Register() {

            //Get the value from name textbox
            var username = $("#username").val();
            var password = $("#password").val();
            var nickname = $("#nickname").val();

            //Hide save button
            $("#registerButton").hide();
            showLoadingOverlay();

            //Set parameters for the webservice call
            var parameters = {};
            parameters.username = username;
            parameters.password = password;
            parameters.nickname = nickname;

            //Call the webservice and handle errors
            $.ajax({
                type: "POST",
                url: WEBSERVICE_URL + "Userwebservices.asmx/AddUser",
                data: JSON.stringify(parameters),
                dataType: "json",
                contentType: "application/json",
                success: function (result) {
                    var parsedJSON = servers = $.parseJSON(result.d);
                    if (parsedJSON.success) {
                        $("#registerButton").show();
                        hideLoadingOverlay();
                        $("#successful").show();
                    }
                    else {
                        hideLoadingOverlay();
                        $("#failed").show();
                        console.log("fuck");
                    }
                },
                error: function (jqXHR, error, errorThrown) {
                    if (jqXHR.status && jqXHR.status == 400) {
                        alert(jqXHR.responseText);
                    } else {
                        hideLoadingOverlay();
                        $("#failed").show();
                        console.log(jqXHR);
                        console.log(error);
                    }
                }
            });

        }

    </script>

</asp:Content>

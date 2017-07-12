<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Website.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <!--<link rel="stylesheet" type="text/css" href="../../Assets/css/FontStyleSheet.css" />-->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.11/css/jquery.dataTables.min.css" />
    <center><img src="https://static.tumblr.com/4d95c6dd7b14fd2242e95f6c4f319d21/oorg7z2/AFjn0s7gl/tumblr_static_tumblr_header.jpg" width=100%/></center>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <center>
    <div class="panel" style="width: 45%;">
        <div class="panel-header">
            <h3 style="font-size: 20px;">Registered <strong>Users</strong></h3>
        </div>
        <div class="panel-content">
            <div id="messageDiv" class="alert media fade in" style="display: none">
                <p>Message</p>
            </div>
            <div style="background-color: #444444; width: 100%; height: 10px; margin-bottom: 5px;" class="col-md-6"></div>
            <table id="users_table" class="display" style="width: 100%">
                <thead>
                    <tr>
                        <th>Username</th>
                        <th>Nickname</th>
                        <th></th>
                        <th></th>
                        <th></th>

                    </tr>
                </thead>
            </table>
        </div>
    </div>
    </center>

    <!-- BEGIN EDIT MODAL -->
    <div class="modal fade" id="editModal" tabindex="-1" role="dialog" aria-labelledby="editModal" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="editTitle">Edit Nickname</h4>
                </div>
                <div class="modal-body body">
                    <div class="input-group">
                        <span class="input-group-addon" id="EditNameLabel">Nickname</span>
                        <input id="EditName" type="text" class="form-control" placeholder="Enter a new nickname." aria-describedby="EditNameLabel" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success edit">Update</button>
                </div>
            </div>
        </div>
    </div>
    <!-- END EDIT MODAL -->

    <!-- BEGIN DELETE MODAL -->
    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="deleteModal" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                    <h4 class="modal-title" id="deleteTitle">Delete User</h4>
                </div>
                <div class="modal-body body" id="deleteBodyModal">
                    <div class="input-group">
                        <span class="input-group-addon" id="DeletePasswordLabel">Enter Password</span>
                        <input id="DeletePassword" type="password" class="form-control" placeholder="Please enter your password." aria-describedby="DeletePasswordLabel">
                    </div>
                    <hr />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-danger delete">Delete</button>
                </div>
            </div>
        </div>
    </div>
    <!-- END DELETE MODAL -->

    <!-- BEGIN PASSWORD MODAL -->
    <div class="modal fade" id="passwordModal" tabindex="-1" role="dialog" aria-labelledby="passwordModal" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="editPasswordTitle">Edit Password</h4>
                </div>
                <div class="modal-body body">
                    <div class="input-group">
                        <span class="input-group-addon" id="CurrentPasswordLabel">Current Password</span>
                        <input id="CurrentPassword" type="password" class="form-control" placeholder="Enter your current password." aria-describedby="CurrentPasswordLabel">
                    </div>
                    <hr />
                    <div class="input-group">
                        <span class="input-group-addon" id="ChangePasswordLabel">New Password</span>
                        <input id="ChangePassword" type="password" class="form-control" placeholder="Enter a new password." aria-describedby="ChangePasswordLabel">
                    </div>
                    <hr />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-danger change">Change Password</button>
                </div>
            </div>
        </div>
    </div>
    <!-- END PASSWORD MODAL -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JavascriptContentPlaceHolder" runat="server">
    <script src="https://cdn.datatables.net/1.10.11/js/jquery.dataTables.min.js"></script>
    <script>

        $(document).ready(function () {
            initializeUsers();
        });

        //##########
        //# Tables #
        //##########

        //Table references
        var usersTable;

        //Current selected user
        var selectedUser;

        //Display users
        function initializeUsers() {
            if (usersTable != undefined) {
                usersTable.ajax.reload();
                return;
            };

            console.log(WEBSERVICE_URL);
            usersTable = $('#users_table').DataTable({
                "ajax": WEBSERVICE_URL + "UserWebservices.asmx/GetAllUsers",
                "order": [[0, 'asc']],
                "bDestroy": true,
                "columns": [
                    {
                        "data": "user.Username"
                    },
                    {
                        "data": "user.Nickname"
                    },
                    {
                        "data": "user.ID",
                        "width": "1px",
                        "render": function (data, type, row) {
                            return '<a href="#" class="btn btn-primary btn-sm modify">Edit Nickname</a>';
                        }
                    },
                    {
                        "data": "user.ID",
                        "width": "1px",
                        "render": function (data, type, row) {
                            return '<a href="#" class="btn btn-primary btn-sm change_password">Change Password</a>';
                        }
                    },
                    {
                        "data": "user.ID",
                        "width": "1px",
                        "render": function (data, type, row) {
                            return '<a href="#" class="btn btn-primary btn-sm delete_user">Delete User</a>';
                        }
                    }
                ]
            });

            $('#users_table tbody').on('click', '.modify', function () {
                var data = usersTable.row($(this).parents('tr')).data();
                selectedUser = data;
                showEditModal();
            });

            $('#users_table tbody').on('click', '.delete_user', function () {
                var data = usersTable.row($(this).parents('tr')).data();
                selectedUser = data;
                showDeleteModal();
            });

            $('#users_table tbody').on('click', '.change_password', function () {
                var data = usersTable.row($(this).parents('tr')).data();
                selectedUser = data;
                showPasswordModal();
            });
        }

        //###########
        //# Show Modals #
        //###########

        //Display the modal that displays the user and allows information updating
        function showEditModal() {
            $("#editTitle").html("Edit Nickname (" + selectedUser.user.Username + ")");

            $("#EditName").val(selectedUser.user.Nickname);

            $("#editModal").modal('show');

            $("#editModal .edit").bind('click', function () {
                $(this).unbind('click');
            });

            $("#editModal .edit").click(function () {
                editNickname();
            });
        }

        //Display the modal that allows deleting a user
        function showDeleteModal() {
            $("#deleteTitle").html("Delete User (" + selectedUser.user.Username + ")");

            $("#deleteModal").modal('show');

            $("#deleteModal .delete").bind('click', function () {
                $(this).unbind('click');
            });

            $("#deleteModal .delete").click(function () {
                deleteUser();
            });
        }

        //Display the password modal
        function showPasswordModal() {
            $("#editPasswordTitle").html("Change Password (" + selectedUser.user.Username + ")");
            $("#CurrentPassword").val('');
            $("#ChangePassword").val('');
            $("#passwordModal").modal('show');

            $("#passwordModal .change").bind('click', function () {
                $(this).unbind('click');
            });

            $("#passwordModal .change").click(function () {
                updatePassword();
            });
        }

        //###########
        //# Actions #
        //###########

        //Edit nickname
        function editNickname() {
            var parameters = {};
            parameters.id = selectedUser.user.ID;
            parameters.nickname = $("#EditName").val();

            showLoadingOverlay();
            $.ajax({
                type: "POST",
                url: WEBSERVICE_URL + "Userwebservices.asmx/EditNickname",
                data: JSON.stringify(parameters),
                dataType: "json",
                contentType: "application/json",
                success: function (result) {
                    var parsedJSON = servers = $.parseJSON(result.d);
                    console.log(parsedJSON);
                    if (parsedJSON.success) {
                        var id = parsedJSON.id;

                        initializeUsers();
                        hideLoadingOverlay();

                        $("#editModal").modal('hide');

                        $("#editModal").find(':input').each(function () {
                            if (this.type == 'checkbox' || this.type == 'radio') {
                                this.checked = false;
                            }
                            else {
                                $(this).val('');
                            }
                        });

                        displayMessage("alert-success", "Nickname updated successfully.");
                    }
                    else {
                        hideLoadingOverlay();
                        displayMessage("alert-danger", "Something went wrong while updating your nickname: " + parsedJSON.message);
                    }
                },
                error: function (jqXHR, error, errorThrown) {
                    if (jqXHR.status && jqXHR.status == 400) {
                        alert(jqXHR.responseText);
                    } else {
                        hideLoadingOverlay();
                        alert("Something went wrong.");
                        console.log(jqXHR);
                        console.log(error);
                    }
                }
            });
        }

        //Delete User
        function deleteUser() {
            if (selectedUser == undefined)
                return;

            var parameters = {};
            parameters.id = selectedUser.user.ID;
            parameters.password = $("#DeletePassword").val();

            showLoadingOverlay();
            $.ajax({
                type: "POST",
                url: WEBSERVICE_URL + "UserWebservices.asmx/DeleteUser",
                data: JSON.stringify(parameters),
                dataType: "json",
                contentType: "application/json",
                success: function (result) {
                    var parsedJSON = servers = $.parseJSON(result.d);
                    if (parsedJSON.success) {

                        initializeUsers();
                        hideLoadingOverlay();
                        $("#deleteModal").modal('hide');
                        displayMessage("alert-success", parsedJSON.message);
                    }
                    else {
                        hideLoadingOverlay();
                        $("#deleteModal").modal('hide');
                        displayMessage("alert-danger", "Unable to delete user: " + parsedJSON.message);
                    }
                },
                error: function (jqXHR, error, errorThrown) {
                    if (jqXHR.status && jqXHR.status == 400) {
                        alert(jqXHR.responseText);
                    } else {
                        hideLoadingOverlay();
                        alert("Something went wrong");
                        console.log(jqXHR);
                        console.log(error);
                    }
                }
            });
        }

        //Update password
        function updatePassword() {
            var parameters = {};
            parameters.id = selectedUser.user.ID;
            parameters.currentPassword = $("#CurrentPassword").val();
            parameters.newPassword = $("#ChangePassword").val();

            showLoadingOverlay();
            $.ajax({
                type: "POST",
                url: WEBSERVICE_URL + "UserWebservices.asmx/UpdatePassword",
                data: JSON.stringify(parameters),
                dataType: "json",
                contentType: "application/json",
                success: function (result) {
                    var parsedJSON = servers = $.parseJSON(result.d);
                    console.log(parsedJSON);
                    if (parsedJSON.success) {

                        initializeUsers();
                        hideLoadingOverlay();
                        $("#passwordModal").modal('hide');
                        displayMessage("alert-success", parsedJSON.message);
                    }
                    else {
                        hideLoadingOverlay();
                        $("#passwordModal").modal('hide');
                        displayMessage("alert-danger", "Unable to update your password: " + parsedJSON.message);
                    }
                },
                error: function (jqXHR, error, errorThrown) {
                    if (jqXHR.status && jqXHR.status == 400) {
                        alert(jqXHR.responseText);
                    } else {
                        hideLoadingOverlay();
                        alert("Something went wrong");
                        console.log(jqXHR);
                        console.log(error);
                    }
                }
            });
        }

        //Display message
        function displayMessage(classStyle, message) {
            $("#messageDiv").hide();
            $("#messageDiv").removeClass().addClass('alert ' + classStyle + ' media fade in');
            $("#messageDiv").text(message);
            $("#messageDiv").show();
        }

    </script>

</asp:Content>

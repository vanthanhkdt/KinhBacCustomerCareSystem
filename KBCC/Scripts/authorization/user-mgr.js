function getRegisters(tb) {
    var ajaxsetup = $.ajax({
        cache: false,
        dataType: 'json',
        url: '../Account/GetRegisters'
    });
    ajaxsetup.done(function (data) {
        console.log(data);
        tb.empty();
        if (data.length > 0) {
            $.each(data, function (i, user) {
                var tr = $('<tr>');
                tr.append($('<td>', { text: user['Id'] }));
                tr.append($('<td>', { text: user['UserName'] }));
                tr.append($('<td>', { text: user['Name'] }));
                tr.append($('<td>', { text: user['Department'] }));
                tr.append($('<td>', { text: user['Email'] }));
                tr.append($('<td>', { text: user['RegistrationTime'] }));
                tr.append($('<td>', { text: user['Premises'] }));
                var ap = $('<a>');
                ap.attr('href', '#');
                ap.attr('title', 'Phê duyệt');
                ap.attr('style', 'color:green;');
                ap.addClass('fa fa-check');
                ap.on('click', function (evt) {
                    ApproveRegister(user['UserName'], tb);
                });

                var rej = $('<a>');
                rej.attr('href', '#');
                rej.attr('title', 'Từ chối. Xóa');
                rej.attr('style', 'color:red;margin-left:10px;');
                rej.addClass('fa fa-remove');
                rej.on('click', function (evt) {
                    RejectRegister(user['UserName'], tb);
                });

                var td = $('<td>');
                td.attr('style', 'text-align:center;');
                td.append(ap);
                td.append(rej);

                tr.append(td);
                tb.append(tr);
            });
        }
    });
    ajaxsetup.fail(function () {
        alert('Connect to server fail!');
    });
}

function ApproveRegister(gen, tb) {
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { gen: gen, state: true },
        url: '../Account/ApproveRegister',
        success: function (result) {
            getRegisters(tb);
        },
        fail: function () {
            alert('Connect to server fail!');
        }
    });
}

function RejectRegister(gen, tb) {
    var retVal = confirm("Bạn chắc chắn muốn từ chối người dùng này?");
    if (!retVal) {
        return false;
    }
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { gen: gen, state: false },
        url: '../Account/ApproveRegister',
        success: function (result) {
            getRegisters(tb);
        },
        fail: function () {
            alert('Connect to server fail!');
        }
    });
}

function AddPages(data, pages) {
    pages.empty();
    for (var i = 1; i <= data['total']; i++) {
        var a = $('<a>');
        a.attr('href', '#');
        a.addClass('btn btn-sm');
        a.text(i);
        if (i == data['current']) {
            a.addClass('btn-primary');
        } else {
            a.on('click', function () { pageClicked(this) });
        }
        pages.append(a);
    }
}

function pageClicked(link) {
    $('#currentPage').val(link.innerText);
    $('#btnsearch').click();
}

function ResetPassword(user) {
    $.ajax({
        cache: false,
        method: 'POST',
        data: { user: user },
        dataType: 'json',
        url: '../Account/InitUserPassword',
        success: function (result) {
            if (result.success) {
                $('#key').val(result.pwd);
                $('#keymodal').modal('show');
            } else {
                alert(result.msg);
            }

        }
    });
}

function AddUsersToTable(users, tb) {
    //alert(users.length);
    tb.empty();
    if (users.length > 0) {

        $.each(users, function (i, u) {
            //img lightbox
            var imgsrc = u.PathProfileImage;
            var atag = $('<a>', { href: imgsrc });
            let img = $('<img>', { src: imgsrc, class: 'img-circle' });
            if (u.PathProfileImage != null && u.PathProfileImage.length > 0) {
                //atag.append("<img src='" + imgsrc + "' class='img-circle' />");
                atag.append(img);
                atag.attr('data-lightbox', u.Department);
                atag.attr('data-title', u.Name);
                atag.attr('data-alt', u.Email);
            }

            var tr = $('<tr>');

            tr.append($('<td>', { text: u['UserName'] }));
            tr.append($('<td>', { class: "imgthumb" }).append(atag));
            tr.append($('<td>', { text: u['Name'] }));
            tr.append($('<td>', { text: u['Department'] }));
            tr.append($('<td>', { text: u['Email'] }));
            tr.append($('<td>', { text: u['RegistrationTime'] }));
            tr.append($('<td>', { text: u['LastLoginTime'] }));
            tr.append($('<td>', { text: u['LastIPAddress'] }));
            tr.append($('<td>', { text: u['Premises'] }));
            var lock = $('<a>');
            lock.attr('href', '#');
            if (u['IsBlocked']) {
                lock.attr('style', 'color:black;');
                lock.addClass('fa fa-lock');
                lock.attr('title', 'Click to unblock user');
            }
            else {
                lock.attr('style', 'color:green;');
                lock.addClass('fa fa-user');
                lock.attr('title', 'Khóa người dùng này');
            }
            lock.on('click', function (evt) {
                //evt.preventDefault();
                processUserState(u['UserName'], !u['IsBlocked']);
            });

            var remove = $('<a>');
            remove.attr('href', '#');
            remove.attr('style', 'color:red;margin-left:10px;');
            remove.addClass('fa fa-trash');
            remove.attr('title', 'Xóa người dùng này');
            remove.on('click', function () {
                removeUser(u['UserName']);
            });

            let pwdchange = $('<a>');
            pwdchange.attr('href', '#');
            pwdchange.addClass('btn btn-sm fa fa-refresh');
            pwdchange.attr('title', 'Đặt lại mật khẩu người dùng này');
            pwdchange.on('click', function () {
                ResetPassword(u['UserName']);
            });


            let td = $('<td>');
            //td.addClass('col-action');

            let div = $('<div>', { class: 'col-action' });

            div.append(lock);
            div.append(remove);
            div.append(pwdchange);
            tr.append(td.append(div));

            tb.append(tr);

        });
    }
}

function processUserState(gen, state) {
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { gen: gen, state: state },
        url: '../Account/ProcessUserState',
        success: function (result) {
            $('#btnsearch').click();
        },
        fail: function () {
            alert('fail');
        }
    });
}

function removeUser(gen) {
    var retval = confirm('Are you sure to delete user ?');
    if (!retval) {
        return false;
    }
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { gen: gen },
        url: '../Account/RemoveUser',
        success: function (result) {
            $('#btnsearch').click();
        }
    });
}

function getUsersByGen(gen, tb, rows, pages) {
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { gen: gen, rows: rows, currentPage: $('#currentPage').val() },
        url: '../Account/getUsersByGen',
        success: function (data) {
            AddUsersToTable(data['users'], tb);
            AddPages(data['paging'], pages);
        }
    });
}

function getUsersByPart(part, tb, rows, pages) {
    //getUsersByPart
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { part: part, rows: rows, currentPage: $('#currentPage').val() },
        url: '../Account/getUsersByPart',
        success: function (data) {
            AddUsersToTable(data['users'], tb);
            AddPages(data['paging'], pages);
        }
    });
}

function getUsersByName(name, tb, rows, pages) {
    //getUsersByName
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { name: name, rows: rows, currentPage: $('#currentPage').val() },
        url: '../Account/getUsersByName',
        success: function (data) {
            AddUsersToTable(data['users'], tb);
            AddPages(data['paging'], pages);
        }
    });
}

function getUsers(tb, rows, pages) {
    $.ajax({
        cache: false,
        dataType: 'json',
        data: { rows: rows, currentPage: $('#currentPage').val() },
        url: '../Account/getUsers',
        success: function (data) {
            AddUsersToTable(data['users'], tb);
            AddPages(data['paging'], pages);
        },
        fail: function () {
            alert('Get users list fail!');
        }
    });
}
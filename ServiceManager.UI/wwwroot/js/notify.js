var unreadCount = null
var notifyLoaded = false;

$('#navbarDropdown').on('show.bs.dropdown', function () {
    loadNotify();
});

function loadUnreadCount() {
    $.ajax({
        type: "GET",
        url: '/RequestsNotify/GetCount',
        success: function (response) {
            if (response == unreadCount)
                return;
            unreadCount = Number(response);
            
            updateNotifyIcon();
        },
        failure: function (response) {
            console.error(response.responseText);
        },
        error: function (response) {
            console.error(response.responseText);
        }
    });
}

function updateNotifyIcon() {
    $(".notify-count").text(unreadCount);

    if (unreadCount == 0) {
        $("#notify-icon").removeClass("fa-solid");
        $("#notify-icon").addClass("fa-regular");
        $(".notify-count").hide();

        $("#notifi-any").hide();
        $("#notifi-empty").show();
    }
    else {
        $("#notify-icon").removeClass("fa-regular");
        $("#notify-icon").addClass("fa-solid");
        $(".notify-count").show();

        $("#notifi-any").show();
        $("#notifi-empty").hide();
    }
}

function loadNotify() {
    if (notifyLoaded) return;

    notifyLoaded = true;
    $.ajax({
        type: "GET",
        url: '/RequestsNotify/GetAll',
        success: function (response) {
            $("#notify-container").html(response);
            $('.notify-request').on('click', function (e) {
                console.log("request/edit");
                console.log(e);
                console.log(e.currentTarget);
                showEditModal(`/Requests/Edit/${e.currentTarget.dataset.requestId}`);
            });

        },
        failure: function (response) {
            notifyLoaded = false;
            console.error(response.responseText);
        },
        error: function (response) {
            notifyLoaded = false;
            console.error(response.responseText);
        }
    });
}


function read(e) {
    $.ajax({
        type: "POST",
        url: `/RequestsNotify/Read?notifyId=${e.dataset.notifyId}`,
        success: function (response) {

        },
        failure: function (response) {
            notifyLoaded = false;
            console.error(response.responseText);
        },
        error: function (response) {
            notifyLoaded = false;
            console.error(response.responseText);
        }
    });

    $(e.parentElement.parentElement).remove();
    unreadCount--;
    updateNotifyIcon();
}

function notifyClick(url) {
    showEditModal(url);
}
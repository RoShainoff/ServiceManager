$(document).ready(function () {
    //bootstrapSelectDefaultSetting();
    removeModalInit();

    document.addEventListener("reloadend", e => {
        document.getElementById("items-count").textContent = e.detail.grid.pager.totalRows
        $("#grid-loader").attr("style", "display: none !important");
        $('#mvc-grid .selectpicker').selectpicker();
    });
    loadUnreadCount();
    MvcGridRusLang();
});

function MvcGridRusLang() {
    MvcGrid.lang = {
        default: {
            "equals": "Равно",
            "not-equals": "Не равно",
        },
        text: {
            "contains": "Содержит",
            "consists-of":"Состоит из",
            "equals": "Равно",
            "not-equals": "Не равно",
            "starts-with": "Начинается с",
            "ends-with": "Заканчивается на"
        },
        number: {
            "equals": "Равно",
            "not-equals": "Не равно",
            "less-than": "Меньше, чем",
            "greater-than": "Больше чем",
            "less-than-or-equal": "Меньше или равно",
            "greater-than-o-requal": "Больше или равно"
        },
        date: {
            "equals": "Равно",
            "not-equals": "Не равно",
            "earlier-than": "Раньше чем",
            "later-than": "Позже чем",
            "earlier-than-or-equal": "Раньше или равно",
            "later-than-or-equal": "Позже или равно"
        },
        guid: {
            "equals": "Равно",
            "not-equals": "Не равно",
        },
        filter: {
            "apply": "✓",
            "remove": "✘"
        },
        operator: {
            "select": "",
            "and": "и",
            "or": "или"
        }
    };
}

function bootstrapSelectDefaultSetting() {
    $.fn.selectpicker.Constructor.DEFAULTS.noneSelectedText = 'Выберите..';
}

function removeModalInit() {
    $('#modal-remove').on('show.bs.modal', function (event) {
        var button = event.relatedTarget
        var url = button.getAttribute('data-bs-url')
        this.dataset.url = url;
    })

    $('#modal-remove #btnConfirm').on('click', function (event) {
        var url = $('#modal-remove')[0].dataset.url;
        console.log(url)

        $.ajax({
            type: "DELETE",
            url: url,
            success: function (response) {
                removeModalHide();
            },
            failure: function (response) {
                console.error(response.responseText);
                removeModalHide();
            },
            error: function (response) {
                console.error(response.responseText);
                removeModalHide();
            }
        });
    })
}

function showEditModal(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (response) {
            $("#modal-edit-container").html(response);
            $('#modal-edit-container .selectpicker').selectpicker();

            $("#modal-edit-container form").each(function () { $.data($(this)[0], 'validator', false); });
            $.validator.unobtrusive.parse("#modal-edit-container form");

            $("#modal-edit").modal('show')
        },
        failure: function (response) {
            console.error(response.responseText);
        },
        error: function (response) {
            console.error(response.responseText);
        }
    });
}

function directoryModalCheck(response) {
    if (response.IsSuccess) {
        directoryModalHide();
    }
    else {
        toastrError(response.ErrorMessage);
    }
}

function editModalHide() {
    $('#modal-edit').modal('hide')
    gridReload();
}

function removeModalHide() {
    $('#modal-remove').modal('hide')
    gridReload();
}

function gridReload() {
    const element = document.querySelector("#mvc-grid");
    const grid = new MvcGrid(element);
    grid.reload();
}

function showLoginModal() {
    var modal = $("#loginModal");
    if (modal.length > 0) {
        modal.modal('show')
        return;
    }

    $.ajax({
        type: "POST",
        url: '/Account/LoginModal',
        success: function (response) {
            $("#for-modal").html(response);
            $("#loginModal").modal('show')
        },
        failure: function (response) {
            console.error(response.responseText);
        },
        error: function (response) {
            console.error(response.responseText);
        }
    });
}

function reloadUserName() {
    $.ajax({
        type: "POST",
        url: '/Account/GetFullName',
        success: function (response) {
            $(".user-fullname").text(response);
        }
    });
}

function formAjaxFailure(result1) {
    console.error(arguments)
    var responseJSON = result1.responseJSON;
}

function execute(action) {
    var requestId = $("#request-id").val();

    $.ajax({
        type: "GET",
        url: `/Requests/Execute?requestId=${requestId}&action=${action}`,
        success: function (response) {
            $("#modal-edit").modal('hide')

            $("#modal-execute-container").html(response);
            $('#modal-execute-container .selectpicker').selectpicker();

            $("#modal-execute-container form").each(function () { $.data($(this)[0], 'validator', false); });
            $.validator.unobtrusive.parse("#modal-execute-container form");


            $("#modal-execute").modal('show');

            $('#modal-execute').on('hide.bs.modal', function () {
                $('#modal-edit').modal('show');
            });
        },
        failure: function (response) {
            console.error(response.responseText);
        },
        error: function (response) {
            console.error(response.responseText);
        }
    });

}

function executeHide() {
    $('#modal-execute').off('hide.bs.modal');
    $("#modal-execute").modal('hide');
    $("#modal-edit").modal('hide');
    gridReload();
}
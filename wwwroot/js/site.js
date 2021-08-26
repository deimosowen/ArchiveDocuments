function formatBytes(a, b = 2) { if (0 === a) return "0 Bytes"; const c = 0 > b ? 0 : b, d = Math.floor(Math.log(a) / Math.log(1024)); return parseFloat((a / Math.pow(1024, d)).toFixed(c)) + " " + ["байт", "КБ", "МБ", "ГБ", "ТБ", "PБ", "EB", "ZB", "YB"][d] }
!function (r) { "use strict"; function t() { } t.prototype.send = function (t, o, i, n, e, s, c, a) { c = { heading: t, text: o, position: i, loaderBg: n, icon: e, hideAfter: s = s || 4e3, stack: c = c || 1 }; a && (c.showHideTransition = a), r.toast().reset("all"), r.toast(c) }, r.NotificationApp = new t, r.NotificationApp.Constructor = t }(window.jQuery);

$(function() {
    const url = new URL(window.location.href);
    const status = url.searchParams.get('actionStatus');
    switch (status) {
    case 'Success':
        $.NotificationApp.send("Готово!", "", 'top-right', '#5ba035', 'success');
        break;
    case 'AddSuccess':
        $.NotificationApp.send("Готово!", "Запись успешно добавлена.", 'top-right', '#5ba035', 'success');
        break;
    case 'EditSuccess':
        $.NotificationApp.send("Готово!", "Запись успешно изменена.", 'top-right', '#5ba035', 'success');
        break;
    case 'DeleteSuccess':
        $.NotificationApp.send("Готово!", "Запись успешно удалена.", 'top-right', '#5ba035', 'success');
        break;
    case 'AddError':
        $.NotificationApp.send("Ошибка!", "Запись не была добавлена.", 'top-right', '#bf441d', 'error');
        break;
    case 'EditError':
        $.NotificationApp.send("Ошибка!", "Запись не была изменена.", 'top-right', '#bf441d', 'error');
        break;
    case 'DeleteError':
        $.NotificationApp.send("Ошибка!", "Запись не была удалена.", 'top-right', '#bf441d', 'error');
        break;
    case 'Error':
        $.NotificationApp.send("Ошибка!",
            "Произошла ошибка. Повторите попытку позже.",
            'top-right',
            '#bf441d',
            'error');
        break;
    }
    url.searchParams.delete('actionStatus');
    history.pushState(null, document.title, url);
});

var createModal = function (data) {
    if (document.getElementById(data.id) || !data.content)
        return false;
    var divModal = document.createElement("div");
    divModal.setAttribute("id", data.id);
    divModal.setAttribute("class", "modal fade");
    divModal.setAttribute("style", "display: none");
    divModal.setAttribute("data-bs-keyboard", "false");
    divModal.innerHTML = data.content;
    if (data.backdrop) divModal.setAttribute("data-bs-backdrop", "static");
    $("body").append(divModal);
    $(divModal).modal("show");
    $(divModal).on("hidden.bs.modal", function () {
        divModal.remove();
    });
    return divModal;
};

$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});
$('[data-toggle="modal"]').on('click',
    function (e) {
        e.preventDefault();
        e.stopPropagation();
        $.ajax({
            url: $(this).attr('href'),
            success: (data) => {
                createModal({
                    content: data,
                    id: 'modal1',
                    backdrop: $(this).data('modal-backdrop') || false
                });
            },
            error: (data, textStatus) => {
            }
        });
    });
$('[data-toggle="formatBytes"]').each(function (t) { $(this).text(formatBytes($(this).text(), 1)) });

$(document).on('click', '[data-toggle="remove"]',
    function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        Swal.fire({
            title: "Удаление записи",
            html: `Вы уверены, что хотите <b class="text-danger">удалить</b> запись?<br>`,
            icon: "warning",
            showCancelButton: true,
            showLoaderOnConfirm: true,
            confirmButtonColor: '#f86262',
            confirmButtonText: 'Да, удалить',
            cancelButtonText: 'Закрыть',
            returnInputValueOnDeny: true,
            allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
            if (result.value) {
                form.submit();
            }
        });
    });
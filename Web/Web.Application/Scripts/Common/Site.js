
$(function () {
    $(window).ajaxError(function (event, xhr, settings) {
        alert('¡Error! Ocurrió un error inesperado :(');
    });

    $(window).ajaxSuccess(function (event, xhr, settings) {
        var Response = {};
        try {
            Response = xhr.responseJSON;
        } catch (e) {
            Response = null;
        }
        if (Response != null) {
            if (Response.ResponseId == 'Common') {
                //if (Response.BtnSubmitId != null) {
                //    $('#' + Response.BtnSubmitId).button('reset');
                //}
                if (Response.IsSuccess) {
                    alert('¡Éxito! Datos guardados correctamente :)');
                    if (Response.IsRedirected) {
                        window.location = Response.RedirectTo;
                    }
                    if (Response.IsReloaded) {
                        window.location = window.location.href;
                    }
                } else {
                    alert('¡Error! Ocurrió un error al guardar los datos :(');
                }
                if (Response.Modal != null) {
                    if (Response.Modal.Ajax != null) {
                        $.ajax({
                            method: Response.Modal.Ajax.Method,
                            url: Response.Modal.Ajax.Url,
                            data: Response.Modal.Ajax.Data,
                            dataType: Response.Modal.Ajax.DataType,
                            cache: Response.Modal.Ajax.Cache,
                            async: Response.Modal.Ajax.Async,
                            success: function (response) {
                                if (Response.Modal.Ajax.DataType == 'html') {
                                    $('#' + Response.Modal.Ajax.UpdateElementId).html(response);
                                }
                            }
                        });
                    }
                    if (Response.Modal.CloseModalId != null) {
                        $('#' + Response.Modal.CloseModalId).modal('hide');
                    }
                    if (Response.Modal.OpenModalId != null) {
                        $('#' + Response.Modal.OpenModalId).modal('show');
                    }
                }
                if (Response.ListRenderPartialViews != null) {
                    if (Response.ListRenderPartialViews.length > 0) {
                        for (var i = 0; i < Response.ListRenderPartialViews.length; i++) {
                            var Ajax = Response.ListRenderPartialViews[i];
                            $.ajax({
                                method: Ajax.Method,
                                url: Ajax.Url,
                                data: Ajax.Data,
                                dataType: Ajax.DataType,
                                cache: Ajax.Cache,
                                async: Ajax.Async,
                                success: function (response) {
                                    if (Ajax.DataType == 'html') {
                                        $('#' + Ajax.UpdateElementId).html(response);
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    });

    //Pace.on('hide', function () {
    //});

    //$('.btn-submit').on('click', function () {
    //    var $this = $(this);
    //    $this.button('loading');
    //});
});

// Ejecuta la apertura o cierre de modales de forma dinámica
// Url : (string) Url donde se cuentra la vista parcial a cargar
// UpdateElementId : (string) Identificador del DOM donde se reemplazará el contenido de la solicitud
// ModalIdOpen : (string) Identificador del modal a abrir después de completada la renderización de la resputa
// ModalIdClose : (string) Identificador del modal a cerrar despúes de completada la renderización del nuevo modal
function PartialAction(Url, UpdateElementId, ModalIdOpen, ModalIdClose){
    $('#' + UpdateElementId).empty();
    $.ajax({
        method: 'GET',
        url: Url,
        dataType: 'html',
        cache: false,
        async: true,
        success: function (response) {
            $('#' + UpdateElementId).html(response);
            $('#' + ModalIdOpen).modal('show');
        }
    });
    if (ModalIdClose != null) {
        $('#' + ModalIdClose).modal('hide');
    }
}

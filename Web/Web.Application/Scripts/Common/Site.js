// Variable para detener submit de forms
var submitForm = false;

// Ejecuta la llamada de las vistas parciales y reemplazar en Body.
$('body').on('click', 'a.a-ajax', function (evt) {
    evt.preventDefault();
    $this = $(this);
    AjaxHelperToRenderView(url = $this.attr('href'));
});

// Da acciones de redirección a los elementos <a></a> con la clase: a-redirect.
$('body').on('click', 'a.a-redirect', function () {
    window.location = $(this).attr('href');
});

$('#loading').hide();
var fnTimeOut;
$(window).ajaxStart(function () {
    var $this = $('div#loading');
    fnTimeOut = setTimeout(function () {
        $this.show();
    }, 500);
});
$(window).ajaxStop(function () {
    var $this = $('div#loading');
    clearTimeout(fnTimeOut);
    $this.hide();
});

//Maneja los errores de las solicitudes
$(window).ajaxError(function (event, xhr, settings) {
    //console.log(event);
    //console.log(xhr);
    //console.log(settings);

    if (xhr.status === 404) {
        AjaxHelperToRenderView(url = "/Common/NotFound");
        alertify.notify('No se encuentró la página web', 'warning', 5, function () { /*console.log('dismissed');*/ });
    }

    if (xhr.status === 500) {
        AjaxHelperToRenderView(url = "/Common/InternalError");
        alertify.notify('Ocurrió un error interno', 'error', 5, function () { /*console.log('dismissed');*/ });
    }

    if (event.type == 'ajaxError' && settings.type != 'POST') {
        //alertify.notify('Ocurrió un error inesperado', 'error', 5, function () { /*console.log('dismissed');*/ });
        submitForm = false;
    }
});

//Maneja las respuestas de las llamadas ajax de forma general
$(window).ajaxSuccess(function (event, xhr, settings) {
    var response = {}; // Response
    var customResponse = {}; // Header
    try {
        response = xhr.responseJSON;
    } catch (e) {
    }

    //try {
    //    var headers = xhr.getAllResponseHeaders().split("\n");
    //    for (var i = 0; i < headers.length; i++) {
    //        if (headers[i].includes("customresponse")) {//
    //            var stringobj = headers[i].substring(16, headers[i].length);
    //            customResponse = JSON.parse(stringobj);
    //        }
    //    }
    //} catch (e) {
    //}

    if (response != undefined) {
        HandlerResponse(response);
    }
    //if (customResponse != undefined) {
    //    HandlerHeaderResponse(customResponse);
    //}

});


//Pace.on('hide', function () {
// fin de carga ajax
//});

//$('.btn-submit').on('click', function () {
//    var $this = $(this);
//    $this.button('loading');
//});

//if (Response.BtnSubmitId != null) {
//    $('#' + Response.BtnSubmitId).button('reset');
//}

function HandlerError() {

}

// Manejador de las respuestas
function HandlerResponse(response){
    switch(response.ResponseId){
        case ('Common'):
            HandlerResponseCommon(response);
            break;
        default:
            break;
    }
}

function HandlerResponseCommon(response){
    if (response.IsSuccess) {
        alertify.notify('Datos guardados correctamente', 'success', 5, function () { /*console.log('dismissed');*/ });
        submitForm = false;
        if (response.IsRedirected) {
            window.location = response.RedirectTo;
        }
        if (response.IsReloaded) {
            window.location = window.location.href;
        }
    } else {
        alertify.notify('Ocurrió un error al guardar los datos', 'error', 5, function () { /*console.log('dismissed');*/ });
        submitForm = false;
    }
    if (response.Modal) {
        if (response.Modal.Ajax) {
            $.ajax({
                method: response.Modal.Ajax.Method,
                url: response.Modal.Ajax.Url,
                data: response.Modal.Ajax.Data,
                dataType: response.Modal.Ajax.DataType,
                cache: response.Modal.Ajax.Cache,
                async: response.Modal.Ajax.Async,
                success: function (_response) {
                    if (response.Modal.Ajax.DataType == 'html') {
                        $('#' + response.Modal.Ajax.UpdateElementId).html(_response);
                    }
                }
            });
        }
        if (response.Modal.CloseModalId) {
            $('#' + response.Modal.CloseModalId).modal('hide');
        }
        if (response.Modal.OpenModalId) {
            $('#' + response.Modal.OpenModalId).modal('show');
        }
    }
    if (response.ListRenderPartialViews) {
        if (response.ListRenderPartialViews.length > 0) {
            for (var i = 0; i < response.ListRenderPartialViews.length; i++) {
                var Ajax = response.ListRenderPartialViews[i];
                $.ajax({
                    method: Ajax.Method,
                    url: Ajax.Url,
                    data: Ajax.Data,
                    dataType: Ajax.DataType,
                    cache: Ajax.Cache,
                    async: Ajax.Async,
                    success: function (_response) {
                        if (Ajax.DataType == 'html') {
                            $('#' + Ajax.UpdateElementId).html(_response);
                        }
                    }
                });
            }
        }
    }
}

//function HandlerHeaderResponse(response) {
//    if (response.Header != undefined) {
//        var Header = response.Header;
//        Header.Title = Header.Title || '';
//        $('#header-title').html(Header.Title);
//        Header.ListLocation = Header.ListLocation || [];
//        var lis = [];
//        for (var i = 0; i < Header.ListLocation.length; i++) {
//            var item = Header.ListLocation[i];
//            let active = ''; let aclass = '';
//            if (item.IsActive) {
//                active = ' class="active"';
//            }
//            if(item.Name == "Dashboard"){
//                aclass = 'class="a-redirect"';
//            }else{
//                aclass = 'class="a-ajax"';
//            }
//            item.Url = item.Url || "#";
//            lis.push('<li' + active + '><a href="' + item.Url + '"' + aclass + '>' + item.Name + '</a></li>');
//        }
//        $('#header-location').html(lis);
//    }
//}

// Ejecuta la apertura o cierre de modales de forma dinámica
// url : (string) url donde se cuentra la vista parcial a cargar
// updateElementId : (string) Identificador del DOM donde se reemplazará el contenido de la solicitud
// modalIdOpen : (string) Identificador del modal a abrir después de completada la renderización de la resputa
// modalIdClose : (string) Identificador del modal a cerrar despúes de completada la renderización del nuevo modal
function PartialAction(url, updateElementId, modalIdOpen, modalIdClose, method){
    $('#' + updateElementId).empty();
    method = method || 'GET';
    $.ajax({
        method: method,
        url: url,
        dataType: 'html',
        cache: false,
        async: true,
        success: function (response) {
            $('#' + updateElementId).html(response);
            if (modalIdOpen) {
                $('#' + modalIdOpen).modal('show');
                AttachConfirm();
            }
        }
    });
    if (modalIdClose) {
        $('#' + modalIdClose).modal('hide');
    }
}

//Agrega el envío de los formularios para solicitar confirmación personalizada
function AttachConfirm() {
    $('form[data-ajax=true][data-confirm=true]').attr('data-ajax-begin', 'return SubmitForm(this);');
}

// Función para detener y confirmar envío de formulario data-ajax
function SubmitForm(thiss) {
    var $this = $(thiss);
    if ($this.validate()) {
        let title = $this.data('confirmTitle') || 'Guardar';
        let message = $this.data('confirmMessage') || '<h3>¿Desea continuar?</h3>Revise sus datos antes de continuar.';
        alertify.confirm(title, message,
            function () {
                submitForm = true;
                $this.submit();
            },
            function () {
                submitForm = false;
            });
    } else {
        submitForm = false;        
    }
    return submitForm;
}

// Envía la solicitud para vistas principales
function AjaxHelperToRenderView(url, targetUpdateId, method, data, dataType, cache, async) {
    method = method || 'GET';
    dataType = dataType || 'html';
    cache = cache || false;
    async = async || true;
    targetUpdateId = targetUpdateId || 'Body';
    $.ajax({
        method: method,
        url: url,
        data: data,
        dataType: dataType,
        cache: cache,
        async: async,
        success: function (response) {
            $('#' + targetUpdateId).html(response);
        }
    });
}
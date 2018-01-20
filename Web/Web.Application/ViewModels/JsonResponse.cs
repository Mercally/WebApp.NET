using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Application.ViewModels
{
    public class JsonResponse
    {
        /// <summary>
        /// Identificador de la respuesta para recibir los diferentes tipos de respuesta y darle tratamiento especificado
        /// desde la carga ajax.
        /// </summary>
        public string ResponseId { get; set; }
        /// <summary>
        /// Determina si la transacción se realizó exitosamente
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Determina si se hará una redirección
        /// </summary>
        public bool IsRedirected { get; set; }
        /// <summary>
        /// Cadena de mensaje exitoso a mostrar
        /// </summary>
        public string MessageSuccess { get; set; }
        /// <summary>
        /// Cadena de mensaje de error a mostrar
        /// </summary>
        public string MessageError { get; set; }
        /// <summary>
        /// Url que redireccionará cuando la propiedad IsRedirected es verdadera
        /// </summary>
        public string RedirectTo { get; set; }
        /// <summary>
        /// Realiza un refrezco completo de la url actual
        /// </summary>
        public bool IsReloaded { get; set; }
        /// <summary>
        /// Objecto modal, proporciona propiedades para manejo de modales
        /// </summary>
        public Modal Modal { get; set; }
        /// <summary>
        /// Opciones para renderizar las vistas parciales
        /// </summary>
        public List<Ajax> ListRenderPartialViews { get; set; }
        /// <summary>
        /// Opciones para renderizar las vistas en el body
        /// </summary>
        public List<Ajax> ListRenderViews { get; set; }
        /// <summary>
        /// Identificador del botón submit para detener imagen de carga
        /// </summary>
        public string BtnSubmitId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Header Header { get; set; }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        /// <param name="ResponseId">Identificador de la respuesta</param>
        /// <param name="MessageSuccess">Mensaje de éxito</param>
        /// <param name="MessageError">Mensaje de error</param>
        public JsonResponse(string ResponseId = "Common", string MessageSuccess = null, string MessageError = null)
        {
            if (!string.IsNullOrEmpty(MessageSuccess))
            {
                this.MessageSuccess = MessageSuccess;
            }
            else
            {
                this.MessageSuccess = "Datos guardados exitosamente!";
            }

            if (!string.IsNullOrEmpty(MessageError))
            {
                this.MessageError = MessageError;
            }
            else
            {
                this.MessageError = "Error al guardar los datos!";
            }

            this.ResponseId = ResponseId;
            this.Modal = null;
            this.Header = null;
            this.ListRenderPartialViews = new List<Ajax>();
        }
    }

    public class Modal
    {
        /// <summary>
        /// (Opcional) Identificador del elemento modal a cerrar
        /// </summary>
        public string CloseModalId { get; set; }
        /// <summary>
        /// (Opcional) Identificador del elemento modal a abrir
        /// </summary>
        public string OpenModalId { get; set; }
        /// <summary>
        /// Identificador del elemento donde se rendizará el resultado de la carga ajax
        /// </summary>
        public string UpdateElementId { get; set; }
        /// <summary>
        /// (Opcional) Objeto para construir una petición ajax de renderizado de resultados
        /// </summary>
        public Ajax Ajax { get; set; }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Modal()
        {
            Ajax = null;
        }
    }

    public class Ajax
    {
        public Ajax()
        {
            Async = true;
            Cache = false;
            Method = "GET";
            DataType = "html";
        }
        
        /// <summary>
        /// Url de la petición
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Parametro para habilitar funcion asincrona
        /// </summary>
        public bool Async { get; set; }
        /// <summary>
        /// Parametro para habilitar el cache
        /// </summary>
        public bool Cache { get; set; }
        /// <summary>
        /// (Opcional) Tipo de datos que retorna la solicitud (Default: Html)
        /// html, json, xml, script
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// (Opcional) Representación JsonP de la data a enviar
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// (Opcional) Método de la petición Post o Get, Default: Get
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Identificador del elemento donde se rendizará el resultado de la carga ajax
        /// </summary>
        public string UpdateElementId { get; set; }
    }

    public class Header
    {
        public string Title { get; set; }
        public List<Location> ListLocation { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}
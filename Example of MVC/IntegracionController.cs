using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Globalization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ARANET.Controllers.Construccion
{

    public class IntegracionController : Controller
    {
        #region Variables
            protected const string strModuloKey_DesarrolloGrupo = "ModuloId_Integracion_DesarrolloGrupos";
        #endregion
        
        #region DesarrolloGrupos
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public PartialViewResult DesarrolloGrupos_Index()
        {
            if (Session["UsuarioModel"] != null)
            {
                //Se valida permisos de lectura
                //--------------------------------
                Serv_Modulo Serv_M = new Serv_Modulo();
                Modulo_Usuario objPermisos = new Modulo_Usuario();
                objPermisos = Serv_M.ObtenerPermisosUsuario(strModuloKey_DesarrolloGrupo);
                if (objPermisos.Lectura == true)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string json = js.Serialize(objPermisos);
                    ViewBag.Permisos = json;

                    return PartialView(@"~\Views\Construccion\Integracion\DesarrolloGrupos\_Index.cshtml");
                }
                else
                {
                    return PartialView(@"~\Views\Seguridad\_RedirectToHome.cshtml");
                }
            }
            else
            {
                return PartialView(@"~\Views\Seguridad\_RedirectToLogin.cshtml");
            }
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult DesarrolloGrupos_ObtenerGrupos()
        {
            AjaxResult objResult = new AjaxResult();

            try
            {
                Serv_DesarrolloGrupos objServ = new Serv_DesarrolloGrupos();
                List<DesarrolloGrupos> lstDesarrolloGrupos;

                if (Session["UsuarioModel"] != null)
                {
                    Usuario objUsuario = (Usuario)Session["UsuarioModel"];

                    string Grupo_Nombre = "";
                    int Estatus = -1;

                    if (Request["Grupo_Nombre"] != null)
                    {
                        if (string.IsNullOrEmpty(Request["Grupo_Nombre"].ToString()) == false)
                            Grupo_Nombre = Request["Grupo_Nombre"].ToString();
                    }

                    if (Request["EstatusFiltro"] != null)
                    {
                        if (string.IsNullOrEmpty(Request["EstatusFiltro"].ToString()) == false)
                            Estatus = int.Parse(Request["EstatusFiltro"].ToString());
                    }

                    lstDesarrolloGrupos = objServ.ObtenerGrupos(Grupo_Nombre, Estatus);

                    objResult.AJAX_Estatus = 1;
                    objResult.AJAX_Descripcion = "";

                    return new System.Web.Mvc.JsonResult()
                    {
                        Data = lstDesarrolloGrupos,
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    objResult.AJAX_Estatus = 2;
                    objResult.AJAX_Descripcion = "La sesión ha caducado, favor de ingresar nuevamente al sistema";
                }
            }
            catch (Exception ex)
            {
                //Registro de error en base de datos
                ErrorLogDB.RegistraError(this, ex);

                //Muestra de error en pantalla
                objResult.AJAX_Estatus = 2;
                objResult.AJAX_Descripcion = "Ocurrio un error al consultar la lista de DesarrolloGrupos.";
            }

            return (Json(objResult, JsonRequestBehavior.AllowGet));
        }


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult DesarrolloGrupos_ObtenerGruposActivos(string Grupo_Nombre)
        {
            AjaxResult objResult = new AjaxResult();

            try
            {
                Serv_DesarrolloGrupos objServ = new Serv_DesarrolloGrupos();
                List<DesarrolloGrupos> lstDesarrolloGrupos;

                if (Session["UsuarioModel"] != null)
                {
                    Usuario objUsuario = (Usuario)Session["UsuarioModel"];

                    lstDesarrolloGrupos = objServ.ObtenerGrupos(Grupo_Nombre, 1);

                    objResult.AJAX_Estatus = 1;
                    objResult.AJAX_Descripcion = "";

                    return new System.Web.Mvc.JsonResult()
                    {
                        Data = lstDesarrolloGrupos,
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    objResult.AJAX_Estatus = 2;
                    objResult.AJAX_Descripcion = "La sesión ha caducado, favor de ingresar nuevamente al sistema";
                }
            }
            catch (Exception ex)
            {
                //Registro de error en base de datos
                ErrorLogDB.RegistraError(this, ex);

                //Muestra de error en pantalla
                objResult.AJAX_Estatus = 2;
                objResult.AJAX_Descripcion = "Ocurrio un error al consultar la lista de DesarrolloGrupos.";
            }

            return (Json(objResult, JsonRequestBehavior.AllowGet));
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public PartialViewResult DesarrolloGrupos_Detalle(int Grupo_Id)
        {
            if (Session["UsuarioModel"] != null)
            {
                //Se valida permisos de lectura
                //--------------------------------
                Serv_Modulo Serv_M = new Serv_Modulo();
                Modulo_Usuario objPermisos = new Modulo_Usuario();
                objPermisos = Serv_M.ObtenerPermisosUsuario(strModuloKey_DesarrolloGrupo);

                if (objPermisos.Lectura == true)
                {
                    try
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string json = js.Serialize(objPermisos);
                        ViewBag.Permisos = json;

                        DesarrolloGrupos objDesarrolloGrupos = new DesarrolloGrupos();
                        Serv_DesarrolloGrupos objServ = new Serv_DesarrolloGrupos();

                        if (Grupo_Id != 0)
                        {
                            objDesarrolloGrupos = objServ.ObtenerPorId(Grupo_Id);
                        }

                        return PartialView(@"~\Views\Construccion\Integracion\DesarrolloGrupos\_Detalle.cshtml", objDesarrolloGrupos);
                    }
                    catch (Exception ex)
                    {
                        //Registro de error en base de datos
                        ErrorLogDB.RegistraError(this, ex);
                        return PartialView();
                    }
                }
                else
                {
                    return PartialView(@"~\Views\Seguridad\_RedirectToHome.cshtml");
                }
            }
            else
            {
                return PartialView(@"~\Views\Seguridad\_RedirectToLogin.cshtml");
            }
        }

        [HttpPost]
        public JsonResult DesarrolloGrupos_Inserta(DesarrolloGrupos objDesarrolloGrupos)
        {
            AjaxResult objResult = new AjaxResult();
            try
            {
                // validar permisos
                Serv_Modulo servModulo = new Serv_Modulo();
                Modulo_Usuario objPermisos = new Modulo_Usuario();
                objPermisos = servModulo.ObtenerPermisosUsuario(strModuloKey_DesarrolloGrupo);
                if (objPermisos.Insertar)
                {

                    //
                    Serv_DesarrolloGrupos objServ = new Serv_DesarrolloGrupos();
                    int iEstatus = 0;
                    string strDescripcion = "";

                    int usuarioID = 0;

                    if (Session["UsuarioModel"] != null)
                    {
                        Usuario objUsr = (Usuario)Session["UsuarioModel"];

                        usuarioID = objUsr.UsuarioID;
                    }

                    objServ.Inserta(objDesarrolloGrupos.Grupo_Nombre, objDesarrolloGrupos.Estatus, usuarioID, ref iEstatus, ref strDescripcion);

                    if (iEstatus == 1)
                    {
                        objResult.AJAX_Estatus = iEstatus;
                        objResult.AJAX_Descripcion = strDescripcion;
                    }
                    else
                    {
                        // Se manda una excepción
                        throw new ApplicationException(strDescripcion);
                    }
                }
                else
                {
                    objResult.AJAX_Estatus = 2;
                    objResult.AJAX_Descripcion = "El usuario no tiene permiso de crear registros.";
                }
            }
            catch (Exception e)
            {
                ErrorLogDB.RegistraError(this, e);


                //Muestra de error en pantalla
                objResult.AJAX_Estatus = 2;
                objResult.AJAX_Descripcion = "Ocurrio un error al agregar el registro.";
            }

            return (Json(objResult, JsonRequestBehavior.AllowGet));
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult DesarrolloGrupos_Actualiza(DesarrolloGrupos objDesarrolloGrupos)
        {
            AjaxResult objResult = new AjaxResult();

            try
            {
                Serv_DesarrolloGrupos objServ = new Serv_DesarrolloGrupos();

                int iEstatus = -1;
                string strDescripcion = "";
                int UsuarioID = 0;

                if (Session["UsuarioModel"] != null)
                {
                    Usuario objUsr = (Usuario)Session["UsuarioModel"];

                    UsuarioID = objUsr.UsuarioID;

                    objServ.Actualiza(objDesarrolloGrupos.Grupo_Id, objDesarrolloGrupos.Grupo_Nombre, objDesarrolloGrupos.Estatus, UsuarioID, ref iEstatus, ref strDescripcion);

                    if (iEstatus == 1) //Actualización exitosa
                    {
                        objResult.AJAX_Estatus = 1;
                        objResult.AJAX_Descripcion = strDescripcion;
                    }
                    else
                    {
                        //Se manda una excepcion
                        throw new ApplicationException(strDescripcion);
                    }
                }
                else
                {
                    objResult.AJAX_Estatus = 2;
                    objResult.AJAX_Descripcion = "La sesión finalizó. Inicie sesión nuevamente.";
                }
            }
            catch (Exception ex)
            {
                //Registro de error en base de datos
                ErrorLogDB.RegistraError(this, ex);

                //Muestra de error en pantalla
                objResult.AJAX_Estatus = 2;
                objResult.AJAX_Descripcion = "Ocurrio un error al actualizar.";
            }

            return (Json(objResult, JsonRequestBehavior.AllowGet));
        }
        #endregion
        

    }
}

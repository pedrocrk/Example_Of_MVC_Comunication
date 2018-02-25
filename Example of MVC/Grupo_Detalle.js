var ActualizaInfo = 0;

function Cambia(objchk, objNombre) {
    if ($(objchk).is(':checked')) {
        $('#' + objNombre).prop('checked', true);
    }
    else {
        $('#' + objNombre).prop('checked', false);
    }
}

//Funcion encargada ejecutar el guardado de la información
//--------------------------------------------------------
function Actualiza() {

    $("#divLoadingM").show();

    $.post("/Integracion/DesarrolloGrupos_Actualiza", $("#frmDesarrolloGrupos").serialize())
        .done(function (data) {
            if (data.AJAX_Estatus == 1) { //Actualizacion correcta

                Notificacion('Administración de DesarrolloGrupos', data.AJAX_Descripcion, 'success');
                ActualizaInfo = 1;
                $('#btnCloseW').click();
            }
            else if (data.AJAX_Estatus == 2) { //Error
                Notificacion('Administración de DesarrolloGrupos', data.AJAX_Descripcion, 'error');
            }

            GetDesarrolloGrupos();

            $("#divLoadingM").hide();
        })
        .fail(function (data) {
            Notificacion('Administración de DesarrolloGrupos', 'Ocurrio un error en la actualización del registro', 'error');
            $("#divLoadingM").hide();
        });
}

//Funcion para deshabilitar controles cuando no se tiene permisos de edicion o creación
//----------------------------------------------------------------------------------------
function DeshabilitaControles() {
    $('#btnGuardar').hide(); //No tiene permisos de edición
    $("#Grupo_Nombre").prop('disabled', true);
    $("#Estatus2").prop('disabled', true);
}

//Funcion encargada de controlar la seguridad 
//--------------------------------------------
function SeguridadEdicion() {

    var objPermisosDetalle = JSON.parse($("#PermisoUsrDetalle").val());

    if ($("#Grupo_Id").val() == 0 || $("#Grupo_Id").val() == '') {
        if (objPermisosDetalle.Insertar == 0) {
            DeshabilitaControles();
        }
    }
    else {
        if (objPermisosDetalle.Edicion == 0) {
            DeshabilitaControles();
        }
    }

}

//Funcion encargada ejecutar el guardado de la información
//--------------------------------------------------------
function Inserta() {

    $("#divLoadingM").show();

    $.post("/Integracion/DesarrolloGrupos_Inserta", $("#frmDesarrolloGrupos").serialize())
        .done(function (data) {
            if (data.AJAX_Estatus == 1) { //Actualizacion correcta
                Notificacion('Administración de DesarrolloGrupos', data.AJAX_Descripcion, 'success');
                ActualizaInfo = 1;
                $('#btnCloseW').click();
            }
            else if (data.AJAX_Estatus == 2) { //Error
                Notificacion('Administración de DesarrolloGrupos', data.AJAX_Descripcion, 'error');
            }

            GetDesarrolloGrupos();

            $("#divLoadingM").hide();
        })
        .fail(function (data) {
            Notificacion('Administración de DesarrolloGrupos', 'Ocurrio un error en la creación del registro', 'error');
            $("#divLoadingM").hide();
        });
}

//Funcion principal de guardado
//-----------------------------
function Guarda() {
    if ($("#frmDesarrolloGrupos").valid()) {
        if ($("#Grupo_Id").val() == '0' || $("#Grupo_Id").val() == '') {
            Inserta();
        } else {
            Actualiza();
        }
    }

}

function InicializarDetalle() {

    Inicializa_Validacion($("#frmDesarrolloGrupos"));

    $("#btnGuardar").click(function () {
        Guarda();
        return (false);
    });

    //Valida la seguridad en la vista
    SeguridadEdicion();

    $('#ModalDesarrolloGrupos').on('hidden.bs.modal', function (e) {
        if (ActualizaInfo == 1) {
            GetDesarrolloGrupos();
        }
    })

    $('#ModalDesarrolloGrupos').modal({
        keyboard: false,
        backdrop: 'static',
        show: true
    });

};

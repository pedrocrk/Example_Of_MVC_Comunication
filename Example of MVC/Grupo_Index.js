//SE CLOCAN LOS ESTILOS PARA LOS ICONOS DE LAS COLUMNAS DE TODOS LOS GRID
//-------------------------------------------------------------------------
function onGridDataBound(e) {
    $(".k-grid-Editar").removeClass("k-button").addClass("fa fa-pencil-square-o fa-lg");
}

//Apertura de ventana de edición
//------------------------------
function DetalleDesarrolloGrupos(e) {

    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    $("#divLoading").show();

    $('#divDetalle').load('/Integracion/DesarrolloGrupos_Detalle', { Grupo_Id: dataItem.Grupo_Id }, function () {
        $("#divLoading").hide();
    });
}

///Se abre la ventana Nuevo.cshtml para agregar un nuevo elemento
//----------------------------------------------------------------
function NuevoDesarrolloGrupos() {
    $("#divLoading").show();

    $('#divDetalle').load('/Integracion/DesarrolloGrupos_Detalle', { Grupo_Id: 0 }, function () {
        $("#divLoading").hide();
    });

    return (false);
}

//Funcion encargada del llenado del grid
//----------------------------------------
function GetDesarrolloGrupos() {
    
    $("#divLoading").show();
    var dt = new Date();

    $.post("/Integracion/DesarrolloGrupos_ObtenerGrupos", $("#frmFiltrosDesarrolloGrupos").serialize())
        .done(function (data) {
            if (data.AJAX_Estatus == 2) {
                Notificacion('Administración de DesarrolloGrupos', data.AJAX_Descripcion, 'error');
            }
            else {
                KendoGrid_Bind('gridDesarrolloGrupos', data);

                //Se oculta sección de filtrado
                OcultaSeccionFiltro();
            }

            $("#divLoading").hide();
        })
        .fail(function () {
            Notificacion('Administración de DesarrolloGrupos', 'Ocurrio un error al obtener los registros', 'error');
            $("#divLoading").hide();
        });
}

//Funcion encargada de controlar la seguridad 
//--------------------------------------------
function SeguridadIndex() {
    var permisosIndex = JSON.parse($("#PermisoUsr").val());

    if (permisosIndex.Insertar == 0) {
        $('#btnNuevo').hide(); //No tiene permisos de creación
    }
}

function Inicializa() {

    GetDesarrolloGrupos();

    //Valida la seguridad en la vista
    SeguridadIndex();

    $("#btnBuscar").click(function () {
        GetDesarrolloGrupos();
        return (false);
    });
};

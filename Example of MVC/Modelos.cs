using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kendo.Mvc.UI;

namespace PROYECT.Modelos
{
    public class DesarrolloGrupos
    {
        public int Grupo_Id { get; set; }
        [Required(ErrorMessage = "El nombre del grupo es requerido")]
        public string Grupo_Nombre { get; set; }
        public bool Estatus { get; set; }
        public string Estatus_Nombre { get; set; }
    }
}

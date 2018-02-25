using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARANET.Modelos.Seguridad;

namespace PROYECT.Modelos
{
    public class Serv_DesarrolloGrupos
    {
        public List<DesarrolloGrupos> ObtenerGrupos(string Grupo_Nombre, int Estatus)
        {
            List<DesarrolloGrupos> lstDesarrolloGrupos = new List<DesarrolloGrupos>();
            DataTable dtResult = new DataTable();

            SqlParameter[] arrSqlParam = new SqlParameter[3];
            arrSqlParam[0] = new SqlParameter("@OPCION", SqlDbType.Int);
            arrSqlParam[0].Value = 1;

            arrSqlParam[1] = new SqlParameter("@Grupo_Nombre", SqlDbType.VarChar);
            if (string.IsNullOrEmpty(Grupo_Nombre))
            {
                arrSqlParam[1].Value = DBNull.Value;
            }
            else
            {
                arrSqlParam[1].Value = Grupo_Nombre;
            }

            arrSqlParam[2] = new SqlParameter("@Estatus", SqlDbType.Bit);
            if (Estatus == 0 || Estatus == 1)
            {
                arrSqlParam[2].Value = Estatus; 
            }
            else
            {
                arrSqlParam[2].Value = DBNull.Value;
            }

            dtResult = SQL_Conn.ExecuteDataTable(CommandType.StoredProcedure, "Integracion_DesarrolloGrupos_SP", arrSqlParam);

            DesarrolloGrupos desarrolloGrupos = new DesarrolloGrupos();

            foreach (DataRow row in dtResult.Rows)
            {
                desarrolloGrupos = new DesarrolloGrupos();

                if (row["Grupo_Id"].ToString() != "" && row["Grupo_Id"] != null)
                    desarrolloGrupos.Grupo_Id = Convert.ToInt32(row["Grupo_Id"]);

                desarrolloGrupos.Grupo_Nombre = row["Grupo_Nombre"].ToString();

                if (row["Estatus"].ToString() != "" && row["Estatus"] != null)
                    desarrolloGrupos.Estatus = Convert.ToBoolean(row["Estatus"]);

                desarrolloGrupos.Estatus_Nombre = row["Estatus_Nombre"].ToString();

                lstDesarrolloGrupos.Add(desarrolloGrupos);

                desarrolloGrupos = null;
            }

            return lstDesarrolloGrupos;
        }

        public DesarrolloGrupos ObtenerPorId(int Grupo_Id)
        {
            DataTable dtResult = new DataTable();

            SqlParameter[] arrSqlParam = new SqlParameter[2];
            arrSqlParam[0] = new SqlParameter("@OPCION", SqlDbType.Int);
            arrSqlParam[0].Value = 2;

            arrSqlParam[1] = new SqlParameter("@PK_Grupo_Id", SqlDbType.Int);
            arrSqlParam[1].Value = Grupo_Id;

            dtResult = SQL_Conn.ExecuteDataTable(CommandType.StoredProcedure, "Integracion_DesarrolloGrupos_SP", arrSqlParam);

            DesarrolloGrupos desarrolloGrupos = new DesarrolloGrupos();

            if (dtResult.Rows.Count > 0)
            {
                DataRow row = dtResult.Rows[0];

                if (row["Grupo_Id"].ToString() != "" && row["Grupo_ID"] != null)
                    desarrolloGrupos.Grupo_Id = Convert.ToInt32(row["Grupo_ID"]);

                desarrolloGrupos.Grupo_Nombre = row["Grupo_Nombre"].ToString();

                if (row["Estatus"].ToString() != "" && row["Estatus"] != null)
                    desarrolloGrupos.Estatus = Convert.ToBoolean(row["Estatus"]);

                desarrolloGrupos.Estatus_Nombre = row["Estatus_Nombre"].ToString();
            }

            return desarrolloGrupos;
        }

        public void Actualiza(int Grupo_Id, string Grupos_Nombre, bool Estatus, int Usuario_Id, ref int iEstatus, ref string strDescripcion)
        {
            DataTable dtResult = new DataTable();

            try
            {
                SqlParameter[] arrSqlParam = new SqlParameter[5];
                arrSqlParam[0] = new SqlParameter("@OPCION", SqlDbType.Int);
                arrSqlParam[0].Value = 3;

                arrSqlParam[1] = new SqlParameter("@PK_Grupo_Id", SqlDbType.Int);
                arrSqlParam[1].Value = Grupo_Id;

                arrSqlParam[2] = new SqlParameter("@Grupo_Nombre", SqlDbType.VarChar);
                arrSqlParam[2].Value = Grupos_Nombre;

                arrSqlParam[3] = new SqlParameter("@Estatus", SqlDbType.Bit);
                arrSqlParam[3].Value = Estatus;

                arrSqlParam[4] = new SqlParameter("@FK_Usuario_Id", SqlDbType.Int);
                arrSqlParam[4].Value = Usuario_Id;

                dtResult = SQL_Conn.ExecuteDataTable(CommandType.StoredProcedure, "Integracion_DesarrolloGrupos_SP", arrSqlParam);

                if (dtResult.Rows.Count > 0)
                {
                    iEstatus = int.Parse(dtResult.Rows[0]["Estatus"].ToString());
                    strDescripcion = dtResult.Rows[0]["Descripcion_Estatus"].ToString();
                }
            }
            catch (Exception Ex)
            {
                iEstatus = 0;
                strDescripcion = "Error al realizar la operación: " + Ex.Message;
            }
        }

        public void Inserta(string Grupos_Nombre, bool Estatus, int Usuario_Id, ref int iEstatus, ref string strDescripcion)
        {
            DataTable dtResult = new DataTable();

            try
            {
                SqlParameter[] arrSqlParam = new SqlParameter[4];
                arrSqlParam[0] = new SqlParameter("@OPCION", SqlDbType.Int);
                arrSqlParam[0].Value = 4;

                arrSqlParam[1] = new SqlParameter("@Grupo_Nombre", SqlDbType.NVarChar);
                arrSqlParam[1].Value = Grupos_Nombre;

                arrSqlParam[2] = new SqlParameter("@Estatus", SqlDbType.Int);
                arrSqlParam[2].Value = Estatus;

                arrSqlParam[3] = new SqlParameter("@FK_Usuario_Id", SqlDbType.Int);
                arrSqlParam[3].Value = Usuario_Id;

                dtResult = SQL_Conn.ExecuteDataTable(CommandType.StoredProcedure, "Integracion_DesarrolloGrupos_SP", arrSqlParam);

                if (dtResult.Rows.Count > 0)
                {
                    /// = int.Parse(dtResult.Rows[0]["TipoProyecto_Id"].ToString());  revisar
                    iEstatus = int.Parse(dtResult.Rows[0]["Estatus"].ToString());
                    strDescripcion = dtResult.Rows[0]["Descripcion_Estatus"].ToString();
                }
            }
            catch (Exception Ex)
            {
                iEstatus = 0;
                strDescripcion = "Error al realizar la operación: " + Ex.Message;
            }
        }
    }

 
}

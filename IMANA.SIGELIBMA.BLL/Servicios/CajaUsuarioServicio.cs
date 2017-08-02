using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMANA.SIGELIBMA.BLL.Servicios
{
    public class CajaUsuarioServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public CajaUsuarioServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<CajaUsuario> ObtenerTodos() {
            try
            {
                List<CajaUsuario> cajasUsuarios = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajasUsuarios = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                cajasUsuarios = unitOfWork.Repository<CajaUsuario>().GetAll().ToList();
                
                return cajasUsuarios;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public CajaUsuario ObtenerPorId(CajaUsuario cajaUsuariop)
        {
            try
            {
                CajaUsuario cajaUsuario = null;

                cajaUsuario = (CajaUsuario) unitOfWork.Repository<CajaUsuario>().GetById(cajaUsuariop.Sesion);

                return cajaUsuario;
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        public bool Agregar(CajaUsuario cajaUsuariop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajasUsuarios = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<CajaUsuario>().Add(cajaUsuariop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(CajaUsuario cajaUsuariop)
        {
            try
            {
 
                unitOfWork.Repository<CajaUsuario>().Update(cajaUsuariop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(CajaUsuario cajaUsuariop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajasUsuarios = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<CajaUsuario>().Update(cajaUsuariop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}

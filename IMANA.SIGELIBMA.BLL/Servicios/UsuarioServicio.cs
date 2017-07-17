

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;
using System.Data.Entity;

namespace IMANA.SIGELIBMA.BLL.Servicios
{
    public class UsuarioServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public UsuarioServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Usuario> ObtenerTodos() {
            try
            {
                List<Usuario> usuarios = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    usuarios = unitOfWork.Repository<Usuarioe>().ObtenerTodos().ToList();
                //}
                usuarios = unitOfWork.Repository<Usuario>().GetAll().ToList();
                
                return usuarios;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Usuario ObtenerPorId(Usuario usuariop)
        {
            try
            {
                Usuario usuario = null;

                usuario = (Usuario)unitOfWork.Repository<Usuario>().GetById(usuariop.Cedula);

                return usuario;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Usuario usuariop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    usuarios = unitOfWork.Repository<Usuarioe>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Usuario>().Add(usuariop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Usuario usuariop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    usuarios = unitOfWork.Repository<Usuarioe>().ObtenerTodos().ToList();
                //}
                usuariop.Estado = 0;
                unitOfWork.Repository<Usuario>().Update(usuariop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Usuario usuariop)
        {
            try
            {
                unitOfWork.Repository<Usuario>().Update(usuariop);
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

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
    public class UsuarioRolesServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public UsuarioRolesServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<UsuarioRoles> ObtenerTodos() {
            try
            {
                List<UsuarioRoles> usuariosRoles = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    usuariosRoles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                usuariosRoles = unitOfWork.Repository<UsuarioRoles>().GetAll().ToList();

                return usuariosRoles;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public UsuarioRoles ObtenerPorId(UsuarioRoles usuarioRolesp)
        {
            try
            {
                UsuarioRoles usuarioRoles = null;

                //usuarioRoles = (UsuarioRoles) unitOfWork.Repository<UsuarioRoles>().GetById(usuarioRolesp.Usuario, usuarioRoles.Rol);

                return usuarioRoles;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(UsuarioRoles usuarioRolesp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    usuariosRoles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<UsuarioRoles>().Add(usuarioRolesp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(UsuarioRoles usuarioRolesp)
        {
            try
            {
 
                unitOfWork.Repository<UsuarioRoles>().Update(usuarioRolesp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(UsuarioRoles usuarioRolesp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    usuariosRoles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<UsuarioRoles>().Update(usuarioRolesp);
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

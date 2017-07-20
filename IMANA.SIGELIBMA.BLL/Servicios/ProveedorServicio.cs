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
    public class ProveedorServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public ProveedorServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Proveedor> ObtenerTodos() {
            try
            {
                List<Proveedor> proveedores = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    proveedores = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                proveedores = unitOfWork.Repository<Proveedor>().GetAll().ToList();
                
                return proveedores;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Proveedor ObtenerPorId(Proveedor proveedorp)
        {
            try
            {
                Proveedor proveedor = null;

                proveedor = (Proveedor) unitOfWork.Repository<Proveedor>().GetById(proveedorp.Codigo);

                return proveedor;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Proveedor proveedorp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    proveedores = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Proveedor>().Add(proveedorp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Proveedor proveedorp)
        {
            try
            {
 
                unitOfWork.Repository<Proveedor>().Update(proveedorp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Proveedor proveedorp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    proveedores = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Proveedor>().Update(proveedorp);
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

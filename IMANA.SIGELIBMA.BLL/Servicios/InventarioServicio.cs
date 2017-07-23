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
    public class InventarioServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public InventarioServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Inventario> ObtenerTodos() {
            try
            {
                List<Inventario> inventarios = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    inventarios = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                inventarios = unitOfWork.Repository<Inventario>().GetAll().ToList();
                
                return inventarios;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Inventario ObtenerPorId(Inventario inventariop)
        {
            try
            {
                Inventario inventario = null;

                inventario = (Inventario) unitOfWork.Repository<Inventario>().GetById(inventariop.CodigoLibro);

                return inventario;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Inventario inventariop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    inventarios = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Inventario>().Add(inventariop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Inventario inventariop)
        {
            try
            {
 
                unitOfWork.Repository<Inventario>().Update(inventariop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Inventario inventariop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    inventarios = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Inventario>().Update(inventariop);
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

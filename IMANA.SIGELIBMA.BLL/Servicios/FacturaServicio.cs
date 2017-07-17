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
    public class FacturaServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public FacturaServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Factura> ObtenerTodos() {
            try
            {
                List<Factura> facturas = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    facturas = unitOfWork.Repository<Facturae>().ObtenerTodos().ToList();
                //}
                facturas = unitOfWork.Repository<Factura>().GetAll().ToList();
                
                return facturas;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Factura ObtenerPorId(Factura facturap)
        {
            try
            {
                Factura rol = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    facturas = unitOfWork.Repository<Facturae>().ObtenerTodos().ToList();
                //}
                rol = (Factura) unitOfWork.Repository<Factura>().GetById(facturap.Numero);

                return rol;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Factura facturap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    facturas = unitOfWork.Repository<Facturae>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Factura>().Add(facturap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Factura facturap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    facturas = unitOfWork.Repository<Facturae>().ObtenerTodos().ToList();
                //}
                facturap.Estado = 0;
                unitOfWork.Repository<Factura>().Update(facturap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Factura facturap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    facturas = unitOfWork.Repository<Facturae>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Factura>().Update(facturap);
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

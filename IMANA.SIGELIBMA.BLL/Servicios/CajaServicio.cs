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
    public class CajaServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public CajaServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Caja> ObtenerTodos() {
            try
            {
                List<Caja> cajas = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajas = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                cajas = unitOfWork.Repository<Caja>().GetAll().ToList();
                
                return cajas;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Caja ObtenerPorId(Caja cajap)
        {
            try
            {
                Caja caja = null;

                caja = (Caja) unitOfWork.Repository<Caja>().GetById(cajap.Codigo);

                return caja;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public int Abrir(Caja cajap, Usuario usuariop, decimal monto)
        {
            try
            {
                Caja cajaDb = ObtenerPorId(cajap);
                if (cajaDb != null)
                {
                    //poner caja abierta
                    cajaDb.Estado = 1;
                    unitOfWork.Repository<Caja>().Update(cajaDb);
                    //crear sesion para ligar movimientos
                    CajaUsuario cu = new CajaUsuario();
                    cu.Caja = cajaDb.Codigo;
                    cu.Usuario = usuariop.Cedula;
                    cu.Apertura = DateTime.Now;
                    unitOfWork.Repository<CajaUsuario>().Add(cu);
                    unitOfWork.Save();
                    //registrar movimiento
                    MovimientoCaja mov = new MovimientoCaja();
                    mov.Caja = cajaDb.Codigo;
                    mov.Fecha = DateTime.Now;
                    mov.Monto = monto;
                    mov.Tipo = 1;
                    mov.Descripcion = "Apetura";
                    mov.SesionId = cu.Sesion;
                    unitOfWork.Repository<MovimientoCaja>().Add(mov);
                    unitOfWork.Save();
                    return cu.Sesion;
                }
                else
                {
                    return 0;
                }
                

                
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Cerrar(Caja cajap,int sesion, decimal monto)
        {
            try
            {
                Caja cajaDb = ObtenerPorId(cajap);
                CajaUsuario cu = unitOfWork.Repository<CajaUsuario>().GetById( sesion );
                cu.Cierre = DateTime.Now;
                unitOfWork.Repository<CajaUsuario>().Update(cu);
                unitOfWork.Save();

                cajaDb.Estado = 2;
                unitOfWork.Repository<Caja>().Update(cajaDb);
                unitOfWork.Save();

                MovimientoCaja mov = new MovimientoCaja();
                mov.Caja = cajap.Codigo;
                mov.Fecha = DateTime.Now;
                mov.Monto = monto;
                mov.Tipo = 2;
                mov.Descripcion = "Cierre";
                mov.SesionId = sesion;
                unitOfWork.Repository<MovimientoCaja>().Add(mov);
                unitOfWork.Save();
                return true;

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Caja cajap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajas = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Caja>().Add(cajap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Caja cajap)
        {
            try
            {
 
                unitOfWork.Repository<Caja>().Update(cajap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Caja cajap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajas = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Caja>().Update(cajap);
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

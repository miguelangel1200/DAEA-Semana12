using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDemo2.Models;

namespace APIDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly DemoContext _context;

        public InvoicesController(DemoContext context)
        {
            _context = context;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
          if (_context.Invoices == null)
          {
              return NotFound();
          }
            return await _context.Invoices.Include(x => x.Details).ThenInclude(y => y.Product).ThenInclude(z => z.Category).ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
          if (_context.Invoices == null)
          {
              return NotFound();
          }
            var invoice = await _context.Invoices.FindAsync(id);


            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceID)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
          if (_context.Invoices == null)
          {
              return Problem("Entity set 'DemoContext.Invoices'  is null.");
          }
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceID }, invoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            if (_context.Invoices == null)
            {
                return NotFound();
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            //_context.Invoices.Remove(invoice);
            //await _context.SaveChangesAsync();

            // Lógica para cancelar la factura
            CancelarFactura(id);
            
            return NoContent();
        }

        private bool InvoiceExists(int id)
        {
            return (_context.Invoices?.Any(e => e.InvoiceID == id)).GetValueOrDefault();
        }




        // Método para crear una factura
        private void CrearFactura(Invoice factura)
        {
            // Lógica para crear la factura
            int idCliente = factura.CustomerID;
            int numeroFactura = factura.InvoiceID;
            DateTime fechaFactura = factura.CreatedOn;
            List<Detail> detalles = factura.Details;

            // Aquí puedes implementar la lógica necesaria para crear la factura
            // Utiliza los datos proporcionados (idCliente, numeroFactura, fechaFactura, detalles) para crear la factura según tus requisitos

            // Ejemplo: Recorriendo los detalles de la factura
            foreach (var detalle in detalles)
            {
                int productoId = detalle.ProductID;
                int precioProducto = detalle.Price;
                int cantidad = detalle.Amount;

                // Aquí puedes utilizar los datos de cada detalle para crear los registros de productos en la factura
                // Implementa la lógica según tus necesidades
            }
        }

        // Método para listar una factura
        private Invoice ListarFactura(int numeroFactura)
        {

            // Lógica para listar la factura
            Invoice factura = _context.Invoices.Include(i => i.Details).FirstOrDefault(i => i.InvoiceID == numeroFactura);

            if (factura == null)
            {
                return null;
            }

            // Aquí puedes implementar la lógica necesaria para listar la factura según tus requisitos
            // Utiliza los datos de la factura para obtener la información necesaria y devolverla en el formato deseado

            return factura;
        }

        // Método para cancelar una factura
        private void CancelarFactura(int numeroFactura)
        {
            // Lógica para cancelar la factura
            Invoice factura = _context.Invoices.FirstOrDefault(i => i.InvoiceID == i.InvoiceID);

            if (factura != null)
            {
                factura.IsCancelled = true;
                _context.SaveChanges();
            }

            // Aquí puedes implementar la lógica necesaria para cancelar la factura según tus requisitos
            // Utiliza el número de factura proporcionado (numeroFactura) para realizar la cancelación de la factura
        }


    }
}

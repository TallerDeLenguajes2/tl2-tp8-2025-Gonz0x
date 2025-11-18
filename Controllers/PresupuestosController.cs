using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Models;
using tl2_tp8_2025_Gonz0x.Repositorios;
using tl2_tp8_2025_Gonz0x.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
   public class PresupuestosController : Controller
   {
      private readonly PresupuestosRepository _presupuestosRepository;
      private readonly ProductosRepository _productosRepository;

      public PresupuestosController()
      {
         _presupuestosRepository = new PresupuestosRepository();
         _productosRepository = new ProductosRepository();
      }

      // GET: /Presupuestos
      [HttpGet]
      public IActionResult Index()
      {
         List<Presupuestos> presupuestos = _presupuestosRepository.ListarPresupuestos();
         return View(presupuestos);
      }

      // GET: /Presupuestos/Details/5
      [HttpGet]
      public IActionResult Details(int id)
      {
         var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
         if (presupuesto == null)
               return NotFound();

         return View(presupuesto);
      }

      // GET: /Presupuestos/Create
      [HttpGet]
      public IActionResult Create()
      {
         return View();
      }

      // POST: /Presupuestos/Create
      /* [HttpPost]
      public IActionResult Create(Presupuestos nuevo)
      {
         if (!ModelState.IsValid)
               return View(nuevo);
         
         nuevo.FechaCreacion = DateTime.Now;  //aseguro la fecha
         _presupuestosRepository.CrearPresupuesto(nuevo);
         return RedirectToAction("Index");
      }  */

      [HttpPost]
      public IActionResult Create(PresupuestoViewModel vm)
      {
/*          if (vm.FechaCreacion > DateTime.Now)
         {
            ModelState.AddModelError("FechaCreacion", "La fecha no puede ser futura.");
         }
 */
         if (!ModelState.IsValid)
         {
            return View(vm);
         }

         var p = new Presupuestos()
         {
            NombreDestinatario = vm.NombreDestinatario,
            FechaCreacion = vm.FechaCreacion
         };

         _presupuestosRepository.CrearPresupuesto(p);
         return RedirectToAction("Index");
      }


      // GET: /Presupuestos/Edit/5
      [HttpGet]
      public IActionResult Edit(int id)
      {
         var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
         if (presupuesto == null)
            return NotFound();

         var vm = new PresupuestoViewModel
         {
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion
         };

    return View(vm);
      }

      // POST: /Presupuestos/Edit/5
      [HttpPost]
      public IActionResult Edit(int id, PresupuestoViewModel vm)
      {
         if (!ModelState.IsValid)
         {
            return View(vm);
         }

         var pres = _presupuestosRepository.ObtenerPresupuestoPorId(id);
         if(pres == null) return NotFound();

         pres.NombreDestinatario = vm.NombreDestinatario;
         pres.FechaCreacion = vm.FechaCreacion;

         _presupuestosRepository.ModificarPresupuesto(id, pres);

         return RedirectToAction("Index");
      }


      // GET: /Presupuestos/Delete/5
      public IActionResult Delete(int id)
      {
         var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
         if (presupuesto == null)
               return NotFound();

         return View(presupuesto);
      }

      // POST: /Presupuestos/Delete/5
      [HttpPost]
      public IActionResult Delete(Presupuestos presupuesto)
      {
         try
         {
            _presupuestosRepository.EliminarPresupuesto(presupuesto.IdPresupuesto);
            return RedirectToAction("Index");
         }
         catch
         {
            TempData["Error"] = "No se puede eliminar: El presupuesto tiene productos cargados.";
            return RedirectToAction("Index");
         }
      }

      [HttpGet]
      public IActionResult AgregarProducto(int id)
      {
         // 1. Obtener los productos para el SelectList
         List<Productos> productos = _productosRepository.ListarProductos();

         // 2. Crear el ViewModel
         AgregarProductoViewModel model = new AgregarProductoViewModel
         {
         IdPresupuesto = id, // Pasamos el ID del presupuesto actual
         // 3. Crear el SelectList
         ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
         };
         return View(model);

      }

      [HttpPost]
      public IActionResult AgregarProducto(AgregarProductoViewModel vm)
      {
         if (!ModelState.IsValid)
         {
            vm.ListaProductos = new SelectList(_productosRepository.ListarProductos(), "IdProducto", "Descripcion");
            return View(vm);
         }

         _presupuestosRepository.AgregarProductoAPresupuesto(
            vm.IdPresupuesto,
            vm.IdProducto,
            vm.Cantidad
         );
         

         return RedirectToAction("Details", new { id = vm.IdPresupuesto });
      }

      // POST: Presupuestos/AgregarProducto
/*       [HttpPost]
      public IActionResult AgregarProducto(AgregarProductoViewModel model)
      {
      // 1. Chequeo de Seguridad para la Cantidad
      if (!ModelState.IsValid)
      {
      // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
      // debemos recargar el SelectList porque se pierde en el POST.
      var productos = _productoRepo.GetAll();
      model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");

      // Devolvemos el modelo con los errores y el dropdown recargado
      return View(model);
      }

      // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
      _repo.AddDetalle(model.IdPresupuesto, model.IdProducto, model.Cantidad);

      // 3. Redirigimos al detalle del presupuesto
      return RedirectToAction(nameof(Details), new { id = model.IdPresupuesto });
      } */

   }
}

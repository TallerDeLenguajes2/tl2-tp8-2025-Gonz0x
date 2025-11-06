using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Models;
using tl2_tp8_2025_Gonz0x.Repositorios.PresupuestosRepository;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
   public class PresupuestosController : Controller
   {
      private readonly PresupuestosRepository _presupuestosRepository;

      public PresupuestosController()
      {
         _presupuestosRepository = new PresupuestosRepository();
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
      [HttpPost]
      public IActionResult Create(Presupuestos nuevo)
      {
         if (!ModelState.IsValid)
               return View(nuevo);

         _presupuestosRepository.CrearPresupuesto(nuevo);
         return RedirectToAction("Index");
      }
      // GET: /Presupuestos/Edit/5
      public IActionResult Edit(int id)
      {
         var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
         if (presupuesto == null)
               return NotFound();

         return View(presupuesto);
      }

      // POST: /Presupuestos/Edit/5
      [HttpPost]
      public IActionResult Edit(Presupuestos presupuesto)
      {
         if (!ModelState.IsValid)
               return View(presupuesto);

         _presupuestosRepository.ModificarPresupuesto(presupuesto.IdPresupuesto, presupuesto);
         return RedirectToAction(nameof(Index));
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
      public IActionResult Delete(int id)
      {
         try
         {
               _presupuestosRepository.EliminarPresupuesto(id);
               return RedirectToAction("Index");
         }
         catch
         {
               TempData["Error"] = "No se puede eliminar: El presupuesto tiene productos cargados.";
               return RedirectToAction("Index");
         }
      }
   }
}

using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Models;
using tl2_tp8_2025_Gonz0x.Repositorios;
using tl2_tp8_2025_Gonz0x.ViewModels;
using tl2_tp8_2025_Gonz0x.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
   public class PresupuestosController : Controller
   {
      private readonly IPresupuestoRepository _presupuestosRepository;
      private readonly IProductoRepository _productosRepository;
      private readonly IAuthenticationService _auth;
      private readonly ILogger<PresupuestosController> _logger;

        // Constructor con DI
      public PresupuestosController(
         IPresupuestoRepository presupuestosRepository,
         IProductoRepository productosRepository,
         IAuthenticationService auth,
         ILogger<PresupuestosController> logger
      )
      {
         _presupuestosRepository = presupuestosRepository;
         _productosRepository = productosRepository;
         _auth = auth;
         _logger = logger;
      }

      // ADMIN o CLIENTE
      private IActionResult CheckReadPermissions()
      {
         if (!_auth.IsAuthenticated())
               return RedirectToAction("Index", "Login");

         if (!(_auth.HasAccessLevel("Administrador") ||
               _auth.HasAccessLevel("Cliente")))
               return RedirectToAction(nameof(AccesoDenegado));

         return null;
      }

      // SOLO ADMIN
      private IActionResult CheckAdminPermissions()
      {
         if (!_auth.IsAuthenticated())
               return RedirectToAction("Index", "Login");

         if (!_auth.HasAccessLevel("Administrador"))
               return RedirectToAction(nameof(AccesoDenegado));

         return null;
      }

      /*
      private readonly PresupuestosRepository _presupuestosRepository;
      private readonly ProductosRepository _productosRepository;

      public PresupuestosController()
      {
         _presupuestosRepository = new PresupuestosRepository();
         _productosRepository = new ProductosRepository();
      }
      */

      // GET: /Presupuestos
      [HttpGet]
      public IActionResult Index()
      {
         try
         {
            var perm = CheckReadPermissions();
            if (perm != null) return perm;

            var presupuestos = _presupuestosRepository.ListarPresupuestos();
            return View(presupuestos);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.ToString());
            return View("Error");
         }
      }


      // GET: /Presupuestos/Details/5
      [HttpGet]
      public IActionResult Details(int id)
      {
         try
         {
            var perm = CheckReadPermissions();
            if (perm != null) return perm;

            var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
            return View(presupuesto);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.ToString());
            return View("Error");
         }
      }


      // GET: /Presupuestos/Create
      [HttpGet]
      public IActionResult Create()
      {
         var perm = CheckAdminPermissions();
         if (perm != null) return perm;
         
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
         try
         {
            var perm = CheckAdminPermissions();
            if (perm != null) return perm;

            if (!ModelState.IsValid)
                  return View(vm);

            var p = new Presupuestos
            {
                  NombreDestinatario = vm.NombreDestinatario,
                  FechaCreacion = vm.FechaCreacion
            };

            _presupuestosRepository.CrearPresupuesto(p);
            _logger.LogInformation("Presupuesto para '{Dest}' creado correctamente.", vm.NombreDestinatario);
            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.ToString());
            return View("Error");
         }
      }



      // GET: /Presupuestos/Edit/5
      [HttpGet]
      public IActionResult Edit(int id)
      {

         var perm = CheckAdminPermissions();
         if (perm != null) return perm;

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
         try
         {
            var perm = CheckAdminPermissions();
            if (perm != null) return perm;

            if (!ModelState.IsValid)
                  return View(vm);

            var pres = _presupuestosRepository.ObtenerPresupuestoPorId(id);
            pres.NombreDestinatario = vm.NombreDestinatario;

            _presupuestosRepository.ModificarPresupuesto(id, pres);
            _logger.LogInformation("Presupuesto ID {Id} editado exitosamente para el cliente {Destinatario}.", id, pres.NombreDestinatario);
            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.ToString());
            return View("Error");
         }
      }



      // GET: /Presupuestos/Delete/5
      public IActionResult Delete(int id)
      {

         var perm = CheckAdminPermissions();
         if (perm != null) return perm;

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
            var perm = CheckAdminPermissions();
            if (perm != null) return perm;

            _presupuestosRepository.EliminarPresupuesto(presupuesto.IdPresupuesto);
            _logger.LogInformation("Presupuesto ID {Id} eliminado correctamente.", presupuesto.IdPresupuesto);
            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.ToString());
            TempData["Error"] = "No se puede eliminar el presupuesto.";
            return RedirectToAction("Index");
         }
      }


      [HttpGet]
      public IActionResult AgregarProducto(int id)
      {
         var perm = CheckAdminPermissions();
         if (perm != null) return perm;

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
         try
         {
            var perm = CheckAdminPermissions();
            if (perm != null) return perm;

            if (!ModelState.IsValid)
            {
                  vm.ListaProductos = new SelectList(
                     _productosRepository.ListarProductos(),
                     "IdProducto",
                     "Descripcion"
                  );
                  return View(vm);
            }

            _presupuestosRepository.AgregarProductoAPresupuesto(
                  vm.IdPresupuesto,
                  vm.IdProducto,
                  vm.Cantidad
            );
            _logger.LogInformation("Producto ID {Prod} (Cant: {Cant}) añadido al Presupuesto ID {Pres}", vm.IdProducto, vm.Cantidad, vm.IdPresupuesto);
            return RedirectToAction("Details", new { id = vm.IdPresupuesto });
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.ToString());
            return View("Error");
         }
      }


      [HttpGet]
      public IActionResult AccesoDenegado()
      {
         return View();
      }

   }
}

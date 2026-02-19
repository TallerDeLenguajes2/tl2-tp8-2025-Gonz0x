using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Models;
using tl2_tp8_2025_Gonz0x.Repositorios;
using tl2_tp8_2025_Gonz0x.ViewModels; //  AGREGO
using tl2_tp8_2025_Gonz0x.Interfaces;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
    public class ProductosController : Controller
    {

        private readonly IProductoRepository _productosRepository;
        private readonly IAuthenticationService _auth;
        private readonly ILogger<ProductosController> _logger;
        // CONSTRUCTOR CON INYECCIÓN DE DEPENDENCIAS
        public ProductosController(
            IProductoRepository productosRepository,
            IAuthenticationService auth,
            ILogger<ProductosController> logger
        )
        {
            _productosRepository = productosRepository;
            _auth = auth;
            _logger = logger;
        }

        private IActionResult CheckReadPermissions()
        {
            if (!_auth.IsAuthenticated())
                return RedirectToAction("Index", "Login");

            if (!(_auth.HasAccessLevel("Administrador") || _auth.HasAccessLevel("Cliente")))
                return RedirectToAction(nameof(AccesoDenegado));

            return null;
        }

        private IActionResult CheckAdminPermissions()
        {
            if (!_auth.IsAuthenticated())
                return RedirectToAction("Index", "Login");

            if (!_auth.HasAccessLevel("Administrador"))
                return RedirectToAction(nameof(AccesoDenegado));

            return null;
        }
        // GET: /Productos
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var perm = CheckReadPermissions();
                if (perm != null) return perm;       

                return View(_productosRepository.ListarProductos());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                //return View("Error");
                return RedirectToAction("Error", "Home");
            }
        }


        // GET: /Productos/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var perm = CheckReadPermissions();
                if (perm != null) return perm;

                var producto = _productosRepository.ObtenerProductoPorId(id);
                if (producto == null)
                    return NotFound();

                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return View("Error");
            }
        }



        // GET: /Productos/Create
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                var perm = CheckAdminPermissions();
                if (perm != null) return perm;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return View("Error");
            }
        }

        // POST: /Productos/Create
        [HttpPost]
        public IActionResult Create(ProductoViewModel vm)
        {
            try
            {
                var perm = CheckAdminPermissions();
                if (perm != null) return perm;

                if (!ModelState.IsValid)
                    return View(vm);

                var producto = new Productos
                {
                    Descripcion = vm.Descripcion,
                    Precio = vm.Precio
                };

                _productosRepository.CrearProducto(producto);
                _logger.LogInformation("Producto '{Desc}' creado exitosamente por el usuario {User}.", vm.Descripcion, _auth.CurrentUser()?.User);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return View("Error");
            }
        }

        // GET: /Productos/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var perm = CheckAdminPermissions();
                if (perm != null) return perm;

                var producto = _productosRepository.ObtenerProductoPorId(id);

                var vm = new ProductoViewModel
                {
                    IdProducto = producto.IdProducto,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return View("Error");
            }
        }

        // POST: /Productos/Edit
        [HttpPost]
        public IActionResult Edit(int id, ProductoViewModel vm)
        {
            try
            {
                var perm = CheckAdminPermissions();
                if (perm != null) return perm;

                if (!ModelState.IsValid)
                    return View(vm);

                var producto = _productosRepository.ObtenerProductoPorId(id);
                producto.Descripcion = vm.Descripcion;
                producto.Precio = vm.Precio;

                _productosRepository.ModificarProducto(id, producto);
                _logger.LogInformation("Producto ID {Id} editado correctamente.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return View("Error");
            }
        }

        // GET: /Productos/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var perm = CheckAdminPermissions();
                if (perm != null) return perm;

                var producto = _productosRepository.ObtenerProductoPorId(id);
                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return View("Error");
            }
        }

        // POST: /Productos/Delete
        [HttpPost]
        public IActionResult Delete(Productos producto)
        {
            try
            {
                var perm = CheckAdminPermissions();
                if (perm != null) return perm;

                _productosRepository.EliminarProducto(producto.IdProducto);
                // ✅ LOG: Información de éxito
                _logger.LogInformation("Producto ID {Id} eliminado correctamente.", producto.IdProducto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // ❌ LOG: Error serializado
                _logger.LogError(ex.ToString());

                // ⚠️ MANEJO DE ERROR
                if (ex.Message.Contains("FOREIGN KEY"))
                {
                    TempData["Error"] = "No se puede eliminar el producto porque está incluido en uno o más presupuestos. Primero debe eliminar los presupuestos asociados.";
                }
                else
                {
                    TempData["Error"] = "Ocurrió un error inesperado al intentar eliminar el producto.";
                }
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

    }
}
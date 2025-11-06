using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Models;
using tl2_tp8_2025_Gonz0x.Repositorios.ProductosRepository;

namespace tl2_tp8_2025_Gonz0x
{
    public class ProductosController : Controller
    {
        private readonly ProductosRepository _productosRepository;

        public ProductosController()
        {
            _productosRepository = new ProductosRepository();
        }

        // GET: /Productos
        [HttpGet]
        public IActionResult Index()
        {
            List<Productos> productos = _productosRepository.ListarProductos();
            return View(productos);
        }

        // GET: /Productos/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var producto = _productosRepository.ObtenerProductoPorId(id);
            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // GET: /Productos/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Productos/Create
        [HttpPost]
        public IActionResult Create(Productos nuevo)
        {
            if (!ModelState.IsValid)
                return View(nuevo);

            _productosRepository.CrearProducto(nuevo);
            return RedirectToAction("Index");
        }

        // GET: /Productos/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var producto = _productosRepository.ObtenerProductoPorId(id);
            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: /Productos/Edit
        [HttpPost]
        public IActionResult Edit(Productos productoEditado)
        {
            if (!ModelState.IsValid)
                return View(productoEditado);

            _productosRepository.ModificarProducto(productoEditado);
            return RedirectToAction("Index");
        }

        // GET: /Productos/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var producto = _productosRepository.ObtenerProductoPorId(id);
            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: /Productos/Delete
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _productosRepository.EliminarProducto(id);
            return RedirectToAction("Index");
        }
    }
}

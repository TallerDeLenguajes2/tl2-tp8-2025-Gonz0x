using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Models;
using tl2_tp8_2025_Gonz0x.Repositorios;
using tl2_tp8_2025_Gonz0x.ViewModels; //  AGREGO
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
        /*[HttpPost]
        public IActionResult Create(Productos nuevo)
        {
            if (!ModelState.IsValid)
                return View(nuevo);

            _productosRepository.CrearProducto(nuevo);
            return RedirectToAction("Index", "Productos");
        } */

        [HttpPost]
        public IActionResult Create(ProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);//Devolvemos el ViewModel con los datos y errores a la Vista
            }
            // 2. SI ES VÁLIDO: Mapeo Manual de VM a Modelo de Dominio
            var producto = new Productos()
            {
                Descripcion = vm.Descripcion,
                Precio = vm.Precio
            };
            // 3. Llamada al Repositorio
            _productosRepository.CrearProducto(producto);
            return RedirectToAction("Index");
        }


        // GET: /Productos/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var producto = _productosRepository.ObtenerProductoPorId(id);
            if (producto == null)
                return NotFound();

            var vm = new ProductoViewModel
            {
                IdProducto = producto.IdProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio
            };

            return View(vm);
        }

        // POST: /Productos/Edit
/*         [HttpPost]
        public IActionResult Edit(Productos productoEditado)
        {
            if (!ModelState.IsValid)
                return View(productoEditado);

            // Pasamos el Id y el objeto completo
            _productosRepository.ModificarProducto(productoEditado.IdProducto, productoEditado);

            return RedirectToAction("Index", "Productos");
        } */

        [HttpPost]
        public IActionResult Edit(int id, ProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var prod = _productosRepository.ObtenerProductoPorId(id);
            if (prod == null) return NotFound();

            prod.Descripcion = vm.Descripcion;
            prod.Precio = vm.Precio;

            _productosRepository.ModificarProducto(id, prod);
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
        public IActionResult Delete(Productos producto)
        {
            _productosRepository.EliminarProducto(producto.IdProducto);
            return RedirectToAction("Index");
        }
    }
}
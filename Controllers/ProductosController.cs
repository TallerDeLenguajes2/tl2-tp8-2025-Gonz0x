using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Models;
using tl2_tp8_2025_Gonz0x.Repositorios;
using tl2_tp8_2025_Gonz0x.ViewModels; //  AGREGO
using tl2_tp8_2025_Gonz0x.Interfaces;

namespace tl2_tp8_2025_Gonz0x
{
    public class ProductosController : Controller
    {

        private readonly IProductoRepository _productosRepository;
        private readonly IAuthenticationService _auth;

        // CONSTRUCTOR CON INYECCIÓN DE DEPENDENCIAS
        public ProductosController(
            IProductoRepository productosRepository,
            IAuthenticationService auth
        )
        {
            _productosRepository = productosRepository;
            _auth = auth;
        }
        /*
        private readonly ProductosRepository _productosRepository;

        public ProductosController()
        {
            _productosRepository = new ProductosRepository();
        }
        */
        // GET: /Productos
        [HttpGet]
        public IActionResult Index()
        {
            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");        
            
            if (!(_auth.HasAccessLevel("Administrador") || _auth.HasAccessLevel("Cliente")))
            {
                return RedirectToAction(nameof(AccesoDenegado));
            }  

            List<Productos> productos = _productosRepository.ListarProductos();
            return View(productos);
        }

        // GET: /Productos/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {

            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");        
            
            if (!(_auth.HasAccessLevel("Administrador") || _auth.HasAccessLevel("Cliente")))
            {
                return RedirectToAction(nameof(AccesoDenegado));
            }  

            var producto = _productosRepository.ObtenerProductoPorId(id);
            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // GET: /Productos/Create
        [HttpGet]
        public IActionResult Create()
        {

            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");

            if (!_auth.HasAccessLevel("Administrador"))
            return RedirectToAction(nameof(AccesoDenegado));       

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

            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");

            if (!_auth.HasAccessLevel("Administrador"))
            return RedirectToAction(nameof(AccesoDenegado));              

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

            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");

            if (!_auth.HasAccessLevel("Administrador"))
            return RedirectToAction("AccesoDenegado"); 

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

            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");

            if (!_auth.HasAccessLevel("Administrador"))
            return RedirectToAction("AccesoDenegado");            

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

            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");

            if (!_auth.HasAccessLevel("Administrador"))
            return RedirectToAction("AccesoDenegado"); 

            var producto = _productosRepository.ObtenerProductoPorId(id);
            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: /Productos/Delete
        [HttpPost]
        public IActionResult Delete(Productos producto)
        {

            if (!_auth.IsAuthenticated())
            return RedirectToAction("Index", "Login");

            if (!_auth.HasAccessLevel("Administrador"))
            return RedirectToAction("AccesoDenegado"); 

            _productosRepository.EliminarProducto(producto.IdProducto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

    }
}
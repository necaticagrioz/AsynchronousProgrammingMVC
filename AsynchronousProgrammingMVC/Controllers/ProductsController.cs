using AsynchronousProgrammingMVC.Infrastructure.Repositories.Interfaces;
using AsynchronousProgrammingMVC.Models.DTO_s.ProductDTO;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;
using AsynchronousProgrammingMVC.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AsynchronousProgrammingMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetFilteredList
                (
                    select: x => new ProductVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Image = x.Image != null ? x.Image : "noimage.png",
                        UnitPrice = x.UnitPrice,
                        CategoryName = x.Category.Name,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate
                    },
                    where: x => x.Status != Models.Entities.Abstract.Status.Passive,
                    orderBy: x => x.OrderByDescending(z => z.CreatedDate),
                    join: x => x.Include(z => z.Category)
                );

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.Categories = new SelectList
                (
                    await _categoryRepository.GetByDefaults(x => x.Status != Models.Entities.Abstract.Status.Passive), "Id", "Name"
                );

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDTO model)
        {
            ViewBag.Categories = new SelectList
                (
                    await _categoryRepository.GetByDefaults(x => x.Status != Models.Entities.Abstract.Status.Passive), "Id", "Name"
                );

            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";
                if (model.UploadImage != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    imageName = $"{Guid.NewGuid()}_{model.UploadImage.FileName}";
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await model.UploadImage.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                var product = _mapper.Map<Product>(model);
                product.Image = imageName;
                await _productRepository.Add(product);
                TempData["Succces"] = "Ürün eklenmiştir!!";
                return RedirectToAction("Index");
            }
            TempData["Warning"] = "Aşağıda belirtilen kurallara uyunuz!!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product != null) 
            {
                var model = _mapper.Map<UpdateProductDTO>(product);
                model.Categories = await _categoryRepository.GetByDefaults(x => x.Status != Models.Entities.Abstract.Status.Passive);
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                string imageName = model.Image;
                if (model.UploadImage is not null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                   
                    if (!string.Equals(imageName, "noimage.png"))
                    {
                        string oldPath = Path.Combine(uploadDir, model.Image);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    //İsimleri çakışmaması için Guid kullandık.
                    imageName = $"{Guid.NewGuid()}_{model.UploadImage.FileName}";

                    //FilePath =>  Dosya yolu + Dosya adı
                    string filePath = Path.Combine(uploadDir, imageName);

                    //FileStream class'ı dosya yazma işlemi yapar.
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);

                    //Yüklenen dosya server'a kopyalanır.
                    await model.UploadImage.CopyToAsync(fileStream);

                    //Ve işlem kapatılır. Kapatılmazsa kaydedilmez. 
                    fileStream.Close();
                }

                var product = _mapper.Map<Product>(model);
                product.Image = imageName;
                await _productRepository.Update(product);
                TempData["Success"] = "Ürün güncellendi!!";
                return RedirectToAction("Index");
            }
            TempData["Warning"] = "Aşağıdaki kurallara uyunuz!!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product is not null)
            {
                await _productRepository.Delete(product);
                TempData["Success"] = "Ürün silinmiştir.";
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> ProductsByCategory(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category is not null)
            {
                ViewBag.CategoryName = category.Name;
                List<ProductsByCategoryVM> products;

                if (await _productRepository.Any(x => x.CategoryId == category.Id && x.Status != Models.Entities.Abstract.Status.Passive))
                {
                    products = await _productRepository.GetFilteredList
                    (
                        select: x => new ProductsByCategoryVM
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Image = x.Image,
                            Description = x.Description,
                            CategoryName = x.Category.Name,
                            UnitPrice = x.UnitPrice,
                            Categories = _categoryRepository.GetCategories(),
                        },
                        where: x => x.Status != Models.Entities.Abstract.Status.Passive && x.CategoryId == category.Id,
                        join: x => x.Include(z => z.Category),
                        orderBy: x => x.OrderByDescending(z => z.CreatedDate)
                    );
                }
                else
                {
                    products = await _productRepository.GetFilteredList
                    (
                        select: x => new ProductsByCategoryVM
                        {
                            Categories = _categoryRepository.GetCategories(),
                        }
                    );
                };
                return View(products);
            }
            return NotFound();
        }
    }
}

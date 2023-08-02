using AsynchronousProgrammingMVC.Infrastructure.Repositories.Interfaces;
using AsynchronousProgrammingMVC.Models.DTO_s.CategoryDTO;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;
using AsynchronousProgrammingMVC.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsynchronousProgrammingMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetFilteredList
                (
                    select: x => new CategoryVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate
                    },
                    where: x => x.Status != Models.Entities.Abstract.Status.Passive,
                    orderBy: x => x.OrderByDescending(z => z.CreatedDate));

            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory() => View();

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryRepository.Any(x => x.Name == model.Name && x.Status != Models.Entities.Abstract.Status.Passive))
                {
                    TempData["Warning"] = "Bu isim zaten kayıtlı!!";
                    return View(model);
                }

                var category = _mapper.Map<Category>(model);
                await _categoryRepository.Add(category);
                TempData["Success"] = "Kategori eklendi!";
                return RedirectToAction("Index");
            }

            TempData["Warning"] = "Kurallara dikkat ederek tekrar deneyiniz!!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category is not null)
            {
                var model = _mapper.Map<UpdateCategoryDTO>(category);
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO model)
        {
            if (ModelState.IsValid) 
            {
                if (await _categoryRepository.Any(x => x.Name == model.Name && x.Id != model.Id && x.Status != Models.Entities.Abstract.Status.Passive))
                {
                    TempData["Warning"] = "Bu isim zaten kayıtlıdır.";
                    return View(model);
                }

                var category = _mapper.Map<Category>(model);
                await _categoryRepository.Update(category);
                TempData["Success"] = "Kategori güncellendi!!";
                return RedirectToAction("Index");
            }
            TempData["Warning"] = "Lütfen kurallara uygun bir şekilde formu doldurunuz!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category is not null)
            {
                await _categoryRepository.Delete(category);
                TempData["Success"] = "Kategori silindi!";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

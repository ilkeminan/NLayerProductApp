using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class CategoriesController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _service;

        public CategoriesController(IMapper mapper, ICategoryService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            var categoryDtos = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            //return Ok(categoryDtos);
            return CreateActionResult(CustomResponseDTO<List<CategoryDTO>>.Success(200, categoryDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return CreateActionResult(CustomResponseDTO<CategoryDTO>.Success(200, categoryDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(CategoryDTO categoryDTO)
        {
            var category = await _service.AddAsync(_mapper.Map<Category>(categoryDTO));
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return CreateActionResult(CustomResponseDTO<CategoryDTO>.Success(201, categoryDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTO categoryDTO)
        {
            await _service.UpdateAsync(_mapper.Map<Category>(categoryDTO));
            return CreateActionResult(CustomResponseDTO<NoContentDTO>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var category = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(category);
            return CreateActionResult(CustomResponseDTO<NoContentDTO>.Success(204));
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
        {
            var categoryWithProductsDto = await _service.GetSingleCategoryByIdWithProductsAsync(categoryId);
            return CreateActionResult(CustomResponseDTO<CategoryWithProductsDTO>.Success(200, categoryWithProductsDto));
        }
    }
}

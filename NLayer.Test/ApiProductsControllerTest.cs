using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NLayer.API.Controllers;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Service.Mapping;

namespace NLayer.Test
{
    public class ApiProductsControllerTest
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;
        private readonly IMapper _mapper;
        private List<Product> products;

        public ApiProductsControllerTest()
        {
            var myProfile = new MapProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mapper, _mockService.Object);

            products = new List<Product>() { 
                new Product {Id = 1, Name = "IPhone 13", Stock = 70, Price = 30000, CategoryId = 1, CreatedDate = DateTime.Now },
                new Product {Id = 2, Name = "IPhone 14", Stock = 100, Price = 40000, CategoryId = 1, CreatedDate = DateTime.Now },
                new Product {Id = 3, Name = "Lenovo Ideapad Gaming 3", Stock = 20, Price = 35000, CategoryId = 2, CreatedDate = DateTime.Now },
                new Product {Id = 4, Name = "Xiaomi Akýllý Süpürge", Stock = 15, Price = 10000, CategoryId = 3, CreatedDate = DateTime.Now }
            };
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAll_ActionExecutes_Returns200StatusCode()
        {
            _mockService.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            var result = await _controller.GetAll() as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestCase(1)]
        [TestCase(3)]
        public async Task GetById_ActionExecutes_Returns200StatusCodeAndProduct(int id)
        {
            var product = products.First(x => x.Id == id);
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(product);

            var result = await _controller.GetById(id) as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(200, result.StatusCode);
            var customResponseDTO = result.Value as CustomResponseDTO<ProductDTO>;
            Assert.AreEqual(product.Name, customResponseDTO.Data.Name);
        }

        [TestCase(1)]
        public async Task Save_ActionExecutes_Returns201StatusCode(int id)
        {
            var product = products.First(x => x.Id == id);
            var productDto = _mapper.Map<ProductDTO>(product);
            _mockService.Setup(x => x.AddAsync(It.IsAny<Product>()));

            var result = await _controller.Save(productDto) as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(201, result.StatusCode);
        }

        [TestCase(2)]
        public async Task Update_ActionExecutes_Returns204StatusCode(int id)
        {
            var product = products.First(x => x.Id == id);
            var productUpdateDto = new ProductUpdateDTO() { Id = product.Id, Name = product.Name, Stock = product.Stock, Price = product.Price, CategoryId = product.CategoryId};
            _mockService.Setup(x => x.UpdateAsync(It.IsAny<Product>()));

            var result = await _controller.Update(productUpdateDto) as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestCase(4)]
        public async Task Remove_ActionExecutes_Returns204StatusCode(int id)
        {
            var product = products.First(x => x.Id == id);
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(product);
            _mockService.Setup(x => x.RemoveAsync(product));

            var result = await _controller.Remove(id) as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task GetProductsWithCategory_ActionExecutes_Returns200StatusCodeAndProductsWithCategories()
        {
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDTO>>(products);
            _mockService.Setup(x => x.GetProductsWithCategory()).ReturnsAsync(productsWithCategoryDto);

            var result = await _controller.GetProductsWithCategory() as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(200, result.StatusCode);
            var customResponseDTO = result.Value as CustomResponseDTO<List<ProductWithCategoryDTO>>;
            Assert.AreEqual(products.Count, customResponseDTO.Data.Count);
        }
    }
}
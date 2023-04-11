using NLayer.Core.DTOs;

namespace NLayer.MVC.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<List<CategoryDTO>>>("categories");
            return response.Data;
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<CategoryDTO>>($"categories/{id}");
            return response.Data;
        }

        public async Task<CategoryDTO> SaveAsync(CategoryDTO category)
        {
            var response = await _httpClient.PostAsJsonAsync("categories", category);
            if (!response.IsSuccessStatusCode) return null;
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDTO<CategoryDTO>>();
            return responseBody.Data;
        }

        public async Task<bool> UpdateAsync(CategoryDTO category)
        {
            var response = await _httpClient.PutAsJsonAsync("categories", category);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"categories/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<CategoryWithProductsDTO> GetSingleCategoryByIdWithProducts()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<CategoryWithProductsDTO>>("categories/GetSingleCategoryByIdWithProducts");
            return response.Data;
        }
    }
}

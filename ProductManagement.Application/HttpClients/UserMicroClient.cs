using ProductManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductManagement.Application.HttpClients
{
    public class UserMicroClient
    {
        private readonly HttpClient _httpClient;
        public UserMicroClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserDTOForMicro?> GetUserById(Guid userId)
        {
            var result = await _httpClient.GetAsync($"/api/v1/users/id/{userId}");
            if (!result.IsSuccessStatusCode)
            {
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    // Read the error content from the response
                    var errorContent = await result.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP request failed with status code {result.StatusCode}: {errorContent}");
                }
            }
            var content = await result.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<UserDTOForMicro>(content, options);
        }
    }
}

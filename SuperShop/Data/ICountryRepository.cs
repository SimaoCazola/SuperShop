using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        // Metodo para retornar todas os Paises e cidas 
        IQueryable GetCountriesWithCities();


        // Metodo que retorna cidades e pais por ID
        Task<Country> GetCountriesWithCitiesAsync(int id);


        //Metodo que retorna a cidade por ID
        Task<City> GetCityAsync(int id); 


        // Metodo que retorna o modelo para mostrar na view
        Task AddCityAsync(CityViewModel model);


        //Metodo que retornar a alteracao das cidades
        Task<int> UpdateCityAsync(City city);

        //Metodo que retornar o apagar cidade
        Task<int> DeleteCityAsync(City city);

    }
}

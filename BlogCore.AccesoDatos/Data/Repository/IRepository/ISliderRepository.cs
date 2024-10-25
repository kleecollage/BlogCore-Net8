using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository;

public interface ISliderRepository: IRepository<Slider>
{
    void Update(Slider slider);
}
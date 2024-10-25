namespace BlogCore.AccesoDatos.Data.Repository.IRepository;

public interface IContenedorTrabajo: IDisposable
{
    // Aqui se deben ir agregando los diferentes repositorios
    ICategoriaRepository Categoria { get; }
    IArticuloRepository Articulo { get; }
    ISliderRepository Slider { get; }
    void Save();
}
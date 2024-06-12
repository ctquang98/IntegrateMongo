namespace IntegrateMongo1.Models
{
    public interface IBooksService
    {
        Task<List<AppBook>> GetAll(GetAllParams _params);
        Task<AppBook> GetById(string id);
        Task<AppBook> Insert(AppBookDto book);
        Task<AppBook> Update(string Id, AppBookDto book);
        Task<bool> Delete(string id);
    }
}

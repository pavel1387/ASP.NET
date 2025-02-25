namespace Pcf.Preferences.DataAccess.Data;

public class EfDbInitializer
    : IDbInitializer
{
    private readonly DataContext _dataContext;

    public EfDbInitializer(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public void InitializeDb()
    {
        _dataContext.AddRange(FakeDataFactory.Preferences);
        _dataContext.SaveChanges();
    }
}
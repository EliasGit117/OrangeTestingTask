using Movies.Domain.Entities;

namespace Movies.Tests;

public class MocksData
{
    public IEnumerable<User> Users { get; set; }
    public  IEnumerable<FavoriteMovie> FavoriteMovies { get; set; }
}

public static class ConfigConsts
{
    public static string _publicPassword = "AccountForTests117";
    public static string _hashedPassword = "$2a$11$4OupgU.mzL4aINU9XGSK5uA1FliyR1a2171mNTdbXtflDh2iuDnzO";
    
    public static List<int> MoviesIds = new List<int>()
    {
        238, 278, 240, 424, 129, 389, 155, 122, 704264, 820067, 13, 1084225, 245891, 823999, 50014, 772515
    }; 
    
    public static MocksData GenereateMocks()
    {
        var mocks = new MocksData { Users = GetMockUsers() };
        mocks.FavoriteMovies = GetMockMovies(mocks.Users);

        return mocks;
    }
    
    private static IEnumerable<User> GetMockUsers() =>
        new List<User>()
        {
            new ()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "First_User",
                Password = _publicPassword,
                LastModified = DateTime.Now
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Second_User",
                Password = _publicPassword,
                LastModified = DateTime.Now
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Third_User",
                Password = _publicPassword,
                LastModified = DateTime.Now
            }
        };

    
    private static List<List<T>> Partition<T>(this List<T> values, int chunkSize)
    {
        return values.Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
    
    private static IEnumerable<FavoriteMovie> GetMockMovies(IEnumerable<User> users)
    {
        var _users = users.ToList();
        List<List<int>> movieIds = new List<int>(MoviesIds).Partition(_users.Count);
        var favMovies = new List<FavoriteMovie>();

        for (var i = 0; i < _users.Count; i++)
        {
            foreach (var movieId in movieIds[i])
            {
                favMovies.Add(new FavoriteMovie()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    InternalId = movieId,
                    UserId = _users[i].Id
                });
            }
        }

        return favMovies;
    }
}
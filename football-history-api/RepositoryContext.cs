using Microsoft.EntityFrameworkCore;

namespace football.history.api;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) {}
}
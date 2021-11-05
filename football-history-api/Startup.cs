using System.Linq;
using System.Reflection;
using football.history.api.Builders;
using football.history.api.Builders.Statistics;
using football.history.api.Domain;
using football.history.api.Repositories;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeduction;
using football.history.api.Repositories.Season;
using football.history.api.Repositories.Team;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace football.history.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILeagueTableBuilder, LeagueTableBuilder>();
            services.AddTransient<IStatisticsBuilder, StatisticsBuilder>();
            services.AddTransient<IRowComparerFactory, RowComparerFactory>();
            services.AddTransient<IPlayOffWinnerChecker, PlayOffWinnerChecker>();
            services.AddTransient<IPlayOffWinnerCalculator, PlayOffWinnerCalculator>();
            services.AddTransient<IRowBuilder, RowBuilder>();
            services.AddTransient<ILeaguePositionBuilder, LeaguePositionBuilder>();

            services.AddTransient<IHistoricalRecordBuilder, HistoricalRecordBuilder>();
            services.AddTransient<ITeamBuilder, TeamBuilder>();
            services.AddTransient<ISeasonBuilder, SeasonBuilder>();
            
            services.AddTransient<IMatchCommandBuilder, MatchCommandBuilder>();
            services.AddTransient<ICompetitionCommandBuilder, CompetitionCommandBuilder>();
            services.AddTransient<IPointDeductionCommandBuilder, PointDeductionCommandBuilder>();
            services.AddTransient<IPositionCommandBuilder, PositionCommandBuilder>();

            services.AddTransient<IDatabaseConnection, DatabaseConnection>();
            
            services.AddTransient<IHistoricalSeasonRepository, HistoricalSeasonRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ISeasonRepository, SeasonRepository>();

            services.AddTransient<ICompetitionRepository, CompetitionRepository>();
            services.AddTransient<IMatchRepository, MatchRepository>();
            services.AddTransient<IPointDeductionRepository, PointDeductionRepository>();
            services.AddTransient<IPositionRepository, PositionRepository>();

            var connString = Configuration.GetConnectionString("FootballHistory");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connString));

            services.AddControllers();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(2, 0);
            });

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v2",
                        new OpenApiInfo
                        {
                            Title = "Football History API v2",
                            Version = "v2"
                        });

                    options.OperationFilter<RemoveVersionFromParameter>();
                    options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                    options.DocInclusionPredicate((version, desc) =>
                    {
                        if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                            return false;

                        var versions = methodInfo.DeclaringType!
                            .GetCustomAttributes(true)
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions);

                        var maps = methodInfo
                            .GetCustomAttributes(true)
                            .OfType<MapToApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions)
                            .ToArray();

                        return versions.Any(v => $"v{v.ToString()}" == version)
                               && (!maps.Any() || maps.Any(v => $"v{v.ToString()}" == version));
                    });
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(
                    builder =>
                    {
                        builder.WithOrigins(
                            Configuration.GetSection("WhitelistedUrls").Get<string[]>());
                    });
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
                });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
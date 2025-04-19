using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using FluentValidation;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using Ambev.DeveloperEvaluation.ORM.Interceptators;
using MongoDB.Driver.Core.Configuration;

namespace Ambev.DeveloperEvaluation.WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            var builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            var connectionStringPostgres = builder.Configuration.GetConnectionString("PostgreSqlConnection");

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddSingleton<UtcDateInterceptor>();

            builder.Services.AddDbContext<DefaultContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<UtcDateInterceptor>();
                options.UseNpgsql(connectionStringPostgres, b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM"));
                options.AddInterceptors(interceptor);
            });


            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
                options.InstanceName = "AmbevCache_";
            });

            var connectionStringMongo = builder.Configuration.GetConnectionString("MongoDbConnection");
            if (string.IsNullOrEmpty(connectionStringMongo))
            {
                throw new InvalidOperationException("MongoDbConnection string is not configured.");
            }

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionStringMongo));

            builder.Services.AddSingleton(sp =>
            {
                var mongoUrl = new MongoUrl(connectionStringMongo);
                var databaseName = mongoUrl.DatabaseName;

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new InvalidOperationException("Database name is not specified in the MongoDbConnection string.");
                }

                return new MongoDbContext(connectionStringMongo, databaseName);
            });

            builder.Services.AddScoped(sp =>
            {
                var mongoDbContext = sp.GetRequiredService<MongoDbContext>();
                return mongoDbContext.GetCollection<Sale>("Sales");
            });

            builder.Services.AddScoped(sp =>
            {
                var mongoDbContext = sp.GetRequiredService<MongoDbContext>();
                return mongoDbContext.GetCollection<ExternalCustomer>("ExternalCustomers");
            });

            builder.Services.AddScoped(sp =>
            {
                var mongoDbContext = sp.GetRequiredService<MongoDbContext>();
                return mongoDbContext.GetCollection<User>("Users");
            });

            builder.Services.AddScoped(sp =>
            {
                var mongoDbContext = sp.GetRequiredService<MongoDbContext>();
                return mongoDbContext.GetCollection<ExternalProduct>("ExternalProducts");
            });

            builder.Services.AddScoped(sp =>
            {
                var mongoDbContext = sp.GetRequiredService<MongoDbContext>();
                return mongoDbContext.GetCollection<ExternalBranch>("ExternalBranches");
            });

            builder.Services.AddValidatorsFromAssembly(typeof(GetUserQuery).Assembly);

            var app = builder.Build();
            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

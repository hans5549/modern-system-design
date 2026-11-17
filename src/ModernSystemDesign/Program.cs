using Microsoft.EntityFrameworkCore;
using ModernSystemDesign.Components;
using ModernSystemDesign.Data;
using ModernSystemDesign.Repositories;
using ModernSystemDesign.Services;
using Serilog;

namespace ModernSystemDesign;

public class Program
{
    public static void Main(string[] args)
    {
        // Bootstrap logger: captures failures that happen before the real, configuration-driven logger exists.
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Sinks, levels and overrides live in the "Serilog" section of appsettings*.json.
            builder.Host.UseSerilog((context, services, logger) => logger
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services));

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddDbContextFactory<AppDbContext>(o =>
                o.UseSqlite(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<INoteService, NoteService>();

            var app = builder.Build();

            // No migrations: the schema is created from the model on first run.
            // shortcut: EnsureCreated never alters an existing DB, so delete app.db when the model changes (switch to migrations or manual DDL once data matters).
            using (var db = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
                db.Database.EnsureCreated();

            // Must come before the handlers whose requests should be logged.
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            Environment.ExitCode = 1; // swallowed exception must still fail the process for supervisors
        }
        finally
        {
            Log.CloseAndFlush(); // flush buffered sinks before the process exits
        }
    }
}

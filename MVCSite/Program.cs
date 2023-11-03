using MVCSite.Services;

namespace MVCSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			builder.Services.AddScoped<GameLogicService>();


			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapControllerRoute(
	                name: "search",
	                pattern: "Timeline/Search/{query}",
	                defaults: new { controller = "Timeline", action = "Search" });

                endpoints.MapControllerRoute(
                    name: "SelectAlly",
                    pattern: "AutoBattler/SelectAlly/{query}",
                    defaults: new { controller = "AutoBattler", action = "SelectAlly" });

                endpoints.MapControllerRoute(
                    name: "SelectEnemy",
                    pattern: "AutoBattler/SelectEnemy/{query}",
                    defaults: new { controller = "AutoBattler", action = "SelectEnemy" });

                endpoints.MapControllerRoute(
                   name: "Initiate",
                   pattern: "AutoBattler/Initiate/",
                   defaults: new { controller = "AutoBattler", action = "Initiate" });


                // Outras rotas
            });


			app.Run();
        }
    }
}
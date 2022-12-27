using System.Text;
using DWUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCoreApiRest.Utils;
using Newtonsoft.Json;

namespace NetCoreApiRest
{
	public class Startup
	{
		readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
		//private LoggerD4 log = new LoggerD4("ProfilerController");

		public Startup(IConfiguration configuration)
		{
			//Se manda llamar el appSetting para aceeder al recurso appsettings.json
			ConfigurationSite.WSConfig(configuration);

			//Conexion a la base de datos
			DBHelperDapper.conexion[0] = ConfigurationSite._cofiguration.GetConnectionString("default");
			DBHelperDapper.conexion[1] = ConfigurationSite._cofiguration.GetConnectionString("noUserVar");

			//log.info("db string: "+ ConfigurationSite._cofiguration.GetConnectionString("default"));

			//Cargar los permisos del sistema
			ConfigurationSite.GetPermission();
		}

		/// <summary>
		/// Metodo para generar los servicios en tiempo de ejecucion para realizar la autenticacion por medio de jwt
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			//Configuracion del metodo de autorizacion por token
			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(jwtBearerOptions =>
			{
				//Configuracion para poder realizar peticiones http y https
				jwtBearerOptions.RequireHttpsMetadata = false;

				//Configuracion de la estructura del token al ser generado
				jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateActor = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = ConfigurationSite._cofiguration["ApiAuth:Issuer"],
					ValidAudience = ConfigurationSite._cofiguration["ApiAuth:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationSite._cofiguration["ApiAuth:SecretKey"]))
				};
			});

			services.AddCors(options =>
			{
				options.AddPolicy(MyAllowSpecificOrigins,
				builder =>
				{
					builder.WithOrigins(ConfigurationSite._cofiguration["ApiAuth:UrlAllow"].Split(',')).AllowAnyHeader().AllowAnyMethod();
				});
			});

			//Configurar los permisos a los diferentes Web Method
			services.AddAuthorization(options =>
			{
				foreach (var per in ConfigurationSite.permissions)
					options.AddPolicy(per.name, policy => policy.RequireClaim("Permission", per.id.ToString()));
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);			
		}

		/// <summary>
		/// Metodo de configuracion para inicializar el componente de los servicios configurados e inicial la canalizacion HTTP
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();

			//Configuracion para responder la pagina por default
			app.UseStatusCodePages(async context =>
			{
				if(context.HttpContext.Request.Path == "/")
				{
					context.HttpContext.Response.ContentType = "text/plain";
					context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

					await context.HttpContext.Response.WriteAsync(
						"Status code page, status code: " +
						context.HttpContext.Response.StatusCode);
				}					
			});

			//Configurar el metodo de respuesta para responder el codigo de estatus y el json de respuesta
			app.Use(async (context, next) => 
			{
				await next();
				if(context.Response.StatusCode == StatusCodes.Status401Unauthorized || context.Response.StatusCode == StatusCodes.Status403Forbidden)
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					context.Response.ContentType = "application/json; charset=utf-8";
					string messageEng = ConfigurationSite._cofiguration["MessagesResponse:MessageEng"];
					string messageEsp = ConfigurationSite._cofiguration["MessagesResponse:MessageEsp"];
					var result = JsonConvert.SerializeObject(new { messageEsp, messageEng });
					await context.Response.WriteAsync(result);
				}
			});

			//Configuracion para utilizar la autenticacion configurada
			app.UseCors(MyAllowSpecificOrigins);
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}

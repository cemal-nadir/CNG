#nullable enable
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CNG.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddConsoleLogService(this IServiceCollection services)
		{
			services.AddLogging(c => c.AddFilter("Default", LogLevel.Information).AddFilter("Microsoft", LogLevel.Information).AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning).AddConsole());
			Console.WriteLine("Console Log Service is installed");
		}

		public static void AddSwaggerServiceWithBearer(this IServiceCollection services, string name,
			string? version = null, string? description = null, string? contactName = null, string? contactMail = null,
			string? contactUrl = null,bool xmlAccess=false)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = name + ".API",
					Version = version??"v1",
					Description = description??".NET Core",
					Contact = new OpenApiContact()
					{
						Name = contactName?? name + " Api Service",
						Email = contactMail??null,
						Url =contactUrl!=null ? new Uri(contactUrl):null

					}
				});
				var securityScheme = new OpenApiSecurityScheme()
				{
					Description = "JWT Authorization: Bearer {token}",
					Name = "Authorization",
					In = (ParameterLocation)1,
					Type = (SecuritySchemeType)1,
					Scheme = "bearer",
					Reference = new OpenApiReference()
					{
						Type = (ReferenceType)6,
						Id = "Bearer"
					}
				};
				c.AddSecurityDefinition("Bearer", securityScheme);
				var swaggerGenOptions = c;
				var securityRequirement = new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme()
						{
							Reference = new OpenApiReference
							{
								Type = (ReferenceType)6,
								Id = "Bearer"
							}
						},
						new List<string>()
					}
				};
				swaggerGenOptions.AddSecurityRequirement(securityRequirement);

                if (!xmlAccess) return;
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{name}.xml");
                c.IncludeXmlComments(xmlPath);

            });
			Console.WriteLine("Swagger Service is installed");
		}

		public static void AddSwaggerServiceWithBasic(this IServiceCollection services, string name,
			string? version = null, string? description = null, string? contactName = null, string? contactMail = null,
			string? contactUrl = null,bool xmlAccess=false)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = name + ".API",
					Version = version ?? "v1",
					Description = description ?? ".NET Core",
					Contact = new OpenApiContact()
					{
						Name = contactName ?? name + " Api Service",
						Email = contactMail ?? null,
						Url = contactUrl != null ? new Uri(contactUrl) : null

					}
				});
				var securityScheme = new OpenApiSecurityScheme()
				{
					Description = "Basic Authorization: Basic {token}",
					Name = "Authorization",
					In = (ParameterLocation)1,
					Type = (SecuritySchemeType)1,
					Scheme = "basic",
					Reference = new OpenApiReference()
					{
						Type = (ReferenceType)6,
						Id = "Basic"
					}
				};
				c.AddSecurityDefinition("Basic", securityScheme);
				var swaggerGenOptions = c;
				var securityRequirement = new OpenApiSecurityRequirement
				{ { new OpenApiSecurityScheme()
			{
				Reference = new OpenApiReference
				{
					Type = (ReferenceType) 6,
					Id = "Basic"
				}
			}, new List<string>() } };
				swaggerGenOptions.AddSecurityRequirement(securityRequirement);

                if (!xmlAccess) return;
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{name}.xml");
                c.IncludeXmlComments(xmlPath);
            });
			Console.WriteLine("Swagger Service is installed");
		}

		public static void UseSwaggerService(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Service");
				c.DefaultModelsExpandDepth(-1);
				c.DocExpansion(DocExpansion.None);
			});
			Console.WriteLine("Swagger Enabled");
		}
	}
}

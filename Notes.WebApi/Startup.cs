﻿using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Notes.WebApi.Middleware;

namespace Notes.WebApi;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
        });
        services.AddApplication();
        services.AddPersistence(Configuration);
        services.AddControllers();
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });
        
        services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5250";
                options.Audience = "NotesWebAPI";
                options.RequireHttpsMetadata = false;
            });

        services.AddSwaggerGen(cfg =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            cfg.IncludeXmlComments(xmlPath);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCustomExceptionHandler();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
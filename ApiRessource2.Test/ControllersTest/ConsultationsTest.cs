﻿using ApiRessource2.Controllers;
using ApiRessource2.Test.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRessource2.Test.ControllersTest
{
    public class ConsultationsTest : IDisposable
    {
        private readonly DataContext _context;
        private readonly ConsultationsController _controller;


        public ConsultationsTest()
        {
            var contentRootPath = "C:\\Users\\benja\\Documents\\GitHub\\ApiRessource2";
            var environment = new TestWebHostEnvironment(contentRootPath);

            var services = new ServiceCollection();
            services.AddSingleton<IWebHostEnvironment>(environment);
            services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase("onf_resourcedev2test"));
            var serviceProvider = services.BuildServiceProvider();

            _context = serviceProvider.GetService<DataContext>();
            _controller = new ConsultationsController(_context);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _context.Database.EnsureDeleted();
        }
    }
}

using JA.FinancePark.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JA.FinancePark.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JafpaControllerBase : ControllerBase
    {
        protected readonly IAppSettings _appSettings;

        public JafpaControllerBase(IServiceProvider serviceProvider)
        {
            _appSettings = serviceProvider.GetRequiredService<IAppSettings>();
        }
    }
}

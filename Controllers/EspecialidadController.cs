using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Turnos.Controllers
{
    public class EspecialidadController : Controller
    {

        public EspecialidadController()
        {



        }

        public IActionResult Index()
        {

            return View();
            
        }
        
    }
}


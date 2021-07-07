using AtService.CustomConatiner;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtService.HeadController
{
    public abstract class UPSController: Controller
    {
        public UPSController()
        {
            IoCContainer.BuildUp(this);
        }

        ~UPSController()
        {
            IoCContainer.Teardown(this);
        }
    }
}

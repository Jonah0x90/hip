using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ZLSoft.AppContext
{
    [FilterBase, /*FilterMenuLink,FilterCheckLogin,FilterException, */FilterNoCache]
    public class MVCController:SessionController
    {
        
    }
}

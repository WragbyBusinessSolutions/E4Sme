using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Helpers
{
  public class EmailTemplateHelper
  {
    public string GetTemplate(string MaitoGet)
    {

      var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emailtemplate", (MaitoGet + ".html"));

      return File.ReadAllText(path);
    }
  }
}

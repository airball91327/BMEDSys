using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.RepairModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDIS.Areas.BMED.Components.AttainFile
{
    public class BMEDAttainFileUploadViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string doctype, string docid)
        {
            AttainFileModel attainFile = new AttainFileModel();
            attainFile.DocType = doctype;
            attainFile.DocId = docid;
            attainFile.SeqNo = 2;
            attainFile.IsPublic = "N";
            attainFile.FileLink = "default";

            return View(attainFile);
        }
    }
}

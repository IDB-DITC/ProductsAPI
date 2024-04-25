using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FastReport.Web;
using ProductsAPI.Data;
using FastReport.Export.PdfSimple;
using System.Text;
using FastReport;
using FastReport.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebReportController : ControllerBase
	{

		private readonly IWebHostEnvironment _webHost;
		private FastReport.Export.Image.ImageExport imgExp;
		private FastReport.Export.PdfSimple.PDFSimpleExport pdfExp;

		public WebReportController( IWebHostEnvironment webHost)
		{
			_webHost = webHost;
		}

		[HttpGet("{id:int?}")]
		//[Route("get")]
		public ActionResult<string?> Get(int id)
		{
			try
			{
				WebReport webReport = new WebReport();

				webReport.Report.Load(_webHost.ContentRootPath + "\\Reports\\ProductInfo.frx");

				MsSqlDataConnection sqlConnection = new MsSqlDataConnection();


				sqlConnection.ConnectionString = "Server=.;Database=ProductsDbContext-55;Trusted_Connection=True;MultipleActiveResultSets=true;";


				webReport.Report.SetParameterValue("CONN", sqlConnection.ConnectionString);


				webReport.Report.SetParameterValue("CatID", id);

				webReport.Report.Prepare();


				PDFSimpleExport export = new PDFSimpleExport();
				string pdf;
				byte[] pdfBytes;
				MemoryStream ms = new MemoryStream();

				webReport.Report.Export(export, ms);
				ms.Position = 0;
				pdfBytes = ms.ToArray();

				 pdf = Convert.ToBase64String(pdfBytes);
				return Ok(pdf);
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}
		}

	}
}

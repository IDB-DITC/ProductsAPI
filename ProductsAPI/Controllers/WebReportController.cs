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


		private readonly ProductsAPIContext _context;
		private readonly IWebHostEnvironment _webHost;
		private FastReport.Export.Image.ImageExport imgExp;
		private FastReport.Export.PdfSimple.PDFSimpleExport pdfExp;

		public WebReportController(ProductsAPIContext context, IWebHostEnvironment webHost)
		{
			_context = context;
			_webHost = webHost;
		}

		[HttpGet("{id:int?}")]
		//[Route("get")]
		public ActionResult<string?> Get(int id=0)
		{
			try
			{
				//Report report = new();

				//report.RegisterData(_context.Product.ToList(), "Products");


				WebReport webReport = new WebReport();

				webReport.Report.Load(_webHost.ContentRootPath + "\\Reports\\ProductInfo.frx");




				MsSqlDataConnection sqlConnection = new MsSqlDataConnection();

				//sqlConnection.ConnectionString = _context.Database.GetConnectionString();


				sqlConnection.ConnectionString = "Server=.;Database=ProductsDbContext-55;Trusted_Connection=True;MultipleActiveResultSets=true;";

				//sqlConnection.CreateAllTables();
				//webReport.Report.Dictionary.Connections.Add(sqlConnection);


				webReport.Report.SetParameterValue("CONN", sqlConnection.ConnectionString);


				webReport.Report.SetParameterValue("CatID", id);




				//MsSqlDataConnection connection = new MsSqlDataConnection();

				//connection.ConnectionString = "";
				//WebReport WebReport = new WebReport();
				//WebReport.Width = "1000";
				//WebReport.Height = "1000";

				//WebReport.Report.Load(Environment.CurrentDirectory + "/Reports/ProductInfo.frx"); //Loading a report into a WebReport object

				//webReport.Report.RegisterData(_context.Product.ToList(), "Product"); //Registering a data source in a report


				webReport.Report.Prepare();

				//var html = webReport.Render();

				PDFSimpleExport export = new PDFSimpleExport();
				string pdf;
				byte[] pdfBytes;
				MemoryStream ms = new MemoryStream();

				webReport.Report.Export(export, ms);
				ms.Position = 0;
				pdfBytes = ms.ToArray();



				//return WebReport;
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

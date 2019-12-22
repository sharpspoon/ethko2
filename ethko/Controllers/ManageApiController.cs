using DataTables;
using System.Web;
using System.Web.Http;

namespace ethko.Controllers
{
    //[Authorize]
    public class ManageApiController : ApiController
    {
        ethko_dbEntities1 entities = new ethko_dbEntities1();

        //[Route("api/staff")]
        //[HttpGet]
        //[HttpPost]
        //public IHttpActionResult Staff()
        //{
        //    var request = HttpContext.Current.Request;
        //    var settings = Properties.Settings.Default;

        //    using (var db = new Database(settings.DbType, settings.DbConnection))
        //    {
        //        var response = new Editor(db, "datatables_demo")
        //            .Model<StaffModel>()
        //            .Field(new Field("first_name")
        //                .Validator(Validation.NotEmpty())
        //            )
        //            .Field(new Field("last_name"))
        //            .Field(new Field("extn")
        //                .Validator(Validation.Numeric())
        //            )
        //            .Field(new Field("age")
        //                .Validator(Validation.Numeric())
        //                .SetFormatter(Format.IfEmpty(null))
        //            )
        //            .Field(new Field("salary")
        //                .Validator(Validation.Numeric())
        //                .SetFormatter(Format.IfEmpty(null))
        //            )
        //            .Field(new Field("start_date")
        //                .Validator(Validation.DateFormat(
        //                    Format.DATE_ISO_8601,
        //                    new ValidationOpts { Message = "Please enter a date in the format yyyy-mm-dd" }
        //                ))
        //                .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
        //                .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
        //            )
        //            .Process(request)
        //            .Data();

        //        return Json(response);
        //    }
        //}
    }
}
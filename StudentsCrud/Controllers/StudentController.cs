using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentsCrud.Models;
using Smartsheet.Api.OAuth;
using Microsoft.Extensions.Configuration;
using StudentsCrud.Controllers;
using StudentsCrud.Helper;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace StudentsCrud.Controllers
{
	[Route("Api/Student")]
	public class StudentController : Controller
	{
		IConfiguration _iconfiguration;
		public StudentController(IConfiguration iconfiguration)
		{
			_iconfiguration = iconfiguration;
		}

		static Dictionary<string, long> columnMap = new Dictionary<string, long>();

		[HttpGet("GetStudentDetails")]
		public List<Student> GetStudentDetails(int pageSize, int page)
		{
			List<Student> result = new List<Student>();
			try
			{
				Token token = new Token();
				String accessToken = _iconfiguration["AccessToken"];
				long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
				token.AccessToken = accessToken;
				SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(token.AccessToken).Build();
				Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, pageSize, page);
				foreach (Row tmpRow in sheet.Rows)
				{
					Student StudentData = null;
					foreach (Cell tmpCell in tmpRow.Cells)
					{
						StudentData = new Student()
						{
							StudentName = Convert.ToString(tmpRow.Cells[0].Value),
							Grade = Convert.ToString(tmpRow.Cells[1].Value),
							Address = Convert.ToString(tmpRow.Cells[2].Value),
							City = Convert.ToString(tmpRow.Cells[3].Value),
							Country = Convert.ToString(tmpRow.Cells[4].Value),
							Postal = Convert.ToString(tmpRow.Cells[5].Value),
							Phone = Convert.ToString(tmpRow.Cells[6].Value),
							Email = Convert.ToString(tmpRow.Cells[7].Value),
							RowId = Convert.ToInt64(tmpRow.Id.Value),
							TotalRowCount=Convert.ToInt32(sheet.TotalRowCount)
						};
					}
					result.Add(StudentData);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		[HttpGet("StudentdetailByrowId/{id}")]
		public Row StudentdetailByrowId(long id)
		{
			String accessToken = _iconfiguration["AccessToken"];
			long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
			SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
			Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
			Row row = smartsheet.SheetResources.RowResources.GetRow(sheetid, id, null, null);
			return row;
		}

		//[HttpPost("AddorUpdatestudent")]		
		//public object AddorUpdatestudent(Student st)
		//{
		//	string status = "";
		//	try
		//	{
		//		String accessToken = _iconfiguration["AccessToken"];
		//		long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
		//		SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
		//		Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);

		//		if (st == null)
		//		{
		//			return BadRequest("Given data is null");
		//		}
		//		else
		//		{
		//			if (st.RowId == 0)
		//			{

		//				// Add rows to sheet

		//				columnMap.Clear();
		//				foreach (Column column in sheet.Columns)
		//					columnMap.Add(column.Title, (long)column.Id);

		//				List<Cell> cells = new List<Cell>();
		//				Cell[] cellsA = null;
		//				Row rowA = null;

		//				cellsA = new Cell[]
		//				{
		//			 new Cell.AddCellBuilder(sheet.Columns[0].Id, st.StudentName).Build()
		//			,new Cell.AddCellBuilder(sheet.Columns[1].Id, st.Grade).Build()
		//			,new Cell.AddCellBuilder(sheet.Columns[2].Id, st.Address).Build()
		//			,new Cell.AddCellBuilder(sheet.Columns[3].Id, st.City).Build()
		//			,new Cell.AddCellBuilder(sheet.Columns[4].Id, st.Country).Build()
		//			,new Cell.AddCellBuilder(sheet.Columns[5].Id, st.Postal).Build()
		//			,new Cell.AddCellBuilder(sheet.Columns[6].Id, st.Phone).Build()
		//			,new Cell.AddCellBuilder(sheet.Columns[7].Id, st.Email).Build()

		//				};
		//				rowA = new Row.AddRowBuilder(true, null, null, null, null).SetCells(cellsA).Build();
		//				IList<Row> newRows = smartsheet.SheetResources.RowResources.AddRows(sheetid, new Row[] { rowA });
		//				return status="Data inserted Successfully";
		//			}
		//			else
		//			{
		//				if (st.RowId > 0)
		//				{
		//					var obj = StudentdetailByrowId(st.RowId);
		//					if (obj.Id > 0)
		//					{

		//					}
		//					var cellToUpdateB = new Cell
		//					{
		//						ColumnId = 1888812600190852,
		//						Value = "A"
		//					};

		//					// Identify row and add new cell values to it
		//					var rowToUpdate = new Row
		//					{
		//						Id = 6572427401553796,
		//						Cells = new Cell[] { cellToUpdateB }
		//					};
		//					IList<Row> updatedRow = smartsheet.SheetResources.RowResources.UpdateRows(sheetid, new Row[] { rowToUpdate });
		//					return status = "Updated Successfully";
		//				}
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.Write(ex.Message);
		//	}
		//	return status;

		//}
		[HttpDelete("Delete/{id}")]
		public string Delete(long id)
		{
			String accessToken = _iconfiguration["AccessToken"];
			long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
			SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
			Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
			smartsheet.SheetResources.RowResources.DeleteRows(sheetid, new long[] { id }, true);
			return "Record Deleted Successfully!!";
		}

		[HttpPost]
		[Route("AddorUpdatestudent")]
		public object AddorUpdatestudent([FromBody] Student student)
		{
			string status = "";

			return status;

		}

		[HttpPost("Addstudent")]
		public object Addstudent([FromBody] Student st)
		{
			string status = "";
			try
			{
				String accessToken = _iconfiguration["AccessToken"];
				long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
				SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
				Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);

				if (st == null)
				{
					return BadRequest("Given data is null");
				}
				else
				{
					if (st.RowId == 0)
					{
						// Add rows to sheet
						columnMap.Clear();
						foreach (Column column in sheet.Columns)
							columnMap.Add(column.Title, (long)column.Id);

						List<Cell> cells = new List<Cell>();
						Cell[] cellsA = null;
						Row rowA = null;

						cellsA = new Cell[]
						{
					 new Cell.AddCellBuilder(sheet.Columns[0].Id, st.StudentName).Build()
					,new Cell.AddCellBuilder(sheet.Columns[1].Id, st.Grade).Build()
					,new Cell.AddCellBuilder(sheet.Columns[2].Id, st.Address).Build()
					,new Cell.AddCellBuilder(sheet.Columns[3].Id, st.City).Build()
					,new Cell.AddCellBuilder(sheet.Columns[4].Id, st.Country).Build()
					,new Cell.AddCellBuilder(sheet.Columns[5].Id, st.Postal).Build()
					,new Cell.AddCellBuilder(sheet.Columns[6].Id, st.Phone).Build()
					,new Cell.AddCellBuilder(sheet.Columns[7].Id, st.Email).Build()

						};
						rowA = new Row.AddRowBuilder(true, null, null, null, null).SetCells(cellsA).Build();
						IList<Row> newRows = smartsheet.SheetResources.RowResources.AddRows(sheetid, new Row[] { rowA });
						return status = "Data inserted Successfully";
					}



				}
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			return status;

		}
		[HttpPost("Updatestudent")]
		public object Updatestudent([FromBody] Student st)
		{
			try
			{
				String accessToken = _iconfiguration["AccessToken"];
				long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
				SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
				Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
				columnMap.Clear();
				foreach (Column column in sheet.Columns)
					columnMap.Add(column.Title, (long)column.Id);
				if (st == null)
				{
					return BadRequest("Given data is null");
				}
				else
				{
					if (st.RowId != 0)
					{
						Cell[] cell = new Cell[]
						{ new Cell.UpdateCellBuilder(sheet.Columns[0].Id, st.StudentName).Build()
							,new Cell.AddCellBuilder(sheet.Columns[1].Id, st.Grade).Build()
							,new Cell.AddCellBuilder(sheet.Columns[2].Id, st.Address).Build()
							,new Cell.AddCellBuilder(sheet.Columns[3].Id, st.City).Build()
							,new Cell.AddCellBuilder(sheet.Columns[4].Id, st.Country).Build()
							,new Cell.AddCellBuilder(sheet.Columns[5].Id, st.Postal).Build()
							,new Cell.AddCellBuilder(sheet.Columns[6].Id, st.Phone).Build()
							,new Cell.AddCellBuilder(sheet.Columns[7].Id, st.Email).Build()
						};
						Row row = new Row.UpdateRowBuilder(st.RowId).SetCells(cell).Build();
						IList<Row> updatedRow = smartsheet.SheetResources.RowResources.UpdateRows(sheetid, new Row[] { row });
					}
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			return "Updated Successfully";

		}
	
		[HttpGet]
		public RedirectResult oauthurl()
		{
			OAuthFlow oauth = new OAuthFlowBuilder()
				 .SetTokenURL("https://api.smartsheet.com/2.0/token")
				 .SetAuthorizationURL("https://www.smartsheet.com/b/authorize")
				 .SetClientId(_iconfiguration["SmartsheetClientId"])
				 .SetClientSecret(_iconfiguration["SmartsheetClientSecret"])
				 .SetRedirectURL("https://localhost:44398/callback")
				 .Build();

			string url = oauth.NewAuthorizationURL
			(
				new Smartsheet.Api.OAuth.AccessScope[]
				{
				Smartsheet.Api.OAuth.AccessScope.READ_SHEETS,
				Smartsheet.Api.OAuth.AccessScope.WRITE_SHEETS,
				Smartsheet.Api.OAuth.AccessScope.SHARE_SHEETS,
				Smartsheet.Api.OAuth.AccessScope.DELETE_SHEETS,
				Smartsheet.Api.OAuth.AccessScope.CREATE_SHEETS,
				Smartsheet.Api.OAuth.AccessScope.READ_USERS,
				Smartsheet.Api.OAuth.AccessScope.ADMIN_USERS,
				Smartsheet.Api.OAuth.AccessScope.ADMIN_SHEETS,
				Smartsheet.Api.OAuth.AccessScope.ADMIN_WORKSPACES,
				},
				"key=Test"
			);
			return RedirectPermanent(url);
		}

		[Route("callback")]
		[HttpGet]
		public string OauthCallback(string code, int expires_in, string state)
		{
			OAuthFlow oauth = new OAuthFlowBuilder()
				.SetTokenURL("https://api.smartsheet.com/2.0/token")
				.SetAuthorizationURL("https://www.smartsheet.com/b/authorize")
				.SetClientId(_iconfiguration["SmartsheetClientId"])
				.SetClientSecret(_iconfiguration["SmartsheetClientSecret"])
				.SetRedirectURL("https://localhost:44398/callback")
				.Build();

			AuthorizationResult authResult = oauth.ExtractAuthorizationResult("https://localhost:44398/callback" + Request.QueryString.ToString());
			Token token = oauth.ObtainNewToken(authResult);
			return token.AccessToken;
		}
		[HttpPost("GetAccessToken")]
		public string GetAccessToken(AccessTokenData data)
		{
			string response = string.Empty;
			try
			{
				using (var Client = new HttpClient())
				{
					HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
					{
					 {"grant_type", "authorization_code"},
					 {"client_id", _iconfiguration["SmartsheetClientId"]},
					 {"client_secret", _iconfiguration["SmartsheetClientSecret"]},
					 {"code" ,data.Code }
					});
					HttpResponseMessage message = Client.PostAsync(AccessTokenData.TokenUrl, content).Result;
					response = message.Content.ReadAsStringAsync().Result;
				}

				Newtonsoft.Json.Linq.JObject obj = JObject.Parse(response);
				data.AuthToken = (string)obj["access_token"];
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return response;
		}

		[HttpPost("GetRefreshAccessToken")]
		public string GetRefreshAccessToken(AccessTokenData data)
		{
			string response = string.Empty;
			try
			{
				using (var Client = new HttpClient())
				{
					HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
					{
					 {"grant_type", "refresh_token"},
					 {"client_id", _iconfiguration["SmartsheetClientId"]},
					 {"client_secret", _iconfiguration["SmartsheetClientSecret"]},
					 {"refresh_token" ,data.RefreshToken }
					});
					HttpResponseMessage message = Client.PostAsync(AccessTokenData.TokenUrl, content).Result;
					response = message.Content.ReadAsStringAsync().Result;
				}
				Newtonsoft.Json.Linq.JObject obj = JObject.Parse(response);
				data.AuthToken = (string)obj["access_token"];
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return response;
		}

	}

}

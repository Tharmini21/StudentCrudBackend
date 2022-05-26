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

namespace StudentsCrud.Controllers
{
	[Route("Api/Student")]
	public class StudentController : Controller
	{

		static Dictionary<string, long> columnMap = new Dictionary<string, long>(); // Map from friendly column name to column Id 
																					// GET: StudentController/Details/5
		public ActionResult Details()
		{
			// Initialize client. Uses API access token from environment variable SMARTSHEET_ACCESS_TOKEN
			String accessToken = "RGQpN47l4kJBvLaOc4jlTONkd2jaexjuUe3pA";
			SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
			// Get current user
			UserProfile userProfile = smartsheet.UserResources.GetCurrentUser();

			long sheetid = 4988319210727300;
			Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
			Console.WriteLine("Loaded " + sheet.Rows.Count + " rows from sheet: " + sheet.Name);

			// Build column map for later reference
			foreach (Column column in sheet.Columns)
				columnMap.Add(column.Title, (long)column.Id);

			Console.WriteLine("Done (Hit enter)");
			Console.ReadLine();
			return View();
		}

		[HttpGet("GetStudentDetails")]
		public List<Student> GetStudentDetails()
		{
			List<Student> result = new List<Student>();
			try
			{
				Token token = new Token();
				String accessToken = "RGQpN47l4kJBvLaOc4jlTONkd2jaexjuUe3pA";
				token.AccessToken = accessToken;
				long sheetid = 4988319210727300;
				SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(token.AccessToken).Build();
				Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
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
							RowId = Convert.ToString(tmpRow.Id.Value)
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

		[Route("StudentdetailByrowId")]
		[HttpPost]
		public Row StudentdetailByrowId(long id)
		{
			String accessToken = "RGQpN47l4kJBvLaOc4jlTONkd2jaexjuUe3pA";
			SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
			long sheetid = 4988319210727300;
			Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);

			Row row = smartsheet.SheetResources.RowResources.GetRow(
  sheetid,               // sheetId
  id,                   // rowId
  null,                           // IEnumerable<RowInclusion> include
  null                            // IEnumerable<RowExclusion> exclude
);
			return row;
		}
		[HttpPost]
		[Route("AddorUpdatestudent")]
		public object AddorUpdatestudent(Student st)
		{
			try
			{
				if (st.RowId != "")
				{

					// Add rows to sheet
					String accessToken = "RGQpN47l4kJBvLaOc4jlTONkd2jaexjuUe3pA";
					SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
					long sheetid = 4988319210727300;
					Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
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
				}
				else
				{

				}
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			return "Data not insert";

		}
		[Route("Delete")]
		[HttpPost]
		public string Delete(int id)
		{
			String accessToken = "RGQpN47l4kJBvLaOc4jlTONkd2jaexjuUe3pA";
			SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
			long sheetid = 4988319210727300;
			Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
			smartsheet.SheetResources.RowResources.DeleteRows(
  sheetid,                                    // sheetId
  new long[] { id },     // rowIds
  true                                                 // Boolean ignoreRowsNotFound
);
			return "Delete Successfuly";
			
		}


	}
}

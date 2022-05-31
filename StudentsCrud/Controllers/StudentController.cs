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
		public List<Student> GetStudentDetails()
		{
			List<Student> result = new List<Student>();
			try
			{
				Token token = new Token();
				String accessToken = _iconfiguration["AccessToken"];
				long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
				token.AccessToken = accessToken;
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
							RowId = Convert.ToInt64(tmpRow.Id.Value)
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
			Row row = smartsheet.SheetResources.RowResources.GetRow(sheetid,id,null,null);
			return row;
		}
		[HttpPost("AddorUpdatestudent")]
		public object AddorUpdatestudent([FromBody]Student st)
		{
			try
			{
				String accessToken = _iconfiguration["AccessToken"];
				long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
				SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
				Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
				if (st.RowId ==0)
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
					
				}
				else
				{
					if (st.RowId > 0)
					{
						var obj=StudentdetailByrowId(st.RowId);
						if (obj.Id > 0)
						{
							
						}
						var cellToUpdateB = new Cell
						{
							ColumnId = 1888812600190852,
							Value = "A"
						};

						// Identify row and add new cell values to it
						var rowToUpdate = new Row
						{
							Id = 6572427401553796,
							Cells = new Cell[] { cellToUpdateB }
						};
						IList<Row> updatedRow = smartsheet.SheetResources.RowResources.UpdateRows(sheetid,new Row[] { rowToUpdate });
						return "Updated Successfully";
					}
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}

			return "Data inserted Successfully";
		}
		[HttpDelete("Delete/{id}")]
		public string Delete(long id)
		{
			String accessToken = _iconfiguration["AccessToken"];
			long sheetid = Convert.ToInt64(_iconfiguration["SheetId"]);
			SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
			Sheet sheet = smartsheet.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
			smartsheet.SheetResources.RowResources.DeleteRows(sheetid, new long[] { id },true);
			return "Record Deleted Successfully!!";
		}
	}
}

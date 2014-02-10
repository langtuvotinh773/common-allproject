 // convert chuyen sang list (arr)
 //89;90;91;92;93;
strId = strId.Substring(0, strId.Length - 1);
List<int> ints = strId.Split(';').ToList().ConvertAll<int>(s => Convert.ToInt32(s));
int[] arrRequestId = strId.Split(';').Select(n => Convert.ToInt32(n)).ToArray();

//function Loai bo dau 
public string bodau(string accented)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = accented.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
		}
//--------------------------------------------------
public class StringUtil

{

    private static readonly string[] VietnameseSigns = new string[]

    {

        "aAeEoOuUiIdDyY",

        "áà??ãâ?????a?????",

        "ÁÀ??ÃÂ?????A?????",

        "éè???ê?????",

        "ÉÈ???Ê?????",

        "óò??õô?????o?????",

        "ÓÒ??ÕÔ?????O?????",

        "úù??uu?????",

        "ÚÙ??UU?????",

        "íì??i",

        "ÍÌ??I",

        "d",

        "Ð",

        "ý????",

        "Ý????"

    };

 

    public static string RemoveSign4VietnameseString(string str)

    {

        //tien hanh thay the bo dau cho chuoi

        for (int i = 1; i < VietnameseSigns.Length; i++)

        {

            for (int j = 0; j < VietnameseSigns[i].Length; j++)

                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

        }

        return str;

    }

}
///////////////////////////////// START Foreach Datatable IN VIEW MVC
/// View  (w/ Model strongly typed as System.Data.DataTable)
<table border="1" cellpadding="5">
<thead>
   <tr>
      @foreach (System.Data.DataColumn col in Model.Columns)
      {
         <th>@col.Caption</th>
      }
   </tr>
</thead>
<tbody>
@foreach(System.Data.DataRow row in Model.Rows)
{
   <tr>
      @foreach (var cell in row.ItemArray)
      {
         <td>@cell.ToString()</td>
      }
   </tr>
}      
</tbody>
/// Controller
public ActionResult Index()
{
    ViewData["Message"] = "Welcome to ASP.NET MVC!";

    DataTable dt = new DataTable("MyTable");
    dt.Columns.Add(new DataColumn("Col1", typeof(string)));
    dt.Columns.Add(new DataColumn("Col2", typeof(string)));
    dt.Columns.Add(new DataColumn("Col3", typeof(string)));

    for (int i = 0; i < 3; i++)
    {
        DataRow row = dt.NewRow();
        row["Col1"] = "col 1, row " + i;
        row["Col2"] = "col 2, row " + i;
        row["Col3"] = "col 3, row " + i;
        dt.Rows.Add(row);
    }

    return View(dt); //passing the DataTable as my Model
}
///////////////////////////////// END Foreach Datatable IN VIEW MVC
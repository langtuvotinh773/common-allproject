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
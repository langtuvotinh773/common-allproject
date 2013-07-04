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